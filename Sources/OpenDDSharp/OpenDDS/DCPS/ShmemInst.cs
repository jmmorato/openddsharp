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
    /// Provides access to the configurable options for the Shared Memory transport.
    /// </summary>
    /// <remarks>
    /// The shared memory transport type can only provide communication between transport instances on the same host.
    /// As part of transport negotiation, if there are multiple transport instances available for
    /// communication between hosts, the shared memory transport instances will be skipped so
    /// that other types can be used.
    /// </remarks>
    public class ShmemInst : TransportInst
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
        /// Gets or sets the size (in bytes) of the single shared-memory pool allocated by this
        /// transport instance. Defaults to 16 megabytes.
        /// </summary>
        public ulong PoolSize
        {
            get => GetPoolSize();
            set => SetPoolSize(value);
        }

        /// <summary>
        /// Gets or sets the size (in bytes) of the control area allocated for each data link.
        /// This allocation comes out of the shared-memory pool defined by <see cref="PoolSize" />.
        /// Defaults to 4 kilobytes.
        /// </summary>
        public ulong DatalinkControlSize
        {
            get => GetDatalinkControlSize();
            set => SetDatalinkControlSize(value);
        }

        /// <summary>
        /// Gets the host name.
        /// </summary>
        public string HostName
        {
            get => GetHostName();
        }

        /// <summary>
        /// Gets the pool name.
        /// </summary>
        public string PoolName
        {
            get => GetPoolName();
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShmemInst"/> class.
        /// </summary>
        /// <param name="inst">The base <see cref="TransportInst" /> object created with the <see cref="TransportRegistry" />.</param>
        public ShmemInst(TransportInst inst) : base(inst != null ? inst.ToNative() : IntPtr.Zero)
        {
            if (inst == null)
            {
                throw new ArgumentNullException(nameof(inst));
            }

            _native = UnsafeNativeMethods.ShmemInstNew(inst.ToNative());
        }
        #endregion

        #region Methods
        private bool GetIsReliable()
        {
            return UnsafeNativeMethods.GetIsReliable(_native);
        }

        private ulong GetPoolSize()
        {
            return UnsafeNativeMethods.GetPoolSize(_native).ToUInt64();
        }

        private void SetPoolSize(ulong value)
        {
            UnsafeNativeMethods.SetPoolSize(_native, new UIntPtr(value));
        }

        private ulong GetDatalinkControlSize()
        {
            return UnsafeNativeMethods.GetDatalinkControlSize(_native).ToUInt64();
        }

        private void SetDatalinkControlSize(ulong value)
        {
            UnsafeNativeMethods.SetDatalinkControlSize(_native, new UIntPtr(value));
        }

        private string GetHostName()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetHostName(_native));
        }

        private string GetPoolName()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetPoolName(_native));
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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "ShmemInst_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ShmemInstNew(IntPtr inst);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "ShmemInst_GetIsReliable", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetIsReliable(IntPtr si);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "ShmemInst_GetPoolSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetPoolSize(IntPtr si);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "ShmemInst_SetPoolSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetPoolSize(IntPtr si, UIntPtr value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "ShmemInst_GetDatalinkControlSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetDatalinkControlSize(IntPtr si);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "ShmemInst_SetDatalinkControlSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetDatalinkControlSize(IntPtr si, UIntPtr value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "ShmemInst_GetHostName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetHostName(IntPtr si);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "ShmemInst_GetPoolName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetPoolName(IntPtr si);
        }
        #endregion
    }
}
