using System.Diagnostics;
using BenchmarkDotNet.Running;
using OpenDDSharp;
using OpenDDSharp.BenchmarkPerformance.Configurations;
using OpenDDSharp.BenchmarkPerformance.PerformanceTests;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;

const string RTPS_DISCOVERY = "RtpsDiscovery";
const int DOMAIN_ID = 42;

var artifactsPath = Path.Combine(Environment.CurrentDirectory, "PerformanceTestArtifacts");

string input;
if (args.Length == 0)
{
    Console.WriteLine("Menu: ");
    Console.WriteLine("[1] Latency Performance Test");
    Console.WriteLine("[2] Throughput Performance Test");
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
    case "-1":
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

        var test = new OpenDDSharpLatencyTest(1000, 100, 2048);
        Stopwatch stopwatch = new();
        stopwatch.Start();
        test.Run();
        stopwatch.Stop();
        test.Dispose();

        Console.WriteLine($"OpenDDSharp Latency Test {stopwatch.Elapsed.TotalSeconds}");

        ParticipantService.Instance.Shutdown();

        Ace.Fini();

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
    case "1":
        {
            var config = new LatencyTestConfiguration
            {
                ArtifactsPath = artifactsPath,
            };
            _ = BenchmarkRunner.Run<LatencyTest>(config);
            break;
        }
}

Console.WriteLine("Press any button to exit the application.");