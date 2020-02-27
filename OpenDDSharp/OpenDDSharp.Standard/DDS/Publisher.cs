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
    /// A Publisher is the object responsible for the actual dissemination of publications.
    /// </summary>
    /// <remarks>
    /// <para>The Publisher acts on the behalf of one or several <see cref="DataWriter" /> objects that belong to it. When it is informed of a change to the
    /// data associated with one of its <see cref="DataWriter" /> objects, it decides when it is appropriate to actually send the data-update message.
    /// In making this decision, it considers any extra information that goes with the data(timestamp, writer, etc.) as well as the QoS
    /// of the Publisher and the <see cref="DataWriter" />.</para>
    /// <para>All operations except for the operations SetQos, GetQos, SetListener, GetListener, Enable, GetStatusCondition,
    /// CreateDataWriter, and DeleteDataWriter return the value <see cref="ReturnCode.NotEnabled" /> if the Publisher has not been enabled yet.</para>
    /// </remarks>
    public class Publisher : Entity
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        internal Publisher(IntPtr native) : base(NarrowBase(native))
        {
            _native = native;
        }
        #endregion

        #region Methods
        public DataWriter CreateDataWriter(Topic topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic));
            }

            DataWriterQosWrapper qos = default(DataWriterQosWrapper);
            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateDataWriter86(_native, topic.ToNative(), ref qos, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreateDataWriter64(_native, topic.ToNative(), ref qos, IntPtr.Zero, 0u));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new DataWriter(native);
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_CreateDataWriter", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateDataWriter64(IntPtr pub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] ref DataWriterQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_CreateDataWriter", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateDataWriter86(IntPtr pub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] ref DataWriterQosWrapper qos, IntPtr a_listener, uint mask);
        }
        #endregion
    }
}
