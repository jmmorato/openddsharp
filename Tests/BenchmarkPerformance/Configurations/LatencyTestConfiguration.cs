﻿using System.Runtime.InteropServices;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Jobs;
using OpenDDSharp.BenchmarkPerformance.CustomColumns;
using OpenDDSharp.BenchmarkPerformance.Helpers;

namespace OpenDDSharp.BenchmarkPerformance.Configurations;

internal class LatencyTestConfiguration : ManualConfig
{
    public LatencyTestConfiguration(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        }

        if (name.Equals("dry", StringComparison.InvariantCultureIgnoreCase))
        {
            AddJob(Job.Dry.WithStrategy(RunStrategy.Throughput).WithRuntime(CoreRuntime.Core80));
        }
        else if (name.Equals("short", StringComparison.InvariantCultureIgnoreCase))
        {
            AddJob(Job.ShortRun
                .WithUnrollFactor(1)
                .WithStrategy(RunStrategy.Throughput)
                .WithRuntime(CoreRuntime.Core80)
                .WithArguments([new MsBuildArgument(@"/p:Platform=""" + BenchmarkHelpers.GetPlatformString() + @"""")]));


            // Does not run JSON tests in this configuration.
            AddFilter(new NameFilter(n => !n.Contains("JSON", StringComparison.CurrentCultureIgnoreCase)));
        }
        else
        {
            AddJob(Job.Default
                .WithIterationCount(10)
                .WithUnrollFactor(1)
                .WithInvocationCount(10)
                .WithWarmupCount(5)
                .WithRuntime(CoreRuntime.Core80)
                .WithStrategy(RunStrategy.Throughput));
        }

        // Cannot be run without a valid RTI Connext license.
        AddFilter(new NameFilter(n => !n.Contains("RTI", StringComparison.CurrentCultureIgnoreCase)));

        AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
        HideColumns(StatisticColumn.Mean);
        HideColumns(StatisticColumn.Error);
        HideColumns(StatisticColumn.StdDev);
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
        WithBuildTimeout(TimeSpan.FromMinutes(30));
    }
}
