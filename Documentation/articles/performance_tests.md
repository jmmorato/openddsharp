# OpenDDSharp Performance Tests

## Introduction

The main goals of OpenDDSharp is to provide an open source high-performance C# DDS implementation.

The main challenge for the implementation is to write and read the samples from managed memory to unmanaged memory,
and vice versa, as fast as possible.

First versions of the library were using the default marshaling mechanism provided by the .NET runtime. However,
the difficulties for the implementation of complex structures, and the performance obtained with the default marshaling
led the development to provide another custom kind of marshaling.

[OpenDDS v3.25.0](https://github.com/OpenDDS/OpenDDS/releases/tag/DDS-3.25) introduced a new `encode_to_string`, and
`decode_from_string` methods to the `TypeSupport` classes. These methods allow the serialization and deserialization of
the data to and from a string using JSON representation. OpenDDSharp uses these methods to provide a custom marshaling
mechanism based on JSON, therefore only string pointers are exchanged between the managed and unmanaged memory.

Early after the first releases of the library with the new JSON marshaling mechanism, the community reported that the
performance of the JSON marshaling mechanism was not as good as expected, especially for large payloads:

https://github.com/jmmorato/openddsharp/discussions/230
https://github.com/jmmorato/openddsharp/discussions/270

The main reason for the performance issues is that the JSON marshaling mechanism is not optimized for performance
purposes. A noticeable bottleneck was found in the OpenDDS side using the `rapidjson` library. The bottleneck was kind
of expected, because the implementation of the JSON serialization/deserialization was never meant to be used for
high-performance scenarios but for debugging purposes.

The new custom marshaling mechanism is based on the Common Data Representation (CDR) format, which is the default format
for OpenDDS. The CDR format is a binary format that is used by DDS to serialize and deserialize the data on the wire, 
therefore, the OpenDDS implementation if fully optimized for high-performance scenarios. On the other hand,
there is no C# library that provides a CDR serialization/deserialization mechanism, so OpenDDSharp provides a custom
CDR implementation based on the [XCDR1 specification](https://www.omg.org/cgi-bin/doc?formal/02-06-51.pdf).

The following sections contain the results of performance tests for OpenDDSharp. The test are designed to measure the
latency and throughput of the library when using the JSON and CDR marshaling mechanisms.

The latency test measures the time it takes to write and read a sample from the DataWriter to the DataReader.

The throughput test measures the time it takes to write and read a large number of samples from the DataWriter to the
DataReader.

Both tests are executed with different configurations of instances, samples, and payload sizes to measure the
performance of the library in different scenarios.

The tests are executed using the [BenchmarkDotNet](https://benchmarkdotnet.org/) library and the results are compared
with the native OpenDDS C++ implementation.

### OpenDDSharp vNext

#### Latency Default

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) Darwin 24.2.0  
Apple M3, 1 CPU, 8 logical and 8 physical cores  
.NET SDK 8.0.406

Test Configuration:

- TotalInstances=100, 150, 200
- TotalSamples=20, 30
- TotalPayload=2048, 4096, 8192
- InvocationCount=10
- IterationCount=10
- RunStrategy=Throughput
- UnrollFactor=1
- WarmupCount=5

| Method               | TotalInstances | TotalSamples | TotalPayload |  Latency Avg. | Latency Std. Dev. | Latency Minimum | Latency Maximum |   Latency 50% |   Latency 90% |   Latency 99% |
|----------------------|----------------|--------------|--------------|--------------:|------------------:|----------------:|----------------:|--------------:|--------------:|--------------:|
| **'OpenDDS Native'** | **100**        | **20**       | **2048**     | **0.0769 ms** |     **0.0019 ms** |   **0.0681 ms** |   **0.0878 ms** | **0.0755 ms** | **0.0767 ms** | **0.0760 ms** |
| 'OpenDDSharp CDR'    | 100            | 20           | 2048         |     0.0808 ms |         0.0067 ms |       0.0750 ms |       0.2590 ms |     0.0820 ms |     0.0830 ms |     0.0800 ms |
| 'OpenDDSharp JSON'   | 100            | 20           | 2048         |     0.1956 ms |         0.0081 ms |       0.1820 ms |       0.3070 ms |     0.1940 ms |     0.2030 ms |     0.1960 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **100**        | **20**       | **4096**     | **0.0754 ms** |     **0.0017 ms** |   **0.0714 ms** |   **0.0876 ms** | **0.0762 ms** | **0.0752 ms** | **0.0772 ms** |
| 'OpenDDSharp CDR'    | 100            | 20           | 4096         |     0.0827 ms |         0.0087 ms |       0.0730 ms |       0.1980 ms |     0.0950 ms |     0.0870 ms |     0.0820 ms |
| 'OpenDDSharp JSON'   | 100            | 20           | 4096         |     0.2971 ms |         0.0105 ms |       0.2810 ms |       0.4060 ms |     0.2960 ms |     0.2970 ms |     0.3010 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **100**        | **20**       | **8192**     | **0.0764 ms** |     **0.0022 ms** |   **0.0732 ms** |   **0.0862 ms** | **0.0758 ms** | **0.0745 ms** | **0.0750 ms** |
| 'OpenDDSharp CDR'    | 100            | 20           | 8192         |     0.0822 ms |         0.0067 ms |       0.0780 ms |       0.1860 ms |     0.0810 ms |     0.0810 ms |     0.0830 ms |
| 'OpenDDSharp JSON'   | 100            | 20           | 8192         |     0.4838 ms |         0.0128 ms |       0.4730 ms |       0.6040 ms |     0.4770 ms |     0.4800 ms |     0.4800 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **100**        | **30**       | **2048**     | **0.0764 ms** |     **0.0011 ms** |   **0.0736 ms** |   **0.0854 ms** | **0.0766 ms** | **0.0778 ms** | **0.0752 ms** |
| 'OpenDDSharp CDR'    | 100            | 30           | 2048         |     0.0815 ms |         0.0137 ms |       0.0730 ms |       0.6550 ms |     0.0990 ms |     0.0780 ms |     0.0800 ms |
| 'OpenDDSharp JSON'   | 100            | 30           | 2048         |     0.1898 ms |         0.0075 ms |       0.1810 ms |       0.3290 ms |     0.1970 ms |     0.1910 ms |     0.1900 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **100**        | **30**       | **4096**     | **0.0766 ms** |     **0.0018 ms** |   **0.0722 ms** |   **0.0859 ms** | **0.0814 ms** | **0.0809 ms** | **0.0800 ms** |
| 'OpenDDSharp CDR'    | 100            | 30           | 4096         |     0.0803 ms |         0.0090 ms |       0.0740 ms |       0.2510 ms |     0.0780 ms |     0.0810 ms |     0.0790 ms |
| 'OpenDDSharp JSON'   | 100            | 30           | 4096         |     0.2832 ms |         0.0096 ms |       0.2760 ms |       0.3970 ms |     0.2850 ms |     0.2820 ms |     0.2820 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **100**        | **30**       | **8192**     | **0.0766 ms** |     **0.0023 ms** |   **0.0730 ms** |   **0.0864 ms** | **0.0742 ms** | **0.0748 ms** | **0.0745 ms** |
| 'OpenDDSharp CDR'    | 100            | 30           | 8192         |     0.0832 ms |         0.0095 ms |       0.0760 ms |       0.2010 ms |     0.0800 ms |     0.0820 ms |     0.0820 ms |
| 'OpenDDSharp JSON'   | 100            | 30           | 8192         |     0.4792 ms |         0.0134 ms |       0.4660 ms |       0.6230 ms |     0.4850 ms |     0.4820 ms |     0.4810 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **150**        | **20**       | **2048**     | **0.0771 ms** |     **0.0015 ms** |   **0.0736 ms** |   **0.0883 ms** | **0.0776 ms** | **0.0765 ms** | **0.0762 ms** |
| 'OpenDDSharp CDR'    | 150            | 20           | 2048         |     0.0804 ms |         0.0105 ms |       0.0760 ms |       0.4930 ms |     0.0770 ms |     0.0800 ms |     0.0790 ms |
| 'OpenDDSharp JSON'   | 150            | 20           | 2048         |     0.1897 ms |         0.0291 ms |       0.1810 ms |       1.2510 ms |     0.1930 ms |     0.1870 ms |     0.1890 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **150**        | **20**       | **4096**     | **0.0760 ms** |     **0.0018 ms** |   **0.0720 ms** |   **0.0874 ms** | **0.0768 ms** | **0.0810 ms** | **0.0759 ms** |
| 'OpenDDSharp CDR'    | 150            | 20           | 4096         |     0.0821 ms |         0.0077 ms |       0.0740 ms |       0.2190 ms |     0.0820 ms |     0.0810 ms |     0.0800 ms |
| 'OpenDDSharp JSON'   | 150            | 20           | 4096         |     0.2816 ms |         0.0096 ms |       0.2740 ms |       0.3920 ms |     0.2820 ms |     0.2830 ms |     0.2790 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **150**        | **20**       | **8192**     | **0.0766 ms** |     **0.0023 ms** |   **0.0732 ms** |   **0.0895 ms** | **0.0750 ms** | **0.0748 ms** | **0.0759 ms** |
| 'OpenDDSharp CDR'    | 150            | 20           | 8192         |     0.0825 ms |         0.0077 ms |       0.0770 ms |       0.2530 ms |     0.0800 ms |     0.0840 ms |     0.0800 ms |
| 'OpenDDSharp JSON'   | 150            | 20           | 8192         |     0.4872 ms |         0.0153 ms |       0.4760 ms |       0.7530 ms |     0.4850 ms |     0.4880 ms |     0.4790 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **150**        | **30**       | **2048**     | **0.0768 ms** |     **0.0023 ms** |   **0.0689 ms** |   **0.0954 ms** | **0.0772 ms** | **0.0752 ms** | **0.0757 ms** |
| 'OpenDDSharp CDR'    | 150            | 30           | 2048         |     0.0796 ms |         0.0072 ms |       0.0760 ms |       0.4570 ms |     0.0790 ms |     0.0780 ms |     0.0800 ms |
| 'OpenDDSharp JSON'   | 150            | 30           | 2048         |     0.1868 ms |         0.0071 ms |       0.1790 ms |       0.3100 ms |     0.1860 ms |     0.1850 ms |     0.1860 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **150**        | **30**       | **4096**     | **0.0758 ms** |     **0.0018 ms** |   **0.0674 ms** |   **0.0984 ms** | **0.0804 ms** | **0.0767 ms** | **0.0759 ms** |
| 'OpenDDSharp CDR'    | 150            | 30           | 4096         |     0.0808 ms |         0.0061 ms |       0.0760 ms |       0.2550 ms |     0.0820 ms |     0.0800 ms |     0.0800 ms |
| 'OpenDDSharp JSON'   | 150            | 30           | 4096         |     0.2869 ms |         0.0099 ms |       0.2790 ms |       0.4160 ms |     0.2840 ms |     0.2850 ms |     0.2860 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **150**        | **30**       | **8192**     | **0.0767 ms** |     **0.0025 ms** |   **0.0685 ms** |   **0.0869 ms** | **0.0815 ms** | **0.0808 ms** | **0.0776 ms** |
| 'OpenDDSharp CDR'    | 150            | 30           | 8192         |     0.0838 ms |         0.0116 ms |       0.0780 ms |       0.2970 ms |     0.0830 ms |     0.0820 ms |     0.0820 ms |
| 'OpenDDSharp JSON'   | 150            | 30           | 8192         |     0.4758 ms |         0.0135 ms |       0.4660 ms |       0.6330 ms |     0.4740 ms |     0.4780 ms |     0.4740 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **200**        | **20**       | **2048**     | **0.0758 ms** |     **0.0015 ms** |   **0.0682 ms** |   **0.0887 ms** | **0.0765 ms** | **0.0750 ms** | **0.0757 ms** |
| 'OpenDDSharp CDR'    | 200            | 20           | 2048         |     0.0801 ms |         0.0064 ms |       0.0740 ms |       0.2400 ms |     0.0790 ms |     0.0780 ms |     0.0810 ms |
| 'OpenDDSharp JSON'   | 200            | 20           | 2048         |     0.1878 ms |         0.0067 ms |       0.1790 ms |       0.2970 ms |     0.1880 ms |     0.1870 ms |     0.1870 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **200**        | **20**       | **4096**     | **0.0757 ms** |     **0.0017 ms** |   **0.0707 ms** |   **0.0875 ms** | **0.0763 ms** | **0.0759 ms** | **0.0729 ms** |
| 'OpenDDSharp CDR'    | 200            | 20           | 4096         |     0.0820 ms |         0.0120 ms |       0.0740 ms |       0.3770 ms |     0.0790 ms |     0.0820 ms |     0.0780 ms |
| 'OpenDDSharp JSON'   | 200            | 20           | 4096         |     0.2831 ms |         0.0103 ms |       0.2760 ms |       0.5430 ms |     0.2840 ms |     0.2830 ms |     0.2830 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **200**        | **20**       | **8192**     | **0.0778 ms** |     **0.0027 ms** |   **0.0735 ms** |   **0.0901 ms** | **0.0764 ms** | **0.0800 ms** | **0.0761 ms** |
| 'OpenDDSharp CDR'    | 200            | 20           | 8192         |     0.0823 ms |         0.0088 ms |       0.0750 ms |       0.3300 ms |     0.0810 ms |     0.0820 ms |     0.0840 ms |
| 'OpenDDSharp JSON'   | 200            | 20           | 8192         |     0.4784 ms |         0.0139 ms |       0.4680 ms |       0.7480 ms |     0.4740 ms |     0.4900 ms |     0.4760 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **200**        | **30**       | **2048**     | **0.0758 ms** |     **0.0014 ms** |   **0.0697 ms** |   **0.0947 ms** | **0.0757 ms** | **0.0759 ms** | **0.0754 ms** |
| 'OpenDDSharp CDR'    | 200            | 30           | 2048         |     0.0808 ms |         0.0062 ms |       0.0750 ms |       0.2400 ms |     0.0800 ms |     0.0850 ms |     0.0800 ms |
| 'OpenDDSharp JSON'   | 200            | 30           | 2048         |     0.1908 ms |         0.0091 ms |       0.1810 ms |       0.4980 ms |     0.2000 ms |     0.1910 ms |     0.1900 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **200**        | **30**       | **4096**     | **0.0759 ms** |     **0.0019 ms** |   **0.0686 ms** |   **0.0926 ms** | **0.0759 ms** | **0.0763 ms** | **0.0752 ms** |
| 'OpenDDSharp CDR'    | 200            | 30           | 4096         |     0.0796 ms |         0.0057 ms |       0.0750 ms |       0.2350 ms |     0.0800 ms |     0.0800 ms |     0.0830 ms |
| 'OpenDDSharp JSON'   | 200            | 30           | 4096         |     0.2836 ms |         0.0096 ms |       0.2760 ms |       0.4040 ms |     0.2820 ms |     0.2840 ms |     0.2810 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **200**        | **30**       | **8192**     | **0.0764 ms** |     **0.0026 ms** |   **0.0682 ms** |   **0.0933 ms** | **0.0785 ms** | **0.0747 ms** | **0.0740 ms** |
| 'OpenDDSharp CDR'    | 200            | 30           | 8192         |     0.0833 ms |         0.0104 ms |       0.0750 ms |       0.3430 ms |     0.0810 ms |     0.0790 ms |     0.0820 ms |
| 'OpenDDSharp JSON'   | 200            | 30           | 8192         |     0.4938 ms |         0.0159 ms |       0.4720 ms |       0.6700 ms |     0.4950 ms |     0.4950 ms |     0.4890 ms |

#### Latency Payload

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) Darwin 24.2.0  
Apple M3, 1 CPU, 8 logical and 8 physical cores  
.NET SDK 8.0.406

Test Configuration:
- TotalInstances=1000
- TotalSamples=10
- TotalPayload=16384, 32768, 65536
- InvocationCount=10
- IterationCount=10
- RunStrategy=Throughput
- UnrollFactor=1
- WarmupCount=5

| Method               | TotalInstances | TotalSamples | TotalPayload |  Latency Avg. | Latency Std. Dev. | Latency Minimum | Latency Maximum |   Latency 50% |   Latency 90% |   Latency 99% |
|----------------------|----------------|--------------|--------------|--------------:|------------------:|----------------:|----------------:|--------------:|--------------:|--------------:|
| **'OpenDDS Native'** | **1000**       | **10**       | **16384**    | **0.0796 ms** |     **0.0028 ms** |   **0.0726 ms** |   **0.0892 ms** | **0.0799 ms** | **0.0827 ms** | **0.0779 ms** |
| 'OpenDDSharp CDR'    | 1000           | 10           | 16384        |     0.0878 ms |         0.0085 ms |       0.0800 ms |       0.2620 ms |     0.0890 ms |     0.0900 ms |     0.0880 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **1000**       | **10**       | **32768**    | **0.0839 ms** |     **0.0017 ms** |   **0.0751 ms** |   **0.0960 ms** | **0.0832 ms** | **0.0839 ms** | **0.0841 ms** |
| 'OpenDDSharp CDR'    | 1000           | 10           | 32768        |     0.0941 ms |         0.0107 ms |       0.0850 ms |       0.2780 ms |     0.0940 ms |     0.0950 ms |     0.0970 ms |
|                      |                |              |              |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **1000**       | **10**       | **65536**    | **0.1040 ms** |     **0.0049 ms** |   **0.0888 ms** |   **0.1278 ms** | **0.1082 ms** | **0.0942 ms** | **0.0947 ms** |
| 'OpenDDSharp CDR'    | 1000           | 10           | 65536        |     0.1230 ms |         0.0139 ms |       0.1060 ms |       0.2250 ms |     0.1120 ms |     0.1240 ms |     0.1240 ms |

#### Latency Samples

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) Darwin 24.2.0  
Apple M3, 1 CPU, 8 logical and 8 physical cores  
.NET SDK 8.0.406

