using System.Globalization;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using JsonWrapper;

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

                var publicationTime = DateTime.Now.Ticks;
                _dataWriter.Write(sample);

                _evt.Wait();

                var receptionTime = DateTime.Now.Ticks;
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
        _dpf = ParticipantService.Instance.GetDomainParticipantFactory();

        _participant = _dpf.CreateParticipant(DOMAIN_ID);
        var guid = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        var configName = "openddsharp_rtps_interop_" + guid;
        var instName = "internal_openddsharp_rtps_transport_" + guid;

        var config = TransportRegistry.Instance.CreateConfig(configName);
        var inst = TransportRegistry.Instance.CreateInst(instName, "rtps_udp");
        var rui = new RtpsUdpInst(inst);
        config.Insert(rui);

        TransportRegistry.Instance.BindConfig(configName, _participant);

        var typeSupport = new KeyedOctetsTypeSupport();
        var typeName = typeSupport.GetTypeName();
        typeSupport.RegisterType(_participant, typeName);

        _topic = _participant.CreateTopic("LatencyTest", typeName);

        _publisher = _participant.CreatePublisher();

        var dwQos = new DataWriterQos
        {
            Reliability = { Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos },
        };
        var dw = _publisher.CreateDataWriter(_topic, dwQos);
        _dataWriter = new KeyedOctetsDataWriter(dw);

        _subscriber = _participant.CreateSubscriber();
        var drQos = new DataReaderQos
        {
            Reliability = { Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos },
        };
        var dr =  _subscriber.CreateDataReader(_topic, drQos);
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
        while (true)
        {
            var conditions = new List<Condition>();
            _waitSet.Wait(conditions);

            var samples = new List<KeyedOctets>();
            var sampleInfos = new List<SampleInfo>();
            _dataReader.Take(samples, sampleInfos);
            _count += 1;//samples.Count;

            _evt.Set();

            if (_count == _totalSamples * _totalInstances)
            {
                return;
            }

            // foreach (var cond in  conditions)
            // {
            //     if (cond == _statusCondition && cond.TriggerValue)
            //     {
            //         var samples = new List<KeyedOctets>();
            //         var sampleInfos = new List<SampleInfo>();
            //         _dataReader.Take(samples, sampleInfos);
            //         _count += 1;//samples.Count;
            //
            //         _evt.Set();
            //
            //         if (_count == _totalSamples * _totalInstances)
            //         {
            //             return;
            //         }
            //     }
            // }
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