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
        break;
    }
    case "-2": // Throughput Short Performance Test
    {
        Ace.Init();

        var config = new ThroughputTestConfiguration("short")
        {
            ArtifactsPath = artifactsPath,
        };

        ThroughputTest.TotalSamplesValues = [10_000];
        ThroughputTest.TotalPayloadValues = [2_048, 4_096, 8_192];

        _ = BenchmarkRunner.Run<ThroughputTest>(config);

        TransportRegistry.Instance.Release();
        ParticipantService.Instance.Shutdown();

        Ace.Fini();
        break;
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
        LatencyTest.TotalInstancesValues = [100];
        LatencyTest.TotalSamplesValues = [10, 20, 30, 40, 50];
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

        ThroughputTest.TotalSamplesValues = [5_000, 10_000, 15_000, 20_000];
        ThroughputTest.TotalPayloadValues = [16_384];

        _ = BenchmarkRunner.Run<ThroughputTest>(config);

        TransportRegistry.Instance.Release();
        ParticipantService.Instance.Shutdown();

        Ace.Fini();
        break;
    }
}