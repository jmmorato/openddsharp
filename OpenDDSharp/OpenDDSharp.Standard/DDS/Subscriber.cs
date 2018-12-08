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
    public class Subscriber
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        internal Subscriber(IntPtr native)
        {
            _native = native;
        }
        #endregion

        #region Methods
        public DataReader CreateDataReader(Topic topic)
        {
            if (Environment.Is64BitProcess)
            {
                DataReaderQosWrapper qos = new DataReaderQosWrapper();
                IntPtr native = CreateDataReader64(_native, topic.ToNative(), ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new DataReader(native);
            }
            else
            {
                DataReaderQosWrapper qos = new DataReaderQosWrapper();
                IntPtr native = CreateDataReader86(_native, topic.ToNative(), ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new DataReader(native);
            }
        }
        #endregion

        #region PInvoke
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "Subscriber_CreateDataReader", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateDataReader64(IntPtr sub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] ref DataReaderQosWrapper qos, IntPtr a_listener, uint mask);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "Subscriber_CreateDataReader", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateDataReader86(IntPtr sub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] ref DataReaderQosWrapper qos, IntPtr a_listener, uint mask);
        #endregion
    }
}
