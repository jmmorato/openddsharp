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
    /// A GuardCondition object is a specific <see cref="Condition" /> whose <see cref="Condition.TriggerValue" /> is completely under the control of the application.
    /// </summary>
    /// <remarks>
    /// <para>GuardCondition has no factory. When first created the <see cref="Condition.TriggerValue" /> is set to <see langword="false"/>.</para>
    /// <para>The purpose of the GuardCondition is to provide the means for the application to manually wakeup a <see cref="WaitSet" />. This is
    /// accomplished by attaching the GuardCondition to the <see cref="WaitSet" /> and then setting the <see cref="Condition.TriggerValue" /> by means of the
    /// <see cref="TriggerValue" /> set operation.</para>
    /// </remarks>
    public class GuardCondition : Condition
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether the trigger value of the <see cref="GuardCondition" /> is active or not.
        /// </summary>
        public new bool TriggerValue
        {
            get => GetTriggerValue();
            set => SetTriggerValue(value);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GuardCondition"/> class.
        /// </summary>
        public GuardCondition() : this(CreateGuardCondition())
        {
        }

        internal GuardCondition(IntPtr native) : base(NarrowBase(native))
        {
            _native = native;
        }
        #endregion

        #region Methods
        private static IntPtr CreateGuardCondition()
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateGuardCondition86(),
                                               () => UnsafeNativeMethods.CreateGuardCondition64());
        }

        private static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
                                               () => UnsafeNativeMethods.NarrowBase64(ptr));
        }

        private bool GetTriggerValue()
        {
            throw new NotImplementedException();
        }

        private void SetTriggerValue(bool value)
        {
            throw new NotImplementedException();
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "GuardCondition_CreateGuardConditione", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateGuardCondition64();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "GuardCondition_CreateGuardCondition", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateGuardCondition86();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "GuardCondition_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "GuardCondition_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);
        }
        #endregion
    }
}
