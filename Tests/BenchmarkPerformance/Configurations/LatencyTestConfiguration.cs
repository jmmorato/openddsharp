using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using OpenDDSharp.BenchmarkPerformance.CustomColumns;

namespace OpenDDSharp.BenchmarkPerformance.Configurations;

internal class LatencyTestConfiguration : ManualConfig
{
    public LatencyTestConfiguration()
    {
        AddJob(Job.Default.WithIterationCount(1).WithToolchain(InProcessEmitToolchain.Instance));
        AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
        AddColumn(new LatencyAverageColumn());
        AddColumn(new LatencyDeviationColumn());
        AddColumn(new LatencyMinimumColumn());
        AddColumn(new LatencyMaximumColumn());
        AddColumn(new LatencyFiftyColumn());
        AddColumn(new LatencyNinetyColumn());
        AddColumn(new LatencyNinetyNineColumn());
        AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
        AddExporter(PlainExporter.Default);
        AddExporter(CsvExporter.Default);
        AddExporter(MarkdownExporter.Default);
    }
}