Test Configuration:
- TotalInstances=1000
- TotalSamples=10, 20, 30
- TotalPayload=16384
- InvocationCount=10
- IterationCount=10
- RunStrategy=Throughput
- UnrollFactor=1
- WarmupCount=5

| Method               | TotalInstances | TotalSamples | TotalPayload |    Ratio |  Latency Avg. | Latency Std. Dev. | Latency Minimum | Latency Maximum |   Latency 50% |   Latency 90% |   Latency 99% |
|----------------------|----------------|--------------|--------------|---------:|--------------:|------------------:|----------------:|----------------:|--------------:|--------------:|--------------:|
| **'OpenDDS Native'** | **100**        | **10**       | **16384**    | **1.00** | **0.0792 ms** |     **0.0028 ms** |   **0.0745 ms** |   **0.0877 ms** | **0.0767 ms** | **0.0775 ms** | **0.0815 ms** |
| 'OpenDDSharp CDR'    | 100            | 10           | 16384        |     1.10 |     0.0864 ms |         0.0084 ms |       0.0800 ms |       0.1910 ms |     0.0910 ms |     0.0850 ms |     0.0880 ms |
|                      |                |              |              |          |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **200**        | **10**       | **16384**    | **1.00** | **0.0795 ms** |     **0.0030 ms** |   **0.0742 ms** |   **0.0912 ms** | **0.0760 ms** | **0.0818 ms** | **0.0766 ms** |
| 'OpenDDSharp CDR'    | 200            | 10           | 16384        |     1.10 |     0.0875 ms |         0.0081 ms |       0.0810 ms |       0.1880 ms |     0.0840 ms |     0.0840 ms |     0.0830 ms |
|                      |                |              |              |          |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **500**        | **10**       | **16384**    | **1.00** | **0.0793 ms** |     **0.0028 ms** |   **0.0730 ms** |   **0.0981 ms** | **0.0822 ms** | **0.0825 ms** | **0.0770 ms** |
| 'OpenDDSharp CDR'    | 500            | 10           | 16384        |     1.11 |     0.0882 ms |         0.0079 ms |       0.0810 ms |       0.1930 ms |     0.0900 ms |     0.0870 ms |     0.0830 ms |
|                      |                |              |              |          |               |                   |                 |                 |               |               |               |
| **'OpenDDS Native'** | **1000**       | **10**       | **16384**    | **1.00** | **0.0795 ms** |     **0.0028 ms** |   **0.0714 ms** |   **0.0928 ms** | **0.0821 ms** | **0.0815 ms** | **0.0775 ms** |
| 'OpenDDSharp CDR'    | 1000           | 10           | 16384        |     1.10 |     0.0874 ms |         0.0088 ms |       0.0800 ms |       0.2680 ms |     0.0900 ms |     0.0910 ms |     0.0850 ms |

