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

namespace OpenDDSharp.OpenDDS.DCPS
{
    /// <summary>
    /// Represent a DCPSInfoRepository discovery.
    /// </summary>
    /// <remarks>
    /// An OpenDDS DCPSInfoRepo is a service on a local or remote node used for participant
    /// discovery. Confguring how participants should fnd DCPSInfoRepo is the purpose of this class.
    /// </remarks>
    public class InfoRepoDiscovery : Discovery
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoRepoDiscovery"/> class.
        /// </summary>
        /// <param name="key">Unique key value for the repository.</param>
        /// <param name="ior">Repository IOR or host:port.</param>
        public InfoRepoDiscovery(string key, string ior)
        {
            _native = UnsafeNativeMethods.InfoRepoDiscoveryNew(key, ior);
            FromNative(UnsafeNativeMethods.NarrowBase(_native));
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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_new", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr InfoRepoDiscoveryNew(string key, string ior);
        }
        #endregion
    }
}
