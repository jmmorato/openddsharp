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

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// DataWriter allows the application to set the value of the data to be published under a given <see cref="Topic" />.
    /// </summary>
    /// <remarks>
    /// <para>A DataWriter is attached to exactly one <see cref="Publisher" /> that acts as a factory for it.</para>
    /// <para>A DataWriter is bound to exactly one <see cref="Topic" /> and therefore to exactly one data type. The <see cref="Topic" /> 
    /// must exist prior to the DataWriter’s creation.</para>
    /// <para>The DataWriter must be specialized for each particular application data-type.</para>
    /// <para>All operations except for the operations <see cref="DataWriter::SetQos" />, <see cref="DataWriter::GetQos" />, SetListener,
    /// <see cref="DataWriter::GetListener" />, <see cref="Entity::Enable" />, and <see cref="Entity::StatusCondition" />
    /// return the value <see cref="ReturnCode::NotEnabled" /> if the DataWriter has not been enabled yet.</para>
    /// </remarks>
    public class DataWriter
    {
        #region Fields
        protected readonly IntPtr _native;
        #endregion

        #region Constructors
        protected internal DataWriter(IntPtr native)
        {
            _native = native;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Blocks the calling thread until either all data written by the <see cref="DataWriter" /> is
        /// acknowledged by all matched <see cref="DataReader" /> entities that have <see cref="ReliabilityQosPolicyKind.ReliableReliabilityQos" />, or else the duration
        ///	specified by the maxWait parameter elapses, whichever happens first.
        /// </summary>
        /// <remarks>
        /// <para>This operation is intended to be used only if the <see cref="DataWriter" /> has configured <see cref="ReliabilityQosPolicyKind.ReliableReliabilityQos" />. 
        /// Otherwise the operation will return immediately with <see cref="ReturnCode.Ok" />.</para>
        /// <para>A return value of <see cref="ReturnCode.Ok" /> indicates that all the samples
        /// written have been acknowledged by all reliable matched data readers; a return value of <see cref="ReturnCode.Timeout" /> indicates that maxWait
        ///	elapsed before all the data was acknowledged.</para>
        /// </remarks>
        /// <param name="maxWait">The maximum <see cref="Duration" /> time to wait for the acknowledgments.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode WaitForAcknowledgments(Duration maxWait)
        {
            if (Environment.Is64BitProcess)
            {
                return WaitForAcknowledgments64(_native, maxWait);
            }
            else
            {
                return WaitForAcknowledgments86(_native, maxWait);
            }
        }

        /// <summary>
        /// Allows access to the <see cref="PublicationMatchedStatus" /> communication status.
        /// </summary>
        /// <param name="status">The <see cref="PublicationMatchedStatus" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetPublicationMatchedStatus(ref PublicationMatchedStatus status)
        {
            if (Environment.Is64BitProcess)
            {
                return GetPublicationMatchedStatus64(_native, ref status);
            }
            else
            {
                return GetPublicationMatchedStatus86(_native, ref status);
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public IntPtr ToNative()
        {
            return _native;
        }
        #endregion

        #region PInvoke
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "DataWriter_WaitForAcknowledgments", CallingConvention = CallingConvention.Cdecl)]
        private static extern ReturnCode WaitForAcknowledgments64(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] Duration duration);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "DataWriter_WaitForAcknowledgments", CallingConvention = CallingConvention.Cdecl)]
        private static extern ReturnCode WaitForAcknowledgments86(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] Duration duration);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "DataWriter_GetPublicationMatchedStatus", CallingConvention = CallingConvention.Cdecl)]
        private static extern ReturnCode GetPublicationMatchedStatus64(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] ref PublicationMatchedStatus status);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "DataWriter_GetPublicationMatchedStatus", CallingConvention = CallingConvention.Cdecl)]
        private static extern ReturnCode GetPublicationMatchedStatus86(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] ref PublicationMatchedStatus status);
        #endregion
    }
}
