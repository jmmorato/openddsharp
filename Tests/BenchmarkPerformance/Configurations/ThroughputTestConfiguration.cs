﻿using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using OpenDDSharp.BenchmarkPerformance.CustomColumns;

namespace OpenDDSharp.BenchmarkPerformance.Configurations;

internal class ThroughputTestConfiguration : ManualConfig
{
    public ThroughputTestConfiguration()
    {
        AddJob(Job.Dry.WithIterationCount(1).WithToolchain(InProcessEmitToolchain.Instance));
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
