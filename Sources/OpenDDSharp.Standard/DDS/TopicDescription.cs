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
using OpenDDSharp.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// Default implementation of the <see cref="ITopicDescription" /> interface.
    /// It is the base class for <see cref="ContentFilteredTopic" />, and <see cref="MultiTopic" />.
    /// </summary>
    public class TopicDescription : ITopicDescription
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
        internal TopicDescription(IntPtr native)
        {
            _native = native;
            _nativeTopicDescription = NarrowTopicDescription(native);
        }
        #endregion

        #region Methods
        internal static IntPtr NarrowTopicDescription(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowTopicDescription86(ptr),
                                               () => UnsafeNativeMethods.NarrowTopicDescription64(ptr));
        }

        private string GetTypeName()
        {
            return MarshalHelper.ExecuteAnyCpu(() => Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetTypeName86(_native)),
                                               () => Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetTypeName64(_native)));
        }

        private string GetName()
        {
            return MarshalHelper.ExecuteAnyCpu(() => Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetName86(_native)),
                                               () => Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetName64(_native)));
        }

        private DomainParticipant GetParticipant()
        {
            IntPtr ptrParticipant = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetParticipant86(_native),
                                                                () => UnsafeNativeMethods.GetParticipant64(_native));

            DomainParticipant participant = null;

            if (!ptrParticipant.Equals(IntPtr.Zero))
            {
                IntPtr ptr = DomainParticipant.NarrowBase(ptrParticipant);

                Entity entity = EntityManager.Instance.Find(ptr);
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "TopicDescription_NarrowTopicDescription", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowTopicDescription64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "TopicDescription_NarrowTopicDescription", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowTopicDescription86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "TopicDescription_GetTypeName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetTypeName64(IntPtr t);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "TopicDescription_GetTypeName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetTypeName86(IntPtr t);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "TopicDescription_GetName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetName64(IntPtr t);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "TopicDescription_GetName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetName86(IntPtr t);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "TopicDescription_GetParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetParticipant64(IntPtr t);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "TopicDescription_GetParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetParticipant86(IntPtr t);
        }
        #endregion
    }
}