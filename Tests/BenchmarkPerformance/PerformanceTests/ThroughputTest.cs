using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using BenchmarkDotNet.Attributes;
using CdrWrapper;
using OpenDDSharp.BenchmarkPerformance.CustomColumns;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required by BenchmarkDotNet.")]
public class ThroughputTest
{
    private const int DOMAIN_ID_CDR = 42;
    private const int DOMAIN_ID_JSON = 43;
    private const string RTPS_DISCOVERY = "RtpsDiscovery";

    private CDRThroughputTest _cdrThroughputTest;
    private JSONThroughputTest _jsonThroughputTest;
    private RtiConnextThroughputTest _rtiConnextThroughputTest;
    private ulong _samplesReceived;
    private DomainParticipantFactory _dpf;
    private DomainParticipant _participantCdr;
    private DomainParticipant _participantJson;

    /// <summary>
    /// Gets or sets the current number of instance for the test.
    /// </summary>
    [Params(5_000, 10_000, 15_000, 20_000)]
    public int TotalSamples { get; set; }

    /// <summary>
    /// Gets or sets the payload size for the test.
    /// </summary>
    [Params(1024, 2048, 4096, 8192)]
    public ulong TotalPayload { get; set; }

    private TransportConfig _configCdr;
    private TransportInst _instCdr;
    private TransportConfig _configJson;
    private TransportInst _instJson;

    [GlobalSetup(Target = nameof(OpenDDSharpCDRThroughputTest))]
    public void OpenDDSharpGlobalSetupCDR()
    {
        // Ace.Init();

        var disc = new RtpsDiscovery(RTPS_DISCOVERY)
        {
            // SedpMulticast = false,
            // SedpLocalAddress = "127.0.0.1:",
            // SpdpLocalAddress = "127.0.0.1:",
            // ResendPeriod = new TimeValue
            // {
            //     Seconds = 1,
            //     MicroSeconds = 0,
            // },
        };

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

    [GlobalSetup(Target = nameof(OpenDDSharpJSONThroughputTest))]
    public void OpenDDSharpGlobalSetupJSON()
    {
        Ace.Init();

        var disc = new RtpsDiscovery(RTPS_DISCOVERY)
        {
            // SedpMulticast = false,
            // SedpLocalAddress = "127.0.0.1:",
            // SpdpLocalAddress = "127.0.0.1:",
            // ResendPeriod = new TimeValue
            // {
            //     Seconds = 1,
            //     MicroSeconds = 0,
            // },
        };

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

    [GlobalCleanup(Target = nameof(OpenDDSharpCDRThroughputTest))]
    public void OpenDDSharpGlobalCleanupCDR()
    {
        _dpf.DeleteParticipant(_participantCdr);

        TransportRegistry.Instance.RemoveConfig(_configCdr);
        TransportRegistry.Instance.RemoveInst(_instCdr);

        // TransportRegistry.Instance.Release();
        // ParticipantService.Instance.Shutdown();

        // Ace.Fini();
    }

    [GlobalCleanup(Target = nameof(OpenDDSharpJSONThroughputTest))]
    public void OpenDDSharpGlobalCleanupJSON()
    {
        _dpf.DeleteParticipant(_participantJson);

        TransportRegistry.Instance.RemoveConfig(_configJson);
        TransportRegistry.Instance.RemoveInst(_instJson);
        //
        // TransportRegistry.Instance.Release();
        // ParticipantService.Instance.Shutdown();
        //
        // Ace.Fini();
    }

    [IterationSetup(Target = nameof(OpenDDSharpCDRThroughputTest))]
    public void OpenDDSharpCDRIterationSetup()
    {
        _cdrThroughputTest = new CDRThroughputTest(TotalSamples, TotalPayload, _participantCdr);
    }

    [IterationSetup(Target = nameof(OpenDDSharpJSONThroughputTest))]
    public void OpenDDSharpJSONIterationSetup()
    {
        _jsonThroughputTest = new JSONThroughputTest(TotalSamples, TotalPayload, _participantJson);
    }

    [IterationSetup(Target = nameof(RtiConnextThroughputTest))]
    public void RtiConnextIterationSetup()
    {
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