#### Throughput Default

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) Darwin 24.2.0  
Apple M3, 1 CPU, 8 logical and 8 physical cores  
.NET SDK 8.0.406

Test Configuration:

- TotalSamples=10000, 20000  
- TotalPayload=2048, 4096, 8192  
- InvocationCount=10  
- IterationCount=10  
- RunStrategy=Throughput    
- UnrollFactor=1
- WarmupCount=5  

| Method               | TotalSamples | TotalPayload |         Mean |    Ratio |       Error |      StdDev | Throughput (MB/sec) |    Allocated |
|----------------------|--------------|--------------|-------------:|---------:|------------:|------------:|--------------------:|-------------:|
| **'OpenDDS Native'** | **10000**    | **2048**     | **398.7 ms** | **1.00** | **1.21 ms** | **0.80 ms** |   **48.988 MB/sec** |     **74 B** |
| 'OpenDDSharp CDR'    | 10000        | 2048         |     408.8 ms |     1.03 |     1.61 ms |     1.07 ms |       47.780 MB/sec |   70468561 B |
| 'OpenDDSharp JSON'   | 10000        | 2048         |     756.2 ms |     1.90 |     3.20 ms |     2.12 ms |       25.826 MB/sec |  351868503 B |
|                      |              |              |              |          |             |             |                     |              |
| **'OpenDDS Native'** | **10000**    | **4096**     | **411.4 ms** | **1.00** | **0.98 ms** | **0.65 ms** |   **94.940 MB/sec** |     **74 B** |
| 'OpenDDSharp CDR'    | 10000        | 4096         |     429.7 ms |     1.04 |     3.34 ms |     2.21 ms |       90.901 MB/sec |  131937849 B |
| 'OpenDDSharp JSON'   | 10000        | 4096         |   1,540.0 ms |     3.74 |    23.38 ms |    15.47 ms |       25.365 MB/sec |  685617578 B |
|                      |              |              |              |          |             |             |                     |              |
| **'OpenDDS Native'** | **10000**    | **8192**     | **419.5 ms** | **1.00** | **4.59 ms** | **3.03 ms** |  **186.220 MB/sec** |     **74 B** |
| 'OpenDDSharp CDR'    | 10000        | 8192         |     466.8 ms |     1.11 |     4.59 ms |     3.04 ms |      167.375 MB/sec |  254797454 B |
| 'OpenDDSharp JSON'   | 10000        | 8192         |   2,806.4 ms |     6.69 |    59.49 ms |    39.35 ms |       27.838 MB/sec | 1351157394 B |
|                      |              |              |              |          |             |             |                     |              |
| **'OpenDDS Native'** | **20000**    | **2048**     | **781.5 ms** | **1.00** | **4.16 ms** | **2.18 ms** |   **49.984 MB/sec** |     **74 B** |
| 'OpenDDSharp CDR'    | 20000        | 2048         |     817.5 ms |     1.05 |     5.18 ms |     3.08 ms |       47.785 MB/sec |  140915517 B |
| 'OpenDDSharp JSON'   | 20000        | 2048         |   1,792.0 ms |     2.29 |    22.24 ms |    13.23 ms |       21.799 MB/sec |  703780385 B |
|                      |              |              |              |          |             |             |                     |              |
| **'OpenDDS Native'** | **20000**    | **4096**     | **820.8 ms** | **1.00** | **5.20 ms** | **3.44 ms** |   **95.181 MB/sec** |     **74 B** |
| 'OpenDDSharp CDR'    | 20000        | 4096         |     896.0 ms |     1.09 |     6.73 ms |     4.45 ms |       87.194 MB/sec |  263810619 B |
| 'OpenDDSharp JSON'   | 20000        | 4096         |   3,044.8 ms |     3.71 |    12.46 ms |     6.52 ms |       25.658 MB/sec | 1368037497 B |
|                      |              |              |              |          |             |             |                     |              |
| **'OpenDDS Native'** | **20000**    | **8192**     | **830.5 ms** | **1.00** | **2.96 ms** | **1.96 ms** |  **188.142 MB/sec** |     **74 B** |
| 'OpenDDSharp CDR'    | 20000        | 8192         |     915.5 ms |     1.10 |     9.95 ms |     5.92 ms |      170.667 MB/sec |  509591558 B |
| 'OpenDDSharp JSON'   | 20000        | 8192         |   5,355.1 ms |     6.45 |   260.54 ms |   172.33 ms |       29.178 MB/sec | 2698018365 B |

