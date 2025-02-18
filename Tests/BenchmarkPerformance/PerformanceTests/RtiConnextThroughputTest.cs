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

internal sealed class RtiConnextThroughputTest : IDisposable
{
    private const int DOMAIN_ID = 42;

    private readonly Random _random = new ();
    private readonly int _totalSamples;
    private readonly KeyedOctetsTopicType _sample;

    private DomainParticipant _participant;
    private Topic<KeyedOctetsTopicType> _topic;
    private Publisher _publisher;
    private DataWriter<KeyedOctetsTopicType> _dataWriter;
    private Subscriber _subscriber;
    private DataReader<KeyedOctetsTopicType> _dataReader;
    private WaitSet _waitSet;
    private Thread _readerThread;
    private ulong _samplesReceived;

    public RtiConnextThroughputTest(int totalSamples, ulong totalPayload)
    {
        _totalSamples = totalSamples;

        var payload = new byte[totalPayload];
        _random.NextBytes(payload);

        InitializeDDSEntities();

        _sample = new KeyedOctetsTopicType
        {
            Key = "1",
        };
        _sample.Value.AddRange(payload);
    }

    public ulong Run()
    {
        _samplesReceived = 0;
        _readerThread = new Thread(ReaderThread)
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

        var topicQos = _participant.DefaultTopicQos
            .WithReliability(c => c.Kind = ReliabilityKind.Reliable)
            .WithReliability(c => c.MaxBlockingTime = Duration.Infinite)
            .WithHistory(c => c.Kind = HistoryKind.KeepAll);
        _topic = _participant.CreateTopic<KeyedOctetsTopicType>(Guid.NewGuid().ToString(), topicQos);

        _publisher = _participant.CreatePublisher();
        _subscriber = _participant.CreateSubscriber();

        var dwQos = _publisher.DefaultDataWriterQos
            .WithReliability(c => c.Kind = ReliabilityKind.Reliable)
            .WithReliability(c => c.MaxBlockingTime = Duration.Infinite)
            .WithHistory(c => c.Kind = HistoryKind.KeepAll);
        var drQos = _subscriber.DefaultDataReaderQos
            .WithReliability(c => c.Kind = ReliabilityKind.Reliable)
            .WithReliability(c => c.MaxBlockingTime = Duration.Infinite)
            .WithHistory(c => c.Kind = HistoryKind.KeepAll);

        _dataWriter = _publisher.CreateDataWriter(_topic, dwQos);
        _dataReader = _subscriber.CreateDataReader(_topic, drQos);

        _waitSet = new WaitSet();

        _dataReader.StatusCondition.EnabledStatuses = StatusMask.DataAvailable;
        _waitSet.AttachCondition(_dataReader.StatusCondition);

        _dataReader.WaitForPublications(1, 5_000);
        _dataWriter.WaitForSubscriptions(1, 5_000);
    }

    private void ReaderThread()
    {
        int i = 0;
        while (true)
        {
            _ = _waitSet.Wait();

            using var samples = _dataReader.Take();
            _samplesReceived += (ulong)samples.Count;
            // Console.WriteLine(samples.Count);

            if (_samplesReceived >= (ulong)_totalSamples)
            {
                Console.WriteLine(i);
                return;
            }

            i++;
        }
    }
}