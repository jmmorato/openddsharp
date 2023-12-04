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
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.DDS;

/// <summary>
/// QueryCondition objects are specialized <see cref="ReadCondition" /> objects that allow the application to also
/// specify a filter on the locally available data.
/// </summary>
/// <remarks>
/// The query is similar to an SQL WHERE clause and can be parameterized by arguments that are dynamically changeable
/// by the <see cref="SetQueryParameters" /> operation.
/// </remarks>
public class QueryCondition : ReadCondition
{
    #region Fields
    private readonly IntPtr _native;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the QueryExpression associated with the <see cref="QueryCondition" />.
    /// That is, the expression specified when the <see cref="QueryCondition" /> was created.
    /// </summary>
    public string QueryExpression => GetQueryExpression();
    #endregion

    #region Constructors
    internal QueryCondition(IntPtr native, DataReader reader)
        : base(UnsafeNativeMethods.QueryConditionNarrowBase(native), reader)
    {
        _native = native;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Gets the query parameters associated with the <see cref="QueryCondition" />. That is, the parameters
    /// specified on the last successful call to <see cref="SetQueryParameters" />, or if
    /// <see cref="SetQueryParameters" /> was never called, the arguments specified when the
    /// <see cref="QueryCondition" /> was created.
    /// </summary>
    /// <param name="queryParameters">The query parameters list to be filled up.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetQueryParameters(IList<string> queryParameters)
    {
        if (queryParameters == null)
        {
            return ReturnCode.BadParameter;
        }
        queryParameters.Clear();

        var seq = IntPtr.Zero;

        var ret = UnsafeNativeMethods.GetQueryParameters(_native, ref seq);

        if (ret == ReturnCode.Ok && !seq.Equals(IntPtr.Zero))
        {
            seq.PtrToStringSequence(ref queryParameters, false);
        }

        return ret;
    }

    /// <summary>
    /// Changes the query parameters associated with the <see cref="QueryCondition" />.
    /// </summary>
    /// <param name="queryParameters">The query parameters values to be set.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode SetQueryParameters(params string[] queryParameters)
    {
        if (queryParameters == null)
        {
            return ReturnCode.BadParameter;
        }

        var seq = IntPtr.Zero;
        queryParameters.StringSequenceToPtr(ref seq, false);
        if (seq.Equals(IntPtr.Zero))
        {
            return ReturnCode.Error;
        }

        var ret = UnsafeNativeMethods.SetQueryParameters(_native, seq);

        return ret;
    }

    private string GetQueryExpression()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetQueryExpression(_native));
    }

    internal new IntPtr ToNative()
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
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "QueryCondition_NarrowBase")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr QueryConditionNarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "QueryCondition_GetQueryExpresion")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetQueryExpression(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "QueryCondition_GetQueryParameters")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ReturnCode GetQueryParameters(IntPtr ptr, ref IntPtr seq);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "QueryCondition_SetQueryParameters")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ReturnCode SetQueryParameters(IntPtr ptr, IntPtr seq);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "QueryCondition_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr QueryConditionNarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "QueryCondition_GetQueryExpresion", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr GetQueryExpression(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "QueryCondition_GetQueryParameters", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode GetQueryParameters(IntPtr ptr, ref IntPtr seq);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "QueryCondition_SetQueryParameters", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode SetQueryParameters(IntPtr ptr, IntPtr seq);
#endif
}