#### Throughput Payload

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) Darwin 24.2.0  
Apple M3, 1 CPU, 8 logical and 8 physical cores  
.NET SDK 8.0.406   

Test Configuration:

- TotalSamples=100000
- TotalPayload=16384, 32768, 65536
- InvocationCount=10
- IterationCount=10
- RunStrategy=Throughput  
- UnrollFactor=1
- WarmupCount=5

| Method               | TotalSamples | TotalPayload |        Mean |    Ratio |        Error |       StdDev | Throughput (MB/sec) |     Allocated |
|----------------------|--------------|--------------|------------:|---------:|-------------:|-------------:|--------------------:|--------------:|
| **'OpenDDS Native'** | **100000**   | **16384**    | **4.455 s** | **1.00** | **0.0943 s** | **0.0624 s** |  **350.725 MB/sec** |      **74 B** |
| 'OpenDDSharp CDR'    | 100000       | 16384        |     5.234 s |     1.18 |     0.4156 s |     0.2749 s |      298.521 MB/sec |  5007834370 B |
|                      |              |              |             |          |              |              |                     |               |
| **'OpenDDS Native'** | **100000**   | **32768**    | **6.071 s** | **1.00** | **0.0217 s** | **0.0143 s** |  **514.738 MB/sec** |      **74 B** |
| 'OpenDDSharp CDR'    | 100000       | 32768        |     6.836 s |     1.13 |     0.2466 s |     0.1290 s |      457.127 MB/sec |  9933237773 B |
|                      |              |              |             |          |              |              |                     |               |
| **'OpenDDS Native'** | **100000**   | **65536**    | **7.229 s** | **1.00** | **0.0878 s** | **0.0581 s** |  **864.552 MB/sec** |      **74 B** |
| 'OpenDDSharp CDR'    | 100000       | 65536        |     8.202 s |     1.13 |     0.0426 s |     0.0282 s |      762.000 MB/sec | 19765681308 B |

