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

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.OpenDDS.DCPS;

/// <summary>
/// Singleton object to obtain the <see cref="DomainParticipantFactory" />.
/// </summary>
public sealed class ParticipantService
{
    #region Fields
    private static readonly object _lock = new ();
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
                return _instance ??= new ParticipantService();
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
    public bool IsShutdown => UnsafeNativeMethods.GetIsShutdown();

    /// <summary>
    /// Gets or sets the default discovery.
    /// </summary>
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
        var native = UnsafeNativeMethods.GetDomainParticipantFactory();

        return native.Equals(IntPtr.Zero) ? null : new DomainParticipantFactory(native);
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
        var argc = args.Length + 1;
        var argv = new string[argc];

        // Don't need the program name (can't be NULL though, else ACE_Arg_Shifter fails)
        argv[0] = string.Empty;
        for (var i = 0; i < args.Length; i++)
        {
            argv[i + 1] = args[i];
        }

        var native = UnsafeNativeMethods.GetDomainParticipantFactory(argc, argv);

        return native.Equals(IntPtr.Zero) ? null : new DomainParticipantFactory(native);
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

        UnsafeNativeMethods.AddDiscovery(discovery.ToNative());
    }

    /// <summary>
    /// Set the discovery repository for a specific domain id.
    /// </summary>
    /// <param name="domain">The domain id.</param>
    /// <param name="repo">The repository key.</param>
    public void SetRepoDomain(int domain, string repo)
    {
        SetRepoDomain(domain, repo, true);
    }

    /// <summary>
    /// Set the discovery repository for a specific domain id.
    /// </summary>
    /// <param name="domain">The domain id.</param>
    /// <param name="repo">The repository key.</param>
    /// <param name="attachParticipant">Indicates if the current participant should be attached to the new repository.</param>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
    public void SetRepoDomain(int domain, string repo, bool attachParticipant)
    {
        UnsafeNativeMethods.SetRepoDomain(domain, repo, attachParticipant);
    }

    /// <summary>
    /// Stop being a participant in the service.
    /// </summary>
    /// <remarks>
    /// Required Precondition: All DomainParticipants have been deleted.
    /// </remarks>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "We keep the singleton access to match OpenDDS API.")]
    public ReturnCode Shutdown()
    {
        return UnsafeNativeMethods.Shutdown();
    }

    private static string GetDefaultDiscovery()
    {
        var ptr = UnsafeNativeMethods.NativeGetDefaultDiscovery();

        var defaultDiscovery = Marshal.PtrToStringAnsi(ptr);
        ptr.ReleaseNativePointer();

        return defaultDiscovery;
    }

    private static void SetDefaultDiscovery(string value)
    {
        UnsafeNativeMethods.NativeSetDefaultDiscovery(value);
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
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_new")]
    public static partial void ParticipantServiceNew();

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_GetDomainParticipantFactory")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetDomainParticipantFactory();

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_GetDomainParticipantFactoryParameters", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetDomainParticipantFactory(int argc, string[] argv);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_AddDiscovery")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void AddDiscovery(IntPtr discovery);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_GetDefaultDiscovery")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr NativeGetDefaultDiscovery();

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_SetDefaultDiscovery", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void NativeSetDefaultDiscovery(string defaultDiscovery);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_SetRepoDomain", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetRepoDomain(int domain, string repo, [MarshalAs(UnmanagedType.I1)]bool attachParticipant);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_Shutdown")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ReturnCode Shutdown();

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_GetIsShutdown")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool GetIsShutdown();
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_new", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ParticipantServiceNew();

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_GetDomainParticipantFactory", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetDomainParticipantFactory();

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_GetDomainParticipantFactoryParameters", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr GetDomainParticipantFactory(int argc, string[] argv);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_AddDiscovery", CallingConvention = CallingConvention.Cdecl)]
    public static extern void AddDiscovery(IntPtr discovery);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_GetDefaultDiscovery", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr NativeGetDefaultDiscovery();

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_SetDefaultDiscovery", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern void NativeSetDefaultDiscovery(string defaultDiscovery);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_SetRepoDomain", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern void SetRepoDomain(int domain, string repo, [MarshalAs(UnmanagedType.I1)]bool attachParticipant);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_Shutdown", CallingConvention = CallingConvention.Cdecl)]
    public static extern ReturnCode Shutdown();

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "ParticipantService_GetIsShutdown", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static extern bool GetIsShutdown();
#endif
}