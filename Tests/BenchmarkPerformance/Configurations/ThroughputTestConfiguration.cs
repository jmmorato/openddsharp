using System.Runtime.InteropServices;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using OpenDDSharp.BenchmarkPerformance.CustomColumns;
using OpenDDSharp.BenchmarkPerformance.Helpers;

namespace OpenDDSharp.BenchmarkPerformance.Configurations;

internal class ThroughputTestConfiguration : ManualConfig
{
    public ThroughputTestConfiguration(string name)
    {
        name ??= string.Empty;

        var job = Job.Default
            .WithIterationCount(10)
            .WithUnrollFactor(1)
            .WithInvocationCount(10)
            .WithWarmupCount(5)
            .WithStrategy(RunStrategy.Throughput);

        if (name.Equals("dry", StringComparison.InvariantCultureIgnoreCase))
        {
            job = Job.Dry.WithStrategy(RunStrategy.Throughput);
        }
        else if (name.Equals("short", StringComparison.InvariantCultureIgnoreCase))
        {
            job = Job.ShortRun
                .WithUnrollFactor(1)
                .WithStrategy(RunStrategy.Throughput);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Due to the error building the external process in the github-hosted runners,
            // we need to use the in-process emit toolchain
            job = job.WithToolchain(new InProcessEmitToolchain(TimeSpan.FromMinutes(30), true));
        }

        AddJob(job);

        // Cannot be run without a valid RTI Connext license.
        AddFilter(new NameFilter(n => !n.Contains("RTI", StringComparison.CurrentCultureIgnoreCase)));

        // Does not run JSON tests
        AddFilter(new NameFilter(n => !n.Contains("JSON", StringComparison.CurrentCultureIgnoreCase)));

        AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
        AddColumn(new ThroughputPerSecondColumn());
        AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
        AddExporter(PlainExporter.Default);
        AddExporter(CsvExporter.Default);
        AddExporter(MarkdownExporter.Default);
        AddDiagnoser(MemoryDiagnoser.Default);
        AddExporter(JsonExporter.FullCompressed);
        WithBuildTimeout(TimeSpan.FromMinutes(30));
    }
}
