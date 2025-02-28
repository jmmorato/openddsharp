using OpenDDSharp.Marshaller;
using OpenDDSharp.BenchmarkPerformance.Helpers;

namespace OpenDDSharp.BenchmarkPerformance.PerformanceTests;

internal sealed class OpenDDSLatencyTest(int totalInstances, int totalSamples, ulong totalPayload, IntPtr participant) : IDisposable
{
    private readonly IntPtr _ptr = UnsafeNativeMethods.LatencyInitialize(totalInstances, totalSamples, totalPayload, participant);

    public IList<TimeSpan> Latencies
    {
        get
        {
            var ptr = UnsafeNativeMethods.LatencyGetLatencies(_ptr);
            IList<double> list = new List<double>();
            ptr.PtrToSequence(ref list);
            ptr.ReleaseNativePointer();
            return list.Select(TimeSpan.FromMilliseconds).ToList();
        }
    }

    public ulong Run()
    {
        return UnsafeNativeMethods.LatencyRun(_ptr);
    }

    public void Dispose()
    {
        UnsafeNativeMethods.LatencyFinalize(_ptr);
    }
}
