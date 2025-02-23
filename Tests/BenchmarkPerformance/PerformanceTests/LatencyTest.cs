using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using BenchmarkDotNet.Attributes;
using OpenDDSharp.BenchmarkPerformance.CustomColumns;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by BenchmarkDotNet.")]
public class LatencyTest
{
    private const int DOMAIN_ID_CDR = 42;
    private const int DOMAIN_ID_JSON = 43;
    private const string RTPS_DISCOVERY = "RtpsDiscovery";

    private CDRLatencyTest _cdrLatencyTest;
    private JSONLatencyTest _jsonLatencyTest;
    private RtiConnextLatencyTest _rtiConnextLatencyTest;
    private IList<TimeSpan> _latencyHistory;
    private DomainParticipantFactory _dpf;
    private DomainParticipant _participantCdr;
    private DomainParticipant _participantJson;
    private TransportConfig _configCdr;
    private TransportInst _instCdr;
    private TransportConfig _configJson;
    private TransportInst _instJson;

    /// <summary>
    /// Gets or sets the current number of instance for the test.
    /// </summary>
    [Params(50, 100, 150, 200)]
    public int TotalInstances { get; set; }

    /// <summary>
    /// Gets or sets the current number of samples for the test.
    /// </summary>
    [Params(10, 20, 30)]
    public int TotalSamples { get; set; }

    /// <summary>
    /// Gets or sets the payload size for the test.
    /// </summary>
    [Params(1024, 2048, 4096, 8192)]
    public ulong TotalPayload { get; set; }

    [GlobalSetup(Target = nameof(OpenDDSharpCDRLatencyTest))]
    public void OpenDDSharpGlobalSetupCDR()
    {
        var disc = new RtpsDiscovery(RTPS_DISCOVERY);

        ParticipantService.Instance.AddDiscovery(disc);
        ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;
        ParticipantService.Instance.SetRepoDomain(DOMAIN_ID_CDR, RTPS_DISCOVERY);

        _dpf = ParticipantService.Instance.GetDomainParticipantFactory();

        var guidCdr = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        var configNameCdr = "openddsharp_tcp_" + guidCdr;
        var instNameCdr = "internal_openddsharp_tcp_" + guidCdr;

        _configCdr = TransportRegistry.Instance.CreateConfig(configNameCdr);
        _instCdr = TransportRegistry.Instance.CreateInst(instNameCdr, "tcp");
        var transportCdr = new TcpInst(_instCdr)
        {
            LocalAddress = IPAddress.Loopback.ToString(),
        };
        _configCdr.Insert(transportCdr);

        _participantCdr = _dpf.CreateParticipant(DOMAIN_ID_CDR);
        TransportRegistry.Instance.BindConfig(configNameCdr, _participantCdr);
    }

    [GlobalSetup(Target = nameof(OpenDDSharpJSONLatencyTest))]
    public void OpenDDSharpGlobalSetupJSON()
    {
        var disc = new RtpsDiscovery(RTPS_DISCOVERY);

        ParticipantService.Instance.AddDiscovery(disc);
        ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;
        ParticipantService.Instance.SetRepoDomain(DOMAIN_ID_JSON, RTPS_DISCOVERY);

        _dpf = ParticipantService.Instance.GetDomainParticipantFactory();

        // Create JSON participant
        var guidJson = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        var configNameJson = "openddsharp_tcp_" + guidJson;
        var instNameCdrJson = "internal_openddsharp_tcp_" + guidJson;

        _configJson = TransportRegistry.Instance.CreateConfig(configNameJson);
        _instJson = TransportRegistry.Instance.CreateInst(instNameCdrJson, "tcp");
        var transportJson = new TcpInst(_instJson)
        {
            LocalAddress = IPAddress.Loopback.ToString(),
        };
        _configJson.Insert(transportJson);

        _participantJson = _dpf.CreateParticipant(DOMAIN_ID_JSON);
        TransportRegistry.Instance.BindConfig(configNameJson, _participantJson);
    }

    [GlobalCleanup(Target = nameof(OpenDDSharpCDRLatencyTest))]
    public void OpenDDSharpGlobalCleanupCDR()
    {
        _dpf.DeleteParticipant(_participantCdr);

        TransportRegistry.Instance.RemoveConfig(_configCdr);
        TransportRegistry.Instance.RemoveInst(_instCdr);
    }

    [GlobalCleanup(Target = nameof(OpenDDSharpJSONLatencyTest))]
    public void OpenDDSharpGlobalCleanupJSON()
    {
        _dpf.DeleteParticipant(_participantJson);

        TransportRegistry.Instance.RemoveConfig(_configJson);
        TransportRegistry.Instance.RemoveInst(_instJson);
    }

    [IterationSetup(Target = nameof(OpenDDSharpCDRLatencyTest))]
    public void OpenDDSharpCDRIterationSetup()
    {
        _cdrLatencyTest = new CDRLatencyTest(TotalInstances, TotalSamples, TotalPayload, _participantCdr);
    }

    [IterationSetup(Target = nameof(OpenDDSharpJSONLatencyTest))]
    public void OpenDDSharpJSONIterationSetup()
    {
        _jsonLatencyTest = new JSONLatencyTest(TotalInstances, TotalSamples, TotalPayload, _participantJson);
    }

    [IterationSetup(Target = nameof(RtiConnextLatencyTest))]
    public void RtiConnextIterationSetup()
    {
        _rtiConnextLatencyTest = new RtiConnextLatencyTest(TotalInstances, TotalSamples, TotalPayload);
    }

    [IterationCleanup(Target = nameof(OpenDDSharpCDRLatencyTest))]
    public void OpenDDSharpCDRIterationCleanup()
    {
        _cdrLatencyTest.Dispose();

        LatencyStatistics("openddsharpcdr");

        _latencyHistory.Clear();
    }

    [IterationCleanup(Target = nameof(OpenDDSharpJSONLatencyTest))]
    public void OpenDDSharpJSONIterationCleanup()
    {
        _jsonLatencyTest.Dispose();

        LatencyStatistics("openddsharpjson");

        _latencyHistory.Clear();
    }

    [IterationCleanup(Target = nameof(RtiConnextLatencyTest))]
    public void RtiConnextIterationCleanup()
    {
        _rtiConnextLatencyTest?.Dispose();

        LatencyStatistics("rticonnext");

        _latencyHistory.Clear();
    }

    [Benchmark(Description = "OpenDDSharp CDR")]
    public void OpenDDSharpCDRLatencyTest()
    {
        _latencyHistory = _cdrLatencyTest.Run();
    }

    [Benchmark(Description = "OpenDDSharp JSON")]
    public void OpenDDSharpJSONLatencyTest()
    {
        _latencyHistory = _jsonLatencyTest.Run();
    }

    // Cannot run without a valid RTI Connext license.
    // [Benchmark(Description = "RTI Connext")]
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
