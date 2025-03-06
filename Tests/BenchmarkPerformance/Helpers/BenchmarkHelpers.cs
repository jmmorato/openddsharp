using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenDDSharp.DDS;
using Rti.Dds.Publication;
using Rti.Dds.Subscription;

namespace OpenDDSharp.BenchmarkPerformance.Helpers;

internal static class BenchmarkHelpers
{
    internal static bool WaitForSubscriptions(this DataWriter writer, int subscriptionsCount, int milliseconds)
    {
        ArgumentNullException.ThrowIfNull(writer);

        PublicationMatchedStatus status = default;
        writer.GetPublicationMatchedStatus(ref status);
        var count = milliseconds / 100;
        while (status.CurrentCount != subscriptionsCount && count > 0)
        {
            Thread.Sleep(100);
            writer.GetPublicationMatchedStatus(ref status);
            count--;
        }

        return count != 0 || status.CurrentCount == subscriptionsCount;
    }

    internal static bool WaitForPublications(this DataReader reader, int publicationsCount, int milliseconds)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var handles = new List<InstanceHandle>();
        reader.GetMatchedPublications(handles);
        var count = milliseconds / 100;
        while (handles.Count != publicationsCount && count > 0)
        {
            Thread.Sleep(100);
            reader.GetMatchedPublications(handles);
            count--;
        }

        return count != 0 || handles.Count == publicationsCount;
    }

    public static bool WaitForSubscriptions(this AnyDataWriter dw, int subscriptionsCount, int milliseconds)
    {
        var status = dw.PublicationMatchedStatus;

        var count = milliseconds / 100;
        while (status.CurrentCount.Value != subscriptionsCount && count > 0)
        {
            Thread.Sleep(100);
            status = dw.PublicationMatchedStatus;
            count--;
        }

        return count != 0 || status.CurrentCount.Value == subscriptionsCount;
    }

    public static bool WaitForPublications(this AnyDataReader dr, int publicationsCount, int milliseconds)
    {
        var status = dr.SubscriptionMatchedStatus;

        var count = milliseconds / 100;
        while (status.CurrentCount.Value != publicationsCount && count > 0)
        {
            Thread.Sleep(100);
            status = dr.SubscriptionMatchedStatus;
            count--;
        }

        return count != 0 || status.CurrentCount.Value == publicationsCount;
    }

    public static string GetPlatformString()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "x64";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var handle = Process.GetCurrentProcess().Handle;
            UnsafeNativeMethods.IsWow64Process2(handle, out var processMachine, out var nativeMachine);

            return processMachine == 0x00 ? "x64" : "x86";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "Arm64" : "x64";
        }

        throw new PlatformNotSupportedException();
    }
}