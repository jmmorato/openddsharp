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

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// Allows the creation and destruction of <see cref="DomainParticipant" /> objects.
    /// </summary>
    public sealed class DomainParticipantFactory
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        internal DomainParticipantFactory(IntPtr native)
        {
            _native = native;
        }
        #endregion

        #region Methods
        public DomainParticipant CreateParticipant(int domainId)
        {
            DomainParticipantQosWrapper qos = default(DomainParticipantQosWrapper);

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateParticipant86(_native, domainId, ref qos, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreateParticipant64(_native, domainId, ref qos, IntPtr.Zero, 0u));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new DomainParticipant(native);
        }

        public ReturnCode DeleteParticipant(DomainParticipant dp)
        {
            if (dp == null)
            {
                throw new ArgumentNullException(nameof(dp));
            }

            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeleteParticipant86(_native, dp.ToNative()),
                                               () => UnsafeNativeMethods.DeleteParticipant64(_native, dp.ToNative()));
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipantFactory_CreateParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateParticipant64(IntPtr dpf, int domainId, [MarshalAs(UnmanagedType.Struct), In] ref DomainParticipantQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipantFactory_CreateParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateParticipant86(IntPtr dpf, int domainId, [MarshalAs(UnmanagedType.Struct), In] ref DomainParticipantQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipantFactory_DeleteParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteParticipant64(IntPtr dpf, IntPtr dp);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipantFactory_DeleteParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteParticipant86(IntPtr dpf, IntPtr dp);
        }
        #endregion
    }
}
