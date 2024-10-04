using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using OpenDDSharp.BenchmarkPerformance.CustomColumns;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by BenchmarkDotNet.")]
public class LatencyTest
{
    private const int DOMAIN_ID = 42;
    private const string RTPS_DISCOVERY = "RtpsDiscovery";

    private OpenDDSharpLatencyTest _openDDSharpLatencyTest;
    private RtiConnextLatencyTest _rtiConnextLatencyTest;
    private IList<TimeSpan> _latencyHistory;

    /// <summary>
    /// Gets or sets the current number of instance for the test.
    /// </summary>
    [Params(10, 50, 100, 500, 1_000)]
    public int TotalInstances { get; set; }

    /// <summary>
    /// Gets or sets the current number of samples for the test.
    /// </summary>
    [Params(10, 50, 100)]
    public int TotalSamples { get; set; }

    /// <summary>
    /// Gets or sets the payload size for the test.
    /// </summary>
    [Params(512, 1024, 2048)]
    public ulong TotalPayload { get; set; }

    [GlobalSetup(Target = nameof(OpenDDSharpLatencyTest))]
    public void OpenDDSharpGlobalSetup()
    {
        Ace.Init();

        var disc = new RtpsDiscovery(RTPS_DISCOVERY)
        {
            SedpMulticast = false,
            SedpLocalAddress = "127.0.0.1:0",
            SpdpLocalAddress = "127.0.0.1:0",
            ResendPeriod = new TimeValue
            {
                Seconds = 1,
                MicroSeconds = 0,
            },
        };

        ParticipantService.Instance.AddDiscovery(disc);
        ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;
        ParticipantService.Instance.SetRepoDomain(DOMAIN_ID, RTPS_DISCOVERY);
    }

    [GlobalCleanup(Target = nameof(OpenDDSharpLatencyTest))]
    public void OpenDDSharpGlobalCleanup()
    {
        ParticipantService.Instance.Shutdown();

        Ace.Fini();
    }

    [IterationSetup(Target = nameof(OpenDDSharpLatencyTest))]
    public void OpenDDSharpIterationSetup()
    {
        _openDDSharpLatencyTest = new OpenDDSharpLatencyTest(TotalInstances, TotalSamples, TotalPayload);
    }

    [IterationSetup(Target = nameof(RtiConnextLatencyTest))]
    public void RtiConnextIterationSetup()
    {
        _rtiConnextLatencyTest = new RtiConnextLatencyTest(TotalInstances, TotalSamples, TotalPayload);
    }

    [IterationCleanup(Target = nameof(OpenDDSharpLatencyTest))]
    public void OpenDDSharpIterationCleanup()
    {
        _openDDSharpLatencyTest?.Dispose();

        LatencyStatistics("openddsharp");

        _latencyHistory.Clear();
    }

    [IterationCleanup(Target = nameof(RtiConnextLatencyTest))]
    public void RtiConnextIterationCleanup()
    {
        _rtiConnextLatencyTest?.Dispose();

        LatencyStatistics("rticonnext");

        _latencyHistory.Clear();
    }

    [Benchmark]
    public void OpenDDSharpLatencyTest()
    {
        _latencyHistory = _openDDSharpLatencyTest.Run();
    }

    // [Benchmark]
    public void RtiConnextLatencyTest()
    {
        _latencyHistory= _rtiConnextLatencyTest.Run();
    }

    private void LatencyStatistics(string name)
    {
        var sentSamples = TotalSamples * TotalInstances;
        if (_latencyHistory.Count != sentSamples)
        {
            throw new InvalidOperationException($"Lost samples detected {_latencyHistory.Count}/{sentSamples}.");
        }

        var count = _latencyHistory.Count;
        var latencySum = _latencyHistory.Sum(t => t.TotalMilliseconds);
        var latencySumSquare = _latencyHistory.Sum(t => Math.Pow(t.TotalMilliseconds, 2));

        var latencyAve = latencySum / count;
        var latencyStd = Math.Sqrt((latencySumSquare / _latencyHistory.Count) - (latencyAve * latencyAve));

        var sorted = _latencyHistory.OrderBy(t => t.TotalMilliseconds).ToList();
        var latencyMin = sorted.First();
        var latencyMax = sorted.Last();

        Directory.CreateDirectory(LatencyAverageColumn.OutputFolder);
        Directory.CreateDirectory(LatencyDeviationColumn.OutputFolder);
        Directory.CreateDirectory(LatencyMinimumColumn.OutputFolder);
        Directory.CreateDirectory(LatencyMaximumColumn.OutputFolder);
        Directory.CreateDirectory(LatencyFiftyColumn.OutputFolder);
        Directory.CreateDirectory(LatencyNinetyColumn.OutputFolder);
        Directory.CreateDirectory(LatencyNinetyNineColumn.OutputFolder);

        var averageFile = $"{name}-latency-average.{TotalInstances}.{TotalSamples}.{TotalPayload}.txt";
        File.WriteAllText(Path.Combine(LatencyAverageColumn.OutputFolder, averageFile),
            latencyAve.ToString("0.0000", CultureInfo.InvariantCulture));

        var deviationFile = $"{name}-latency-deviation.{TotalInstances}.{TotalSamples}.{TotalPayload}.txt";
        File.WriteAllText(Path.Combine(LatencyDeviationColumn.OutputFolder, deviationFile),
            latencyStd.ToString("0.0000", CultureInfo.InvariantCulture));

        var minimumFile = $"{name}-latency-minimum.{TotalInstances}.{TotalSamples}.{TotalPayload}.txt";
        File.WriteAllText(Path.Combine(LatencyMinimumColumn.OutputFolder, minimumFile),
            latencyMin.TotalMilliseconds.ToString("0.0000", CultureInfo.InvariantCulture));

        var maximumFile = $"{name}-latency-maximum.{TotalInstances}.{TotalSamples}.{TotalPayload}.txt";
        File.WriteAllText(Path.Combine(LatencyMaximumColumn.OutputFolder, maximumFile),
            latencyMax.TotalMilliseconds.ToString("0.0000", CultureInfo.InvariantCulture));

        var fiftyFile = $"{name}-latency-fifty.{TotalInstances}.{TotalSamples}.{TotalPayload}.txt";
        File.WriteAllText(Path.Combine(LatencyFiftyColumn.OutputFolder, fiftyFile),
            _latencyHistory[count * 50 / 100].TotalMilliseconds.ToString("0.0000", CultureInfo.InvariantCulture));

        var ninetyFile = $"{name}-latency-ninety.{TotalInstances}.{TotalSamples}.{TotalPayload}.txt";
        File.WriteAllText(Path.Combine(LatencyNinetyColumn.OutputFolder, ninetyFile),
            _latencyHistory[count * 90 / 100].TotalMilliseconds.ToString("0.0000", CultureInfo.InvariantCulture));

        var ninetyNineFile = $"{name}-latency-ninety-nine.{TotalInstances}.{TotalSamples}.{TotalPayload}.txt";
        File.WriteAllText(Path.Combine(LatencyNinetyNineColumn.OutputFolder, ninetyNineFile),
            _latencyHistory[count * 99 / 100].TotalMilliseconds.ToString("0.0000", CultureInfo.InvariantCulture));
    }
}
