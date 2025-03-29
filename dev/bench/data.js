window.BENCHMARK_DATA = {
  "lastUpdate": 1743254376505,
  "repoUrl": "https://github.com/jmmorato/openddsharp",
  "entries": {
    "throughput_ubuntu-22.04-arm_linux-arm64": [
      {
        "commit": {
          "author": {
            "name": "jmmorato",
            "username": "jmmorato"
          },
          "committer": {
            "name": "jmmorato",
            "username": "jmmorato"
          },
          "id": "dc76de1fb021f6d3c491a81483fd704c02cce7c9",
          "message": "[feature] Continuous Benchmarking workflow",
          "timestamp": "2025-03-27T17:21:01Z",
          "url": "https://github.com/jmmorato/openddsharp/pull/285/commits/dc76de1fb021f6d3c491a81483fd704c02cce7c9"
        },
        "date": 1743254376197,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "OpenDDSharp.BenchmarkPerformance.PerformanceTests.ThroughputTest.OpenDDSNativeThroughputTest(TotalSamples: 10000, TotalPayload: 8192)",
            "value": 314718631.16999996,
            "unit": "ns",
            "range": "± 8146108.007142596"
          },
          {
            "name": "OpenDDSharp.BenchmarkPerformance.PerformanceTests.ThroughputTest.OpenDDSharpCDRThroughputTest(TotalSamples: 10000, TotalPayload: 8192)",
            "value": 286690987.55,
            "unit": "ns",
            "range": "± 15447619.529949399"
          },
          {
            "name": "OpenDDSharp.BenchmarkPerformance.PerformanceTests.ThroughputTest.OpenDDSNativeThroughputTest(TotalSamples: 10000, TotalPayload: 16384)",
            "value": 347349431.4888889,
            "unit": "ns",
            "range": "± 3124125.221671865"
          },
          {
            "name": "OpenDDSharp.BenchmarkPerformance.PerformanceTests.ThroughputTest.OpenDDSharpCDRThroughputTest(TotalSamples: 10000, TotalPayload: 16384)",
            "value": 474314270.87,
            "unit": "ns",
            "range": "± 7271977.226482601"
          },
          {
            "name": "OpenDDSharp.BenchmarkPerformance.PerformanceTests.ThroughputTest.OpenDDSNativeThroughputTest(TotalSamples: 10000, TotalPayload: 32768)",
            "value": 412287357.64000005,
            "unit": "ns",
            "range": "± 6667064.141171266"
          },
          {
            "name": "OpenDDSharp.BenchmarkPerformance.PerformanceTests.ThroughputTest.OpenDDSharpCDRThroughputTest(TotalSamples: 10000, TotalPayload: 32768)",
            "value": 521790304.85,
            "unit": "ns",
            "range": "± 14634372.67502945"
          },
          {
            "name": "OpenDDSharp.BenchmarkPerformance.PerformanceTests.ThroughputTest.OpenDDSNativeThroughputTest(TotalSamples: 10000, TotalPayload: 65536)",
            "value": 667565508.46,
            "unit": "ns",
            "range": "± 20443688.46264314"
          },
          {
            "name": "OpenDDSharp.BenchmarkPerformance.PerformanceTests.ThroughputTest.OpenDDSharpCDRThroughputTest(TotalSamples: 10000, TotalPayload: 65536)",
            "value": 793674918.7222221,
            "unit": "ns",
            "range": "± 6027938.918974768"
          }
        ]
      }
    ]
  }
}