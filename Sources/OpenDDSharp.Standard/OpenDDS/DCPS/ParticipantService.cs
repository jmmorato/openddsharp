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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.DDS;
using OpenDDSharp.Helpers;

namespace OpenDDSharp.OpenDDS.DCPS
{
    /// <summary>
    /// Singleton object to obtain the <see cref="DomainParticipantFactory" />.
    /// </summary>
    public sealed class ParticipantService
    {
        #region Fields
        private static readonly object _lock = new object();
        private static ParticipantService _instance;
        #endregion

        #region Singleton
        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static ParticipantService Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ParticipantService();
                    }

                    return _instance;
                }
            }
        }
        #endregion

        #region Constructors
        private ParticipantService()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether the participant has been shutdown or not.
        /// </summary>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public bool IsShutdown => GetIsShutdown();

        /// <summary>
        /// Gets or sets the default discovery.
        /// </summary>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public string DefaultDiscovery
        {
            get => GetDefaultDiscovery();
            set => SetDefaultDiscovery(value);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the DDS client environment and get the <see cref="DomainParticipantFactory" />.
        /// </summary>
        /// <returns> The <see cref="DomainParticipantFactory" />.</returns>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public DomainParticipantFactory GetDomainParticipantFactory()
        {
            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDomainParticipantFactory86(),
                                                        () => UnsafeNativeMethods.GetDomainParticipantFactory64());

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new DomainParticipantFactory(native);
        }

        /// <summary>
        /// Initialize the DDS client environment and get the <see cref="DomainParticipantFactory" />.
        /// This method consumes -DCPS* and -ORB* options and their arguments.
        /// </summary>
        /// <param name="args">The array of parameters to be consumed (i.e. -DCPS* and -ORB* options).</param>
        /// <returns> The <see cref="DomainParticipantFactory" />.</returns>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public DomainParticipantFactory GetDomainParticipantFactory(params string[] args)
        {
            int argc = args.Length + 1;
            string[] argv = new string[argc];

            // Don't need the program name (can't be NULL though, else ACE_Arg_Shifter fails)
            argv[0] = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                argv[i + 1] = args[i];
            }

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDomainParticipantFactory86(argc, argv),
                                                        () => UnsafeNativeMethods.GetDomainParticipantFactory64(argc, argv));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new DomainParticipantFactory(native);
        }

        /// <summary>
        /// Add a new <see cref="Discovery" />.
        /// </summary>
        /// <param name="discovery">The <see cref="Discovery" /> to be added.</param>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public void AddDiscovery(Discovery discovery)
        {
            if (discovery == null)
            {
                throw new ArgumentNullException(nameof(discovery));
            }

            MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.AddDiscovery86(discovery.ToNative()),
                                        () => UnsafeNativeMethods.AddDiscovery64(discovery.ToNative()));
        }

        /// <summary>
        /// Set the discovery repository for a specifi domain id.
        /// </summary>
        /// <param name="domain">The domain id.</param>
        /// <param name="repo">The repository key.</param>
        public void SetRepoDomain(int domain, string repo)
        {
            SetRepoDomain(domain, repo, true);
        }

        /// <summary>
        /// Set the discovery repository for a specifi domain id.
        /// </summary>
        /// <param name="domain">The domain id.</param>
        /// <param name="repo">The repository key.</param>
        /// <param name="attachParticipant">Indicates if the current participant should be attached to the new repository.</param>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public void SetRepoDomain(int domain, string repo, bool attachParticipant)
        {
            MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetRepoDomain86(domain, repo, attachParticipant),
                                        () => UnsafeNativeMethods.SetRepoDomain64(domain, repo, attachParticipant));
        }

        /// <summary>
        /// Stop being a participant in the service.
        /// </summary>
        /// <remarks>
        /// Required Precondition: All DomainParticipants have been deleted.
        /// </remarks>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public void Shutdown()
        {
            MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.Shutdown86(),
                                        () => UnsafeNativeMethods.Shutdown64());
        }

        private static bool GetIsShutdown()
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetIsShutdown86(),
                                               () => UnsafeNativeMethods.GetIsShutdown64());
        }

        private static string GetDefaultDiscovery()
        {
            IntPtr ptr = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDefaultDiscovery86(),
                                                     () => UnsafeNativeMethods.GetDefaultDiscovery64());

            string defaultDiscovery = Marshal.PtrToStringAnsi(ptr);
            ptr.ReleaseNativePointer();

            return defaultDiscovery;
        }

        private static void SetDefaultDiscovery(string value)
        {
            MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetDefaultDiscovery86(value),
                                        () => UnsafeNativeMethods.SetDefaultDiscovery64(value));
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ParticipantService_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void ParticipantServiceNew64();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ParticipantService_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void ParticipantServiceNew86();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ParticipantService_GetDomainParticipantFactory", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetDomainParticipantFactory64();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ParticipantService_GetDomainParticipantFactoryParameters", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetDomainParticipantFactory64(int argc, string[] argv);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ParticipantService_GetDomainParticipantFactory")]
            public static extern IntPtr GetDomainParticipantFactory86();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ParticipantService_GetDomainParticipantFactoryParameters", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetDomainParticipantFactory86(int argc, string[] argv);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ParticipantService_AddDiscovery", CallingConvention = CallingConvention.Cdecl)]
            public static extern void AddDiscovery64(IntPtr discovery);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ParticipantService_AddDiscovery", CallingConvention = CallingConvention.Cdecl)]
            public static extern void AddDiscovery86(IntPtr discovery);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ParticipantService_GetDefaultDiscovery", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetDefaultDiscovery64();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ParticipantService_GetDefaultDiscovery", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetDefaultDiscovery86();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ParticipantService_SetDefaultDiscovery", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern void SetDefaultDiscovery64(string defaultDiscovery);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ParticipantService_SetDefaultDiscovery", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr SetDefaultDiscovery86(string defaultDiscovery);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ParticipantService_SetRepoDomain", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern void SetRepoDomain64(int domain, string repo, bool attachParticipant);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ParticipantService_SetRepoDomain", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern void SetRepoDomain86(int domain, string repo, bool attachParticipant);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ParticipantService_Shutdown", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Shutdown64();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ParticipantService_Shutdown", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Shutdown86();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "ParticipantService_GetIsShutdown", CallingConvention = CallingConvention.Cdecl)]
            public static extern bool GetIsShutdown64();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "ParticipantService_GetIsShutdown", CallingConvention = CallingConvention.Cdecl)]
            public static extern bool GetIsShutdown86();
        }
        #endregion
    }
}
