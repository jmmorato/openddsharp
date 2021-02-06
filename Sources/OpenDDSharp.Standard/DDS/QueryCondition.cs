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

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// QueryCondition objects are specialized <see cref="ReadCondition" /> objects that allow the application to also specify a filter on the locally available data.
    /// </summary>
    /// <remarks>
    /// The query is similar to an SQL WHERE clause and can be parameterized by arguments that are dynamically changeable by the <see cref="SetQueryParameters" /> operation.
    /// </remarks>
    public class QueryCondition : ReadCondition
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the queryexpression associated with the <see cref="QueryCondition" />.
        /// That is, the expression specified when the <see cref="QueryCondition" /> was created.
        /// </summary>
        public string QueryExpression => GetQueryExpresion();
        #endregion

        #region Constructors
        internal QueryCondition(IntPtr native, DataReader reader) : base(NarrowBase(native), reader)
        {
            _native = native;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the query parameters associated with the <see cref="QueryCondition" />. That is, the parameters specified on the last
        /// successful call to <see cref="SetQueryParameters" />, or if <see cref="SetQueryParameters" /> was never called, the arguments specified when the
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

            IntPtr seq = IntPtr.Zero;

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetQueryParameters86(_native, ref seq),
                                                         () => UnsafeNativeMethods.GetQueryParameters64(_native, ref seq));

            if (ret == ReturnCode.Ok && !seq.Equals(IntPtr.Zero))
            {
                MarshalHelper.PtrToStringSequence(seq, ref queryParameters, false);
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

            IntPtr seq = IntPtr.Zero;
            MarshalHelper.StringSequenceToPtr(queryParameters, ref seq, false);
            if (seq.Equals(IntPtr.Zero))
            {
                return ReturnCode.Error;
            }

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetQueryParameters86(_native, seq),
                                                         () => UnsafeNativeMethods.SetQueryParameters64(_native, seq));

            return ret;
        }

        private static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
                                               () => UnsafeNativeMethods.NarrowBase64(ptr));
        }

        private string GetQueryExpresion()
        {
            return MarshalHelper.ExecuteAnyCpu(() => Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetQueryExpresion86(_native)),
                                               () => Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetQueryExpresion64(_native)));
        }

        internal new IntPtr ToNative()
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "QueryCondition_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "QueryCondition_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "QueryCondition_GetQueryExpresion", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetQueryExpresion86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "QueryCondition_GetQueryExpresion", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetQueryExpresion64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "QueryCondition_GetQueryParameters", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQueryParameters86(IntPtr ptr, ref IntPtr seq);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "QueryCondition_GetQueryParameters", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQueryParameters64(IntPtr ptr, ref IntPtr seq);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "QueryCondition_SetQueryParameters", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQueryParameters86(IntPtr ptr, IntPtr seq);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "QueryCondition_SetQueryParameters", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQueryParameters64(IntPtr ptr, IntPtr seq);
        }
        #endregion
    }
}
