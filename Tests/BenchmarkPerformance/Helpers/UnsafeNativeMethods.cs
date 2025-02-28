using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenDDSharp.BenchmarkPerformance.Helpers;

/// <summary>
/// This class suppresses stack walks for unmanaged code permission.
/// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full
/// security review to make sure that the usage is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
[ExcludeFromCodeCoverage]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Native p/invoke calls.")]
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented", Justification = "Partial required for the source generator.")]
internal static partial class UnsafeNativeMethods
{
    private const string TEST_LIBRARY_NAME = "OpenDDSPerformanceTests";

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "global_setup", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr NativeGlobalSetup(string configName);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "global_cleanup")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void NativeGlobalCleanup(IntPtr ptr);

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

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "throughput_initialize")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr ThroughputInitialize(int totalSamples, ulong payloadSize, IntPtr participant);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "throughput_run")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial uint ThroughputRun(IntPtr test);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "throughput_finalize")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void ThroughputFinalize(IntPtr c);
}