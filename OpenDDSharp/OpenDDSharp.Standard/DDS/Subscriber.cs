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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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

        #region Properties
        /// <summary>
        /// Gets the attached <see cref="SubscriberListener"/>.
        /// </summary>
        [SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "Keep coherency with the setter method and DDS API.")]
        public SubscriberListener Listener { get; internal set; }
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
        public DataReader CreateDataReader(ITopicDescription topicDescription)
        {
            return CreateDataReader(topicDescription, null);
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
        public DataReader CreateDataReader(ITopicDescription topicDescription, DataReaderQos qos)
        {
            return CreateDataReader(topicDescription, qos, null, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="DataReader" /> with the desired QoS policies and attaches to it the specified <see cref="DataReaderListener" />.
        /// The specified <see cref="DataReaderListener" /> will be attached with the default <see cref="StatusMask" />.
        /// </summary>
        /// <param name="topicDescription">The <see cref="ITopicDescription" /> that the <see cref="DataReader" /> will be associated with.</param>
        /// <param name="qos">The <see cref="DataReaderQos" /> policies to be used for creating the new <see cref="DataReader" />.</param>
        /// <param name="listener">The <see cref="DataReaderListener" /> to be attached to the newly created <see cref="DataReader" />.</param>
        /// <returns>The newly created <see cref="DataReader" /> on success, otherwise <see langword="null"/>.</returns>
        public DataReader CreateDataReader(ITopicDescription topicDescription, DataReaderQos qos, DataReaderListener listener)
        {
            return CreateDataReader(topicDescription, qos, listener, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="DataReader" /> with the desired QoS policies and attaches to it the specified <see cref="DataReaderListener" />.
        /// </summary>
        /// <remarks>
        /// <para>The returned <see cref="DataReader" /> will be attached and belong to the <see cref="Subscriber" />.</para>
        /// <para>The <see cref="ITopicDescription"/> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to
        /// create this <see cref="Subscriber" />. If the <see cref="ITopicDescription"/> was created from a different <see cref="DomainParticipant" />, the operation will fail and
        /// return a <see langword="null"/> result.</para>
        /// </remarks>
        /// <param name="topicDescription">The <see cref="ITopicDescription" /> that the <see cref="DataReader" /> will be associated with.</param>
        /// <param name="qos">The <see cref="DataReaderQos" /> policies to be used for creating the new <see cref="DataReader" />.</param>
        /// <param name="listener">The <see cref="DataReaderListener" /> to be attached to the newly created <see cref="DataReader" />.</param>
        /// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
        /// <returns>The newly created <see cref="DataReader" /> on success, otherwise <see langword="null"/>.</returns>
        public DataReader CreateDataReader(ITopicDescription topicDescription, DataReaderQos qos, DataReaderListener listener, StatusMask statusMask)
        {
            if (topicDescription is null)
            {
                throw new ArgumentNullException(nameof(topicDescription));
            }

            DataReaderQosWrapper qosWrapper = default;
            if (qos is null)
            {
                qos = new DataReaderQos();
                var ret = GetDefaultDataReaderQos(qos);
                if (ret == ReturnCode.Ok)
                {
                    qosWrapper = qos.ToNative();
                }
            }
            else
            {
                qosWrapper = qos.ToNative();
            }

            IntPtr nativeListener = IntPtr.Zero;
            if (listener != null)
            {
                nativeListener = listener.ToNative();
            }

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateDataReader86(_native, topicDescription.ToNativeTopicDescription(), qosWrapper, nativeListener, statusMask),
                                                        () => UnsafeNativeMethods.CreateDataReader64(_native, topicDescription.ToNativeTopicDescription(), qosWrapper, nativeListener, statusMask));

            qos.Release();

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            var dr = new DataReader(native)
            {
                Listener = listener,
            };

            EntityManager.Instance.Add((dr as Entity).ToNative(), dr);
            ContainedEntities.Add(dr);

            return dr;
        }

        /// <summary>
        /// Gets the default value of the <see cref="DataReader" /> QoS, that is, the QoS policies which will be used for newly
        /// created <see cref="DataReader" /> entities in the case where the QoS policies are defaulted in the CreateDataReader operation.
        /// </summary>
        /// <remarks>
        /// The values retrieved GetDefaultDataReaderQos will match the set of values specified on the last successful call to
        /// <see cref="SetDefaultDataReaderQos" />, or else, if the call was never made, the default DDS standard values.
        /// </remarks>
        /// <param name="qos">The <see cref="DataReaderQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetDefaultDataReaderQos(DataReaderQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            DataReaderQosWrapper qosWrapper = default;
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDefaultDataReaderQos86(_native, ref qosWrapper),
                                                  () => UnsafeNativeMethods.GetDefaultDataReaderQos64(_native, ref qosWrapper));

            if (ret == ReturnCode.Ok)
            {
                qos.FromNative(qosWrapper);
            }

            qos.Release();

            return ret;
        }

        /// <summary>
        /// Sets a default value of the <see cref="DataReader" /> QoS policies which will be used for newly created <see cref="DataReader" /> entities
        /// in the case where the QoS policies are defaulted in the CreateDataReader operation.
        /// </summary>
        /// <remarks>
        /// This operation will check that the resulting policies are self consistent; if they are not, the operation will have no effect and
        /// return <see cref="ReturnCode.InconsistentPolicy" />.
        /// </remarks>
        /// <param name="qos">The default <see cref="DataReaderQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetDefaultDataReaderQos(DataReaderQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            var qosNative = qos.ToNative();
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetDefaultDataReaderQos86(_native, qosNative),
                                                  () => UnsafeNativeMethods.SetDefaultDataReaderQos64(_native, qosNative));
            qos.Release();

            return ret;
        }

        /// <summary>
        /// Deletes a <see cref="DataReader" /> that belongs to the <see cref="Subscriber" />.
        /// </summary>
        /// <remarks>
        /// <para>If the <see cref="DataReader" /> does not belong to the <see cref="Subscriber" />, the operation returns the error <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// <para>The deletion of a <see cref="DataReader" /> is not allowed if there are any existing <see cref="ReadCondition" /> or <see cref="QueryCondition" /> objects that are
        /// attached to the <see cref="DataReader" />. If the DeleteDataReader operation is called on a <see cref="DataReader" /> with any of these existing objects
        /// attached to it, it will return <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// <para>The DeleteDataReader operation must be called on the same <see cref="Subscriber" /> object used to create the <see cref="DataReader" />. If
        /// DeleteDataReader is called on a different <see cref="Subscriber" />, the operation will have no effect and it will return
        /// <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// </remarks>
        /// <param name="dataReader">The <see cref="DataReader" /> to be deleted.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteDataReader(DataReader dataReader)
        {
            if (dataReader == null)
            {
                return ReturnCode.Ok;
            }

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeleteDataReader86(_native, dataReader.ToNative()),
                                                         () => UnsafeNativeMethods.DeleteDataReader64(_native, dataReader.ToNative()));
            if (ret == ReturnCode.Ok)
            {
                EntityManager.Instance.Remove((dataReader as Entity).ToNative());
                ContainedEntities.Remove(dataReader);
            }

            return ret;
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

            qos.Release();

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

            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetQos86(_native, qosNative),
                                                  () => UnsafeNativeMethods.SetQos64(_native, qosNative));

            qos.Release();

            return ret;
        }

        /// <summary>
        /// Allows access to the attached <see cref="SubscriberListener" />.
        /// </summary>
        /// <returns>The attached <see cref="SubscriberListener" />.</returns>
        [Obsolete(nameof(GetListener) + " is deprecated, please use Listener property instead.")]
        [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Keep coherency with the setter method and DDS API.")]
        public SubscriberListener GetListener()
        {
            return Listener;
        }

        /// <summary>
        /// Sets the <see cref="SubscriberListener" /> using the <see cref="StatusMask.DefaultStatusMask" />.
        /// </summary>
        /// <param name="listener">The <see cref="SubscriberListener" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetListener(SubscriberListener listener)
        {
            return SetListener(listener, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Sets the <see cref="SubscriberListener" />.
        /// </summary>
        /// <param name="listener">The <see cref="SubscriberListener" /> to be set.</param>
        /// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetListener(SubscriberListener listener, StatusMask mask)
        {
            Listener = listener;
            IntPtr ptr = IntPtr.Zero;
            if (listener != null)
            {
                ptr = listener.ToNative();
            }

            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetListener86(_native, ptr, mask),
                                               () => UnsafeNativeMethods.SetListener64(_native, ptr, mask));
        }

        private static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
                                               () => UnsafeNativeMethods.NarrowBase64(ptr));
        }

        /// <summary>
        /// Internal use only.
        /// </summary>
        /// <returns>The native pointer.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal new IntPtr ToNative()
        {
            return _native;
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Subscriber_GetDefaultDataReaderQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultDataReaderQos64(IntPtr sub, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataReaderQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_GetDefaultDataReaderQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultDataReaderQos86(IntPtr sub, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataReaderQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Subscriber_SetDefaultDataReaderQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultDataReaderQos64(IntPtr sub, [MarshalAs(UnmanagedType.Struct), In] DataReaderQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_SetDefaultDataReaderQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultDataReaderQos86(IntPtr sub, [MarshalAs(UnmanagedType.Struct), In] DataReaderQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Subscriber_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos64(IntPtr sub, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos86(IntPtr sub, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Subscriber_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos64(IntPtr sub, [MarshalAs(UnmanagedType.Struct), In] SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos86(IntPtr sub, [MarshalAs(UnmanagedType.Struct), In] SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Subscriber_SetListener", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetListener64(IntPtr sub, IntPtr listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_SetListener", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetListener86(IntPtr sub, IntPtr listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Subscriber_DeleteDataReader", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern ReturnCode DeleteDataReader64(IntPtr s, IntPtr dr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Subscriber_DeleteDataReader", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern ReturnCode DeleteDataReader86(IntPtr s, IntPtr dr);
        }
        #endregion
    }
}
