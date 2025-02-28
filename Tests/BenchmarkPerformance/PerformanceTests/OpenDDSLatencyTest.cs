using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Marshaller;

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

/// <summary>
/// This class suppresses stack walks for unmanaged code permission.
/// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full
/// security review to make sure that the usage is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Native p/invoke calls.")]
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented", Justification = "Partial required for the source generator.")]
internal static partial class UnsafeNativeMethods
{
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "latency_initialize")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr LatencyInitialize(int totalInstances, int totalSamples, ulong payloadSize, IntPtr participant);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "latency_run")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial uint LatencyRun(IntPtr test);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "latency_finalize")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void LatencyFinalize(IntPtr test);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "latency_get_latencies")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr LatencyGetLatencies(IntPtr test);
}