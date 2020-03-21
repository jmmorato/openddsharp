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
        /// <summary>
        /// Creates a new <see cref="DataReader" /> with the default QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The returned <see cref="DataReader" /> will be attached and belong to the <see cref="Subscriber" />.</para>
        /// <para>The <see cref="ITopicDescription"/> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to
        /// create this <see cref="Subscriber" />. If the <see cref="ITopicDescription"/> was created from a different <see cref="DomainParticipant" />, the operation will fail and
        /// return a <see langword="null"/> result.</para>
        /// </remarks>
        /// <param name="topicDescription">The <see cref="ITopicDescription" /> that the <see cref="DataReader" /> will be associated with.</param>
        /// <returns>The newly created <see cref="DataReader" /> on success, otherwise <see langword="null"/>.</returns>
        // TODO: Change ITopicDescription when implemented in Topic class.
        public DataReader CreateDataReader(Topic topicDescription)
        {
            return CreateDataReader(topicDescription, new DataReaderQos());
        }

        /// <summary>
        /// Creates a new <see cref="DataReader" /> with the desired QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The returned <see cref="DataReader" /> will be attached and belong to the <see cref="Subscriber" />.</para>
        /// <para>The <see cref="ITopicDescription"/> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to
        /// create this <see cref="Subscriber" />. If the <see cref="ITopicDescription"/> was created from a different <see cref="DomainParticipant" />, the operation will fail and
        /// return a <see langword="null"/> result.</para>
        /// </remarks>
        /// <param name="topicDescription">The <see cref="ITopicDescription" /> that the <see cref="DataReader" /> will be associated with.</param>
        /// <param name="qos">The <see cref="DataReaderQos" /> policies to be used for creating the new <see cref="DataReader" />.</param>
        /// <returns>The newly created <see cref="DataReader" /> on success, otherwise <see langword="null"/>.</returns>
        // TODO: Change ITopicDescription when implemented in Topic class.
        public DataReader CreateDataReader(Topic topicDescription, DataReaderQos qos)
        {
            if (topicDescription is null)
            {
                throw new ArgumentNullException(nameof(topicDescription));
            }

            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

            DataReaderQosWrapper qosWrapper = qos.ToNative();
            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateDataReader86(_native, topicDescription.ToNative(), qosWrapper, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreateDataReader64(_native, topicDescription.ToNative(), qosWrapper, IntPtr.Zero, 0u));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new DataReader(native);
        }

        /// <summary>
        /// Gets the <see cref="Subscriber" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="SubscriberQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetQos(SubscriberQos qos)
        {
            if (qos == null)
            {
                return ReturnCode.BadParameter;
            }

            SubscriberQosWrapper qosWrapper = default;
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetQos86(_native, ref qosWrapper),
                                                  () => UnsafeNativeMethods.GetQos64(_native, ref qosWrapper));

            if (ret == ReturnCode.Ok)
            {
                qos.FromNative(qosWrapper);
            }

            return ret;
        }

        /// <summary>
        /// Sets the <see cref="Subscriber" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="SubscriberQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetQos(SubscriberQos qos)
        {
            if (qos == null)
            {
                return ReturnCode.BadParameter;
            }

            var qosNative = qos.ToNative();

            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetQos86(_native, qosNative),
                                               () => UnsafeNativeMethods.SetQos64(_native, qosNative));

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
            public static extern IntPtr CreateDataReader64(IntPtr sub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] DataReaderQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_CreateDataReader", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateDataReader86(IntPtr sub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] DataReaderQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Subscriber_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Subscriber_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] SubscriberQosWrapper qos);
        }
        #endregion
    }
}
