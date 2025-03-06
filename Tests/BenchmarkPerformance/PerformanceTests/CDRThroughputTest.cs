using System.Globalization;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using CdrWrapper;
using OpenDDSharp.BenchmarkPerformance.Helpers;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

internal sealed class CDRThroughputTest : IDisposable
{
    private readonly Random _random = new ();
    private readonly int _totalSamples;
    private readonly KeyedOctets _sample;
    private readonly DomainParticipant _participant;

    private ulong _samplesReceived;

    private Topic _topic;
    private Publisher _publisher;
    private KeyedOctetsDataWriter _dataWriter;
    private Subscriber _subscriber;
    private KeyedOctetsDataReader _dataReader;
    private StatusCondition _statusCondition;
    private WaitSet _waitSet;

    public CDRThroughputTest(int totalSamples, ulong totalPayload, DomainParticipant participant)
    {
        _totalSamples = totalSamples;

        var payload = new byte[totalPayload];
        _random.NextBytes(payload);

        _participant = participant;

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
        var readerThread = new Thread(ReaderThreadProc)
        {
            IsBackground = true,
            Priority = ThreadPriority.AboveNormal,
        };

        var pubThread = new Thread(_ =>
        {
            for (var i = 1; i <= _totalSamples; i++)
            {
                _dataWriter.Write(_sample);
            }
        });

        pubThread.Start();
        readerThread.Start();

        readerThread.Join();

        return _samplesReceived;
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
                    NanoSeconds = Duration.InfiniteNanoSeconds
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
                    NanoSeconds = Duration.InfiniteNanoSeconds
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
        while (true)
        {
            var conditions = new List<Condition>();
            _waitSet.Wait(conditions);

            var samples = new List<KeyedOctets>();
            var sampleInfos = new List<SampleInfo>();

            var result = _dataReader.Take(samples, sampleInfos);
            if (result != ReturnCode.Ok)
            {
                continue;
            }

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
    }
}