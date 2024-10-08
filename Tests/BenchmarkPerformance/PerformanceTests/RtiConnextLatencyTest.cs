using System.Globalization;
using Rti.Dds.Core;
using Rti.Dds.Core.Policy;
using Rti.Dds.Core.Status;
using Rti.Dds.Domain;
using Rti.Dds.Publication;
using Rti.Dds.Subscription;
using Rti.Dds.Topics;
using Rti.Types.Builtin;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

internal sealed class RtiConnextLatencyTest : IDisposable
{
    private const int DOMAIN_ID = 42;

    private readonly ManualResetEventSlim _evt;
    private readonly Random _random = new ();
    private readonly int _totalInstances;
    private readonly int _totalSamples;
    private readonly Dictionary<int, InstanceHandle> _instanceHandles = new();
    private readonly KeyedOctetsTopicType _sample;

    private int _count;

    private DomainParticipant _participant;
    private Topic<KeyedOctetsTopicType> _topic;
    private Publisher _publisher;
    private DataWriter<KeyedOctetsTopicType> _dataWriter;
    private Subscriber _subscriber;
    private DataReader<KeyedOctetsTopicType> _dataReader;
    private WaitSet _waitSet;
    private Thread _readerThread;

    public RtiConnextLatencyTest(int totalInstances, int totalSamples, ulong totalPayload)
    {
        _totalInstances = totalInstances;
        _totalSamples = totalSamples;

        _evt = new ManualResetEventSlim(false);

        var payload = new byte[totalPayload];
        _random.NextBytes(payload);

        InitializeDDSEntities();

        _count = 0;

        _readerThread.Start();

        _sample = new KeyedOctetsTopicType();
        _sample.Value.AddRange(payload);
    }

    public IList<TimeSpan> Run()
    {
        var latencyHistory = new List<TimeSpan>();

        for (var i = 1; i <= _totalSamples; i++)
        {
            for (var j = 1; j <= _totalInstances; j++)
            {
                _sample.Key = j.ToString(CultureInfo.InvariantCulture);

                if (!_instanceHandles.TryGetValue(j, out var instanceHandle))
                {
                    instanceHandle = _dataWriter.RegisterInstance(_sample);
                    _instanceHandles.Add(j, instanceHandle);
                }

                var publicationTime = DateTime.UtcNow.Ticks;
                _dataWriter.Write(_sample);

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

    public void Dispose()
    {
        _evt.Dispose();

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
    }

    private void InitializeDDSEntities()
    {
        var pQos = DomainParticipantFactory.Instance.DefaultParticipantQos
            .WithDiscovery(d => d.EnabledTransports.Clear())
            .WithDiscovery(d => d.EnabledTransports.Add(TransportBuiltinAlias.Udpv4))
            .WithDiscovery(d => d.AcceptUnknownPeers = false)
            .WithDiscovery(d => d.InitialPeers.Clear())
            .WithDiscovery(d => d.InitialPeers.Add("builtin.udpv4://127.0.0.1"))
            .WithTransportBuiltin(t => t.Mask = TransportBuiltinMask.Udpv4);

        _participant = DomainParticipantFactory.Instance.CreateParticipant(DOMAIN_ID, pQos);
        _topic = _participant.CreateTopic<KeyedOctetsTopicType>("LatencyTestRtiConnext");

        _publisher = _participant.CreatePublisher();
        _subscriber = _participant.CreateSubscriber();

        var dwQos = _publisher.DefaultDataWriterQos
            .WithReliability(c => c.Kind = ReliabilityKind.Reliable)
            .WithHistory(c => c.Kind = HistoryKind.KeepLast)
            .WithHistory(c => c.Depth = 1);
        var drQos = _subscriber.DefaultDataReaderQos
            .WithReliability(c => c.Kind = ReliabilityKind.Reliable)
            .WithHistory(c => c.Kind = HistoryKind.KeepLast)
            .WithHistory(c => c.Depth = 1);

        _dataWriter = _publisher.CreateDataWriter(_topic, dwQos);
        _dataReader = _subscriber.CreateDataReader(_topic, drQos);

        _waitSet = new WaitSet();

        _dataReader.StatusCondition.EnabledStatuses = StatusMask.DataAvailable;
        _waitSet.AttachCondition(_dataReader.StatusCondition);

        _readerThread = new Thread(ReaderThread)
        {
            IsBackground = true,
        };

        Thread.Sleep(2_000);
    }

    private void ReaderThread()
    {
        while (true)
        {
            var activeConditions = _waitSet.Wait();

            foreach (var activeCondition in activeConditions)
            {
                if (activeCondition == _dataReader.StatusCondition && activeCondition.TriggerValue)
                {
                    using var samples = _dataReader.Take();
                    _count += samples.Count;

                    _evt.Set();

                    if (_count == _totalSamples * _totalInstances)
                    {
                        return;
                    }
                }
            }
        }
    }
}