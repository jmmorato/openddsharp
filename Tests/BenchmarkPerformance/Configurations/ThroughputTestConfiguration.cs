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

        // Diagnosers
        AddDiagnoser(MemoryDiagnoser.Default);

        // Columns
        AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
        HideColumns("Gen0", "Gen1", "Alloc Ratio");
        AddColumn(new ThroughputPerSecondColumn());

        // Loggers
        AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());

        // Exporters
        AddExporter(PlainExporter.Default);
        AddExporter(CsvExporter.Default);
        AddExporter(MarkdownExporter.Default);
        AddExporter(JsonExporter.FullCompressed);
        AddExporter(CsvMeasurementsExporter.Default);
        AddExporter(RPlotExporter.Default);

        // Increase the build timeout to 30 minutes
        WithBuildTimeout(TimeSpan.FromMinutes(30));
    }
}
