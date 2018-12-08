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
    public class Publisher
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        internal Publisher(IntPtr native)
        {
            _native = native;
        }
        #endregion

        #region Methods
        public DataWriter CreateDataWriter(Topic topic)
        {
            if (Environment.Is64BitProcess)
            {
                DataWriterQosWrapper qos = new DataWriterQosWrapper();
                IntPtr native = CreateDataWriter64(_native, topic.ToNative(), ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new DataWriter(native);
            }
            else
            {
                DataWriterQosWrapper qos = new DataWriterQosWrapper();
                IntPtr native = CreateDataWriter86(_native, topic.ToNative(), ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new DataWriter(native);
            }
        }
        #endregion

        #region PInvoke
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "Publisher_CreateDataWriter", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateDataWriter64(IntPtr pub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] ref DataWriterQosWrapper qos, IntPtr a_listener, uint mask);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "Publisher_CreateDataWriter", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateDataWriter86(IntPtr pub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] ref DataWriterQosWrapper qos, IntPtr a_listener, uint mask);
        #endregion
    }
}
