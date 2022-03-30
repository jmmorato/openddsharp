/*********************************************************************
This file is part of OpenDDSharp.
OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2022 Jose Morato
OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
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
    /// Provides access to the configurable options for the UDP/IP transport.
    /// </summary>
    /// <remarks>
    /// The UDP transport is a bare bones transport that supports best-efort delivery only.
    /// </remarks>
    public class UdpInst : TransportInst
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
        /// Gets or sets total send bufer size in bytes for UDP payload.
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
        /// Gets or sets the hostname and port of the listening socket.
        /// Defaults to a value picked by the underlying OS.
        /// </summary>
        /// <remarks>
        /// If only the host is specifed and the port number is omitted, 
        /// the ':' is still required on the host specifer. 
        /// </remarks>
        public string LocalAddress
        {
            get => GetLocalAddress();
            set => SetLocalAddress(value);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UdpInst"/> class.
        /// </summary>
        /// <param name="inst">The base <see cref="TransportInst" /> object created with the <see cref="TransportRegistry" />.</param>
        public UdpInst(TransportInst inst) : base(inst != null ? inst.ToNative() : IntPtr.Zero)
        {
            if (inst == null)
            {
                throw new ArgumentNullException(nameof(inst));
            }

            _native = UnsafeNativeMethods.UdpInstNew(inst.ToNative());
        }
        #endregion

        #region Methods
        private bool GetIsReliable()
        {
            return UnsafeNativeMethods.GetIsReliable(_native);
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

        private string GetLocalAddress()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetLocalAddress(_native));
        }

        private void SetLocalAddress(string value)
        {
            UnsafeNativeMethods.SetLocalAddress(_native, value);
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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "UdpInst_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr UdpInstNew(IntPtr inst);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "UdpInst_GetIsReliable", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetIsReliable(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "UdpInst_GetSendBufferSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetSendBufferSize(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "UdpInst_SetSendBufferSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetSendBufferSize(IntPtr mi, int value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "UdpInst_GetRcvBufferSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetRcvBufferSize(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "UdpInst_SetRcvBufferSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetRcvBufferSize(IntPtr mi, int value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "UdpInst_GetLocalAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetLocalAddress(IntPtr ird);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "UdpInst_SetLocalAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern void SetLocalAddress(IntPtr ird, string value);
        }
        #endregion
    }
}