#### Throughput Samples

BenchmarkDotNet v0.14.0, macOS Sequoia 15.2 (24C101) Darwin 24.2.0  
Apple M3, 1 CPU, 8 logical and 8 physical cores  
.NET SDK 8.0.406  

Test Configuration:

- TotalSamples=50000, 100000, 150000, 200000
- TotalPayload=16384
- InvocationCount=10
- IterationCount=10
- RunStrategy=Throughput
- UnrollFactor=1
- WarmupCount=5

| Method               | TotalSamples | TotalPayload |        Mean |    Ratio |        Error |       StdDev | Throughput (MB/sec) |     Allocated |
|----------------------|--------------|--------------|------------:|---------:|-------------:|-------------:|--------------------:|--------------:|
| **'OpenDDS Native'** | **50000**    | **16384**    | **2.135 s** | **1.00** | **0.0036 s** | **0.0024 s** |  **365.912 MB/sec** |      **74 B** |
| 'OpenDDSharp CDR'    | 50000        | 16384        |     2.339 s |     1.10 |     0.0153 s |     0.0101 s |      334.058 MB/sec |  2503793412 B |
|                      |              |              |             |          |              |              |                     |               |
| **'OpenDDS Native'** | **100000**   | **16384**    | **4.256 s** | **1.00** | **0.0347 s** | **0.0229 s** |  **367.145 MB/sec** |      **74 B** |
| 'OpenDDSharp CDR'    | 100000       | 16384        |     4.651 s |     1.09 |     0.0368 s |     0.0219 s |      335.958 MB/sec |  5007095820 B |
|                      |              |              |             |          |              |              |                     |               |
| **'OpenDDS Native'** | **150000**   | **16384**    | **6.443 s** | **1.00** | **0.0573 s** | **0.0341 s** |  **363.761 MB/sec** |      **74 B** |
| 'OpenDDSharp CDR'    | 150000       | 16384        |     7.065 s |     1.10 |     0.2142 s |     0.1417 s |      331.749 MB/sec |  7514520565 B |
|                      |              |              |             |          |              |              |                     |               |
| **'OpenDDS Native'** | **200000**   | **16384**    | **8.449 s** | **1.00** | **0.0837 s** | **0.0554 s** |  **369.878 MB/sec** |      **74 B** |
| 'OpenDDSharp CDR'    | 200000       | 16384        |     9.314 s |     1.10 |     0.0547 s |     0.0326 s |      335.501 MB/sec | 10014268015 B |

## Conclusions

OpenDDSharp CDR implementation performs better in terms of latency and throughput compared to the OpenDDSharp JSON
implementation.

For small payloads, the difference in latency ands throughput between OpenDDSharp CDR and OpenDDS Native is acceptable.
Still, as the payload size increases, the latency and the throughput time increases compared to OpenDDS Native,
but the results obtained are much better than the JSON implementation. Also, the memory footprint of the CDR implementation
is lower than the JSON implementation.

For the next versions of OpenDDSharp, the default implementation will be the CDR one, and the JSON implementation will be
kept as an alternative for those who wish to use it, but all the performance improvements will be focused on the CDR
implementation.

The option to serialize samples using JSON will be kept for the users even when using the CDR implementation, as it can
be useful for debugging purposes or when the user wants to use the JSON serialization for some specific reason.