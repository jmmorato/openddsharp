using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenDDSharp.BuildTasks
{
    class OleMessageFilter : IOleMessageFilter
    {
        private static TaskLoggingHelper _loggingHelper;

        public static void Register(TaskLoggingHelper loggingHelper)
        {
            _loggingHelper = loggingHelper;

            IOleMessageFilter newFilter = new OleMessageFilter();
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                _ = CoRegisterMessageFilter(newFilter, out _);
            }
            else
            {
                throw new COMException("Unable to register message filter because the current thread apartment state is not STA.");
            }
        }

        public static void Revoke()
        {
            _ = CoRegisterMessageFilter(null, out _);
        }

        int IOleMessageFilter.HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo)
        {
            _loggingHelper.LogMessage(MessageImportance.High, "Automation is handling an incoming call...");

            return (int)SERVERCALL.SERVERCALL_ISHANDLED;
        }

        int IOleMessageFilter.RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType)
        {
            if (dwRejectType == (int)SERVERCALL.SERVERCALL_RETRYLATER)
            {
                _loggingHelper.LogMessage(MessageImportance.High, "Automation is busy, waiting 500ms and trying again...");

                Thread.Sleep(500);

                // Retry immediately if return >=0 & <100
                return 99;
            }

            return -1;
        }

        int IOleMessageFilter.MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType)
        {
            _loggingHelper.LogMessage(MessageImportance.High, "Automation is waiting for pending messages...");

            return (int)PENDINGMSG.PENDINGMSG_WAITDEFPROCESS;
        }

        [DllImport("Ole32.dll")]
        private static extern int CoRegisterMessageFilter(IOleMessageFilter newFilter, out IOleMessageFilter oldFilter);
    }

    enum SERVERCALL
    {
        SERVERCALL_ISHANDLED = 0,
        SERVERCALL_REJECTED = 1,
        SERVERCALL_RETRYLATER = 2
    }

    enum PENDINGMSG
    {
        PENDINGMSG_CANCELCALL = 0,
        PENDINGMSG_WAITNOPROCESS = 1,
        PENDINGMSG_WAITDEFPROCESS = 2
    }

    [ComImport(), Guid("00000016-0000-0000-C000-000000000046"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IOleMessageFilter
    {
        [PreserveSig]
        int HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo);

        [PreserveSig]
        int RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType);

        [PreserveSig]
        int MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType);
    }
}
