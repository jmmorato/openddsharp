using OpenDDSharp.BenchmarkPerformance.Helpers;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

public sealed class OpenDDSThroughputTest(int totalSamples, ulong totalPayload, IntPtr participant) : IDisposable
{
    private readonly IntPtr _ptr = UnsafeNativeMethods.ThroughputInitialize(totalSamples, totalPayload, participant);

    public ulong Run()
    {
        return UnsafeNativeMethods.ThroughputRun(_ptr);
    }

    public void Dispose()
    {
        UnsafeNativeMethods.ThroughputFinalize(_ptr);
    }
}
