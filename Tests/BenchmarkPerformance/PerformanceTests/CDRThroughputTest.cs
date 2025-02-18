using System.Globalization;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using CdrWrapper;
using OpenDDSharp.BenchmarkPerformance.Helpers;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

internal sealed class CDRThroughputTest : IDisposable
{
    private const int DOMAIN_ID = 42;

    private readonly Random _random = new ();
    private readonly int _totalSamples;
    private readonly KeyedOctets _sample;

    private ulong _samplesReceived;

    private DomainParticipantFactory _dpf;
    private DomainParticipant _participant;
    private Topic _topic;
    private Publisher _publisher;
    private KeyedOctetsDataWriter _dataWriter;
    private Subscriber _subscriber;
    private KeyedOctetsDataReader _dataReader;
    private StatusCondition _statusCondition;
    private WaitSet _waitSet;
    private Thread _readerThread;

    public CDRThroughputTest(int totalSamples, ulong totalPayload)
    {
        _totalSamples = totalSamples;

        var payload = new byte[totalPayload];
        _random.NextBytes(payload);

        InitializeDDSEntities();

        _sample = new KeyedOctets
        {
            KeyField = "1",
            ValueField = payload,
        };
    }

    public ulong Run()
    {
        _samplesReceived = 0;
        _readerThread = new Thread(ReaderThreadProc)
        {
            IsBackground = true,
            Priority = ThreadPriority.Highest,
        };
        _readerThread.Start();

        for (var i = 1; i <= _totalSamples; i++)
        {
            _dataWriter.Write(_sample);
        }

        _readerThread.Join();

        return _samplesReceived;
    }

    private void InitializeDDSEntities()
    {
        _dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSPendingTimeout", "3");

        var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        var configName = "openddsharp_rtps_interop_" + guid;
        var instName = "internal_openddsharp_rtps_transport_" + guid;

        var config = TransportRegistry.Instance.CreateConfig(configName);
        var inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
        var transport = new RtpsUdpInst(inst)
        {
            UseMulticast = false,
            LocalAddress = "127.0.0.1:",
        };
        config.Insert(transport);

        _participant = _dpf.CreateParticipant(DOMAIN_ID);
        TransportRegistry.Instance.BindConfig(configName, _participant);

        var typeSupport = new KeyedOctetsTypeSupport();
        var typeName = typeSupport.GetTypeName();
        typeSupport.RegisterType(_participant, typeName);

        var topicQos = new TopicQos
        {
            Reliability = { Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos },
            History = { Kind = HistoryQosPolicyKind.KeepAllHistoryQos}
        };
        _topic = _participant.CreateTopic(Guid.NewGuid().ToString(), typeName, topicQos);

        var pubQos = new PublisherQos
        {
            EntityFactory = { AutoenableCreatedEntities = false }
        };
        _publisher = _participant.CreatePublisher(pubQos);

        var dwQos = new DataWriterQos
        {
            Reliability =
            {
                Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                MaxBlockingTime = new Duration
                {
                    Seconds = Duration.InfiniteSeconds,
                    NanoSeconds = Duration.InfiniteNanoseconds
                },
            },
            History = { Kind = HistoryQosPolicyKind.KeepAllHistoryQos },
        };
        var dw = _publisher.CreateDataWriter(_topic, dwQos);
        _dataWriter = new KeyedOctetsDataWriter(dw);

        var subQos = new SubscriberQos
        {
            EntityFactory = { AutoenableCreatedEntities = false }
        };
        _subscriber = _participant.CreateSubscriber(subQos);

        var drQos = new DataReaderQos
        {
            Reliability =
            {
                Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
                MaxBlockingTime = new Duration
                {
                    Seconds = Duration.InfiniteSeconds,
                    NanoSeconds = Duration.InfiniteNanoseconds
                },
            },
            History = { Kind = HistoryQosPolicyKind.KeepAllHistoryQos },
        };
        var dr =  _subscriber.CreateDataReader(_topic, drQos);
        _dataReader = new KeyedOctetsDataReader(dr);

        _waitSet = new WaitSet();
        _statusCondition = _dataReader.StatusCondition;
        _statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
        _waitSet.AttachCondition(_statusCondition);

        _dataWriter.Enable();
        _dataReader.Enable();

        _dataReader.WaitForPublications(1, 5_000);
        _dataWriter.WaitForSubscriptions(1, 5_000);
    }

    private void ReaderThreadProc()
    {
        var samples = new List<KeyedOctets>();
        var sampleInfos = new List<SampleInfo>();
        var conditions = new List<Condition>();
        while (true)
        {
            _waitSet.Wait(conditions);

            _dataReader.Take(samples, sampleInfos);

            _samplesReceived += (ulong)samples.Count;
            if (_samplesReceived >= (ulong)_totalSamples)
            {
                return;
            }
        }
    }

    public void Dispose()
    {
        _publisher.DeleteDataWriter(_dataWriter);
        _publisher.DeleteContainedEntities();
        _participant.DeletePublisher(_publisher);

        _waitSet.DetachCondition(_statusCondition);
        _dataReader.DeleteContainedEntities();
        _subscriber.DeleteDataReader(_dataReader);
        _subscriber.DeleteContainedEntities();
        _participant.DeleteSubscriber(_subscriber);

        _participant.DeleteTopic(_topic);

        _participant.DeleteContainedEntities();
        _dpf.DeleteParticipant(_participant);
    }
}