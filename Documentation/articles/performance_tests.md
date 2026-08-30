# OpenDDSharp Performance Tests

## Introduction

The main goal of OpenDDSharp is to provide an open source high-performance C# DDS implementation.

The main challenge for the current implementation is to write and read the samples from managed memory to unmanaged
memory, and vice versa, as fast as possible.

The first versions of the library were using the default marshaling mechanism provided by the .NET runtime. However,
the difficulties with the implementation of complex structures and the performance obtained with the default marshaling
led the development to provide another custom kind of marshaling.

[OpenDDS v3.25.0](https://github.com/OpenDDS/OpenDDS/releases/tag/DDS-3.25) introduced a new `encode_to_string`, and
`decode_from_string` methods to the `TypeSupport` classes. These methods allow the serialization and deserialization of
the data to and from a string using JSON representation. OpenDDSharp uses these methods to provide a custom marshaling
mechanism based on JSON; therefore, only string pointers are exchanged between the managed and unmanaged memory.

Early after the first releases of the library with the new JSON marshaling mechanism, the community reported that the
performance of the JSON marshaling mechanism was not as good as expected, especially for large payloads:

https://github.com/jmmorato/openddsharp/discussions/230
https://github.com/jmmorato/openddsharp/discussions/270

The main reason for the performance issues is that the JSON marshaling mechanism is not optimized for performance
purposes. A noticeable bottleneck was found in the OpenDDS side using the `rapidjson` library. The bottleneck was kind
of expected because the implementation of the JSON serialization/deserialization was never meant to be used for
high-performance scenarios but for debugging purposes.

The new custom marshaling mechanism is based on the Common Data Representation (CDR) format, which is the default format
for OpenDDS. The CDR format is a binary format used by DDS to serialize and deserialize the data on the wire, 
therefore, the OpenDDS implementation if fully optimized for high-performance scenarios. On the other hand,
there is no C# library that provides a CDR serialization/deserialization mechanism, so OpenDDSharp provides a custom
CDR implementation based on the [XCDR1 specification](https://www.omg.org/cgi-bin/doc?formal/02-06-51.pdf).

The following sections contain the results of performance tests for OpenDDSharp. The test is designed to measure the
latency and throughput of the library when using CDR marshaling mechanisms.

The latency test measures the time it takes to write and read a sample from the DataWriter to the DataReader.

The throughput test measures the time it takes to write and read a large number of samples from the DataWriter to the
DataReader.

Both tests are executed with different configurations of instances, samples, and payload sizes to measure the
performance of the library in different scenarios.

The tests are executed using the [BenchmarkDotNet](https://benchmarkdotnet.org/) library, and the results are compared
with the native OpenDDS C++ implementation.

### OpenDDSharp v3.340.1

#### Latency Payload

```
BenchmarkDotNet v0.15.8, macOS Tahoe 26.5.2 (25F84) [Darwin 25.5.0]
Apple M3, 1 CPU, 8 logical and 8 physical cores
.NET SDK 10.0.201
```

| Method                       | TotalInstances | TotalSamples | TotalPayload |  Latency Avg. | Latency Std. Dev. | Latency Minimum | Latency Maximum |   Latency 50% |   Latency 90% |   Latency 99% | 
|------------------------------|----------------|--------------|--------------|--------------:|------------------:|----------------:|----------------:|--------------:|--------------:|--------------:|
| **&#39;OpenDDS Native&#39;** | **100**        | **20**       | **8192**     | **1.0063 ms** |     **0.0342 ms** |   **0.9434 ms** |   **1.2553 ms** | **0.9807 ms** | **1.0265 ms** | **1.0385 ms** | 
| &#39;OpenDDSharp CDR&#39;    | 100            | 20           | 8192         |     1.0472 ms |         0.0440 ms |       0.9570 ms |       1.3130 ms |     1.0440 ms |     1.0790 ms |     1.1120 ms | 
|                              |                |              |              |               |                   |                 |                 |               |               |               | 
| **&#39;OpenDDS Native&#39;** | **100**        | **20**       | **16384**    | **1.2228 ms** |     **0.0544 ms** |   **1.1207 ms** |   **1.4391 ms** | **1.1803 ms** | **1.2400 ms** | **1.2585 ms** | 
| &#39;OpenDDSharp CDR&#39;    | 100            | 20           | 16384        |     1.1749 ms |         0.0484 ms |       1.0910 ms |       1.4620 ms |     1.1610 ms |     1.2250 ms |     1.2470 ms | 
|                              |                |              |              |               |                   |                 |                 |               |               |               | 
| **&#39;OpenDDS Native&#39;** | **100**        | **20**       | **32768**    | **1.1157 ms** |     **0.0405 ms** |   **1.0177 ms** |   **1.3839 ms** | **1.0730 ms** | **1.2372 ms** | **1.1418 ms** | 
| &#39;OpenDDSharp CDR&#39;    | 100            | 20           | 32768        |     1.0902 ms |         0.0399 ms |       1.0070 ms |       1.3610 ms |     1.0720 ms |     1.1410 ms |     1.1440 ms | 
|                              |                |              |              |               |                   |                 |                 |               |               |               | 
| **&#39;OpenDDS Native&#39;** | **100**        | **20**       | **65536**    | **1.1575 ms** |     **0.0520 ms** |   **1.0444 ms** |   **1.3746 ms** | **1.1664 ms** | **1.2102 ms** | **1.2375 ms** | 
| &#39;OpenDDSharp CDR&#39;    | 100            | 20           | 65536        |     2.6948 ms |         1.9763 ms |       1.0720 ms |      41.4020 ms |     1.7630 ms |     4.7610 ms |     2.7870 ms | 

#### Latency Samples

```
BenchmarkDotNet v0.15.8, macOS Tahoe 26.5.2 (25F84) [Darwin 25.5.0]
Apple M3, 1 CPU, 8 logical and 8 physical cores
.NET SDK 10.0.201
```

| Method                       | TotalInstances | TotalSamples | TotalPayload |  Latency Avg. | Latency Std. Dev. | Latency Minimum | Latency Maximum |   Latency 50% |   Latency 90% |   Latency 99% | 
|------------------------------|----------------|--------------|--------------|--------------:|------------------:|----------------:|----------------:|--------------:|--------------:|--------------:|
| **&#39;OpenDDS Native&#39;** | **100**        | **20**       | **8192**     | **1.0063 ms** |     **0.0342 ms** |   **0.9434 ms** |   **1.2553 ms** | **0.9807 ms** | **1.0265 ms** | **1.0385 ms** | 
| &#39;OpenDDSharp CDR&#39;    | 100            | 20           | 8192         |     1.0472 ms |         0.0440 ms |       0.9570 ms |       1.3130 ms |     1.0440 ms |     1.0790 ms |     1.1120 ms | 
|                              |                |              |              |               |                   |                 |                 |               |               |               | 
| **&#39;OpenDDS Native&#39;** | **100**        | **20**       | **16384**    | **1.2228 ms** |     **0.0544 ms** |   **1.1207 ms** |   **1.4391 ms** | **1.1803 ms** | **1.2400 ms** | **1.2585 ms** | 
| &#39;OpenDDSharp CDR&#39;    | 100            | 20           | 16384        |     1.1749 ms |         0.0484 ms |       1.0910 ms |       1.4620 ms |     1.1610 ms |     1.2250 ms |     1.2470 ms | 
|                              |                |              |              |               |                   |                 |                 |               |               |               | 
| **&#39;OpenDDS Native&#39;** | **100**        | **20**       | **32768**    | **1.1157 ms** |     **0.0405 ms** |   **1.0177 ms** |   **1.3839 ms** | **1.0730 ms** | **1.2372 ms** | **1.1418 ms** | 
| &#39;OpenDDSharp CDR&#39;    | 100            | 20           | 32768        |     1.0902 ms |         0.0399 ms |       1.0070 ms |       1.3610 ms |     1.0720 ms |     1.1410 ms |     1.1440 ms | 
|                              |                |              |              |               |                   |                 |                 |               |               |               | 
| **&#39;OpenDDS Native&#39;** | **100**        | **20**       | **65536**    | **1.1575 ms** |     **0.0520 ms** |   **1.0444 ms** |   **1.3746 ms** | **1.1664 ms** | **1.2102 ms** | **1.2375 ms** | 
| &#39;OpenDDSharp CDR&#39;    | 100            | 20           | 65536        |     2.6948 ms |         1.9763 ms |       1.0720 ms |      41.4020 ms |     1.7630 ms |     4.7610 ms |     2.7870 ms | 

#### Throughput Payload

```
BenchmarkDotNet v0.15.8, macOS Tahoe 26.5.2 (25F84) [Darwin 25.5.0]
Apple M3, 1 CPU, 8 logical and 8 physical cores
.NET SDK 10.0.201
```


| Method               | TotalSamples | TotalPayload |         Mean |        Error |       StdDev |    Ratio |  RatioSD | Throughput (MB/sec) | 
|----------------------|--------------|--------------|-------------:|-------------:|-------------:|---------:|---------:|--------------------:|
| **'OpenDDS Native'** | **10000**    | **8192**     | **429.9 ms** | **29.45 ms** | **19.48 ms** | **1.00** | **0.06** |  **181.710 MB/sec** | 
| 'OpenDDSharp CDR'    | 10000        | 8192         |     450.5 ms |     13.42 ms |      7.98 ms |     1.05 |     0.05 |      173.419 MB/sec | 
|                      |              |              |              |              |              |          |          |                     |
| **'OpenDDS Native'** | **10000**    | **16384**    | **410.9 ms** |  **4.78 ms** |  **2.85 ms** | **1.00** | **0.01** |  **380.238 MB/sec** | 
| 'OpenDDSharp CDR'    | 10000        | 16384        |     466.2 ms |      3.74 ms |      2.47 ms |     1.13 |     0.01 |      335.184 MB/sec | 
|                      |              |              |              |              |              |          |          |                     |
| **'OpenDDS Native'** | **10000**    | **32768**    | **430.8 ms** |  **4.23 ms** |  **2.80 ms** | **1.00** | **0.01** |  **725.318 MB/sec** | 
| 'OpenDDSharp CDR'    | 10000        | 32768        |     513.7 ms |      3.13 ms |      2.07 ms |     1.19 |     0.01 |      608.372 MB/sec | 
|                      |              |              |              |              |              |          |          |                     |
| **'OpenDDS Native'** | **10000**    | **65536**    | **459.8 ms** |  **1.46 ms** |  **0.97 ms** | **1.00** | **0.00** | **1359.193 MB/sec** | 
| 'OpenDDSharp CDR'    | 10000        | 65536        |     594.2 ms |      7.32 ms |      4.36 ms |     1.29 |     0.01 |     1051.757 MB/sec | 

#### Throughput Samples

```
BenchmarkDotNet v0.15.8, macOS Tahoe 26.5.2 (25F84) [Darwin 25.5.0]
Apple M3, 1 CPU, 8 logical and 8 physical cores
.NET SDK 10.0.201
```

| Method               | TotalSamples | TotalPayload |         Mean |        Error |       StdDev |    Ratio |  RatioSD | Throughput (MB/sec) | 
|----------------------|--------------|--------------|-------------:|-------------:|-------------:|---------:|---------:|--------------------:|
| **'OpenDDS Native'** | **5000**     | **16384**    | **179.7 ms** |  **0.91 ms** |  **0.54 ms** | **1.00** | **0.00** |  **434.647 MB/sec** | 
| 'OpenDDSharp CDR'    | 5000         | 16384        |     194.1 ms |      0.63 ms |      0.42 ms |     1.08 |     0.00 |      402.599 MB/sec | 
|                      |              |              |              |              |              |          |          |                     |
| **'OpenDDS Native'** | **10000**    | **16384**    | **363.1 ms** |  **2.16 ms** |  **1.28 ms** | **1.00** | **0.00** |  **430.370 MB/sec** | 
| 'OpenDDSharp CDR'    | 10000        | 16384        |     584.0 ms |     88.17 ms |     58.32 ms |     1.61 |     0.15 |      267.543 MB/sec | 
|                      |              |              |              |              |              |          |          |                     |
| **'OpenDDS Native'** | **15000**    | **16384**    | **648.7 ms** |  **7.32 ms** |  **4.36 ms** | **1.00** | **0.01** |  **361.279 MB/sec** | 
| 'OpenDDSharp CDR'    | 15000        | 16384        |     776.4 ms |     14.34 ms |      9.48 ms |     1.20 |     0.02 |      301.863 MB/sec | 
|                      |              |              |              |              |              |          |          |                     |
| **'OpenDDS Native'** | **20000**    | **16384**    | **924.1 ms** | **28.63 ms** | **18.94 ms** | **1.00** | **0.03** |  **338.153 MB/sec** | 
| 'OpenDDSharp CDR'    | 20000        | 16384        |     970.5 ms |      6.69 ms |      4.43 ms |     1.05 |     0.02 |      321.997 MB/sec | 

