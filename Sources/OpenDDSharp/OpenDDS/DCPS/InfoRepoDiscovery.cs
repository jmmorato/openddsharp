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
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.OpenDDS.DCPS;

/// <summary>
/// Represent a DCPSInfoRepository discovery.
/// </summary>
/// <remarks>
/// An OpenDDS DCPSInfoRepo is a service on a local or remote node used for participant
/// discovery. Configuring how participants should fnd DCPSInfoRepo is the purpose of this class.
/// </remarks>
public class InfoRepoDiscovery : Discovery
{
    #region Fields
    private readonly IntPtr _native;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the port used by the tcp transport for Built-In Topics.
    /// </summary>
    /// <remarks>
    /// If the default of '0' is used, the operating system will choose a port
    /// to use. The default value is 0.
    /// </remarks>
    public int BitTransportPort
    {
        get => GetBitTransportPort();
        set => SetBitTransportPort(value);
    }

    /// <summary>
    /// Gets or sets the IP address identifying the local interface to be used by tcp
    /// transport for the Built-In Topics.
    /// </summary>
    public string BitTransportIp
    {
        get => GetBitTransportIp();
        set => SetBitTransportIp(value);
    }

    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="InfoRepoDiscovery"/> class.
    /// </summary>
    /// <param name="key">Unique key value for the repository.</param>
    /// <param name="ior">Repository IOR or host:port.</param>
    public InfoRepoDiscovery(string key, string ior)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (string.IsNullOrWhiteSpace(ior))
        {
            throw new ArgumentNullException(nameof(ior));
        }

        _native = UnsafeNativeMethods.InfoRepoDiscoveryNew(key, ior);
        FromNative(UnsafeNativeMethods.NarrowBase(_native));
    }
    #endregion

    #region Methods
    private int GetBitTransportPort()
    {
        return UnsafeNativeMethods.GetBitTransportPort(_native);
    }

    private void SetBitTransportPort(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Port number must be greater or equal to 0.");
        }

        UnsafeNativeMethods.SetBitTransportPort(_native, value);
    }

    private string GetBitTransportIp()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetBitTransportIp(_native));
    }

    private void SetBitTransportIp(string value)
    {
        UnsafeNativeMethods.SetBitTransportIp(_native, value);
    }
    #endregion
}

/// <summary>
/// This class suppresses stack walks for unmanaged code permission. (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full security review to make sure that the usage
/// is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_NarrowBase")]
    public static partial IntPtr NarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_new", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr InfoRepoDiscoveryNew(string key, string ior);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_GetBitTransportPort")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int GetBitTransportPort(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_SetBitTransportPort")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetBitTransportPort(IntPtr ird, int portNumber);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_GetBitTransportIp")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetBitTransportIp(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_SetBitTransportIp", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetBitTransportIp(IntPtr ird, string ip);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr NarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_new", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr InfoRepoDiscoveryNew(string key, string ior);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_GetBitTransportPort", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetBitTransportPort(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_SetBitTransportPort", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetBitTransportPort(IntPtr ird, int portNumber);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_GetBitTransportIp", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetBitTransportIp(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "InfoRepoDiscovery_SetBitTransportIp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern void SetBitTransportIp(IntPtr ird, string ip);
#endif
}