using System.Globalization;
using Omg.Dds.Core;
using OpenDDSharp.BenchmarkPerformance.Helpers;
using Rti.Dds.Core;
using Rti.Dds.Core.Policy;
using Rti.Dds.Core.Status;
using Rti.Dds.Domain;
using Rti.Dds.Publication;
using Rti.Dds.Subscription;
using Rti.Dds.Topics;
using Rti.Types.Builtin;
using Guid = System.Guid;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

internal sealed class RtiConnextLatencyTest : IDisposable
{
    private const int DOMAIN_ID = 44;

    private readonly ManualResetEventSlim _evt;
    private readonly Random _random = new ();
    private readonly int _totalInstances;
    private readonly int _totalSamples;
    private readonly KeyedOctetsTopicType _sample;

    private int _count;

    private DomainParticipant _participant;
    private Topic<KeyedOctetsTopicType> _topic;
    private Publisher _publisher;
    private DataWriter<KeyedOctetsTopicType> _dataWriter;
    private Subscriber _subscriber;
    private DataReader<KeyedOctetsTopicType> _dataReader;
    private WaitSet _waitSet;

    public RtiConnextLatencyTest(int totalInstances, int totalSamples, ulong totalPayload)
    {
        _totalInstances = totalInstances;
        _totalSamples = totalSamples;

        _evt = new ManualResetEventSlim(false);

        var payload = new byte[totalPayload];
        _random.NextBytes(payload);

        InitializeDDSEntities();

        _sample = new KeyedOctetsTopicType();
        _sample.Value.AddRange(payload);
    }

    public IList<TimeSpan> Run()
    {
        _count = 0;
        var latencyHistory = new List<TimeSpan>();

        var readerThread = new Thread(ReaderThread)
        {
            IsBackground = true,
            Priority = ThreadPriority.AboveNormal,
        };

        var pubThread = new Thread(_ =>
        {
            for (var i = 1; i <= _totalSamples; i++)
            {
                for (var j = 1; j <= _totalInstances; j++)
                {
                    _sample.Key = j.ToString(CultureInfo.InvariantCulture);

                    var publicationTime = DateTime.UtcNow.Ticks;
                    _dataWriter.Write(_sample);

                    _evt.Wait();

                    var receptionTime = DateTime.UtcNow.Ticks;
                    var latency = TimeSpan.FromTicks(receptionTime - publicationTime);
                    latencyHistory.Add(latency);

                    _evt.Reset();
                }
            }
        });

        pubThread.Start();
        readerThread.Start();

        readerThread.Join();

        return latencyHistory;
    }

    public void Dispose()
    {
        _dataWriter.Dispose();
        _publisher.DisposeContainedEntities();
        _publisher.Dispose();

        _dataReader.DisposeContainedEntities();
        _dataReader.Dispose();
        _subscriber.DisposeContainedEntities();
        _subscriber.Dispose();

        _topic.Dispose();

        _participant.DisposeContainedEntities();
        _participant.Dispose();

        DomainParticipantFactory.Instance.Dispose();

        _evt.Dispose();
    }

    private void InitializeDDSEntities()
    {
        var pQos = DomainParticipantFactory.Instance.DefaultParticipantQos
            .WithDiscovery(d => d.EnabledTransports.Clear())
            .WithDiscovery(d => d.EnabledTransports.Add(TransportBuiltinAlias.Udpv4))
            .WithDiscovery(d => d.AcceptUnknownPeers = false)
            .WithDiscovery(d => d.InitialPeers.Clear())
            .WithDiscovery(d => d.InitialPeers.Add("builtin.udpv4://127.0.0.1"))
            .WithTransportBuiltin(t => t.Mask = TransportBuiltinMask.Udpv4)
            .WithProperty(d => d.Add(new KeyValuePair<string, Property.Entry>("dds.builtin_type.keyed_octets.max_key_size", "8192")))
            .WithProperty(p => p.Add(new KeyValuePair<string, Property.Entry>("dds.builtin_type.keyed_octets.alloc_key_size", "8192")))
            .WithProperty(d => d.Add(new KeyValuePair<string, Property.Entry>("dds.builtin_type.keyed_octets.max_size", "8192")))
            .WithProperty(p => p.Add(new KeyValuePair<string, Property.Entry>("dds.builtin_type.keyed_octets.alloc_size", "8192")));

        _participant = DomainParticipantFactory.Instance.CreateParticipant(DOMAIN_ID, pQos);

        var topicQos = _participant.DefaultTopicQos
            .WithReliability(c => c.Kind = ReliabilityKind.Reliable)
            .WithReliability(c => c.MaxBlockingTime = Duration.Infinite)
            .WithHistory(c => c.Kind = HistoryKind.KeepAll);
        _topic = _participant.CreateTopic<KeyedOctetsTopicType>(Guid.NewGuid().ToString(), topicQos);

        var pubQos = _participant.DefaultPublisherQos
            .WithEntityFactory(EntityFactory.Default.WithAutoEnableCreatedEntities(false));
        _publisher = _participant.CreatePublisher(pubQos);

        var subQos = _participant.DefaultSubscriberQos
            .WithEntityFactory(EntityFactory.Default.WithAutoEnableCreatedEntities(false));
        _subscriber = _participant.CreateSubscriber(subQos);

        var dwQos = _publisher.DefaultDataWriterQos
            .WithReliability(c => c.Kind = ReliabilityKind.Reliable)
            .WithReliability(c => c.MaxBlockingTime = Duration.Infinite)
            .WithHistory(c => c.Kind = HistoryKind.KeepAll)
            .WithProperty(p => p.Add(new KeyValuePair<string, Property.Entry>("dds.builtin_type.keyed_octets.alloc_size", "8192")));;
        var drQos = _subscriber.DefaultDataReaderQos
            .WithReliability(c => c.Kind = ReliabilityKind.Reliable)
            .WithReliability(c => c.MaxBlockingTime = Duration.Infinite)
            .WithHistory(c => c.Kind = HistoryKind.KeepAll)
            .WithProperty(p => p.Add(new KeyValuePair<string, Property.Entry>("dds.builtin_type.keyed_octets.alloc_size", "8192")));

        _dataWriter = _publisher.CreateDataWriter(_topic, dwQos);
        _dataReader = _subscriber.CreateDataReader(_topic, drQos);

        _waitSet = new WaitSet();

        _dataReader.StatusCondition.EnabledStatuses = StatusMask.DataAvailable;
        _waitSet.AttachCondition(_dataReader.StatusCondition);

        _dataWriter.Enable();
        _dataReader.Enable();

        _dataReader.WaitForPublications(1, 5_000);
        _dataWriter.WaitForSubscriptions(1, 5_000);
    }

    private void ReaderThread()
    {
        var total = _totalSamples * _totalInstances;
        while (true)
        {
            _ = _waitSet.Wait();

            using var samples = _dataReader.Take();
            if (samples.Count > 1)
            {
                throw new InvalidDataException("Only one sample should be received.");
            }

            _count += 1;

            _evt.Set();

            if (_count >= total)
            {
                return;
            }
        }
    }
}