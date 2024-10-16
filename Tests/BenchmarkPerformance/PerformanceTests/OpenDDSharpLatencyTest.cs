using System.Globalization;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using CdrWrapper;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

internal sealed class OpenDDSharpLatencyTest : IDisposable
{
    private const int DOMAIN_ID = 42;

    private readonly ManualResetEventSlim _evt;
    private readonly Random _random = new ();
    private readonly int _totalInstances;
    private readonly int _totalSamples;
    private readonly Dictionary<int, InstanceHandle> _instanceHandles = new();
    private readonly KeyedOctets _sample;

    private int _count;

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

    public OpenDDSharpLatencyTest(int totalInstances, int totalSamples, ulong totalPayload)
    {
        _totalInstances = totalInstances;
        _totalSamples = totalSamples;
        _evt = new ManualResetEventSlim(false);

        var payload = new byte[totalPayload];
        _random.NextBytes(payload);

        InitializeDDSEntities();

        _count = 0;

        _readerThread.Start();
        _sample = new KeyedOctets
        {
            ValueField = payload,
        };
    }

    public IList<TimeSpan> Run()
    {
        var latencyHistory = new List<TimeSpan>();

        for (var i = 1; i <= _totalSamples; i++)
        {
            for (var j = 1; j <= _totalInstances; j++)
            {
                _sample.KeyField = j.ToString(CultureInfo.InvariantCulture);

                if (!_instanceHandles.TryGetValue(j, out var instanceHandle))
                {
                    instanceHandle = _dataWriter.RegisterInstance(_sample);
                    _instanceHandles.Add(j, instanceHandle);
                }

                var publicationTime = DateTime.UtcNow.Ticks;

                _dataWriter.Write(_sample, instanceHandle);

                _evt.Wait();

                var receptionTime = DateTime.UtcNow.Ticks;
                var latency = TimeSpan.FromTicks(receptionTime - publicationTime);
                latencyHistory.Add(latency);

                _evt.Reset();
            }
        }

        _readerThread.Join();

        return latencyHistory;
    }

    private void InitializeDDSEntities()
    {
        _dpf = ParticipantService.Instance.GetDomainParticipantFactory();

        var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        var configName = "openddsharp_rtps_interop_" + guid;
        var instName = "internal_openddsharp_rtps_transport_" + guid;

        var config = TransportRegistry.Instance.CreateConfig(configName);
        var inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
        var transport = new RtpsUdpInst(inst)
        {
            UseMulticast = false,
            LocalAddress = "127.0.0.1:0",
        };
        config.Insert(transport);

        _participant = _dpf.CreateParticipant(DOMAIN_ID);
        TransportRegistry.Instance.BindConfig(configName, _participant);

        var typeSupport = new KeyedOctetsTypeSupport();
        var typeName = typeSupport.GetTypeName();
        typeSupport.RegisterType(_participant, typeName);

        _topic = _participant.CreateTopic("LatencyTest", typeName);

        var pubQos = new PublisherQos
        {
            EntityFactory = { AutoenableCreatedEntities = false }
        };
        _publisher = _participant.CreatePublisher(pubQos);

        var dwQos = new DataWriterQos
        {
            Reliability = { Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos },
            History =
            {
                Kind = HistoryQosPolicyKind.KeepLastHistoryQos,
                Depth = 1,
            },
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
            Reliability = { Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos },
            History =
            {
                Kind = HistoryQosPolicyKind.KeepLastHistoryQos,
                Depth = 1,
            },
        };
        var dr =  _subscriber.CreateDataReader(_topic, drQos);
        _dataReader = new KeyedOctetsDataReader(dr);

        _dataWriter.Enable();
        _dataReader.Enable();

        _waitSet = new WaitSet();
        _statusCondition = _dataReader.StatusCondition;
        _statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
        _waitSet.AttachCondition(_statusCondition);

        _readerThread = new Thread(ReaderThreadProc)
        {
            IsBackground = true,
            Priority = ThreadPriority.Highest,
        };

        Thread.Sleep(2_000);
    }

    private void ReaderThreadProc()
    {
        while (true)
        {
            var conditions = new List<Condition>();
            _waitSet.Wait(conditions);

            var sample = new KeyedOctets();
            var sampleInfo = new SampleInfo();
            _dataReader.TakeNextSample(sample, sampleInfo);
            _count += 1;

            _evt.Set();

            if (_count == _totalSamples * _totalInstances)
            {
                return;
            }
        }
    }

    public void Dispose()
    {
        _evt.Dispose();

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