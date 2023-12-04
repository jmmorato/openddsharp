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
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.DDS;

/// <summary>
/// MultiTopic is an implementation of <see cref="ITopicDescription" /> that allows subscriptions to
/// combine/filter/rearrange data coming from several topics. MultiTopic allows a more sophisticated subscription
/// that can select and combine data received from multiple topics into a single resulting type
/// (specified by the inherited type name). The data will then be filtered(selection) and possibly re-arranged
/// (aggregation/projection) according to a subscription expression with the expression parameters.
/// </summary>
/// <remarks>
/// <para>
/// The subscription expression is a string that identifies the selection and re-arrangement of data from the associated
/// topics. It is similar to an SQL clause where the SELECT part provides the fields to be kept, the FROM
/// part provides the names of the topics that are searched for those fields, and the WHERE clause gives the content
/// filter. The topics combined may have different types but they are restricted in that the type of the fields used
/// for the NATURAL JOIN operation must be the same.</para>
/// <para>The expression parameters are a collection of strings that give values to the ‘parameters’ ("%n" tokens) in
/// the subscription expression. The number of supplied parameters must fit with the requested values in the
/// subscription expression (the number of %n tokens).</para>
/// <para>
/// <see cref="DataReader" /> entities associated with a MultiTopic are alerted of data modifications by the usual
/// listener or condition mechanisms whenever modifications occur to the data associated with any of the topics
/// relevant to the MultiTopic.
/// </para>
/// <para>
/// <see cref="DataReader" /> entities associated with a MultiTopic access instances that are “constructed” at
/// the <see cref="DataReader" /> side from the instances written by multiple <see cref="DataWriter" /> entities.
/// The MultiTopic access instance will begin to exist as soon as all the constituting Topic instances are in existence.
/// </para>
/// <para>The view_state and instance_state is computed from the corresponding states of the constituting instances:
/// <list type="bullet">
///     <item><description>The view state of the MultiTopic instance is <see cref="ViewStateKind.NewViewState" /> if at least one of the constituting instances has
/// <see cref="ViewStateKind.NewViewState" />, otherwise it will be  <see cref="ViewStateKind.NotNewViewState" />.</description></item>
///     <item><description>The instance state of the MultiTopic instance is <see cref="InstanceStateKind.AliveInstanceState" /> if the instance state of all the
/// constituting Topic instances is <see cref="InstanceStateKind.AliveInstanceState" />. It is <see cref="InstanceStateKind.NotAliveDisposedInstanceState" /> if at least one of the constituting <see cref="Topic" />
/// instances is <see cref="InstanceStateKind.NotAliveDisposedInstanceState" />. Otherwise it is <see cref="InstanceStateKind.NotAliveNoWritersInstanceState" />.</description></item>
/// </list></para>
/// </remarks>
public class MultiTopic : ITopicDescription
{
    #region Fields
    private readonly IntPtr _native;
    private readonly IntPtr _nativeTopicDescription;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the subscription expression associated with the <see cref="MultiTopic" />.
    /// That is, the expression specified when the <see cref="MultiTopic" /> was created.
    /// </summary>
    public string SubscriptionExpression => GetSubscriptionExpression();

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
    #endregion

    #region Constructors
    internal MultiTopic(IntPtr native)
    {
        _native = native;
        _nativeTopicDescription = NarrowTopicDescription(native);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Gets the expression parameters associated with the <see cref="MultiTopic" />. That is, the parameters specified
    /// on the last successful call to <see cref="MultiTopic.SetExpressionParameters" />, or if it was never called,
    /// the parameters specified when the <see cref="MultiTopic" /> was created.
    /// </summary>
    /// <param name="parameters">The expression parameters list to be filled up.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetExpressionParameters(IList<string> parameters)
    {
        if (parameters == null)
        {
            return ReturnCode.BadParameter;
        }
        parameters.Clear();

        var seq = IntPtr.Zero;

        var ret = UnsafeNativeMethods.MultiTopicGetExpressionParameters(_native, ref seq);

        if (ret == ReturnCode.Ok && !seq.Equals(IntPtr.Zero))
        {
            seq.PtrToStringSequence(ref parameters, false);
        }

        return ret;
    }

    /// <summary>
    /// Changes the expression parameters associated with the <see cref="MultiTopic" />.
    /// </summary>
    /// <param name="parameters">The expression parameters values to be set.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode SetExpressionParameters(params string[] parameters)
    {
        if (parameters == null)
        {
            return ReturnCode.BadParameter;
        }

        IntPtr seq = IntPtr.Zero;
        IList<string> paramList = parameters.ToList();
        paramList.StringSequenceToPtr(ref seq, false);

        return UnsafeNativeMethods.MultiTopicSetExpressionParameters(_native, seq);
    }

    internal static IntPtr NarrowTopicDescription(IntPtr ptr)
    {
        return UnsafeNativeMethods.MultiTopicNativeNarrowTopicDescription(ptr);
    }

    /// <summary>
    /// Internal use only.
    /// </summary>
    /// <returns>The native pointer.</returns>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public IntPtr ToNative()
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

    private string GetTypeName()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.MultiTopicGetTypeName(_native));
    }

    private string GetName()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.MultiTopicGetName(_native));
    }

    private DomainParticipant GetParticipant()
    {
        IntPtr ptrParticipant = UnsafeNativeMethods.MultiTopicGetParticipant(_native);

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

    private string GetSubscriptionExpression()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.MultiTopicGetSubscriptionExpression(_native));
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
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_NarrowTopicDescription")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr MultiTopicNativeNarrowTopicDescription(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_GetExpressionParameters")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ReturnCode MultiTopicGetExpressionParameters(IntPtr ptr, ref IntPtr seq);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_SetExpressionParameters")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ReturnCode MultiTopicSetExpressionParameters(IntPtr ptr, IntPtr parameters);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_GetSubscriptionExpression")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr MultiTopicGetSubscriptionExpression(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_GetTypeName")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr MultiTopicGetTypeName(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_GetName")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr MultiTopicGetName(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_GetParticipant")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr MultiTopicGetParticipant(IntPtr ptr);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_NarrowTopicDescription", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr MultiTopicNativeNarrowTopicDescription(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_GetExpressionParameters", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode MultiTopicGetExpressionParameters(IntPtr ptr, ref IntPtr seq);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_SetExpressionParameters", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern ReturnCode MultiTopicSetExpressionParameters(IntPtr ptr, IntPtr parameters);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_GetSubscriptionExpression", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr MultiTopicGetSubscriptionExpression(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_GetTypeName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr MultiTopicGetTypeName(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_GetName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr MultiTopicGetName(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "MultiTopic_GetParticipant", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr MultiTopicGetParticipant(IntPtr ptr);
#endif
}
