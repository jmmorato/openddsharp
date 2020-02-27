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
    /// A Subscriber is the object responsible for the actual reception of the data resulting from its subscriptions.
    /// </summary>
    /// <remarks>
    /// <para>A Subscriber acts on the behalf of one or several <see cref="DataReader" /> objects that are related to it. When it receives data (from the
    /// other parts of the system), it builds the list of concerned <see cref="DataReader" /> objects, and then indicates to the application that data is
    /// available, through its listener or by enabling related conditions. The application can access the list of concerned <see cref="DataReader" />
    /// objects through the operation GetDataReaders and then access the data available though operations on the <see cref="DataReader" />.</para>
    /// <para>All operations except for the operations <see cref="SetQos" />, <see cref="GetQos" />, SetListener,
    /// <see cref="GetListener" />, <see cref="Entity.Enable" />, <see cref="Entity.StatusCondition" />, CreateDataReader,
    /// return the value <see cref="ReturnCode.NotEnabled" /> if the Subscriber has not been enabled yet.</para>
    /// </remarks>
    public class Subscriber : Entity
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        internal Subscriber(IntPtr native) : base(NarrowBase(native))
        {
            _native = native;
        }
        #endregion

        #region Methods
        public DataReader CreateDataReader(Topic topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic));
            }

            DataReaderQosWrapper qos = default(DataReaderQosWrapper);
            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateDataReader86(_native, topic.ToNative(), ref qos, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreateDataReader64(_native, topic.ToNative(), ref qos, IntPtr.Zero, 0u));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new DataReader(native);
        }

        private static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
                                               () => UnsafeNativeMethods.NarrowBase64(ptr));
        }
        #endregion

        #region Unsafe Native Methods
        /// <summary>
        /// This class suppresses stack walks for unmanaged code permission. (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
        /// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full security review to make sure that the usage
        /// is secure because no stack walk will be performed.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private static class UnsafeNativeMethods
        {
            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Subscriber_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Subscriber_CreateDataReader", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateDataReader64(IntPtr sub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] ref DataReaderQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_CreateDataReader", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateDataReader86(IntPtr sub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] ref DataReaderQosWrapper qos, IntPtr a_listener, uint mask);
        }
        #endregion
    }
}
