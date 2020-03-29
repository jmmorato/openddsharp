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
using OpenDDSharp.Helpers;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// Allows the creation and destruction of <see cref="DomainParticipant" /> objects.
    /// </summary>
    public sealed class DomainParticipantFactory
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        internal DomainParticipantFactory(IntPtr native)
        {
            _native = native;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="DomainParticipant" /> object with the default QoS policies and without listener attached.
        /// </summary>
        /// <param name="domainId">Domain ID that the application intends to join.</param>
        /// <returns> The newly created <see cref="DomainParticipant" /> on success, otherwise <see langword="null"/>.</returns>
        public DomainParticipant CreateParticipant(int domainId)
        {
            return CreateParticipant(domainId, null, null, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a <see cref="DomainParticipant" /> with the desired QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// If the specified QoS policies are not consistent, the operation will fail and no <see cref="DomainParticipant" /> will be created.
        /// </remarks>
        /// <param name="domainId">Domain ID that the application intends to join.</param>
        /// <param name="qos">The <see cref="DomainParticipantQos" /> policies to be used for creating the new <see cref="DomainParticipant" />.</param>
        /// <returns> The newly created <see cref="DomainParticipant" /> on success, otherwise <see langword="null"/>.</returns>
        public DomainParticipant CreateParticipant(int domainId, DomainParticipantQos qos)
        {
            return CreateParticipant(domainId, qos, null, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="DomainParticipant" /> with the desired QoS policies and attaches to it the specified <see cref="DomainParticipantListener" />.
        /// The specified <see cref="DomainParticipantListener" /> will be attached with the default <see cref="StatusMask" />.
        /// </summary>
        /// <remarks>
        /// If the specified QoS policies are not consistent, the operation will fail and no <see cref="DomainParticipant" /> will be created.
        /// </remarks>
        /// <param name="domainId">Domain ID that the application intends to join.</param>
        /// <param name="qos">The <see cref="DomainParticipantQos" /> policies to be used for creating the new <see cref="DomainParticipant" />.</param>
        /// <param name="listener">The <see cref="DomainParticipantListener" /> to be attached to the newly created <see cref="DomainParticipant" />.</param>
        /// <returns> The newly created <see cref="DomainParticipant" /> on success, otherwise <see langword="null"/>.</returns>
        public DomainParticipant CreateParticipant(int domainId, DomainParticipantQos qos, DomainParticipantListener listener)
        {
            return CreateParticipant(domainId, qos, listener, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="DomainParticipant" /> with the desired QoS policies and attaches to it the specified <see cref="DomainParticipantListener" />.
        /// </summary>
        /// <remarks>
        /// If the specified QoS policies are not consistent, the operation will fail and no <see cref="DomainParticipant" /> will be created.
        /// </remarks>
        /// <param name="domainId">Domain ID that the application intends to join.</param>
        /// <param name="qos">The <see cref="DomainParticipantQos" /> policies to be used for creating the new <see cref="DomainParticipant" />.</param>
        /// <param name="listener">The <see cref="DomainParticipantListener" /> to be attached to the newly created <see cref="DomainParticipant" />.</param>
        /// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
        /// <returns> The newly created <see cref="DomainParticipant" /> on success, otherwise <see langword="null"/>.</returns>
        public DomainParticipant CreateParticipant(int domainId, DomainParticipantQos qos, DomainParticipantListener listener, StatusMask statusMask)
        {
            DomainParticipantQosWrapper qosWrapper = default;
            if (qos is null)
            {
                qos = new DomainParticipantQos();
                var ret = ReturnCode.Ok; // TODO: GetDefaultParticipantQos(qos);
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

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateParticipant86(_native, domainId, qosWrapper, nativeListener, statusMask),
                                                        () => UnsafeNativeMethods.CreateParticipant64(_native, domainId, qosWrapper, nativeListener, statusMask));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new DomainParticipant(native)
            {
                Listener = listener,
            };
        }

        /// <summary>
        /// Deletes an existing <see cref="DomainParticipant" />.
        /// </summary>
        /// <remarks>
        /// This operation can only be invoked if all domain entities belonging to the participant have already been deleted.
        /// Otherwise the error <see cref="ReturnCode.PreconditionNotMet" /> is returned.
        /// </remarks>
        /// <param name="dp">The <see cref="DomainParticipant" /> to be deleted.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteParticipant(DomainParticipant dp)
        {
            if (dp == null)
            {
                throw new ArgumentNullException(nameof(dp));
            }

            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeleteParticipant86(_native, dp.ToNative()),
                                               () => UnsafeNativeMethods.DeleteParticipant64(_native, dp.ToNative()));
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipantFactory_CreateParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateParticipant64(IntPtr dpf, int domainId, [MarshalAs(UnmanagedType.Struct), In] DomainParticipantQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipantFactory_CreateParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateParticipant86(IntPtr dpf, int domainId, [MarshalAs(UnmanagedType.Struct), In] DomainParticipantQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipantFactory_DeleteParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteParticipant64(IntPtr dpf, IntPtr dp);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipantFactory_DeleteParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteParticipant86(IntPtr dpf, IntPtr dp);
        }
        #endregion
    }
}
