using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.DDS;

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
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "throughput_global_setup", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr ThroughputGlobalSetup(string configName);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(TEST_LIBRARY_NAME, EntryPoint = "throughput_global_cleanup")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial void ThroughputGlobalCleanup(IntPtr ptr);

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