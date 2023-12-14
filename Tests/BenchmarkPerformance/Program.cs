using BenchmarkDotNet.Running;
using CdrWrapper;
using OpenDDSharp;
using OpenDDSharp.BenchmarkPerformance.Configurations;
using OpenDDSharp.BenchmarkPerformance.PerformanceTests;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.OpenDDS.RTPS;

const int DOMAIN_ID = 42;
const string RTPS_DISCOVERY = "RtpsDiscovery";

var artifactsPath = Path.Combine(Environment.CurrentDirectory, "PerformanceTestArtifacts");

Ace.Init();

var disc = new RtpsDiscovery(RTPS_DISCOVERY)
{
    SedpMulticast = false,
    SedpLocalAddress = "127.0.0.1",
    SpdpLocalAddress = "127.0.0.1",
    ResendPeriod = new TimeValue
    {
        Seconds = 0,
        MicroSeconds = 50_000,
    },
};

ParticipantService.Instance.AddDiscovery(disc);
ParticipantService.Instance.DefaultDiscovery = RTPS_DISCOVERY;
ParticipantService.Instance.SetRepoDomain(DOMAIN_ID, RTPS_DISCOVERY);

//var test = new OpenDDSharpLatencyTest(1000, 100, 512);
//test.Run();
//test.Dispose();

Console.WriteLine("Menu: ");
Console.WriteLine("[1] Latency Performance Test");
Console.WriteLine("[2] Throughput Performance Test");
Console.WriteLine("Anything else will stop the program.");
Console.Write("> ");
var input = Console.ReadLine();
Console.WriteLine();
switch (input)
{
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

Ace.Fini();

Console.WriteLine("Press any button to exit the application.");