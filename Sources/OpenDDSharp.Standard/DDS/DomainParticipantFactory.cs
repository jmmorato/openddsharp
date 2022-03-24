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
                var ret = GetDefaultDomainParticipantQos(qos);
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

            IntPtr native = UnsafeNativeMethods.CreateParticipant(_native, domainId, qosWrapper, nativeListener, statusMask);

            qos.Release();

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            var p = new DomainParticipant(native)
            {
                Listener = listener,
            };

            EntityManager.Instance.Add((p as Entity).ToNative(), p);

            return p;
        }

        /// <summary>
        /// Gets the default value of the <see cref="DomainParticipant" /> QoS, that is, the QoS policies which will be used for
        /// newly created <see cref="DomainParticipant" /> entities in the case where the QoS policies are defaulted in the CreateParticipant operation.
        /// </summary>
        /// <remarks>
        /// The values retrieved <see cref="GetDefaultDomainParticipantQos(DomainParticipantQos)" /> will match the set of values specified on the last successful call to
        /// <see cref="SetDefaultDomainParticipantQos(DomainParticipantQos)" />, or else, if the call was never made, the default values defined by the DDS standard.
        /// </remarks>
        /// <param name="qos">The <see cref="DomainParticipantQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetDefaultDomainParticipantQos(DomainParticipantQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            DomainParticipantQosWrapper qosWrapper = default;
            var ret = UnsafeNativeMethods.GetDefaultDomainParticipantQos(_native, ref qosWrapper);

            if (ret == ReturnCode.Ok)
            {
                qos.FromNative(qosWrapper);
            }

            qos.Release();

            return ret;
        }

        /// <summary>
        /// Sets a default value of the <see cref="DomainParticipant" /> QoS policies which will be used for newly created
        /// <see cref="DomainParticipant" /> entities in the case where the QoS policies are defaulted in the CreateParticipant operation.
        /// </summary>
        /// <remarks>
        /// This operation will check that the resulting policies are self consistent; if they are not,
        /// the operation will have no effect and return <see cref="ReturnCode.InconsistentPolicy" />.
        /// </remarks>
        /// <param name="qos">The default <see cref="DomainParticipantQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetDefaultDomainParticipantQos(DomainParticipantQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            var qosNative = qos.ToNative();
            var ret = UnsafeNativeMethods.SetDefaultDomainParticipantQos(_native, qosNative);
            qos.Release();

            return ret;
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
                return ReturnCode.Ok;
            }

            return UnsafeNativeMethods.DeleteParticipant(_native, dp.ToNative());
        }

        /// <summary>
        /// Gets the <see cref="DomainParticipantFactory" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="DomainParticipantFactoryQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetQos(DomainParticipantFactoryQos qos)
        {
            if (qos == null)
            {
                return ReturnCode.BadParameter;
            }

            DomainParticipantFactoryQosWrapper qosWrapper = default;
            var ret = UnsafeNativeMethods.GetQos(_native, ref qosWrapper);

            if (ret == ReturnCode.Ok)
            {
                qos.FromNative(qosWrapper);
            }

            return ret;
        }

        /// <summary>
        /// Sets the <see cref="DomainParticipantFactory" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="DomainParticipantFactoryQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetQos(DomainParticipantFactoryQos qos)
        {
            if (qos == null)
            {
                return ReturnCode.BadParameter;
            }

            var qosNative = qos.ToNative();
            var ret = UnsafeNativeMethods.SetQos(_native, qosNative);

            return ret;
        }

        /// <summary>
        /// This operation retrieves a previously created <see cref="DomainParticipant" /> belonging to specified <paramref name="domainId"/>.
        /// </summary>
        /// <remarks>
        /// If multiple <see cref="DomainParticipant" /> entities belonging to that <paramref name="domainId"/> exist, then the operation will return one of them.
        /// It is not specified which one.
        /// </remarks>
        /// <param name="domainId">Domain ID of the participant to lookup.</param>
        /// <returns>The <see cref="DomainParticipant" />, if it exists, otherwise <see langword="null"/>.</returns>
        public DomainParticipant LookupParticipant(int domainId)
        {
            IntPtr native = UnsafeNativeMethods.LookupParticipant(_native, domainId);

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return (DomainParticipant)EntityManager.Instance.Find(native);
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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DomainParticipantFactory_CreateParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateParticipant(IntPtr dpf, int domainId, [MarshalAs(UnmanagedType.Struct), In] DomainParticipantQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DomainParticipantFactory_GetDefaultDomainParticipantQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultDomainParticipantQos(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In, Out] ref DomainParticipantQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DomainParticipantFactory_SetDefaultDomainParticipantQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultDomainParticipantQos(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In] DomainParticipantQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DomainParticipantFactory_DeleteParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteParticipant(IntPtr dpf, IntPtr dp);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DomainParticipantFactory_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos(IntPtr dpf, [MarshalAs(UnmanagedType.Struct), In, Out] ref DomainParticipantFactoryQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DomainParticipantFactory_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos(IntPtr dpf, [MarshalAs(UnmanagedType.Struct), In] DomainParticipantFactoryQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DomainParticipantFactory_LookupParticipant", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr LookupParticipant(IntPtr pub, int domain);
        }
        #endregion
    }
}
