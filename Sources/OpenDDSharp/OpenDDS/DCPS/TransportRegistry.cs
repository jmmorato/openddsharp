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

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.OpenDDS.DCPS;

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
                return _instance ??= new TransportRegistry();
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
    private TransportRegistry()
    {
    }
    #endregion

    #region Methods
    /// <summary>
    /// Close the singleton instance of this class.
    /// </summary>
    public static void Close()
    {
        UnsafeNativeMethods.NativeClose();
        TransportInstManager.Instance.Clear();
        TransportConfigManager.Instance.Clear();
    }

    /// <summary>
    /// This will shutdown all TransportImpl objects.
    /// Client Application calls this method to tear down the transport framework.
    /// </summary>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
    public void Release()
    {
        UnsafeNativeMethods.Release();
        TransportInstManager.Instance.Clear();
        TransportConfigManager.Instance.Clear();
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
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "The transport instance's name cannot be null or an empty string.");
        }

        if (string.IsNullOrWhiteSpace(transportType))
        {
            throw new ArgumentNullException(nameof(transportType), "The transport type cannot be null or an empty string.");
        }

        var ptr = UnsafeNativeMethods.CreateInst(name, transportType);

        if (ptr == IntPtr.Zero)
        {
            return null;
        }

        var inst = new TransportInst(ptr);
        TransportInstManager.Instance.Add(ptr, inst);

        return inst;
    }

    /// <summary>
    /// Gets an already created <see cref="TransportInst" />.
    /// </summary>
    /// <param name="name">The name given to the <see cref="TransportInst" /> during the creation.</param>
    /// <returns>The <see cref="TransportInst" /> or null if not found.</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
    public TransportInst GetInst(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var ptr = UnsafeNativeMethods.GetInst(name);

        if (ptr == IntPtr.Zero)
        {
            return null;
        }

        var inst = TransportInstManager.Instance.Find(ptr);
        if (inst == null)
        {
            inst = new TransportInst(ptr);
            TransportInstManager.Instance.Add(ptr, inst);
        }

        return inst;
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
            return;
        }

        UnsafeNativeMethods.RemoveInst(inst.ToNative());
        TransportInstManager.Instance.Remove(inst.ToNative());
    }

    /// <summary>
    /// Creates a new <see cref="TransportConfig" />.
    /// </summary>
    /// <param name="name">A unique name for the config.</param>
    /// <returns>The newly created <see cref="TransportConfig" /> or null if failed.</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
    public TransportConfig CreateConfig(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "The configuration name cannot be null or an empty string.");
        }

        var ptr = UnsafeNativeMethods.CreateConfig(name);

        if (ptr == IntPtr.Zero)
        {
            return null;
        }

        var config = new TransportConfig(ptr);
        TransportConfigManager.Instance.Add(ptr, config);

        return config;
    }

    /// <summary>
    /// Gets an already created <see cref="TransportConfig" />.
    /// </summary>
    /// <param name="name">The name given to the <see cref="TransportConfig" /> during the creation.</param>
    /// <returns>The <see cref="TransportConfig" /> or null if not found.</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
    public TransportConfig GetConfig(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        IntPtr ptr = UnsafeNativeMethods.GetConfig(name);
        if (ptr == IntPtr.Zero)
        {
            return null;
        }

        var config = TransportConfigManager.Instance.Find(ptr);
        if (config == null)
        {
            config = new TransportConfig(ptr);
            TransportConfigManager.Instance.Add(ptr, config);
        }

        return config;
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
            return;
        }

        var native = cfg.ToNative();
        UnsafeNativeMethods.RemoveConfig(native);
        TransportConfigManager.Instance.Remove(native);
    }

    /// <summary>
    /// Gets the specific domain default <see cref="TransportConfig" />.
    /// </summary>
    /// <param name="domain">The requested default <see cref="TransportConfig" /> domain id.</param>
    /// <returns>The default <see cref="TransportConfig" /> domain id if found, otherwise null.</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
    public TransportConfig GetDomainDefaultConfig(int domain)
    {
        if (domain < 0)
        {
            return null;
        }

        IntPtr ptr = UnsafeNativeMethods.GetDomainDefaultConfig(domain);

        if (ptr == IntPtr.Zero)
        {
            return null;
        }

        var cfg = TransportConfigManager.Instance.Find(ptr);
        if (cfg == null)
        {
            cfg = new TransportConfig(ptr);
            TransportConfigManager.Instance.Add(ptr, cfg);
        }

        return cfg;
    }

    /// <summary>
    /// Sets the specific domain default <see cref="TransportConfig" />.
    /// </summary>
    /// <param name="domain">The domain id where the default <see cref="TransportConfig" /> will be applied.</param>
    /// <param name="cfg">The <see cref="TransportConfig" /> to be set.</param>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
    public void SetDomainDefaultConfig(int domain, TransportConfig cfg)
    {
        if (domain < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(domain), "The domain must be greater or equal to zero.");
        }

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
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "The transport config's name cannot be null or an empty string.");
        }

        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var ptr = entity.ToNative();
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

        if (ptr == IntPtr.Zero)
        {
            return null;
        }

        var cfg = TransportConfigManager.Instance.Find(ptr);
        if (cfg == null)
        {
            cfg = new TransportConfig(ptr);
            TransportConfigManager.Instance.Add(ptr, cfg);
        }

        return cfg;
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
}

/// <summary>
/// This class suppresses stack walks for unmanaged code permission.
/// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full
/// security review to make sure that the usage
/// is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_Close")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void NativeClose();

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_Release")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void Release();

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_CreateInst", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr CreateInst(string name, string transportType);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_GetInst", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetInst(string name);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_RemoveInst")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void RemoveInst(IntPtr inst);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_CreateConfig", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr CreateConfig(string name);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_GetConfig", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetConfig(string name);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_RemoveConfig")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void RemoveConfig(IntPtr inst);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_GetDomainDefaultConfig")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetDomainDefaultConfig(int domain);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_SetDomainDefaultConfig")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetDomainDefaultConfig(int domain, IntPtr cfg);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_BindConfigName", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void BindConfigName(string name, IntPtr cfg);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_BindConfigTransport")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void BindConfigTransport(IntPtr tc, IntPtr cfg);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_GetGlobalConfig")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr NativeGetGlobalConfig();

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_SetGlobalConfig")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void NativeSetGlobalConfig(IntPtr cfg);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TransportRegistry_GetReleased")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool GetReleased();
#else
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
    public static extern void RemoveConfig(IntPtr inst);

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
#endif
}