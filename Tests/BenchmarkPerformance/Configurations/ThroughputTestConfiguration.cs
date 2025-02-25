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
            AddJob(Job.Dry
                .WithStrategy(RunStrategy.Throughput)
                .WithToolchain(InProcessEmitToolchain.Instance));
        }
        if (name != null && name.Equals("short", StringComparison.InvariantCultureIgnoreCase))
        {
            AddJob(Job.ShortRun
                .WithStrategy(RunStrategy.Throughput)
                .WithUnrollFactor(1)
                .WithToolchain(InProcessEmitToolchain.Instance));
        }
        else
        {
            AddJob(Job.Default
                .WithIterationCount(5)
                .WithUnrollFactor(1)
                .WithInvocationCount(5)
                .WithWarmupCount(5)
                .WithStrategy(RunStrategy.Throughput)
                .WithToolchain(new InProcessEmitToolchain(TimeSpan.FromMinutes(30), true)));
        }

        AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
        AddColumn(new ThroughputSamplesReceivedColumn());
        AddColumn(new ThroughputMissingSamplesPercentageColumn());
        AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
        AddExporter(PlainExporter.Default);
        AddExporter(CsvExporter.Default);
        AddExporter(MarkdownExporter.Default);
        AddDiagnoser(MemoryDiagnoser.Default);
    }
}
