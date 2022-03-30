/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

namespace OpenDDSharp.OpenDDS.DCPS
{
    /// <summary>
    /// Provides access to the configurable options for the IP Multicast transport.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The multicast transport provides unifed support for best-efort and reliable delivery based
    /// on a transport confguration parameter.
    /// </para>
    /// <para>
    /// Best-efort delivery imposes the least amount of overhead as data is exchanged between
    /// peers, however it does not provide any guarantee of delivery. Data may be lost due to
    /// unresponsive or unreachable peers or received in duplicate.
    /// </para>
    /// <para>
    /// Reliable delivery provides for guaranteed delivery of data to associated peers with no
    /// duplication at the cost of additional processing and bandwidth. Reliable delivery is achieved
    /// through two primary mechanisms: 2-way peer handshaking and negative acknowledgment of missing data.
    /// </para>
    /// </remarks>
    public class MulticastInst : TransportInst
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether the transport is reliable or not.
        /// </summary>
        public bool IsReliable
        {
            get => GetIsReliable();
        }

        /// <summary>
        /// Gets or sets a value indicating whether enables reliable communication.
        /// The default value is true.
        /// </summary>
        /// <remarks>
        /// This option will eventually be deprecated.
        /// </remarks>
        public bool Reliable
        {
            get => GetReliable();
            set => SetReliable(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether enables IPv6 default group address selection.
        /// By default, this option is disabled (false).
        /// </summary>
        public bool DefaultToIpv6
        {
            get => GetDefaultToIpv6();
            set => SetDefaultToIpv6(value);
        }

        /// <summary>
        /// Gets or sets the default port number (when <see cref="GroupAddress" /> is not set)
        /// The default value is 49152 [IANA 2009-11-16].
        /// </summary>
        /// <remarks>
        /// When a group address is specifed, the port number within it is used.
        /// If no group address is specifed, the port offset is used as a port number.
        /// This value should not be set less than 49152.
        /// </remarks>
        public ushort PortOffset
        {
            get => GetPortOffset();
            set => SetPortOffset(value);
        }

        /// <summary>
        /// Gets or sets the multicast group to join to send/receive data.
        /// The default value is:
        ///   224.0.0.128:port [IANA 2009-11-17], or
        ///   [FF01::80]:port [IANA 2009-08-28].
        /// </summary>
        public string GroupAddress
        {
            get => GetGroupAddress();
            set => SetGroupAddress(value);
        }

        /// <summary>
        /// Gets or sets, if non-empty, the address of a local network interface which
        /// is used to join the multicast group.
        /// </summary>
        public string LocalAddress
        {
            get => GetLocalAddress();
            set => SetLocalAddress(value);
        }

        /// <summary>
        /// Gets or sets the exponential base used during handshake retries; smaller
        /// values yield shorter delays between attempts.
        /// The default value is 2.0.
        /// </summary>
        public double SynBackoff
        {
            get => GetSynBackoff();
            set => SetSynBackoff(value);
        }

        /// <summary>
        /// Gets or sets the minimum number of milliseconds to wait between handshake
        /// attempts during association. The default value is 250.
        /// </summary>
        public TimeValue SynInterval
        {
            get => GetSynInterval();
            set => SetSynInterval(value);
        }

        /// <summary>
        /// Gets or sets the maximum number of milliseconds to wait before giving up
        /// on a handshake response during association.
        /// The default value is 30000 (30 seconds).
        /// </summary>
        public TimeValue SynTimeout
        {
            get => GetSynTimeout();
            set => SetSynTimeout(value);
        }

        /// <summary>
        /// Gets or sets the number of datagrams to retain in order to service repair
        /// requests (reliable only). The default value is 32.
        /// </summary>
        public ulong NakDepth
        {
            get => GetNakDepth();
            set => SetNakDepth(value);
        }

        /// <summary>
        /// Gets or sets the minimum number of milliseconds to wait between repair
        /// requests (reliable only). The default value is 500.
        /// </summary>
        public TimeValue NakInterval
        {
            get => GetNakInterval();
            set => SetNakInterval(value);
        }

        /// <summary>
        /// Gets or sets the number of intervals between nak's for a sample
        /// after initial nak. The default value is 4.
        /// </summary>
        public ulong NakDelayIntervals
        {
            get => GetNakDelayIntervals();
            set => SetNakDelayIntervals(value);
        }

        /// <summary>
        /// Gets or sets the maximum number of a missing sample will be nak'ed.
        /// The default value is: 3.
        /// </summary>
        public ulong NakMax
        {
            get => GetNakMax();
            set => SetNakMax(value);
        }

        /// <summary>
        /// Gets or sets the maximum number of milliseconds to wait before giving up
        /// on a repair response (reliable only).
        /// The default value is: 30000 (30 seconds).
        /// </summary>
        public TimeValue NakTimeout
        {
            get => GetNakTimeout();
            set => SetNakTimeout(value);
        }

        /// <summary>
        /// Gets or sets the value of the time-to-live (ttl) field of any
        /// datagrams sent. The default value of one means
        /// that all data is restricted to the local network.
        /// </summary>
        public byte Ttl
        {
            get => GetTtl();
            set => SetTtl(value);
        }

        /// <summary>
        /// Gets or sets the size of the socket receive buffer.
        /// The default value is ACE_DEFAULT_MAX_SOCKET_BUFSIZ if it's defined,
        /// otherwise, 0. If the value is 0, the system default value is used.
        /// </summary>
        public ulong RcvBufferSize
        {
            get => GetRcvBufferSize();
            set => SetRcvBufferSize(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether sending using asynchronous I/O on Windows platforms that support it.
        /// The default value is false.
        /// </summary>
        /// <remarks>
        /// This parameter has no effect on non-Windows platforms and Windows platforms
        /// that don't support asynchronous I/O.
        /// </remarks>
        public bool AsyncSend
        {
            get => GetAsyncSend();
            set => SetAsyncSend(value);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MulticastInst"/> class.
        /// </summary>
        /// <param name="inst">The base <see cref="TransportInst" /> object created with the <see cref="TransportRegistry" />.</param>
        public MulticastInst(TransportInst inst) : base(inst != null ? inst.ToNative() : IntPtr.Zero)
        {
            if (inst == null)
            {
                throw new ArgumentNullException(nameof(inst));
            }

            _native = UnsafeNativeMethods.MulticastInstNew(inst.ToNative());
        }
        #endregion

        #region Methods
        private bool GetIsReliable()
        {
            return UnsafeNativeMethods.GetIsReliable(_native);
        }

        private bool GetReliable()
        {
            return UnsafeNativeMethods.GetReliable(_native);
        }

        private void SetReliable(bool value)
        {
            UnsafeNativeMethods.SetReliable(_native, value);
        }

        private bool GetDefaultToIpv6()
        {
            return UnsafeNativeMethods.GetDefaultToIpv6(_native);
        }

        private void SetDefaultToIpv6(bool value)
        {
            UnsafeNativeMethods.SetDefaultToIpv6(_native, value);
        }

        private ushort GetPortOffset()
        {
            return UnsafeNativeMethods.GetPortOffset(_native);
        }

        private void SetPortOffset(ushort value)
        {
            UnsafeNativeMethods.SetPortOffset(_native, value);
        }

        private string GetGroupAddress()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetGroupAddress(_native));
        }

        private void SetGroupAddress(string value)
        {
            UnsafeNativeMethods.SetGroupAddress(_native, value);
        }

        private string GetLocalAddress()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetLocalAddress(_native));
        }

        private void SetLocalAddress(string value)
        {
            UnsafeNativeMethods.SetLocalAddress(_native, value);
        }

        private double GetSynBackoff()
        {
            return UnsafeNativeMethods.GetSynBackoff(_native);
        }

        private void SetSynBackoff(double value)
        {
            UnsafeNativeMethods.SetSynBackoff(_native, value);
        }

        private TimeValue GetSynInterval()
        {
            return UnsafeNativeMethods.GetSynInterval(_native);
        }

        private void SetSynInterval(TimeValue value)
        {
            UnsafeNativeMethods.SetSynInterval(_native, value);
        }

        private TimeValue GetSynTimeout()
        {
            return UnsafeNativeMethods.GetSynTimeout(_native);
        }

        private void SetSynTimeout(TimeValue value)
        {
            UnsafeNativeMethods.SetSynTimeout(_native, value);
        }

        private ulong GetNakDepth()
        {
            return UnsafeNativeMethods.GetNakDepth(_native).ToUInt64();
        }

        private void SetNakDepth(ulong value)
        {
            UnsafeNativeMethods.SetNakDepth(_native, new UIntPtr(value));
        }

        private TimeValue GetNakInterval()
        {
            return UnsafeNativeMethods.GetNakInterval(_native);
        }

        private void SetNakInterval(TimeValue value)
        {
            UnsafeNativeMethods.SetNakInterval(_native, value);
        }

        private ulong GetNakDelayIntervals()
        {
            return UnsafeNativeMethods.GetNakDelayIntervals(_native).ToUInt64();
        }

        private void SetNakDelayIntervals(ulong value)
        {
            UnsafeNativeMethods.SetNakDelayIntervals(_native, new UIntPtr(value));
        }

        private ulong GetNakMax()
        {
            return UnsafeNativeMethods.GetNakMax(_native).ToUInt64();
        }

        private void SetNakMax(ulong value)
        {
            UnsafeNativeMethods.SetNakMax(_native, new UIntPtr(value));
        }

        private TimeValue GetNakTimeout()
        {
            return UnsafeNativeMethods.GetNakTimeout(_native);
        }

        private void SetNakTimeout(TimeValue value)
        {
            UnsafeNativeMethods.SetNakTimeout(_native, value);
        }

        private byte GetTtl()
        {
            return UnsafeNativeMethods.GetTtl(_native);
        }

        private void SetTtl(byte value)
        {
            UnsafeNativeMethods.SetTtl(_native, value);
        }

        private ulong GetRcvBufferSize()
        {
            return UnsafeNativeMethods.GetRcvBufferSize(_native).ToUInt64();
        }

        private void SetRcvBufferSize(ulong value)
        {
            UnsafeNativeMethods.SetRcvBufferSize(_native, new UIntPtr(value));
        }

        private bool GetAsyncSend()
        {
            return UnsafeNativeMethods.GetAsyncSend(_native);
        }

        private void SetAsyncSend(bool value)
        {
            UnsafeNativeMethods.SetAsyncSend(_native, value);
        }
        #endregion

        #region UnsafeNativeMethods
        /// <summary>
        /// This class suppresses stack walks for unmanaged code permission. (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
        /// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full security review to make sure that the usage
        /// is secure because no stack walk will be performed.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private static class UnsafeNativeMethods
        {
            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr MulticastInstNew(IntPtr inst);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetIsReliable", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetIsReliable(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetReliable", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetReliable(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetReliable", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetReliable(IntPtr mi, [MarshalAs(UnmanagedType.I1)] bool value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetDefaultToIpv6", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetDefaultToIpv6(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetDefaultToIpv6", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetDefaultToIpv6(IntPtr mi, [MarshalAs(UnmanagedType.I1)] bool value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetPortOffset", CallingConvention = CallingConvention.Cdecl)]
            public static extern ushort GetPortOffset(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetPortOffset", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetPortOffset(IntPtr mi, ushort value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetGroupAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetGroupAddress(IntPtr ird);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetGroupAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern void SetGroupAddress(IntPtr ird, string ip);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetLocalAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetLocalAddress(IntPtr ird);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetLocalAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern void SetLocalAddress(IntPtr ird, string ip);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetSynBackoff", CallingConvention = CallingConvention.Cdecl)]
            public static extern double GetSynBackoff(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetSynBackoff", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetSynBackoff(IntPtr mi, double value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetSynInterval", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Struct)]
            public static extern TimeValue GetSynInterval(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetSynInterval", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetSynInterval(IntPtr mi, [MarshalAs(UnmanagedType.Struct)] TimeValue value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetSynTimeout", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Struct)]
            public static extern TimeValue GetSynTimeout(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetSynTimeout", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetSynTimeout(IntPtr mi, [MarshalAs(UnmanagedType.Struct)] TimeValue value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetNakDepth", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetNakDepth(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetNakDepth", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetNakDepth(IntPtr mi, UIntPtr value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetNakInterval", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Struct)]
            public static extern TimeValue GetNakInterval(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetNakInterval", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetNakInterval(IntPtr mi, [MarshalAs(UnmanagedType.Struct)] TimeValue value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetNakDelayIntervals", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetNakDelayIntervals(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetNakDelayIntervals", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetNakDelayIntervals(IntPtr mi, UIntPtr value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetNakMax", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetNakMax(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetNakMax", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetNakMax(IntPtr mi, UIntPtr value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetNakTimeout", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Struct)]
            public static extern TimeValue GetNakTimeout(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetNakTimeout", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetNakTimeout(IntPtr mi, [MarshalAs(UnmanagedType.Struct)] TimeValue value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetTtl", CallingConvention = CallingConvention.Cdecl)]
            public static extern byte GetTtl(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetTtl", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetTtl(IntPtr mi, byte value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetRcvBufferSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetRcvBufferSize(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetRcvBufferSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetRcvBufferSize(IntPtr mi, UIntPtr value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_GetAsyncSend", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetAsyncSend(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "MulticastInst_SetAsyncSend", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetAsyncSend(IntPtr mi, [MarshalAs(UnmanagedType.I1)] bool value);
        }
        #endregion
    }
}
