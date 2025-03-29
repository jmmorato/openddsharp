using BenchmarkDotNet.Running;
using OpenDDSharp;
using OpenDDSharp.BenchmarkPerformance.Configurations;
using OpenDDSharp.BenchmarkPerformance.PerformanceTests;
using OpenDDSharp.OpenDDS.DCPS;

var artifactsPath = Path.Combine(Environment.CurrentDirectory, "PerformanceTestArtifacts");

string input;
if (args.Length == 0)
{
    Console.WriteLine("Menu: ");
    Console.WriteLine("[1] Latency Payload Performance Test");
    Console.WriteLine("[2] Throughput Payload Performance Test");
    Console.WriteLine("[3] Latency Samples Performance Test");
    Console.WriteLine("[4] Throughput Samples Performance Test");
    Console.WriteLine("Anything else will stop the program.");
    Console.Write("> ");
    input = Console.ReadLine();
    Console.WriteLine();
}
else
{
    input = args[0];
}

switch (input)
{
    case "-1": // Latency Short Performance Test
    {
        Ace.Init();

        var config = new LatencyTestConfiguration("short")
        {
            ArtifactsPath = artifactsPath,
        };

        LatencyTest.TotalInstancesValues = [100];
        LatencyTest.TotalSamplesValues = [20];
        LatencyTest.TotalPayloadValues = [2_048, 4_096, 8_192];

        _ = BenchmarkRunner.Run<LatencyTest>(config);

        TransportRegistry.Instance.Release();
        ParticipantService.Instance.Shutdown();

        Ace.Fini();

        // Ace.Init();
        //
        // var disc = new RtpsDiscovery(RTPS_DISCOVERY)
        // {
        //     SedpMulticast = false,
        //     SedpLocalAddress = "127.0.0.1:0",
        //     SpdpLocalAddress = "127.0.0.1:0",
        //     ResendPeriod = new TimeValue
        //     {
        //         Seconds = 1,
        //         MicroSeconds = 0,
        //     },
        // };
        //
        // ParticipantService.Instance.AddDiscovery(disc);
        // ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;
        // ParticipantService.Instance.SetRepoDomain(DOMAIN_ID_CDR, RTPS_DISCOVERY);
        // ParticipantService.Instance.SetRepoDomain(DOMAIN_ID_JSON, RTPS_DISCOVERY);
        //
        // Console.WriteLine();
        // Console.WriteLine("Starting OpenDDSharp JSON Latency Test...");
        //
        // var testJson = new JSONLatencyTest(1000, 100, 2048);
        // var stopwatch = new Stopwatch();
        // stopwatch.Start();
        // testJson.Run();
        // stopwatch.Stop();
        // testJson.Dispose();
        //
        // Console.WriteLine($"OpenDDSharp JSON Latency Test {stopwatch.Elapsed.TotalSeconds}");
        //
        // Console.WriteLine();
        // Console.WriteLine("Starting OpenDDSharp CDR Latency Test...");
        //
        // var testCDR = new CDRLatencyTest(1000, 100, 2048);
        // stopwatch = new Stopwatch();
        // stopwatch.Start();
        // testCDR.Run();
        // stopwatch.Stop();
        // testCDR.Dispose();
        //
        // Console.WriteLine($"OpenDDSharp CDR Latency Test {stopwatch.Elapsed.TotalSeconds}");
        //
        // ParticipantService.Instance.Shutdown();
        //
        // Ace.Fini();

        // Console.WriteLine();
        // Console.WriteLine("Starting RTI Connext Latency Test...");
        //
        // // Requires RTI Connext DDS valid license.
        // var test1 = new RtiConnextLatencyTest(1000, 100, 2048);
        // stopwatch.Reset();
        // stopwatch.Start();
        // test1.Run();
        // stopwatch.Stop();
        // test1.Dispose();
        //
        // Console.WriteLine($"RTI Connext Latency Test {stopwatch.Elapsed.TotalSeconds}");
        break;
    }
    case "-2": // Throughput Short Performance Test
    {
        Ace.Init();

        var config = new ThroughputTestConfiguration("short")
        {
            ArtifactsPath = artifactsPath,
        };

        ThroughputTest.TotalSamplesValues = [20_000];
        ThroughputTest.TotalPayloadValues = [2_048, 4_096, 8_192];

        _ = BenchmarkRunner.Run<ThroughputTest>(config);

        TransportRegistry.Instance.Release();
        ParticipantService.Instance.Shutdown();

        Ace.Fini();
        break;
        // Ace.Init();
        //
        // var disc = new RtpsDiscovery(RTPS_DISCOVERY)
        // {
        //     SedpMulticast = false,
        //     SedpLocalAddress = "127.0.0.1:0",
        //     SpdpLocalAddress = "127.0.0.1:0",
        //     ResendPeriod = new TimeValue
        //     {
        //         Seconds = 1,
        //         MicroSeconds = 0,
        //     },
        // };
        //
        // ParticipantService.Instance.AddDiscovery(disc);
        // ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;
        // ParticipantService.Instance.SetRepoDomain(DOMAIN_ID_CDR, RTPS_DISCOVERY);
        // ParticipantService.Instance.SetRepoDomain(DOMAIN_ID_JSON, RTPS_DISCOVERY);
        //
        // var dpf = ParticipantService.Instance.GetDomainParticipantFactory();
        //
        // var guidCdr = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        // var configNameCdr = "openddsharp_tcp_" + guidCdr;
        // var instNameCdr = "internal_openddsharp_tcp_" + guidCdr;
        //
        // var configCdr = TransportRegistry.Instance.CreateConfig(configNameCdr);
        // var instCdr = TransportRegistry.Instance.CreateInst(instNameCdr, "tcp");
        // var transportCdr = new TcpInst(instCdr)
        // {
        //     LocalAddress = IPAddress.Loopback.ToString(),
        // };
        // configCdr.Insert(transportCdr);
        //
        // var participantCdr = dpf.CreateParticipant(DOMAIN_ID_CDR);
        // TransportRegistry.Instance.BindConfig(configNameCdr, participantCdr);
        //
        // Console.WriteLine();
        // Console.WriteLine("Starting OpenDDSharp CDR Throughput Test...");
        //
        // var testCDR = new CDRThroughputTest(15_000, 2048, participantCdr);
        // var stopwatchCdr = new Stopwatch();
        // stopwatchCdr.Start();
        // testCDR.Run();
        // stopwatchCdr.Stop();
        // testCDR.Dispose();
        //
        // Console.WriteLine($"OpenDDSharp CDR Throughput Test {stopwatchCdr.Elapsed.TotalSeconds}");
        //
        // Console.WriteLine();
        // Console.WriteLine("Starting OpenDDSharp JSON Throughput Test...");
        //
        // var guidJson = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        // var configNameJson = "openddsharp_tcp_" + guidJson;
        // var instNameJson = "internal_openddsharp_tcp_" + guidJson;
        //
        // var configJson = TransportRegistry.Instance.CreateConfig(configNameJson);
        // var instJson = TransportRegistry.Instance.CreateInst(instNameJson, "tcp");
        // var transportJson = new TcpInst(instJson)
        // {
        //     LocalAddress = IPAddress.Loopback.ToString(),
        // };
        // configJson.Insert(transportJson);
        //
        // var participantJson = dpf.CreateParticipant(DOMAIN_ID_JSON);
        // TransportRegistry.Instance.BindConfig(configNameJson, participantJson);
        //
        // var testJson = new JSONThroughputTest(15_000, 2048, participantJson);
        // var stopwatch = new Stopwatch();
        // stopwatch.Start();
        // testJson.Run();
        // stopwatch.Stop();
        // testJson.Dispose();
        //
        // Console.WriteLine($"OpenDDSharp JSON Throughput Test {stopwatch.Elapsed.TotalSeconds}");
        //
        // dpf.DeleteParticipant(participantCdr);
        // dpf.DeleteParticipant(participantJson);
        // TransportRegistry.Instance.Release();
        // ParticipantService.Instance.Shutdown();
        //
        // Ace.Fini();

        // Requires RTI Connext DDS valid license.
        // Console.WriteLine();
        // Console.WriteLine($"RTI Connext Throughput Test {stopwatch.Elapsed.TotalSeconds}");
        //
        // var testRti = new RtiConnextThroughputTest(1_000, 2048);
        // stopwatch.Reset();
        // stopwatch.Start();
        // testRti.Run();
        // stopwatch.Stop();
        // Console.WriteLine($"RTI Connext Throughput Test {stopwatch.Elapsed.TotalSeconds}");
        //
        // testRti.Dispose();
        // testRti = new RtiConnextThroughputTest(1_000, 2048);
        //
        // stopwatch.Reset();
        // stopwatch.Start();
        // testRti.Run();
        // stopwatch.Stop();
        // testRti.Dispose();
        //
        // Console.WriteLine($"RTI Connext Throughput Test {stopwatch.Elapsed.TotalSeconds}");
        // break;
    }
    case "1": // Latency Payload Performance Test
    {
        Ace.Init();

        var config = new LatencyTestConfiguration("default")
        {
            ArtifactsPath = artifactsPath,
        };
        LatencyTest.TotalInstancesValues = [100];
        LatencyTest.TotalSamplesValues = [20];
        LatencyTest.TotalPayloadValues = [8_192, 16_384, 32_768, 65_536];
        _ = BenchmarkRunner.Run<LatencyTest>(config);

        TransportRegistry.Instance.Release();
        ParticipantService.Instance.Shutdown();

        Ace.Fini();
        break;
    }
    case "2": // Throughput Payload Performance Test
    {
        Ace.Init();

        var config = new ThroughputTestConfiguration("default")
        {
            ArtifactsPath = artifactsPath,
        };

        ThroughputTest.TotalSamplesValues = [10_000];
        ThroughputTest.TotalPayloadValues = [8_192, 16_384, 32_768, 65_536];

        _ = BenchmarkRunner.Run<ThroughputTest>(config);

        TransportRegistry.Instance.Release();
        ParticipantService.Instance.Shutdown();

        Ace.Fini();
        break;
    }
    case "3": // Latency Samples Performance Test
    {
        Ace.Init();

        var config = new LatencyTestConfiguration("default")
        {
            ArtifactsPath = artifactsPath,
        };
        LatencyTest.TotalInstancesValues = [1000];
        LatencyTest.TotalSamplesValues = [10, 20, 30];
        LatencyTest.TotalPayloadValues = [16_384];
        _ = BenchmarkRunner.Run<LatencyTest>(config);

        TransportRegistry.Instance.Release();
        ParticipantService.Instance.Shutdown();

        Ace.Fini();
        break;
    }
    case "4": // Throughput Samples Performance Test
    {
        Ace.Init();

        var config = new ThroughputTestConfiguration("default")
        {
            ArtifactsPath = artifactsPath,
        };

        ThroughputTest.TotalSamplesValues = [50_000, 100_000, 150_000, 200_000];
        ThroughputTest.TotalPayloadValues = [16_384];

        _ = BenchmarkRunner.Run<ThroughputTest>(config);

        TransportRegistry.Instance.Release();
        ParticipantService.Instance.Shutdown();

        Ace.Fini();
        break;
    }
}