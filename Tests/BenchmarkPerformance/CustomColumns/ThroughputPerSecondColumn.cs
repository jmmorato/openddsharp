using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace OpenDDSharp.BenchmarkPerformance.CustomColumns;

internal class ThroughputPerSecondColumn : IColumn
{
    public static readonly string OutputFolder = Path.Combine(Path.GetTempPath(), nameof(ThroughputPerSecondColumn));

    /// <inheritdoc/>
    public string Id => nameof(ThroughputPerSecondColumn);

    /// <inheritdoc/>
    public string ColumnName => "Throughput (MB/sec)";

    /// <inheritdoc/>
    public bool AlwaysShow => true;

    /// <inheritdoc/>
    public ColumnCategory Category => ColumnCategory.Custom;

    /// <inheritdoc/>
    public int PriorityInCategory => 0;

    /// <inheritdoc/>
    public bool IsNumeric => true;

    /// <inheritdoc/>
    public UnitType UnitType => UnitType.Dimensionless;

    /// <inheritdoc/>
    public string Legend => "Calculated total throughput (MB/sec)";

    /// <inheritdoc/>
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        var numInstancesParam = benchmarkCase.Parameters.Items.FirstOrDefault(x => x.Name == "TotalSamples");
        var payloadSizeParam = benchmarkCase.Parameters.Items.FirstOrDefault(x => x.Name == "TotalPayload");
        if (numInstancesParam == null)
        {
            return "No TotalInstances parameter";
        }

        if (payloadSizeParam == null)
        {
            return "No TotalPayload parameter";
        }

        var numInstances = numInstancesParam.Value;
        var payloadSize = payloadSizeParam.Value;

        if (!double.TryParse(numInstances.ToString(), out var samples) ||
            !double.TryParse(payloadSize.ToString(), out var size))
        {
            return "N/A";
        }

        var report = summary.Reports.FirstOrDefault(r => r.BenchmarkCase == benchmarkCase);
        if (report.ResultStatistics == null)
        {
            return "N/A";
        }

        var totalSeconds = report.ResultStatistics.Mean / 1_000_000_000;
        var dataSize = samples * size / 1024 / 1024;
        var throughput = dataSize / totalSeconds;

        return throughput.ToString("F") + " MB/sec";
    }

    /// <inheritdoc/>
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) => GetValue(summary, benchmarkCase);

    /// <inheritdoc/>
    public bool IsAvailable(Summary summary) => true;

    /// <inheritdoc/>
    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
}
