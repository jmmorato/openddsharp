using System.Globalization;
using OpenDDSharp.DDS;
using JsonWrapper;
using OpenDDSharp.BenchmarkPerformance.Helpers;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

internal sealed class JSONLatencyTest : IDisposable
{
    private readonly ManualResetEventSlim _evt;
    private readonly Random _random = new ();
    private readonly int _totalInstances;
    private readonly int _totalSamples;
    private readonly Dictionary<int, InstanceHandle> _instanceHandles = new();
    private readonly KeyedOctets _sample;
    private readonly DomainParticipant _participant;

    private int _count;

    private Topic _topic;
    private Publisher _publisher;
    private KeyedOctetsDataWriter _dataWriter;
    private Subscriber _subscriber;
    private KeyedOctetsDataReader _dataReader;
    private StatusCondition _statusCondition;
    private WaitSet _waitSet;

    public JSONLatencyTest(int totalInstances, int totalSamples, ulong totalPayload, DomainParticipant participant)
    {
        _totalInstances = totalInstances;
        _totalSamples = totalSamples;
        _evt = new ManualResetEventSlim(false);

        var payload = new byte[totalPayload];
        _random.NextBytes(payload);

        _participant = participant;

        InitializeDDSEntities();

        _sample = new KeyedOctets
        {
            ValueField = payload,
        };
    }

    public IList<TimeSpan> Run()
    {
        _count = 0;
        var latencyHistory = new List<TimeSpan>();

        var readerThread = new Thread(ReaderThreadProc)
        {
            IsBackground = true,
            Priority = ThreadPriority.Highest,
        };

        var pubThread = new Thread(_ =>
        {
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
        });

        readerThread.Start();
        pubThread.Start();

        readerThread.Join();

        return latencyHistory;
    }

    private void InitializeDDSEntities()
    {
        var typeSupport = new KeyedOctetsTypeSupport();
        var typeName = typeSupport.GetTypeName();
        typeSupport.RegisterType(_participant, typeName);

        var topicQos = new TopicQos
        {
            Reliability = { Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos },
            History = { Kind = HistoryQosPolicyKind.KeepAllHistoryQos}
        };
        _topic = _participant.CreateTopic(Guid.NewGuid().ToString(), typeName);

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
            },
            History =
            {
                Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
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
            Reliability =
            {
                Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos,
            },
            History =
            {
                Kind = HistoryQosPolicyKind.KeepAllHistoryQos,
                Depth = 1,
            },
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
        while (true)
        {
            var conditions = new List<Condition>();
            _waitSet.Wait(conditions);

            var samples = new List<KeyedOctets>();
            var sampleInfos = new List<SampleInfo>();

            _dataReader.Take(samples, sampleInfos);

            if (samples.Count > 1)
            {
                throw new InvalidDataException("Only one sample should be received.");
            }

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
    }
}