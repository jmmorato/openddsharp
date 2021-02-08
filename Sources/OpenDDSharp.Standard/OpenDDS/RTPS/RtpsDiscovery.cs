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
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.OpenDDS.RTPS
{
    /// <summary>
    /// Discovery Strategy class that implements RTPS discovery.
    /// This class implements the Discovery interface for Rtps-based discovery.
    /// </summary>
    /// <remarks>
    /// <para>The RTPS specifcation splits up the discovery protocol into two independent protocols:</para>
    /// <para>    1. Participant Discovery Protocol.</para>
    /// <para>    2. Endpoint Discovery Protocol.</para>
    /// <para>A Participant Discovery Protocol (PDP) specifes how Participants discover each other in the
    /// network. Once two Participants have discovered each other, they exchange information on the
    /// Endpoints they contain using an Endpoint Discovery Protocol (EDP). Apart from this causality
    /// relationship, both protocols can be considered independent.</para>
    /// </remarks>
    public class RtpsDiscovery : Discovery
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RtpsDiscovery"/> class.
        /// </summary>
        /// <param name="key">The discovery unique key.</param>
        public RtpsDiscovery(string key)
        {
            _native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.RtpsDiscoveryNew86(key), () => UnsafeNativeMethods.RtpsDiscoveryNew64(key));
            FromNative(NarrowBase(_native));
        }
        #endregion

        #region Methods
        private static IntPtr NarrowBase(IntPtr ptr)
        {
            if (Environment.Is64BitProcess)
            {
                return UnsafeNativeMethods.NarrowBase64(ptr);
            }
            else
            {
                return UnsafeNativeMethods.NarrowBase86(ptr);
            }
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "RtpsDiscovery_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "RtpsDiscovery_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);


            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "RtpsDiscovery_new", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr RtpsDiscoveryNew64(string key);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "RtpsDiscovery_new", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr RtpsDiscoveryNew86(string key);
        }
        #endregion
    }
}
