﻿/*********************************************************************
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
    /// Provides access to the configurable options for the RTPS UDP transport.
    /// </summary>
    /// <remarks>
    /// The RTPS UDP transport is one of the pluggable transports available to a developer and is necessary
    /// for interoperable communication between implementations.
    /// </remarks>
    public class RtpsUdpInst : TransportInst
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether the transport is reliable or not.
        /// </summary>
        public bool IsReliable => GetIsReliable();

        /// <summary>
        ///  Gets a value indicating whether the transport requires CDR serialization or not.
        /// </summary>
        public bool RequiresCdr => GetRequiresCdr();

        /// <summary>
        /// Gets or sets the total send bufer size in bytes for UDP payload.
        /// The default value is the platform value of
        /// ACE_DEFAULT_MAX_SOCKET_BUFSIZ.
        /// </summary>
        public int SendBufferSize
        {
            get => GetSendBufferSize();
            set => SetSendBufferSize(value);
        }

        /// <summary>
        /// Gets or sets the total receive bufer size in bytes for UDP payload.
        /// The default value is the platform value of
        /// ACE_DEFAULT_MAX_SOCKET_BUFSIZ.
        /// </summary>
        public int RcvBufferSize
        {
            get => GetRcvBufferSize();
            set => SetRcvBufferSize(value);
        }

        /// <summary>
        /// Gets or sets the number of datagrams to retain in order to
        /// service repair requests (reliable only).
        /// The default value is 32.
        /// </summary>
        public ulong NakDepth
        {
            get => GetNakDepth();
            set => SetNakDepth(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the RTPS UDP transport can use Unicast or
        /// Multicast. When set to false the transport uses
        /// Unicast, otherwise a value of true will use Multicast.
        /// The default value is true.
        /// </summary>
        public bool UseMulticast
        {
            get => GetUseMulticast();
            set => SetUseMulticast(value);
        }

        /// <summary>
        /// Gets or sets the value of the time-to-live (ttl) field of any
        /// multicast datagrams sent. This value specifes the
        /// number of hops the datagram will traverse before
        /// being discarded by the network. The default value
        /// of 1 means that all data is restricted to the local
        /// network subnet.
        /// </summary>
        public byte Ttl
        {
            get => GetTtl();
            set => SetTtl(value);
        }

        /// <summary>
        /// Gets or sets the multicast group address.
        /// When the transport is set to multicast, this is the
        /// multicast network address that should be used. If
        /// no port is specifed for the network address, port
        /// 7401 will be used. The default value is 239.255.0.2:7401.
        /// </summary>
        public string MulticastGroupAddress
        {
            get => GetMulticastGroupAddress();
            set => SetMulticastGroupAddress(value);
        }

        /// <summary>
        /// Gets or sets the network interface to be used by this
        /// transport instance. This uses a platform-specifc
        /// format that identifes the network interface.
        /// </summary>
        public string MulticastInterface
        {
            get => GetMulticastInterface();
            set => SetMulticastInterface(value);
        }

        /// <summary>
        /// Gets or sets the address and port to bind the socket.
        /// Port can be omitted but the trailing ':' is required.
        /// </summary>
        public string LocalAddress
        {
            get => GetLocalAddress();
            set => SetLocalAddress(value);
        }

        /// <summary>
        /// Gets or sets the protocol tuning parameter that allows the RTPS
        /// Writer to delay the response (expressed in
        /// milliseconds) to a request for data from a negative
        /// acknowledgment. The default value is 200.
        /// </summary>
        public TimeValue NakResponseDelay
        {
            get => GetNakResponseDelay();
            set => SetNakResponseDelay(value);
        }

        /// <summary>
        /// Gets or sets the protocol tuning parameter that specifes in
        /// milliseconds how often an RTPS Writer announces
        /// the availability of data. The default value is 1000.
        /// </summary>
        public TimeValue HeartbeatPeriod
        {
            get => GetHeartbeatPeriod();
            set => SetHeartbeatPeriod(value);
        }

        /// <summary>
        /// Gets or sets the receive address timeout.
        /// The default value is 5 seconds.
        /// </summary>
        public TimeValue ReceiveAddressDuration
        {
            get => GetReceiveAddressDuration();
            set => SetReceiveAddressDuration(value);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RtpsUdpInst"/> class.
        /// </summary>
        /// <param name="inst">The base <see cref="TransportInst" /> object created with the <see cref="TransportRegistry" />.</param>
        public RtpsUdpInst(TransportInst inst) : base(inst != null ? inst.ToNative() : IntPtr.Zero)
        {
            if (inst == null)
            {
                throw new ArgumentNullException(nameof(inst));
            }

            _native = UnsafeNativeMethods.RtpsUdpInstNew(inst.ToNative());
        }
        #endregion

        #region Methods
        private bool GetIsReliable()
        {
            return UnsafeNativeMethods.GetIsReliable(_native);
        }

        private bool GetRequiresCdr()
        {
            return UnsafeNativeMethods.GetRequiresCdr(_native);
        }

        private int GetSendBufferSize()
        {
            return UnsafeNativeMethods.GetSendBufferSize(_native);
        }

        private void SetSendBufferSize(int value)
        {
            UnsafeNativeMethods.SetSendBufferSize(_native, value);
        }

        private int GetRcvBufferSize()
        {
            return UnsafeNativeMethods.GetRcvBufferSize(_native);
        }

        private void SetRcvBufferSize(int value)
        {
            UnsafeNativeMethods.SetRcvBufferSize(_native, value);
        }

        private ulong GetNakDepth()
        {
            return UnsafeNativeMethods.GetNakDepth(_native).ToUInt64();
        }

        private void SetNakDepth(ulong value)
        {
            UnsafeNativeMethods.SetNakDepth(_native, new UIntPtr(value));
        }

        private bool GetUseMulticast()
        {
            return UnsafeNativeMethods.GetUseMulticast(_native);
        }

        private void SetUseMulticast(bool value)
        {
            UnsafeNativeMethods.SetUseMulticast(_native, value);
        }

        private byte GetTtl()
        {
            return UnsafeNativeMethods.GetTtl(_native);
        }

        private void SetTtl(byte value)
        {
            UnsafeNativeMethods.SetTtl(_native, value);
        }

        private string GetMulticastGroupAddress()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetMulticastGroupAddress(_native));
        }

        private void SetMulticastGroupAddress(string value)
        {
            string full = value;
            if (!full.Contains(":"))
            {
                full += ":0";
            }

            UnsafeNativeMethods.SetMulticastGroupAddress(_native, full);
        }

        private string GetMulticastInterface()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetMulticastInterface(_native));
        }

        private void SetMulticastInterface(string value)
        {
            UnsafeNativeMethods.SetMulticastInterface(_native, value);
        }

        private string GetLocalAddress()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetLocalAddress(_native));
        }

        private void SetLocalAddress(string value)
        {
            UnsafeNativeMethods.SetLocalAddress(_native, value);
        }

        private TimeValue GetNakResponseDelay()
        {
            return UnsafeNativeMethods.GetNakResponseDelay(_native);
        }

        private void SetNakResponseDelay(TimeValue value)
        {
            UnsafeNativeMethods.SetNakResponseDelay(_native, value);
        }

        private TimeValue GetHeartbeatPeriod()
        {
            return UnsafeNativeMethods.GetHeartbeatPeriod(_native);
        }

        private void SetHeartbeatPeriod(TimeValue value)
        {
            UnsafeNativeMethods.SetHeartbeatPeriod(_native, value);
        }

        private TimeValue GetReceiveAddressDuration()
        {
            return UnsafeNativeMethods.GetReceiveAddressDuration(_native);
        }

        private void SetReceiveAddressDuration(TimeValue value)
        {
            UnsafeNativeMethods.SetReceiveAddressDuration(_native, value);
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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr RtpsUdpInstNew(IntPtr inst);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetIsReliable", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetIsReliable(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetRequiresCdr", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetRequiresCdr(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetSendBufferSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetSendBufferSize(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetSendBufferSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetSendBufferSize(IntPtr mi, int value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetRcvBufferSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetRcvBufferSize(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetRcvBufferSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetRcvBufferSize(IntPtr mi, int value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetNakDepth", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetNakDepth(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetNakDepth", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetNakDepth(IntPtr mi, UIntPtr value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetUseMulticast", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetUseMulticast(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetUseMulticast", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetUseMulticast(IntPtr mi, [MarshalAs(UnmanagedType.I1)] bool value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetTtl", CallingConvention = CallingConvention.Cdecl)]
            public static extern byte GetTtl(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetTtl", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetTtl(IntPtr mi, byte value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetMulticastGroupAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetMulticastGroupAddress(IntPtr ird);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetMulticastGroupAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern void SetMulticastGroupAddress(IntPtr ird, string ip);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetMulticastInterface", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetMulticastInterface(IntPtr ird);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetMulticastInterface", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern void SetMulticastInterface(IntPtr ird, string value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetLocalAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetLocalAddress(IntPtr ird);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetLocalAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern void SetLocalAddress(IntPtr ird, string value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetNakResponseDelay", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Struct)]
            public static extern TimeValue GetNakResponseDelay(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetNakResponseDelay", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetNakResponseDelay(IntPtr mi, [MarshalAs(UnmanagedType.Struct)] TimeValue value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetHeartbeatPeriod", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Struct)]
            public static extern TimeValue GetHeartbeatPeriod(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetHeartbeatPeriod", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetHeartbeatPeriod(IntPtr mi, [MarshalAs(UnmanagedType.Struct)] TimeValue value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_GetReceiveAddressDuration", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Struct)]
            public static extern TimeValue GetReceiveAddressDuration(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsUdpInst_SetReceiveAddressDuration", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetReceiveAddressDuration(IntPtr mi, [MarshalAs(UnmanagedType.Struct)] TimeValue value);
        }
        #endregion
    }
}