﻿/*********************************************************************
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// A DataReader allows the application to declare the data it wishes to receive (i.e., make a subscription) and to access the
    /// data received by the attached <see cref="Subscriber" />.
    /// </summary>
    /// <remarks>
    /// <para>A DataReader refers to exactly one <see cref="ITopicDescription" /> (either a <see cref="Topic" />, a <see cref="ContentFilteredTopic" />, or a <see cref="MultiTopic" />)
    /// that identifies the data to be read. The subscription has a unique resulting type. The data-reader may give access to several instances of the
    /// resulting type, which can be distinguished from each other by their key.</para>
    /// <para>All operations except for the operations <see cref="SetQos" />, <see cref="GetQos" />, SetListener,
    /// <see cref="GetListener" />, <see cref="Entity.Enable" />, and <see cref="Entity.StatusCondition" />
    /// return the value <see cref="ReturnCode.NotEnabled" /> if the DataReader has not been enabled yet.</para>
    /// </remarks>
    public class DataReader : Entity
    {
        #region Fields
        private readonly IntPtr _native;
        private readonly ICollection<ReadCondition> _conditions;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs.
        /// </summary>
        public Subscriber Subscriber => GetSubscriber();

        /// <summary>
        /// Gets the <see cref="ITopicDescription" /> associated with the <see cref="DataReader" />.
        /// This is the same <see cref="ITopicDescription" /> that was used to create the <see cref="DataReader" />.
        /// </summary>
        public ITopicDescription TopicDescription => GetTopicDescription();

        /// <summary>
        /// Gets the attached <see cref="DataReaderListener"/>.
        /// </summary>
        [SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "Keep coherency with the setter method and DDS API.")]
        public DataReaderListener Listener { get; internal set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataReader"/> class.
        /// </summary>
        /// <param name="native">The native pointer.</param>
        protected internal DataReader(IntPtr native) : base(NarrowBase(native))
        {
            _native = native;
            _conditions = new List<ReadCondition>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a <see cref="ReadCondition" /> to read samples with any sample states, any view states and any instance states.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="ReadCondition" /> will be attached and belong to the <see cref="DataReader" />.
        /// </remarks>
        /// <returns>The newly created <see cref="ReadCondition" /> on success, otherwise <see langword="null"/>.</returns>
        public ReadCondition CreateReadCondition()
        {
            return CreateReadCondition(SampleStateMask.AnySampleState, ViewStateMask.AnyViewState, InstanceStateMask.AnyInstanceState);
        }

        /// <summary>
        /// Creates a <see cref="ReadCondition" /> to read samples with the desired sample states, view states and instance states.
        /// </summary>
        /// <param name="sampleStates">The desired sample states mask.</param>
        /// <param name="viewStates">The desired view states mask.</param>
        /// <param name="instanceStates">The desired instance states mask.</param>
        /// <returns>The newly created <see cref="ReadCondition" /> on success, otherwise <see langword="null"/>.</returns>
        public ReadCondition CreateReadCondition(SampleStateMask sampleStates, ViewStateMask viewStates, InstanceStateMask instanceStates)
        {
            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateReadCondition86(_native, sampleStates, viewStates, instanceStates),
                                                        () => UnsafeNativeMethods.CreateReadCondition64(_native, sampleStates, viewStates, instanceStates));

            ReadCondition readCondition = null;
            if (native != IntPtr.Zero)
            {
                readCondition = new ReadCondition(native, this);
                _conditions.Add(readCondition);
            }

            return readCondition;
        }

        /// <summary>
        /// Creates a <see cref="QueryCondition" /> to read samples with any sample states, any view states and any instance states.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="QueryCondition" /> will be attached and belong to the <see cref="DataReader" />.
        /// </remarks>
        /// <param name="queryExpression">The query string, which must be a subset of the SQL query language.</param>
        /// <param name="queryParameters">A sequence of strings which are the parameter values used in the SQL query string. The number of values in queryParameters must be equal or greater than the highest referenced n token in the queryExpression.</param>
        /// <returns>The newly created <see cref="QueryCondition" /> on success, otherwise <see langword="null"/>.</returns>
        public QueryCondition CreateQueryCondition(string queryExpression, params string[] queryParameters)
        {
            return CreateQueryCondition(SampleStateMask.AnySampleState, ViewStateMask.AnyViewState, InstanceStateMask.AnyInstanceState, queryExpression, queryParameters);
        }

        /// <summary>
        /// Creates a <see cref="QueryCondition" /> with the desired sample states, view states and instance states.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="QueryCondition" /> will be attached and belong to the <see cref="DataReader" />.
        /// </remarks>
        /// <param name="sampleStates">The desired sample states mask.</param>
        /// <param name="viewStates">The desired view states mask.</param>
        /// <param name="instanceStates">The desired instance states mask.</param>
        /// <param name="queryExpression">The query string, which must be a subset of the SQL query language.</param>
        /// <param name="queryParameters">A sequence of strings which are the parameter values used in the SQL query string. The number of values in queryParameters must be equal or greater than the highest referenced n token in the queryExpression.</param>
        /// <returns>The newly created <see cref="QueryCondition" /> on success, otherwise <see langword="null"/>.</returns>
        public QueryCondition CreateQueryCondition(SampleStateMask sampleStates, ViewStateMask viewStates, InstanceStateMask instanceStates, string queryExpression, params string[] queryParameters)
        {
            IntPtr seq = IntPtr.Zero;
            IList<string> parameters = queryParameters.ToList();
            parameters.StringSequenceToPtr(ref seq, false);

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateQueryCondition86(_native, sampleStates, viewStates, instanceStates, queryExpression, seq),
                                                        () => UnsafeNativeMethods.CreateQueryCondition64(_native, sampleStates, viewStates, instanceStates, queryExpression, seq));

            QueryCondition queryCondition = null;
            if (native != IntPtr.Zero)
            {
                queryCondition = new QueryCondition(native, this);
                _conditions.Add(queryCondition);
            }

            return queryCondition;
        }

        /// <summary>
        /// Deletes a <see cref="ReadCondition" /> attached to the <see cref="DataReader" />. Since <see cref="QueryCondition" /> specializes <see cref="ReadCondition" /> it can
        /// also be used to delete a <see cref="QueryCondition" />.
        /// </summary>
        /// <remarks>
        /// If the <see cref="ReadCondition" /> is not attached to the <see cref="DataReader" />, the operation will return the error <see cref="ReturnCode.PreconditionNotMet" />.
        /// </remarks>
        /// <param name="condition">The <see cref="ReadCondition" /> to be deleted.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteReadCondition(ReadCondition condition)
        {
            if (condition == null)
            {
                return ReturnCode.Ok;
            }

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeleteReadCondition86(_native, condition.ToNative()),
                                                         () => UnsafeNativeMethods.DeleteReadCondition64(_native, condition.ToNative()));

            if (ret == ReturnCode.Ok)
            {
                _conditions.Remove(condition);
            }

            return ret;
        }

        /// <summary>
        /// Deletes all the entities that were created by means of the "create" operations on the <see cref="DataReader" />. That is, it
        /// deletes all contained <see cref="ReadCondition" /> and <see cref="QueryCondition" /> objects.
        /// </summary>
        /// <remarks>
        /// <para>The operation will return <see cref="ReturnCode.PreconditionNotMet" /> if the any of the contained entities is in a state where it cannot be deleted.</para>
        /// <para>Once DeleteContainedEntities returns successfully, the application may delete the <see cref="DataReader" /> knowing that it has no
        /// contained <see cref="ReadCondition" /> and <see cref="QueryCondition" /> objects.</para>
        /// </remarks>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteContainedEntities()
        {
            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeleteContainedEntities86(_native),
                                                         () => UnsafeNativeMethods.DeleteContainedEntities64(_native));

            if (ret == ReturnCode.Ok)
            {
                ClearContainedEntities();
            }

            return ret;
        }

        /// <summary>
        /// Gets the <see cref="DataReader" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="DataReaderQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetQos(DataReaderQos qos)
        {
            if (qos == null)
            {
                return ReturnCode.BadParameter;
            }

            DataReaderQosWrapper qosWrapper = default;
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
        /// Sets the <see cref="DataReader" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="DataReaderQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetQos(DataReaderQos qos)
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
        /// Allows access to the attached <see cref="DataReaderListener" />.
        /// </summary>
        /// <returns>The attached <see cref="DataReaderListener" />.</returns>
        [Obsolete(nameof(GetListener) + " is deprecated, please use Listener property instead.")]
        [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Kept to match OpenDDS API, but will be removed soon.")]
        public DataReaderListener GetListener()
        {
            return Listener;
        }

        /// <summary>
        /// Sets the <see cref="DataReaderListener" /> using the <see cref="StatusMask.DefaultStatusMask" />.
        /// </summary>
        /// <param name="listener">The <see cref="DataReaderListener" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetListener(DataReaderListener listener)
        {
            return SetListener(listener, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Sets the <see cref="DataReaderListener" />.
        /// </summary>
        /// <param name="listener">The <see cref="DataReaderListener" /> to be set.</param>
        /// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetListener(DataReaderListener listener, StatusMask mask)
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

        /// <summary>
        /// Allows access to the <see cref="SampleRejectedStatus" /> communication status.
        /// </summary>
        /// <param name="status">The <see cref="SampleRejectedStatus" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetSampleRejectedStatus(ref SampleRejectedStatus status)
        {
            SampleRejectedStatus s = default;

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetSampleRejectedStatus86(_native, ref s),
                                                         () => UnsafeNativeMethods.GetSampleRejectedStatus64(_native, ref s));
            status = s;

            return ret;
        }

        /// <summary>
        /// Allows access to the <see cref="LivelinessChangedStatus" /> communication status.
        /// </summary>
        /// <param name="status">The <see cref="LivelinessChangedStatus" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetLivelinessChangedStatus(ref LivelinessChangedStatus status)
        {
            LivelinessChangedStatus s = default;

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetLivelinessChangedStatus86(_native, ref s),
                                                         () => UnsafeNativeMethods.GetLivelinessChangedStatus64(_native, ref s));
            status = s;

            return ret;
        }

        /// <summary>
        /// Allows access to the <see cref="RequestedDeadlineMissedStatus" /> communication status.
        /// </summary>
        /// <param name="status">The <see cref="RequestedDeadlineMissedStatus" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetRequestedDeadlineMissedStatus(ref RequestedDeadlineMissedStatus status)
        {
            RequestedDeadlineMissedStatus s = default;

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetRequestedDeadlineMissedStatus86(_native, ref s),
                                                         () => UnsafeNativeMethods.GetRequestedDeadlineMissedStatus64(_native, ref s));
            status = s;

            return ret;
        }

        /// <summary>
        /// Allows access to the <see cref="RequestedIncompatibleQosStatus" /> communication status.
        /// </summary>
        /// <param name="status">The <see cref="RequestedIncompatibleQosStatus" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetRequestedIncompatibleQosStatus(ref RequestedIncompatibleQosStatus status)
        {
            RequestedIncompatibleQosStatusWrapper s = default;

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetRequestedIncompatibleQosStatus86(_native, ref s),
                                                         () => UnsafeNativeMethods.GetRequestedIncompatibleQosStatus64(_native, ref s));
            status.FromNative(s);

            return ret;
        }

        /// <summary>
        /// Allows access to the <see cref="SubscriptionMatchedStatus" /> communication status.
        /// </summary>
        /// <param name="status">The <see cref="SubscriptionMatchedStatus" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetSubscriptionMatchedStatus(ref SubscriptionMatchedStatus status)
        {
            SubscriptionMatchedStatus s = default;

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetSubscriptionMatchedStatus86(_native, ref s),
                                                         () => UnsafeNativeMethods.GetSubscriptionMatchedStatus64(_native, ref s));
            status = s;

            return ret;
        }

        /// <summary>
        /// Allows access to the <see cref="SampleLostStatus" /> communication status.
        /// </summary>
        /// <param name="status">The <see cref="SampleLostStatus" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetSampleLostStatus(ref SampleLostStatus status)
        {
            SampleLostStatus s = default;

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetSampleLostStatus86(_native, ref s),
                                                         () => UnsafeNativeMethods.GetSampleLostStatus64(_native, ref s));
            status = s;

            return ret;
        }

        /// <summary>
        /// Waits until all "historical" data is received.
        /// This operation is intended only for <see cref="DataReader" /> entities that have a non-Volatile <see cref="DurabilityQosPolicyKind" />.
        /// </summary>
        /// <remarks>
        /// The operation WaitForHistoricalData blocks the calling thread until either all "historical" data is received, or else the
        /// duration specified by the maxWait parameter elapses, whichever happens first. A return value of <see cref="ReturnCode.Ok" /> indicates that all the
        /// "historical" data was received; a return value of <see cref="ReturnCode.Timeout" /> indicates that maxWait elapsed before all the data was received.
        /// </remarks>
        /// <param name="maxWait">The maximum <see cref="Duration" /> time to wait for the historical data.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode WaitForHistoricalData(Duration maxWait)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.WaitForHistoricalData86(_native, maxWait),
                                               () => UnsafeNativeMethods.WaitForHistoricalData64(_native, maxWait));
        }

        /// <summary>
        /// Gets the list of publications currently "associated" with the <see cref="DataReader" />; that is, publications that have a
        /// matching <see cref="Topic" /> and compatible QoS that the application has not indicated should be "ignored" by means of the
        /// <see cref="DomainParticipant" /> IgnorePublication operation.
        /// </summary>
        /// <remarks>
        /// The handles returned in the 'publicationHandles' collection are the ones that are used by the DDS implementation to locally identify
        /// the corresponding matched <see cref="DataWriter" /> entities. These handles match the ones that appear in the <see cref="SampleInfo.InstanceHandle" /> property of the
        /// <see cref="SampleInfo" /> when reading the "DCPSPublications" builtin topic.
        /// </remarks>
        /// <param name="publicationHandles">The collection of publication <see cref="InstanceHandle" />s to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetMatchedPublications(ICollection<InstanceHandle> publicationHandles)
        {
            if (publicationHandles == null)
            {
                return ReturnCode.BadParameter;
            }

            IntPtr ptr = IntPtr.Zero;
            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetMatchedPublications86(_native, ref ptr),
                                                         () => UnsafeNativeMethods.GetMatchedPublications64(_native, ref ptr));

            if (ret == ReturnCode.Ok && ptr != IntPtr.Zero)
            {
                ptr.PtrToSequence(ref publicationHandles);
                ptr.ReleaseNativePointer();
            }

            return ret;
        }

        /// <summary>
        /// This operation retrieves information on a publication that is currently "associated" with the <see cref="DataReader" />; that is, a publication
        /// with a matching <see cref="Topic" /> and compatible QoS that the application has not indicated should be "ignored" by means of the
        /// <see cref="DomainParticipant" /> IgnorePublication operation.
        /// </summary>
        /// <remarks>
        /// The publicationHandle must correspond to a publication currently associated with the <see cref="DataReader" /> otherwise the operation
        /// will fail and return <see cref="ReturnCode.BadParameter" />. The operation GetMatchedPublications can be used to find the publications that
        /// are currently matched with the <see cref="DataReader" />.
        /// </remarks>
        /// <param name="publicationHandle">The <see cref="InstanceHandle" /> of the publication data requested.</param>
        /// <param name="publicationData">The <see cref="PublicationBuiltinTopicData" /> structure to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetMatchedPublicationData(InstanceHandle publicationHandle, ref PublicationBuiltinTopicData publicationData)
        {
            PublicationBuiltinTopicDataWrapper data = default;

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetMatchedPublicationData86(_native, ref data, publicationHandle),
                                                         () => UnsafeNativeMethods.GetMatchedPublicationData64(_native, ref data, publicationHandle));

            if (ret == ReturnCode.Ok)
            {
                publicationData.FromNative(data);
            }

            return ret;
        }

        internal static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
                                               () => UnsafeNativeMethods.NarrowBase64(ptr));
        }

        internal override void ClearContainedEntities()
        {
            foreach (ReadCondition c in _conditions)
            {
                c.Release();
            }

            _conditions.Clear();
        }

        private Subscriber GetSubscriber()
        {
            IntPtr ptrSubscriber = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetSubscriber86(_native),
                                                               () => UnsafeNativeMethods.GetSubscriber64(_native));

            Subscriber subscriber = null;

            if (!ptrSubscriber.Equals(IntPtr.Zero))
            {
                IntPtr ptr = Publisher.NarrowBase(ptrSubscriber);
                Entity entity = EntityManager.Instance.Find(ptr);
                if (entity != null)
                {
                    subscriber = (Subscriber)entity;
                }
                else
                {
                    subscriber = new Subscriber(ptrSubscriber);
                    EntityManager.Instance.Add((subscriber as Entity).ToNative(), subscriber);
                }
            }

            return subscriber;
        }

        private ITopicDescription GetTopicDescription()
        {
            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetTopicDescription86(_native),
                                                        () => UnsafeNativeMethods.GetTopicDescription64(_native));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return (ITopicDescription)EntityManager.Instance.Find(native);
        }

        /// <summary>
        /// Internal use only.
        /// </summary>
        /// <returns>The native pointer.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new IntPtr ToNative()
        {
            return _native;
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetMatchedPublications", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetMatchedPublications64(IntPtr dr, ref IntPtr publicationHandles);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetMatchedPublications", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetMatchedPublications86(IntPtr dr, ref IntPtr publicationHandles);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_WaitForHistoricalData", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode WaitForHistoricalData64(IntPtr dr, Duration maxWait);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_WaitForHistoricalData", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode WaitForHistoricalData86(IntPtr dr, Duration maxWait);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataReaderQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataReaderQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] DataReaderQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] DataReaderQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_SetListener", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetListener64(IntPtr dr, IntPtr listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_SetListener", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetListener86(IntPtr dr, IntPtr listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetSubscriber", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetSubscriber64(IntPtr dr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetSubscriber", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetSubscriber86(IntPtr dr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetTopicDescription", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetTopicDescription64(IntPtr dr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetTopicDescription", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetTopicDescription86(IntPtr dr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_CreateReadCondition", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateReadCondition64(IntPtr dr, uint sampleMask, uint viewMask, uint instanceMask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_CreateReadCondition", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateReadCondition86(IntPtr dr, uint sampleMask, uint viewMask, uint instanceMask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_CreateQueryCondition", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr CreateQueryCondition64(IntPtr dr, uint sampleMask, uint viewMask, uint instanceMask, string expr, IntPtr parameters);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_CreateQueryCondition", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr CreateQueryCondition86(IntPtr dr, uint sampleMask, uint viewMask, uint instanceMask, string expr, IntPtr parameters);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_DeleteReadCondition", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteReadCondition64(IntPtr dr, IntPtr rc);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_DeleteReadCondition", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteReadCondition86(IntPtr dr, IntPtr rc);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_DeleteContainedEntities", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteContainedEntities64(IntPtr dr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_DeleteContainedEntities", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteContainedEntities86(IntPtr dr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetSampleRejectedStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetSampleRejectedStatus64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SampleRejectedStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetSampleRejectedStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetSampleRejectedStatus86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SampleRejectedStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetLivelinessChangedStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetLivelinessChangedStatus64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref LivelinessChangedStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetLivelinessChangedStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetLivelinessChangedStatus86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref LivelinessChangedStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetRequestedDeadlineMissedStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetRequestedDeadlineMissedStatus64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref RequestedDeadlineMissedStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetRequestedDeadlineMissedStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetRequestedDeadlineMissedStatus86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref RequestedDeadlineMissedStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetRequestedIncompatibleQosStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetRequestedIncompatibleQosStatus64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref RequestedIncompatibleQosStatusWrapper status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetRequestedIncompatibleQosStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetRequestedIncompatibleQosStatus86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref RequestedIncompatibleQosStatusWrapper status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetSubscriptionMatchedStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetSubscriptionMatchedStatus64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriptionMatchedStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetSubscriptionMatchedStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetSubscriptionMatchedStatus86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriptionMatchedStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetSampleLostStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetSampleLostStatus64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SampleLostStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetSampleLostStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetSampleLostStatus86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SampleLostStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetMatchedPublicationData", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetMatchedPublicationData64(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublicationBuiltinTopicDataWrapper data, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetMatchedPublicationData", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetMatchedPublicationData86(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublicationBuiltinTopicDataWrapper data, int handle);
        }
        #endregion
    }
}
