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
/// ContentFilteredTopic is an implementation of <see cref="ITopicDescription" /> that allows for content-based subscriptions.
/// ContentFilteredTopic describes a more sophisticated subscription that indicates the subscriber does not want to necessarily see
/// all values of each instance published under the <see cref="Topic" />. Rather, it wants to see only the values whose contents satisfy certain
/// criteria. This class therefore can be used to request content-based subscriptions.
/// </summary>
/// <remarks>
/// The selection of the content is done using the filter expression with the expression parameters.
/// <list type="bullet">
///     <item><description>The filter expression is a string that specifies the criteria to select the data samples of interest. It is similar to the WHERE part of an SQL clause.</description></item>
///     <item><description>The expression parameters are a collection of strings that give values to the 'parameters' ("%n" tokens) in the filter expression. The number of supplied parameters must fit with the requested values in the filter expression</description></item>
/// </list>
/// </remarks>
public class ContentFilteredTopic : ITopicDescription
{
    #region Fields
    private readonly IntPtr _native;
    private readonly IntPtr _nativeTopicDescription;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the filter expression associated with the <see cref="ContentFilteredTopic" />. That is, the expression specified when the
    /// <see cref="ContentFilteredTopic" /> was created.
    /// </summary>
    public string FilterExpression => GetFilterExpression();

    /// <summary>
    /// Gets the <see cref="Topic" /> associated with the <see cref="ContentFilteredTopic" />. That is, the <see cref="Topic" /> specified when the
    /// <see cref="ContentFilteredTopic" /> was created.
    /// </summary>
    public Topic RelatedTopic => GetRelatedTopic();

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
    internal ContentFilteredTopic(IntPtr native)
    {
        _native = native;
        _nativeTopicDescription = NarrowTopicDescription(native);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Gets the expression parameters associated with the <see cref="ContentFilteredTopic" />. That is, the parameters specified
    /// on the last successful call to <see cref="SetExpressionParameters" />, or if it was never called, the parameters
    /// specified when the <see cref="ContentFilteredTopic" /> was created.
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

        var ret = UnsafeNativeMethods.GetExpressionParameters(_native, ref seq);

        if (ret == ReturnCode.Ok && !seq.Equals(IntPtr.Zero))
        {
            seq.PtrToStringSequence(ref parameters, false);
        }

        return ret;
    }

    /// <summary>
    /// Changes the expression parameters associated with the <see cref="ContentFilteredTopic" />.
    /// </summary>
    /// <param name="parameters">The expression parameters values to be set.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode SetExpressionParameters(params string[] parameters)
    {
        if (parameters == null)
        {
            return ReturnCode.BadParameter;
        }

        var seq = IntPtr.Zero;
        IList<string> paramList = parameters.ToList();
        paramList.StringSequenceToPtr(ref seq, false);

        return UnsafeNativeMethods.SetExpressionParameters(_native, seq);
    }

    private static IntPtr NarrowTopicDescription(IntPtr ptr)
    {
        return UnsafeNativeMethods.NativeNarrowTopicDescription(ptr);
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
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetTypeName(_native));
    }

    private string GetName()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetName(_native));
    }

    private DomainParticipant GetParticipant()
    {
        var ptrParticipant = UnsafeNativeMethods.GetParticipant(_native);

        if (ptrParticipant.Equals(IntPtr.Zero))
        {
            return null;
        }

        DomainParticipant participant;

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

        return participant;
    }

    private string GetFilterExpression()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetFilterExpression(_native));
    }

    private Topic GetRelatedTopic()
    {
        var nativeTopic = UnsafeNativeMethods.GetRelatedTopic(_native);
        var entity = EntityManager.Instance.Find(Topic.NarrowTopicDescription(nativeTopic));

        Topic topic = null;
        if (entity != null)
        {
            topic = entity as Topic;
        }

        return topic;
    }
    #endregion
}

/// <summary>
/// This class suppresses stack walks for unmanaged code permission.
/// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform
/// a full security review to make sure that the usage is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_NarrowTopicDescription")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr NativeNarrowTopicDescription(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetExpressionParameters")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial ReturnCode GetExpressionParameters(IntPtr ptr, ref IntPtr seq);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_SetExpressionParameters", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial ReturnCode SetExpressionParameters(IntPtr ptr, IntPtr parameters);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetFilterExpression")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr GetFilterExpression(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetRelatedTopic")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr GetRelatedTopic(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetTypeName", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr GetTypeName(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetName", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr GetName(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetParticipant")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    internal static partial IntPtr GetParticipant(IntPtr ptr);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_NarrowTopicDescription", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr NativeNarrowTopicDescription(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetExpressionParameters", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ReturnCode GetExpressionParameters(IntPtr ptr, ref IntPtr seq);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_SetExpressionParameters", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    internal static extern ReturnCode SetExpressionParameters(IntPtr ptr, IntPtr parameters);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetFilterExpression", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr GetFilterExpression(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetRelatedTopic", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr GetRelatedTopic(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetTypeName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    internal static extern IntPtr GetTypeName(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    internal static extern IntPtr GetName(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ContentFilteredTopic_GetParticipant", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr GetParticipant(IntPtr ptr);
#endif
}
