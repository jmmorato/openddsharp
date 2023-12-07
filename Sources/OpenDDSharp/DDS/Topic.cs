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

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.DDS;

/// <summary>
/// Topic is the most basic description of the data to be published and subscribed.
/// A Topic is identified by its name, which must be unique in the whole Domain. In addition (by virtue of implemeting
/// <see cref="ITopicDescription" />) it fully specifies the type of the data that can be communicated when publishing or subscribing to the Topic.
/// Topic is the only <see cref="ITopicDescription" /> that can be used for publications and therefore associated to a <see cref="DataWriter" />.
/// </summary>
public class Topic : Entity, ITopicDescription
{
    #region Fields
    private readonly IntPtr _native;
    private readonly IntPtr _nativeTopicDescription;
    #endregion

    #region Properties
    /// <summary>
    /// Gets type name used to create the <see cref="ITopicDescription" />.
    /// </summary>
    public string TypeName => GetTypeName();

    /// <summary>
    /// Gets the name used to create the <see cref="ITopicDescription" />.
    /// </summary>
    public string Name => GetName();

    /// <summary>
    /// Gets the <see cref="DomainParticipant" /> to which the <see cref="ITopicDescription" /> belongs.
    /// </summary>
    public DomainParticipant Participant => GetParticipant();

    /// <summary>
    /// Gets the attached <see cref="SubscriberListener"/>.
    /// </summary>
    [SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "Keep coherency with the setter method and DDS API.")]
    public TopicListener Listener { get; internal set; }
    #endregion

    #region Constructors
    internal Topic(IntPtr native) : base(NarrowBase(native))
    {
        _native = native;
        _nativeTopicDescription = NarrowTopicDescription(native);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Gets the <see cref="Topic" /> QoS policies.
    /// </summary>
    /// <param name="qos">The <see cref="TopicQos" /> to be filled up.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetQos(TopicQos qos)
    {
        if (qos == null)
        {
            return ReturnCode.BadParameter;
        }

        TopicQosWrapper qosWrapper = default;
        var ret = UnsafeNativeMethods.GetQos(_native, ref qosWrapper);

        if (ret == ReturnCode.Ok)
        {
            qos.FromNative(qosWrapper);
        }

        qos.Release();

        return ret;
    }

    /// <summary>
    /// Sets the <see cref="Topic" /> QoS policies.
    /// </summary>
    /// <param name="qos">The <see cref="TopicQos" /> to be set.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode SetQos(TopicQos qos)
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
    /// Allows access to the attached <see cref="TopicListener" />.
    /// </summary>
    /// <returns>The attached <see cref="TopicListener" />.</returns>
    [Obsolete(nameof(GetListener) + " is deprecated, please use Listener property instead.")]
    [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Keep coherency with the setter method and DDS API.")]
    public TopicListener GetListener()
    {
        return Listener;
    }

    /// <summary>
    /// Sets the <see cref="TopicListener" /> using the <see cref="StatusMask.DefaultStatusMask" />.
    /// </summary>
    /// <param name="listener">The <see cref="TopicListener" /> to be set.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode SetListener(TopicListener listener)
    {
        return SetListener(listener, StatusMask.DefaultStatusMask);
    }

    /// <summary>
    /// Sets the <see cref="TopicListener" />.
    /// </summary>
    /// <param name="listener">The <see cref="TopicListener" /> to be set.</param>
    /// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode SetListener(TopicListener listener, StatusMask mask)
    {
        Listener = listener;
        IntPtr ptr = IntPtr.Zero;
        if (listener != null)
        {
            ptr = listener.ToNative();
        }

        return UnsafeNativeMethods.TopicSetListener(_native, ptr, mask);
    }

    /// <summary>
    /// This method allows the application to retrieve the <see cref="InconsistentTopicStatus" /> of the <see cref="Topic" />.
    /// </summary>
    /// <param name="status">The <see cref="InconsistentTopicStatus" /> structure to be fill up.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetInconsistentTopicStatus(ref InconsistentTopicStatus status)
    {
        InconsistentTopicStatus aux = default;

        ReturnCode ret = UnsafeNativeMethods.GetInconsistentTopicStatus(_native, ref aux);

        status = aux;

        return ret;
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

    /// <summary>
    /// Internal use only.
    /// </summary>
    /// <returns>The native pointer.</returns>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public IntPtr ToNativeTopicDescription()
    {
        return _nativeTopicDescription;
    }

    internal static IntPtr NarrowBase(IntPtr ptr)
    {
        return UnsafeNativeMethods.TopicNativeNarrowBase(ptr);
    }

    internal static IntPtr NarrowTopicDescription(IntPtr ptr)
    {
        return UnsafeNativeMethods.TopicNativeNarrowTopicDescription(ptr);
    }

    private string GetTypeName()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.TopicGetTypeName(_native));
    }

    private string GetName()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.TopicGetName(_native));
    }

    private DomainParticipant GetParticipant()
    {
        IntPtr ptrParticipant = UnsafeNativeMethods.TopicGetParticipant(_native);

        DomainParticipant participant = null;

        if (!ptrParticipant.Equals(IntPtr.Zero))
        {
            var ptr = DomainParticipant.NarrowBase(ptrParticipant);

            var entity = EntityManager.Instance.Find(ptr);
            if (entity != null)
            {
                participant = (DomainParticipant)entity;
            }
            else
            {
                participant = new DomainParticipant(ptrParticipant);
                EntityManager.Instance.Add(ptrParticipant, participant);
            }
        }

        return participant;
    }
    #endregion
}

/// <summary>
/// This class suppresses stack walks for unmanaged code permission.
/// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full
/// security review to make sure that the usage
/// is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "Topic_NarrowBase")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr TopicNativeNarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "Topic_NarrowTopicDescription")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr TopicNativeNarrowTopicDescription(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_GetQos", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetQos(IntPtr t, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicQosWrapper qos);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_SetQos", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode SetQos(IntPtr t, [MarshalAs(UnmanagedType.Struct), In] TopicQosWrapper qos);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "Topic_SetListener")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ReturnCode TopicSetListener(IntPtr t, IntPtr listener, uint mask);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "Topic_GetTypeName")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr TopicGetTypeName(IntPtr t);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "Topic_GetName")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr TopicGetName(IntPtr t);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "Topic_GetParticipant")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr TopicGetParticipant(IntPtr t);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_GetInconsistentTopicStatus", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetInconsistentTopicStatus(IntPtr t, [MarshalAs(UnmanagedType.Struct), In, Out] ref InconsistentTopicStatus status);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr TopicNativeNarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_NarrowTopicDescription", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr TopicNativeNarrowTopicDescription(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_GetQos", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetQos(IntPtr t, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicQosWrapper qos);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_SetQos", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode SetQos(IntPtr t, [MarshalAs(UnmanagedType.Struct), In] TopicQosWrapper qos);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_SetListener", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode TopicSetListener(IntPtr t, IntPtr listener, uint mask);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_GetTypeName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr TopicGetTypeName(IntPtr t);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_GetName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr TopicGetName(IntPtr t);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_GetParticipant", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr TopicGetParticipant(IntPtr t);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Topic_GetInconsistentTopicStatus", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetInconsistentTopicStatus(IntPtr t, [MarshalAs(UnmanagedType.Struct), In, Out] ref InconsistentTopicStatus status);
#endif
}