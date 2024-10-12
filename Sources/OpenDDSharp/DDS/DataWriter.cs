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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.DDS;

/// <summary>
/// DataWriter allows the application to set the value of the data to be published under a given <see cref="Topic" />.
/// </summary>
/// <remarks>
/// <para>A DataWriter is attached to exactly one <see cref="Publisher" /> that acts as a factory for it.</para>
/// <para>A DataWriter is bound to exactly one <see cref="Topic" /> and therefore to exactly one data type. The <see cref="Topic" />
/// must exist prior to the DataWriter’s creation.</para>
/// <para>The DataWriter must be specialized for each particular application data-type.</para>
/// <para>All operations except for the operations <see cref="SetQos" />, <see cref="GetQos" />, SetListener,
/// <see cref="DataWriter.GetListener" />, <see cref="Entity.Enable" />, and <see cref="Entity.StatusCondition" />
/// return the value <see cref="ReturnCode.NotEnabled" /> if the DataWriter has not been enabled yet.</para>
/// </remarks>
public class DataWriter : Entity
{
    #region Fields
    private readonly IntPtr _native;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the attached <see cref="DataWriterListener"/>.
    /// </summary>
    [SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "Keep coherency with the setter method and DDS API.")]
    public DataWriterListener Listener { get; internal set; }

    /// <summary>
    /// Gets the <see cref="Topic" /> associated with the <see cref="DataWriter" />.
    /// This is the same <see cref="Topic" /> that was used to create the <see cref="DataWriter" />.
    /// </summary>
    public Topic Topic => GetTopic();

    /// <summary>
    /// Gets the <see cref="Publisher" /> to which the <see cref="DataWriter" /> belongs.
    /// </summary>
    public Publisher Publisher => GetPublisher();
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="DataWriter"/> class.
    /// </summary>
    /// <param name="native">The native pointer.</param>
    protected internal DataWriter(IntPtr native) : base(UnsafeNativeMethods.NarrowBase(native))
    {
        _native = native;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Gets the <see cref="DataWriter" /> QoS policies.
    /// </summary>
    /// <param name="qos">The <see cref="DataWriterQos" /> to be filled up.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetQos(DataWriterQos qos)
    {
        if (qos == null)
        {
            return ReturnCode.BadParameter;
        }

        DataWriterQosWrapper qosWrapper = default;
        var ret = UnsafeNativeMethods.GetQos(_native, ref qosWrapper);

        if (ret == ReturnCode.Ok)
        {
            qos.FromNative(qosWrapper);
        }

        qos.Release();

        return ret;
    }

    /// <summary>
    /// Sets the <see cref="DataWriter" /> QoS policies.
    /// </summary>
    /// <param name="qos">The <see cref="DataWriterQos" /> to be set.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode SetQos(DataWriterQos qos)
    {
        if (qos == null)
        {
            return ReturnCode.BadParameter;
        }

        var qosNative = qos.ToNative();

        var ret = UnsafeNativeMethods.SetQos(_native, qosNative);

        qos.Release();

        return ret;
    }

    /// <summary>
    /// Allows access to the attached <see cref="DataWriterListener" />.
    /// </summary>
    /// <returns>The attached <see cref="DataWriterListener" />.</returns>
    [Obsolete(nameof(GetListener) + " is deprecated, please use Listener property instead.")]
    [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Keep coherency with the setter method and DDS API.")]
    public DataWriterListener GetListener()
    {
        return Listener;
    }

    /// <summary>
    /// Sets the <see cref="DataWriterListener" /> using the <see cref="StatusMask.DefaultStatusMask" />.
    /// </summary>
    /// <param name="listener">The <see cref="DataWriterListener" /> to be set.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode SetListener(DataWriterListener listener)
    {
        return SetListener(listener, StatusMask.DefaultStatusMask);
    }

    /// <summary>
    /// Sets the <see cref="DataWriterListener" />.
    /// </summary>
    /// <param name="listener">The <see cref="DataWriterListener" /> to be set.</param>
    /// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode SetListener(DataWriterListener listener, StatusMask mask)
    {
        Listener = listener;
        IntPtr ptr = IntPtr.Zero;
        if (listener != null)
        {
            ptr = listener.ToNative();
        }

        return UnsafeNativeMethods.DataWriterSetListener(_native, ptr, mask);
    }

    /// <summary>
    /// Blocks the calling thread until either all data written by the <see cref="DataWriter" /> is
    /// acknowledged by all matched <see cref="DataReader" /> entities that have
    /// <see cref="ReliabilityQosPolicyKind.ReliableReliabilityQos" />, or else the duration
    /// specified by the maxWait parameter elapses, whichever happens first.
    /// </summary>
    /// <remarks>
    /// <para>This operation is intended to be used only if the <see cref="DataWriter" /> has
    /// configured <see cref="ReliabilityQosPolicyKind.ReliableReliabilityQos" />.
    /// Otherwise, the operation will return immediately with <see cref="ReturnCode.Ok" />.</para>
    /// <para>A return value of <see cref="ReturnCode.Ok" /> indicates that all the samples
    /// written have been acknowledged by all reliable matched data readers; a return value of
    /// <see cref="ReturnCode.Timeout" /> indicates that maxWait
    /// elapsed before all the data was acknowledged.</para>
    /// </remarks>
    /// <param name="maxWait">The maximum <see cref="Duration" /> time to wait for the acknowledgments.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode WaitForAcknowledgments(Duration maxWait)
    {
        return UnsafeNativeMethods.WaitForAcknowledgments(_native, maxWait);
    }

    /// <summary>
    /// Allows access to the <see cref="LivelinessLostStatus" /> communication status.
    /// </summary>
    /// <param name="status">The <see cref="LivelinessLostStatus" /> to be filled up.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetLivelinessLostStatus(ref LivelinessLostStatus status)
    {
        LivelinessLostStatus s = default;

        var ret = UnsafeNativeMethods.GetLivelinessLostStatus(_native, ref s);
        status = s;

        return ret;
    }

    /// <summary>
    /// Allows access to the <see cref="OfferedDeadlineMissedStatus" /> communication status.
    /// </summary>
    /// <param name="status">The <see cref="OfferedDeadlineMissedStatus" /> to be filled up.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetOfferedDeadlineMissedStatus(ref OfferedDeadlineMissedStatus status)
    {
        OfferedDeadlineMissedStatus s = default;

        var ret = UnsafeNativeMethods.GetOfferedDeadlineMissedStatus(_native, ref s);
        status = s;

        return ret;
    }

    /// <summary>
    /// Allows access to the <see cref="OfferedIncompatibleQosStatus" /> communication status.
    /// </summary>
    /// <param name="status">The <see cref="OfferedIncompatibleQosStatus" /> to be filled up.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetOfferedIncompatibleQosStatus(ref OfferedIncompatibleQosStatus status)
    {
        OfferedIncompatibleQosStatusWrapper s = default;

        var ret = UnsafeNativeMethods.GetOfferedIncompatibleQosStatus(_native, ref s);
        status.FromNative(s);

        return ret;
    }

    /// <summary>
    /// Allows access to the <see cref="PublicationMatchedStatus" /> communication status.
    /// </summary>
    /// <param name="status">The <see cref="PublicationMatchedStatus" /> to be filled up.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetPublicationMatchedStatus(ref PublicationMatchedStatus status)
    {
        PublicationMatchedStatus s = default;

        var ret = UnsafeNativeMethods.GetPublicationMatchedStatus(_native, ref s);
        status = s;

        return ret;
    }

    /// <summary>
    /// Manually asserts the liveliness of the <see cref="DataWriter" />. This is used in combination with the liveliness QoS
    /// policy to indicate to DDS that the entity remains active.
    /// </summary>
    /// <remarks>
    /// <para>This operation need only be used if the <see cref="LivelinessQosPolicy" /> setting is either
    /// <see cref="LivelinessQosPolicyKind.ManualByParticipantLivelinessQos" />
    /// or <see cref="LivelinessQosPolicyKind.ManualByTopicLivelinessQos" />. Otherwise, it has no effect.</para>
    /// <para>NOTE: Writing data via the write operation on a <see cref="DataWriter" /> asserts liveliness on
    /// the <see cref="DataWriter" /> itself and its <see cref="DomainParticipant" />. Consequently, the use of
    /// AssertLiveliness is only needed if the application is not writing data regularly.</para>
    /// </remarks>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode AssertLiveliness()
    {
        return UnsafeNativeMethods.AssertLiveliness(_native);
    }

    /// <summary>
    /// Gets the collection of subscriptions currently "associated" with the <see cref="DataWriter" />; that is,
    /// subscriptions that have a matching <see cref="Topic" /> and compatible QoS that the application has not
    /// indicated should be "ignored" by means of the <see cref="DomainParticipant" /> IgnoreSubscription operation.
    /// </summary>
    /// <remarks>
    /// The handles returned in the 'subscriptionHandles' collection are the ones that are used by the DDS
    /// implementation to locally identify the corresponding matched <see cref="DataReader" /> entities.
    /// These handles match the ones that appear in the <see cref="SampleInfo.InstanceState" /> property of the
    /// <see cref="SampleInfo" /> when reading the "DCPSSubscriptions" builtin topic.
    /// </remarks>
    /// <param name="subscriptionHandles">
    /// The collection of subscription <see cref="InstanceHandle" />s to be filled up.
    /// </param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetMatchedSubscriptions(ICollection<InstanceHandle> subscriptionHandles)
    {
        if (subscriptionHandles == null)
        {
            return ReturnCode.BadParameter;
        }

        subscriptionHandles.Clear();

        IntPtr seq = IntPtr.Zero;
        ReturnCode ret = UnsafeNativeMethods.GetMatchedSubscriptions(_native, ref seq);

        if (ret == ReturnCode.Ok && !seq.Equals(IntPtr.Zero))
        {
            seq.PtrToSequence(ref subscriptionHandles);
            seq.ReleaseNativePointer();
        }

        return ret;
    }

    /// <summary>
    /// Retrieves information on a subscription that is currently "associated" with the <see cref="DataWriter" />;
    /// that is, a subscription with a matching <see cref="Topic" /> and compatible QoS that the application has
    /// not indicated should be "ignored" by means of the <see cref="DomainParticipant" /> IgnoreSubscription
    /// operation.
    /// </summary>
    /// <remarks>
    /// <para>The subscriptionHandle must correspond to a subscription currently associated with the
    /// <see cref="DataWriter" />, otherwise the operation will fail and return
    /// <see cref="ReturnCode.BadParameter" />. The operation GetMatchedSubscriptions can be
    /// used to find the subscriptions that are currently matched with the <see cref="DataWriter" />.</para>
    /// </remarks>
    /// <param name="subscriptionHandle">The <see cref="InstanceHandle" /> of the subscription data requested.</param>
    /// <param name="subscriptionData">The <see cref="SubscriptionBuiltinTopicData" /> structure to be filled up.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetMatchedSubscriptionData(InstanceHandle subscriptionHandle, ref SubscriptionBuiltinTopicData subscriptionData)
    {
        SubscriptionBuiltinTopicDataWrapper data = default;

        ReturnCode ret = UnsafeNativeMethods.GetMatchedSubscriptionData(_native, ref data, subscriptionHandle);

        if (ret == ReturnCode.Ok)
        {
            subscriptionData.FromNative(data);
        }

        return ret;
    }

    private Topic GetTopic()
    {
        var ptrTopic = UnsafeNativeMethods.GetTopic(_native);

        Topic topic = null;

        if (!ptrTopic.Equals(IntPtr.Zero))
        {
            var ptrTopicDescription = Topic.NarrowTopicDescription(ptrTopic);

            var entity = EntityManager.Instance.Find(ptrTopicDescription);
            if (entity != null)
            {
                topic = (Topic)entity;
            }
            else
            {
                topic = new Topic(ptrTopic);
                EntityManager.Instance.Add(topic.ToNativeTopicDescription(), topic);
            }
        }

        return topic;
    }

    private Publisher GetPublisher()
    {
        IntPtr ptrPublisher = UnsafeNativeMethods.GetPublisher(_native);

        Publisher publisher = null;

        if (!ptrPublisher.Equals(IntPtr.Zero))
        {
            IntPtr ptr = Publisher.NarrowBase(ptrPublisher);
            Entity entity = EntityManager.Instance.Find(ptr);
            if (entity != null)
            {
                publisher = (Publisher)entity;
            }
            else
            {
                publisher = new Publisher(ptrPublisher);
                EntityManager.Instance.Add(((Entity)publisher).ToNative(), publisher);
            }
        }

        return publisher;
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
}

/// <summary>
/// This class suppresses stack walks for unmanaged code permission.
/// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full
/// security review to make sure that the usage is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_NarrowBase")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr NarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetQos", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetQos(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataWriterQosWrapper qos);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_SetQos", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode SetQos(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_WaitForAcknowledgments", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode WaitForAcknowledgments(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] Duration duration);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetPublicationMatchedStatus", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetPublicationMatchedStatus(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublicationMatchedStatus status);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_AssertLiveliness")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ReturnCode AssertLiveliness(IntPtr dw);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_SetListener")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ReturnCode DataWriterSetListener(IntPtr dw, IntPtr listener, uint mask);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetPublisher")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetPublisher(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetTopic")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetTopic(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetLivelinessLostStatus", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetLivelinessLostStatus(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref LivelinessLostStatus status);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetOfferedDeadlineMissedStatus", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetOfferedDeadlineMissedStatus(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref OfferedDeadlineMissedStatus status);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetOfferedIncompatibleQosStatus", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetOfferedIncompatibleQosStatus(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref OfferedIncompatibleQosStatusWrapper status);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetMatchedSubscriptions")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ReturnCode GetMatchedSubscriptions(IntPtr dw, ref IntPtr handles);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetMatchedSubscriptionData", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetMatchedSubscriptionData(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriptionBuiltinTopicDataWrapper data, int handle);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr NarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetQos", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetQos(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataWriterQosWrapper qos);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_SetQos", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode SetQos(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_WaitForAcknowledgments", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode WaitForAcknowledgments(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] Duration duration);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetPublicationMatchedStatus", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetPublicationMatchedStatus(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublicationMatchedStatus status);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_AssertLiveliness", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode AssertLiveliness(IntPtr dw);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_SetListener", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode DataWriterSetListener(IntPtr dw, IntPtr listener, uint mask);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetPublisher", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetPublisher(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetTopic", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetTopic(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetLivelinessLostStatus", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetLivelinessLostStatus(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref LivelinessLostStatus status);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetOfferedDeadlineMissedStatus", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetOfferedDeadlineMissedStatus(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref OfferedDeadlineMissedStatus status);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetOfferedIncompatibleQosStatus", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetOfferedIncompatibleQosStatus(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref OfferedIncompatibleQosStatusWrapper status);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetMatchedSubscriptions", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetMatchedSubscriptions(IntPtr dw, ref IntPtr handles);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataWriter_GetMatchedSubscriptionData", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetMatchedSubscriptionData(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriptionBuiltinTopicDataWrapper data, int handle);
#endif
}