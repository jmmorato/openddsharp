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

namespace OpenDDSharp.OpenDDS.DCPS
{
    /// <summary>
    /// Represents a transport configuration.
    /// </summary>
    public class TransportConfig
    {
        #region Constants
        /// <summary>
        /// The default passive connection duration
        /// </summary>
        public const uint DEFAULT_PASSIVE_CONNECT_DURATION = 60000U;
        #endregion

        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the configuration unique name.
        /// </summary>
        public string Name
        {
            get => GetName();
        }

        /// <summary>
        /// Gets the ordered list of transport instances that
        /// this configuration will utilize.
        /// </summary>
        public IReadOnlyCollection<TransportInst> Transports
        {
            get => GetTransports();
        }

        /// <summary>
        /// Gets or sets a value indicating whether a value of false causes DDS to serialize data in the
        /// source machine's native endianness; a value of true
        /// causes DDS to serialize data in the opposite
        /// endianness. The receiving side will adjust the data
        /// for its endianness so there is no need to match
        /// this option between machines. The purpose of this
        /// option is to allow the developer to decide which
        /// side will make the endian adjustment, if necessary.
        /// The default value is false.
        /// </summary>
        public bool SwapBytes
        {
            get => GetSwapBytes();
            set => SetSwapBytes(value);
        }

        /// <summary>
        /// Gets or sets the timeout (milliseconds) for initial passive
        /// connection establishment. By default, this option
        /// waits for 60 seconds. A value of zero would wait
        /// indefnitely (not recommended).
        /// </summary>
        public uint PassiveConnectDuration
        {
            get => GetPassiveConnectDuration();
            set => SetPassiveConnectDuration(value);
        }
        #endregion

        #region Constructors
        internal TransportConfig(IntPtr native)
        {
            _native = native;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Insert the <see cref="TransportInst" /> in the instances list.
        /// </summary>
        /// <param name="inst">The <see cref="TransportInst" /> to be inserted.</param>
        public void Insert(TransportInst inst)
        {
            if (inst == null)
            {
                throw new ArgumentNullException(nameof(inst));
            }

            UnsafeNativeMethods.Insert(_native, inst.ToNative());
        }

        /// <summary>
        /// Insert the <see cref="TransportInst" /> in sorted order (by name) in the instances_ list.
        /// Use when the names of the TransportInst objects are specifically assigned
        /// to have the sorted order make sense.
        /// </summary>
        /// <param name="inst">The <see cref="TransportInst" /> to be inserted.</param>
        public void SortedInsert(TransportInst inst)
        {
            if (inst == null)
            {
                throw new ArgumentNullException(nameof(inst));
            }

            UnsafeNativeMethods.SortedInsert(_native, inst.ToNative());
        }

        private string GetName()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetName(_native));
        }

        private IReadOnlyCollection<TransportInst> GetTransports()
        {
            var ret = new List<TransportInst>();

            IntPtr seqPtr = UnsafeNativeMethods.GetTransports(_native);

            IList<IntPtr> pointers = new List<IntPtr>();
            seqPtr.PtrToSequence(ref pointers);

            foreach (var ptr in pointers)
            {
                TransportInst managed = TransportInstManager.Instance.Find(ptr);
                if (managed == null)
                {
                    managed = new TransportInst(ptr);
                    TransportInstManager.Instance.Add(ptr, managed);
                }

                ret.Add(managed);
            }
            return ret.AsReadOnly();
        }

        private bool GetSwapBytes()
        {
            return UnsafeNativeMethods.GetSwapBytes(_native);
        }

        private void SetSwapBytes(bool value)
        {
            UnsafeNativeMethods.SetSwapBytes(_native, value);
        }

        private uint GetPassiveConnectDuration()
        {
            return UnsafeNativeMethods.GetPassiveConnectDuration(_native);
        }

        private void SetPassiveConnectDuration(uint value)
        {
            UnsafeNativeMethods.SetPassiveConnectDuration(_native, value);
        }

        internal IntPtr ToNative()
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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportConfig_Insert", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Insert(IntPtr cfg, IntPtr inst);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportConfig_SortedInsert", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SortedInsert(IntPtr cfg, IntPtr inst);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportConfig_GetSwapBytes", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetSwapBytes(IntPtr cfg);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportConfig_SetSwapBytes", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetSwapBytes(IntPtr cfg, [MarshalAs(UnmanagedType.I1)] bool value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportConfig_GetPassiveConnectDuration", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint GetPassiveConnectDuration(IntPtr cfg);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportConfig_SetPassiveConnectDuration", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetPassiveConnectDuration(IntPtr cfg, uint value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportConfig_GetName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetName(IntPtr cfg);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportConfig_GetTransports", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetTransports(IntPtr cfg);
        }
        #endregion
    }
}
