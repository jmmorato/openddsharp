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
        /// <summary>
        /// Creates a new <see cref="DataWriter" /> with the default QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
        /// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
        /// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />, 
        /// the operation will fail and return a <see langword="null"/> result.</para>
        /// </remarks>
        /// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
        /// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
        public DataWriter CreateDataWriter(Topic topic)
        {
            return CreateDataWriter(topic, new DataWriterQos());
        }

        /// <summary>
        /// Creates a new <see cref="DataWriter" /> with the desired QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
        /// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
        /// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />, 
        /// the operation will fail and return a <see langword="null"/> result.</para>
        /// </remarks>
        /// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
        /// <param name="qos">The <see cref="DataWriterQos" /> policies to be used for creating the new <see cref="DataWriter" />.</param>
        /// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
        public DataWriter CreateDataWriter(Topic topic, DataWriterQos qos)
        {
            if (topic is null)
            {
                throw new ArgumentNullException(nameof(topic));
            }

            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

            DataWriterQosWrapper qosWrapper = qos.ToNative();
            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateDataWriter86(_native, topic.ToNative(), qosWrapper, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreateDataWriter64(_native, topic.ToNative(), qosWrapper, IntPtr.Zero, 0u));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            qos.Release();

            return new DataWriter(native);
        }

        /// <summary>
        /// Gets the <see cref="Publisher" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="PublisherQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetQos(PublisherQos qos)
        {
            if (qos == null)
            {
                return ReturnCode.BadParameter;
            }

            PublisherQosWrapper qosWrapper = default;
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetQos86(_native, ref qosWrapper),
                                                  () => UnsafeNativeMethods.GetQos64(_native, ref qosWrapper));

            if (ret == ReturnCode.Ok)
            {
                qos.FromNative(qosWrapper);
            }

            qos.Release();

            return ret;
        }

        /// <summary>
        /// Sets the <see cref="Publisher" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="PublisherQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetQos(PublisherQos qos)
        {
            if (qos == null)
            {
                return ReturnCode.BadParameter;
            }

            var qosNative = qos.ToNative();

            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetQos86(_native, qosNative),
                                                  () => UnsafeNativeMethods.SetQos64(_native, qosNative));

            qos.Release();

            return ret;
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
            public static extern IntPtr CreateDataWriter64(IntPtr pub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_CreateDataWriter", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateDataWriter86(IntPtr pub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] PublisherQosWrapper qos);
        }
        #endregion
    }
}
