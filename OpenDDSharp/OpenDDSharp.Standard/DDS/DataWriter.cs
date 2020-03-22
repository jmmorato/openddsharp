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
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

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
    /// <para>All operations except for the operations <see cref="SetQos" />, <see cref="GetQos" />, SetListener,
    /// <see cref="DataWriter.GetListener" />, <see cref="Entity.Enable" />, and <see cref="Entity.StatusCondition" />
    /// return the value <see cref="ReturnCode.NotEnabled" /> if the DataWriter has not been enabled yet.</para>
    /// </remarks>
    public class DataWriter : Entity
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataWriter"/> class.
        /// </summary>
        /// <param name="native">The native pointer.</param>
        protected internal DataWriter(IntPtr native) : base(NarrowBase(native))
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

            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetQos86(_native, qosNative),
                                                  () => UnsafeNativeMethods.SetQos64(_native, qosNative));

            qos.Release();

            return ret;
        }

        /// <summary>
        /// Blocks the calling thread until either all data written by the <see cref="DataWriter" /> is
        /// acknowledged by all matched <see cref="DataReader" /> entities that have <see cref="ReliabilityQosPolicyKind.ReliableReliabilityQos" />, or else the duration
        /// specified by the maxWait parameter elapses, whichever happens first.
        /// </summary>
        /// <remarks>
        /// <para>This operation is intended to be used only if the <see cref="DataWriter" /> has configured <see cref="ReliabilityQosPolicyKind.ReliableReliabilityQos" />.
        /// Otherwise the operation will return immediately with <see cref="ReturnCode.Ok" />.</para>
        /// <para>A return value of <see cref="ReturnCode.Ok" /> indicates that all the samples
        /// written have been acknowledged by all reliable matched data readers; a return value of <see cref="ReturnCode.Timeout" /> indicates that maxWait
        /// elapsed before all the data was acknowledged.</para>
        /// </remarks>
        /// <param name="maxWait">The maximum <see cref="Duration" /> time to wait for the acknowledgments.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode WaitForAcknowledgments(Duration maxWait)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.WaitForAcknowledgments86(_native, maxWait),
                                               () => UnsafeNativeMethods.WaitForAcknowledgments64(_native, maxWait));
        }

        /// <summary>
        /// Allows access to the <see cref="PublicationMatchedStatus" /> communication status.
        /// </summary>
        /// <param name="status">The <see cref="PublicationMatchedStatus" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetPublicationMatchedStatus(ref PublicationMatchedStatus status)
        {
            PublicationMatchedStatus s = default;
            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetPublicationMatchedStatus86(_native, ref s),
                                                         () => UnsafeNativeMethods.GetPublicationMatchedStatus64(_native, ref s));
            status = s;

            return ret;
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataWriter_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataWriter_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataWriter_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos64(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataWriterQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataWriter_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos86(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataWriterQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataWriter_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos64(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataWriter_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos86(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataWriter_WaitForAcknowledgments", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode WaitForAcknowledgments64(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] Duration duration);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataWriter_WaitForAcknowledgments", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode WaitForAcknowledgments86(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] Duration duration);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataWriter_GetPublicationMatchedStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetPublicationMatchedStatus64(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] ref PublicationMatchedStatus status);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataWriter_GetPublicationMatchedStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetPublicationMatchedStatus86(IntPtr dw, [MarshalAs(UnmanagedType.Struct), In] ref PublicationMatchedStatus status);
        }
        #endregion
    }
}
