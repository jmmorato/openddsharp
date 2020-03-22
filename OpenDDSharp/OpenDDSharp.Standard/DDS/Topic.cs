﻿/*********************************************************************
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
        #endregion

        #region Constructors
        internal Topic(IntPtr native) : base(NarrowBase(native))
        {
            _native = native;
            _nativeTopicDescription = NarrowTopicDescription(native);
        }
        #endregion

        #region Methods
        private static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
                                               () => UnsafeNativeMethods.NarrowBase64(ptr));
        }

        private static IntPtr NarrowTopicDescription(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowTopicDescription86(ptr),
                                               () => UnsafeNativeMethods.NarrowTopicDescription64(ptr));
        }

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

            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetQos86(_native, qosNative),
                                                  () => UnsafeNativeMethods.SetQos64(_native, qosNative));
            qos.Release();

            return ret;

        }

        private string GetTypeName()
        {
            throw new NotImplementedException();
        }

        private string GetName()
        {
            throw new NotImplementedException();
        }

        private DomainParticipant GetParticipant()
        {
            throw new NotImplementedException();
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
        #endregion

        #region Unsafe Native Methods
        /// <summary>
        /// This class suppresses stack walks for unmanaged code permission. (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
        /// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full security review to make sure that the usage
        /// is secure because no stack walk will be performed.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private static class UnsafeNativeMethods
        {
            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Topic_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Topic_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Topic_NarrowTopicDescription", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowTopicDescription64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Topic_NarrowTopicDescription", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowTopicDescription86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Topic_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Topic_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Topic_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] TopicQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Topic_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] TopicQosWrapper qos);

        }
        #endregion
    }
}
