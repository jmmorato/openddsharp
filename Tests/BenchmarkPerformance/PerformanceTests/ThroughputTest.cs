using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using OpenDDSharp.BenchmarkPerformance.CustomColumns;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by BenchmarkDotNet.")]
public class ThroughputTest
{
    private const int DOMAIN_ID = 42;
    private const string RTPS_DISCOVERY = "RtpsDiscovery";

    private CDRThroughputTest _cdrThroughputTest;
    private JSONThroughputTest _jsonThroughputTest;
    private RtiConnextThroughputTest _rtiConnextThroughputTest;
    private ulong _samplesReceived;

    /// <summary>
    /// Gets or sets the current number of instance for the test.
    /// </summary>
    [Params(1_000, 2_000, 5_000, 10_000, 15_000)] //,
    public int TotalSamples { get; set; }

    /// <summary>
    /// Gets or sets the payload size for the test.
    /// </summary>
    [Params(512, 1024, 2048)]
    public ulong TotalPayload { get; set; }

    [GlobalSetup(Targets = [nameof(OpenDDSharpCDRThroughputTest), nameof(OpenDDSharpJSONThroughputTest)])]
    public void OpenDDSharpGlobalSetup()
    {
        Console.WriteLine("OpenDDSharpGlobalSetup");
        Ace.Init();

        var disc = new RtpsDiscovery(RTPS_DISCOVERY)
        {
            SedpMulticast = false,
            SedpLocalAddress = "127.0.0.1:",
            SpdpLocalAddress = "127.0.0.1:",
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

    [GlobalCleanup(Targets = [nameof(OpenDDSharpCDRThroughputTest), nameof(OpenDDSharpJSONThroughputTest)])]
    public void OpenDDSharpGlobalCleanup()
    {
        Console.WriteLine("OpenDDSharpGlobalCleanup");
        ParticipantService.Instance.Shutdown();

        Ace.Fini();
    }

    [IterationSetup(Target = nameof(OpenDDSharpCDRThroughputTest))]
    public void OpenDDSharpCDRIterationSetup()
    {
        Console.WriteLine("OpenDDSharpCDRIterationSetup");
        _cdrThroughputTest = new CDRThroughputTest(TotalSamples, TotalPayload);
    }

    [IterationSetup(Target = nameof(OpenDDSharpJSONThroughputTest))]
    public void OpenDDSharpJSONIterationSetup()
    {
        Console.WriteLine("OpenDDSharpJSONIterationSetup");
        _jsonThroughputTest = new JSONThroughputTest(TotalSamples, TotalPayload);
    }

    [IterationSetup(Target = nameof(RtiConnextThroughputTest))]
    public void RtiConnextIterationSetup()
    {
        Console.WriteLine("RtiConnextIterationSetup");
        _rtiConnextThroughputTest = new RtiConnextThroughputTest(TotalSamples, TotalPayload);
    }

    [IterationCleanup(Target = nameof(OpenDDSharpCDRThroughputTest))]
    public void OpenDDSharpCDRIterationCleanup()
    {
        _cdrThroughputTest.Dispose();

        ThroughputStatistics("openddsharpcdr");
    }

    [IterationCleanup(Target = nameof(OpenDDSharpJSONThroughputTest))]
    public void OpenDDSharpJSONIterationCleanup()
    {
        Console.WriteLine("OpenDDSharpJSONIterationCleanup");
        _jsonThroughputTest.Dispose();

        ThroughputStatistics("openddsharpjson");
    }

    [IterationCleanup(Target = nameof(RtiConnextThroughputTest))]
    public void RtiConnextIterationCleanup()
    {
        _rtiConnextThroughputTest.Dispose();

        ThroughputStatistics("rticonnext");
    }

    [Benchmark]
    public void OpenDDSharpCDRThroughputTest()
    {
        _samplesReceived = _cdrThroughputTest.Run();
    }

    [Benchmark]
    public void OpenDDSharpJSONThroughputTest()
    {
        _samplesReceived = _jsonThroughputTest.Run();
    }

    // Cannot run without a valid RTI Connext license.
    //[Benchmark]
    public void RtiConnextThroughputTest()
    {
        _samplesReceived= _rtiConnextThroughputTest.Run();
    }

    private void ThroughputStatistics(string name)
    {
        double totalSamples = TotalSamples;
        var missingPackets = totalSamples - _samplesReceived;

        Directory.CreateDirectory(ThroughputMissingSamplesPercentageColumn.OutputFolder);
        Directory.CreateDirectory(ThroughputSamplesReceivedColumn.OutputFolder);

        var samplesReceivedFiles = $"{name}-throughput-samples-received.{TotalSamples}.{TotalPayload}.txt";
        File.WriteAllText(Path.Combine(ThroughputSamplesReceivedColumn.OutputFolder, samplesReceivedFiles), _samplesReceived.ToString(CultureInfo.InvariantCulture));

        var missingPercentage = (missingPackets / totalSamples) * 100;
        var missingPercentageFile = $"{name}-throughput-missing-percentage.{TotalSamples}.{TotalPayload}.txt";
        File.WriteAllText(Path.Combine(ThroughputMissingSamplesPercentageColumn.OutputFolder, missingPercentageFile), missingPercentage.ToString("0.##", CultureInfo.InvariantCulture));
    }
}
