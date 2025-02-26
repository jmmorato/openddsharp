using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using OpenDDSharp.BenchmarkPerformance.CustomColumns;

namespace OpenDDSharp.BenchmarkPerformance.Configurations;

internal class LatencyTestConfiguration : ManualConfig
{
    public LatencyTestConfiguration(string name)
    {
        if (name != null && name.Equals("dry", StringComparison.InvariantCultureIgnoreCase))
        {
            AddJob(Job.Dry.WithToolchain(InProcessEmitToolchain.Instance));
            AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());

            AddColumn(new LatencyAverageColumn());
            AddColumn(new LatencyDeviationColumn());
            AddColumn(new LatencyMinimumColumn());
            AddColumn(new LatencyMaximumColumn());
        }
        else if (name != null && name.Equals("short", StringComparison.InvariantCultureIgnoreCase))
        {
            AddJob(Job.ShortRun
                .WithUnrollFactor(1)
                .WithStrategy(RunStrategy.Throughput)
                .WithToolchain(InProcessEmitToolchain.Instance)
                .WithToolchain(new InProcessEmitToolchain(TimeSpan.FromMinutes(30), true)));

            AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
            AddColumn(new LatencyAverageColumn());
            AddColumn(new LatencyDeviationColumn());
            AddColumn(new LatencyMinimumColumn());
            AddColumn(new LatencyMaximumColumn());
        }
        else
        {
            AddJob(Job.Default
                .WithIterationCount(10)
                .WithUnrollFactor(1)
                .WithInvocationCount(10)
                .WithWarmupCount(5)
                .WithStrategy(RunStrategy.Throughput)
                .WithToolchain(InProcessEmitToolchain.Instance)
                .WithToolchain(new InProcessEmitToolchain(TimeSpan.FromMinutes(30), true)));

            AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
            AddColumn(new LatencyAverageColumn());
            AddColumn(new LatencyDeviationColumn());
            AddColumn(new LatencyMinimumColumn());
            AddColumn(new LatencyMaximumColumn());
            AddColumn(new LatencyFiftyColumn());
            AddColumn(new LatencyNinetyColumn());
            AddColumn(new LatencyNinetyNineColumn());
        }

        AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
        AddExporter(PlainExporter.Default);
        AddExporter(CsvExporter.Default);
        AddExporter(MarkdownExporter.Default);
    }
}
