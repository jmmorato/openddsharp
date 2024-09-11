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
    private readonly byte[] _payload;

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

        _payload = new byte[totalPayload];
        _random.NextBytes(_payload);

        InitializeDDSEntities();
    }

    public IList<TimeSpan> Run()
    {
        _count = 0;

        var latencyHistory = new List<TimeSpan>();

        _readerThread.Start();
        var sample = new KeyedOctets
        {
            ValueField = _payload,
        };

        for (var i = 1; i <= _totalSamples; i++)
        {
            for (var j = 1; j <= _totalInstances; j++)
            {
                sample.KeyField = j.ToString(CultureInfo.InvariantCulture);

                var publicationTime = DateTime.UtcNow.Ticks;
                _dataWriter.Write(sample);

                _evt.Wait();

                var receptionTime = DateTime.UtcNow.Ticks;
                var latency = TimeSpan.FromTicks(receptionTime - publicationTime);
                latencyHistory.Add(latency);

                _evt.Reset();
            };
        }

        _readerThread.Join();

        return latencyHistory;
    }

    private void InitializeDDSEntities()
    {
        _dpf = ParticipantService.Instance.GetDomainParticipantFactory("-DCPSConfigFile", "rtps.ini");

        // var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        // var configName = "openddsharp_rtps_interop_" + guid;
        // var instName = "internal_openddsharp_rtps_transport_" + guid;

        // var config = TransportRegistry.Instance.CreateConfig(configName);
        // var inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
        // var rui = new ShmemInst(inst)
        // {
        //     // UseMulticast = false,
        //     // LocalAddress = "127.0.0.1",
        //     // NakResponseDelay = new TimeValue
        //     // {
        //     //     Seconds = 0,
        //     //     MicroSeconds = 50_000,
        //     // },
        //     // HeartbeatPeriod = new TimeValue
        //     // {
        //     //     Seconds = 0,
        //     //     MicroSeconds = 50_000,
        //     // },
        // };
        // config.Insert(rui);

        _participant = _dpf.CreateParticipant(DOMAIN_ID);
        // TransportRegistry.Instance.BindConfig(configName, _participant);

        var typeSupport = new KeyedOctetsTypeSupport();
        var typeName = typeSupport.GetTypeName();
        typeSupport.RegisterType(_participant, typeName);

        _topic = _participant.CreateTopic("LatencyTest", typeName);

        _publisher = _participant.CreatePublisher();

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
        // TransportRegistry.Instance.BindConfig(configName, dw);
        _dataWriter = new KeyedOctetsDataWriter(dw);

        _subscriber = _participant.CreateSubscriber();
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
        // TransportRegistry.Instance.BindConfig(configName, dr);
        _dataReader = new KeyedOctetsDataReader(dr);

        _waitSet = new WaitSet();
        _statusCondition = _dataReader.StatusCondition;
        _statusCondition.EnabledStatuses = StatusKind.DataAvailableStatus;
        _waitSet.AttachCondition(_statusCondition);

        _readerThread = new Thread(ReaderThreadProc)
        {
            IsBackground = true,
        };
    }

    private void ReaderThreadProc()
    {
        var duration = new Duration
        {
            Seconds = 1,
            NanoSeconds = 0,
        };
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

        _dataReader.DeleteContainedEntities();
        _subscriber.DeleteDataReader(_dataReader);
        _subscriber.DeleteContainedEntities();
        _participant.DeleteSubscriber(_subscriber);

        _participant.DeleteTopic(_topic);

        _participant.DeleteContainedEntities();
        _dpf.DeleteParticipant(_participant);
    }
}