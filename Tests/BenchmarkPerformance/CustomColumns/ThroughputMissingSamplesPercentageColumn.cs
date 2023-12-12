using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace OpenDDSharp.BenchmarkPerformance.CustomColumns;

internal class ThroughputMissingSamplesPercentageColumn : IColumn
{
    public static readonly string OutputFolder = Path.Combine(Path.GetTempPath(), nameof(ThroughputMissingSamplesPercentageColumn));

    /// <inheritdoc/>
    public string Id => nameof(ThroughputMissingSamplesPercentageColumn);

    /// <inheritdoc/>
    public string ColumnName => "Lost Samples";

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
    public string Legend => "Percentage of lost samples";

    /// <inheritdoc/>
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        var name = benchmarkCase.Descriptor.WorkloadMethod.Name.
            Replace("TestLatency", string.Empty, StringComparison.InvariantCultureIgnoreCase).
            ToLowerInvariant();

        var numInstancesParam = benchmarkCase.Parameters.Items.FirstOrDefault(x => x.Name == "TotalInstances");
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
        var filename = Path.Combine(OutputFolder, $"{name}-throughput-missing-percentage.{numInstances}.{payloadSize}.txt");
        return File.Exists(filename) ? File.ReadAllText(filename) + "%" : "No file";
    }

    /// <inheritdoc/>
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) => GetValue(summary, benchmarkCase);

    /// <inheritdoc/>
    public bool IsAvailable(Summary summary) => true;

    /// <inheritdoc/>
    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
}
