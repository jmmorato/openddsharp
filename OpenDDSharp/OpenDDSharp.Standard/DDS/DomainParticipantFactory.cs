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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System;
using System.Security;
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS
{
    public class DomainParticipantFactory
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
            if (Environment.Is64BitProcess)
            {
                DomainParticipantQosWrapper qos = new DomainParticipantQosWrapper();               
                IntPtr native = CreateParticipant64(_native, domainId, ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new DomainParticipant(native);
            }
            else
            {
                DomainParticipantQosWrapper qos = new DomainParticipantQosWrapper();
                IntPtr native = CreateParticipant86(_native, domainId, ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new DomainParticipant(native);
            }
        }

        public ReturnCode DeleteParticipant(DomainParticipant dp)
        {
            if (Environment.Is64BitProcess)
            {
                return DeleteParticipant64(_native, dp.ToNative());
            }
            else
            {
                return DeleteParticipant86(_native, dp.ToNative());
            }
        }
        #endregion

        #region PInvoke
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "DomainParticipantFactory_CreateParticipant", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateParticipant64(IntPtr dpf, int domainId, [MarshalAs(UnmanagedType.Struct), In] ref DomainParticipantQosWrapper qos, IntPtr a_listener, uint mask);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "DomainParticipantFactory_CreateParticipant", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateParticipant86(IntPtr dpf, int domainId, [MarshalAs(UnmanagedType.Struct), In] ref DomainParticipantQosWrapper qos, IntPtr a_listener, uint mask);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "DomainParticipantFactory_DeleteParticipant", CallingConvention = CallingConvention.Cdecl)]
        private static extern ReturnCode DeleteParticipant64(IntPtr dpf, IntPtr dp);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "DomainParticipantFactory_DeleteParticipant", CallingConvention = CallingConvention.Cdecl)]
        private static extern ReturnCode DeleteParticipant86(IntPtr dpf, IntPtr dp);
        #endregion
    }
}
