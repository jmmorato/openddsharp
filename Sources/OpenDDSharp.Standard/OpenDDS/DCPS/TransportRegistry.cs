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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.DDS;
using OpenDDSharp.Helpers;

namespace OpenDDSharp.OpenDDS.DCPS
{
    /// <summary>
    /// The TransportRegistry is a singleton object which provides a mechanism to
    /// the application code to configure OpenDDS's use of the transport layer.
    /// </summary>
    public class TransportRegistry
    {
        #region Fields
        private static readonly object _lock = new object();
        private static TransportRegistry _instance;
        #endregion

        #region Singleton
        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static TransportRegistry Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new TransportRegistry();
                    }

                    return _instance;
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether the <see cref="TransportRegistry" /> has been released or not.
        /// </summary>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public bool Released => UnsafeNativeMethods.GetReleased();

        /// <summary>
        /// Gets or sets the global <see cref="TransportConfig" />.
        /// </summary>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public TransportConfig GlobalConfig
        {
            get => GetGlobalConfig();
            set => SetGlobalConfig(value);
        }
        #endregion

        #region Constructors
        private TransportRegistry() { }
        #endregion

        #region Methods
        /// <summary>
        /// Close the singleton instance of this class.
        /// </summary>
        public static void Close()
        {
            UnsafeNativeMethods.NativeClose();
        }

        /// <summary>
        /// This will shutdown all TransportImpl objects.
        /// Client Application calls this method to tear down the transport framework.
        /// </summary>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public void Release()
        {
            UnsafeNativeMethods.Release();
        }

        /// <summary>
        /// Creates a new <see cref="TransportInst" />.
        /// </summary>
        /// <param name="name">A unique name for the transport instance.</param>
        /// <param name="transportType">The transport type for the instance. It should be one of the included transports (i.e. tcp, udp, multicast, shmem, and rtps_udp).</param>
        /// <returns>The newly created <see cref="TransportInst" /> or null if failed.</returns>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public TransportInst CreateInst(string name, string transportType)
        {
            IntPtr ptr = UnsafeNativeMethods.CreateInst(name, transportType);

            if (ptr != IntPtr.Zero)
            {
                return new TransportInst(ptr);
            }

            return null;
        }

        /// <summary>
        /// Gets an already created <see cref="TransportInst" />.
        /// </summary>
        /// <param name="name">The name given to the <see cref="TransportInst" /> during the creation.</param>
        /// <returns>The <see cref="TransportInst" /> or null if not found.</returns>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public TransportInst GetInst(string name)
        {
            IntPtr ptr = UnsafeNativeMethods.GetInst(name);

            if (ptr != IntPtr.Zero)
            {
                return new TransportInst(ptr);
            }

            return null;
        }

        /// <summary>
        /// Removes a <see cref="TransportInst" />.
        /// </summary>
        /// <param name="inst">The <see cref="TransportInst" /> to be removed.</param>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public void RemoveInst(TransportInst inst)
        {
            if (inst == null)
            {
                throw new ArgumentNullException(nameof(inst));
            }

            UnsafeNativeMethods.RemoveInst(inst.ToNative());
        }

        /// <summary>
        /// Creates a new <see cref="TransportConfig" />.
        /// </summary>
        /// <param name="name">A unique name for the config.</param>
        /// <returns>The newly created <see cref="TransportConfig" /> or null if failed.</returns>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public TransportConfig CreateConfig(string name)
        {
            IntPtr ptr = UnsafeNativeMethods.CreateConfig(name);

            if (ptr != IntPtr.Zero)
            {
                return new TransportConfig(ptr);
            }

            return null;
        }

        /// <summary>
        /// Gets an already created <see cref="TransportConfig" />.
        /// </summary>
        /// <param name="name">The name given to the <see cref="TransportConfig" /> during the creation.</param>
        /// <returns>The <see cref="TransportConfig" /> or null if not found.</returns>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public TransportConfig GetConfig(string name)
        {
            IntPtr ptr = UnsafeNativeMethods.GetConfig(name);
            if (ptr != IntPtr.Zero)
            {
                return new TransportConfig(ptr);
            }

            return null;
        }

