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
    /// ReadCondition objects are conditions specifically dedicated to read operations and attached to one <see cref="DataReader" />.
    /// </summary>
    /// <remarks>
    /// ReadCondition objects allow an application to specify the data samples it is interested in (by specifying the desired sample-states,
    /// view-states, and instance-states). This allows the middleware to enable the condition only when suitable information is available.
    /// They are to be used in conjunction with a WaitSet as normal conditions. More than one ReadCondition may be attached to the same <see cref="DataReader" />.
    /// </remarks>
    public class ReadCondition : Condition
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the set of sample-states that are taken into account to determine the trigger value of the <see cref="ReadCondition" />.
        /// These are the sample-states specified when the <see cref="ReadCondition" /> was created.
        /// </summary>
        public SampleStateMask SampleStateMask => GetSampleStateMask();

        /// <summary>
        /// Gets the set of view-states that are taken into account to determine the trigger value of the <see cref="ReadCondition" />.
        /// These are the view-states specified when the <see cref="ReadCondition" /> was created.
        /// </summary>
        public ViewStateMask ViewStateMask => GetViewStateMask();

        /// <summary>
        /// Gets the set of instance-states that are taken into account to determine the trigger value of the <see cref="ReadCondition" />.
        /// These are the instance-states specified when the ReadCondition was created.
        /// </summary>
        public InstanceStateMask InstanceStateMask => GetInstanceStateMask();

        /// <summary>
        /// Gets the <see cref="DataReader" /> associated with the <see cref="ReadCondition" />.
        /// </summary>
        /// <remarks>
        /// Note that there is exactly one <see cref="DataReader" /> associated with each <see cref="ReadCondition" />.
        /// </remarks>
        public DataReader DataReader { get; }
        #endregion

        #region Constructors
        internal ReadCondition(IntPtr native, DataReader reader) : base(NarrowBase(native))
        {
            _native = native;
            DataReader = reader;
        }
        #endregion

        #region Methods
        private static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
                                               () => UnsafeNativeMethods.NarrowBase64(ptr));
        }

        private SampleStateMask GetSampleStateMask()
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetSampleStateMask86(_native),
                                               () => UnsafeNativeMethods.GetSampleStateMask64(_native));
        }

        private ViewStateMask GetViewStateMask()
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetViewStateMask86(_native),
                                               () => UnsafeNativeMethods.GetViewStateMask64(_native));
        }

        private InstanceStateMask GetInstanceStateMask()
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetInstanceStateMask86(_native),
                                               () => UnsafeNativeMethods.GetInstanceStateMask64(_native));
        }

        internal void Release()
        {
            MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.Release86(_native),
                                        () => UnsafeNativeMethods.Release64(_native));
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ReadCondition_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ReadCondition_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ReadCondition_Release", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Release64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ReadCondition_Release", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Release86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ReadCondition_GetSampleStateMask", CallingConvention = CallingConvention.Cdecl)]
            public static extern SampleStateMask GetSampleStateMask64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ReadCondition_GetSampleStateMask", CallingConvention = CallingConvention.Cdecl)]
            public static extern SampleStateMask GetSampleStateMask86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ReadCondition_GetViewStateMask", CallingConvention = CallingConvention.Cdecl)]
            public static extern ViewStateMask GetViewStateMask64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ReadCondition_GetViewStateMask", CallingConvention = CallingConvention.Cdecl)]
            public static extern ViewStateMask GetViewStateMask86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ReadCondition_GetInstanceStateMask", CallingConvention = CallingConvention.Cdecl)]
            public static extern InstanceStateMask GetInstanceStateMask64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ReadCondition_GetInstanceStateMask", CallingConvention = CallingConvention.Cdecl)]
            public static extern InstanceStateMask GetInstanceStateMask86(IntPtr ptr);
        }
        #endregion
    }
}
