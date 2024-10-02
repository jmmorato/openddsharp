using System.Diagnostics;
using BenchmarkDotNet.Running;
using OpenDDSharp.BenchmarkPerformance.Configurations;
using OpenDDSharp.BenchmarkPerformance.PerformanceTests;

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
        var test = new OpenDDSharpLatencyTest(1000, 100, 2048);
        Stopwatch stopwatch = new();
        stopwatch.Start();
        test.Run();
        stopwatch.Stop();
        test.Dispose();

        Console.WriteLine($"OpenDDSharp Latency Test {stopwatch.Elapsed.TotalSeconds}");

        var test1 = new RtiConnextLatencyTest(1000, 100, 2048);
        stopwatch.Reset();
        stopwatch.Start();
        test1.Run();
        stopwatch.Stop();
        test1.Dispose();

        Console.WriteLine($"RTI Connext Latency Test {stopwatch.Elapsed.TotalSeconds}");
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