        /// <summary>
        /// Removes a <see cref="TransportConfig" />.
        /// </summary>
        /// <param name="cfg">The <see cref="TransportConfig" /> to be removed.</param>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public void RemoveConfig(TransportConfig cfg)
        {
            if (cfg == null)
            {
                throw new ArgumentNullException(nameof(cfg));
            }

            UnsafeNativeMethods.RemoveConfig(cfg.ToNative());
        }

        /// <summary>
        /// Gets the specific domain default <see cref="TransportConfig" />.
        /// </summary>
        /// <param name="domain">The requested default <see cref="TransportConfig" /> domain id.</param>
        /// <returns>The default <see cref="TransportConfig" /> domain id if found, otherwise null.</returns>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public TransportConfig GetDomainDefaultConfig(int domain)
        {
            IntPtr ptr = UnsafeNativeMethods.GetDomainDefaultConfig(domain);

            if (ptr != IntPtr.Zero)
            {
                return new TransportConfig(ptr);
            }

            return null;
        }

        /// <summary>
        /// Sets the specific domain default <see cref="TransportConfig" />.
        /// </summary>
        /// <param name="domain">The domain id where the default <see cref="TransportConfig" /> will be applied.</param>
        /// <param name="cfg">The <see cref="TransportConfig" /> to be set.</param>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public void SetDomainDefaultConfig(int domain, TransportConfig cfg)
        {
            if (cfg == null)
            {
                throw new ArgumentNullException(nameof(cfg));
            }

            UnsafeNativeMethods.SetDomainDefaultConfig(domain, cfg.ToNative());
        }

        /// <summary>
        /// Binds a <see cref="TransportConfig" /> to a <see cref="Entity" />.
        /// </summary>
        /// <param name="name">The name given to the <see cref="TransportConfig" /> during the creation.</param>
        /// <param name="entity">The <see cref="Entity" /> to be bound.</param>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public void BindConfig(string name, Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            IntPtr ptr = entity.ToNative();
            if (ptr != IntPtr.Zero)
            {
                UnsafeNativeMethods.BindConfigName(name, ptr);
            }
        }

        /// <summary>
        /// Binds a <see cref="TransportConfig" /> to a <see cref="Entity" />.
        /// </summary>
        /// <param name="cfg">The <see cref="TransportConfig" /> to be applied.</param>
        /// <param name="entity">The <see cref="Entity" /> to be bound.</param>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
        public void BindConfig(TransportConfig cfg, Entity entity)
        {
            if (cfg == null)
            {
                throw new ArgumentNullException(nameof(cfg));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            UnsafeNativeMethods.BindConfigTransport(cfg.ToNative(), entity.ToNative());
        }

        private static TransportConfig GetGlobalConfig()
        {
            IntPtr ptr = UnsafeNativeMethods.NativeGetGlobalConfig();

            if (ptr != IntPtr.Zero)
            {
                return new TransportConfig(ptr);
            }

            return null;
        }

        private static void SetGlobalConfig(TransportConfig cfg)
        {
            if (cfg == null)
            {
                throw new ArgumentNullException(nameof(cfg));
            }

            UnsafeNativeMethods.NativeSetGlobalConfig(cfg.ToNative());
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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_Close", CallingConvention = CallingConvention.Cdecl)]
            public static extern void NativeClose();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_Release", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Release();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_CreateInst", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr CreateInst(string name, string transportType);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_GetInst", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetInst(string name);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_RemoveInst", CallingConvention = CallingConvention.Cdecl)]
            public static extern void RemoveInst(IntPtr inst);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_CreateConfig", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr CreateConfig(string name);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_GetConfig", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetConfig(string name);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_RemoveConfig", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr RemoveConfig(IntPtr inst);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_GetDomainDefaultConfig", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetDomainDefaultConfig(int domain);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_SetDomainDefaultConfig", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetDomainDefaultConfig(int domain, IntPtr cfg);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_BindConfigName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern void BindConfigName(string name, IntPtr cfg);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_BindConfigTransport", CallingConvention = CallingConvention.Cdecl)]
            public static extern void BindConfigTransport(IntPtr tc, IntPtr cfg);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_GetGlobalConfig", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr NativeGetGlobalConfig();

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_SetGlobalConfig", CallingConvention = CallingConvention.Cdecl)]
            public static extern void NativeSetGlobalConfig(IntPtr cfg);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_GetReleased", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.U1)]
            public static extern bool GetReleased();
        }
        #endregion
    }
}
