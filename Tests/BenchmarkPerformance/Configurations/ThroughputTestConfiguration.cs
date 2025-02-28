using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using OpenDDSharp.BenchmarkPerformance.CustomColumns;

namespace OpenDDSharp.BenchmarkPerformance.Configurations;

internal class ThroughputTestConfiguration : ManualConfig
{
    public ThroughputTestConfiguration(string name)
    {
        if (name != null && name.Equals("dry", StringComparison.InvariantCultureIgnoreCase))
        {
            AddJob(Job.Dry.WithStrategy(RunStrategy.Throughput));
        }
        else if (name != null && name.Equals("short", StringComparison.InvariantCultureIgnoreCase))
        {
            AddJob(Job.ShortRun.WithUnrollFactor(1).WithStrategy(RunStrategy.Throughput));
        }
        else
        {
            AddJob(Job.Default
                .WithIterationCount(10)
                .WithUnrollFactor(1)
                .WithInvocationCount(10)
                .WithWarmupCount(5)
                .WithStrategy(RunStrategy.Throughput));
        }

        AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
        AddColumn(new ThroughputPerSecondColumn());
        AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
        AddExporter(PlainExporter.Default);
        AddExporter(CsvExporter.Default);
        AddExporter(MarkdownExporter.Default);
        AddDiagnoser(MemoryDiagnoser.Default);
    }
}
