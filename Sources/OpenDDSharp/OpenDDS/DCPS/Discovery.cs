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
using OpenDDSharp.Helpers;

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.OpenDDS.DCPS;

/// <summary>
/// This class is an abstract class that acts as an interface for both
/// InfoRepo-based discovery and RTPS Discovery.
/// </summary>
public abstract class Discovery
{
    #region Constants
    /// <summary>
    /// The InfoRepo discovery default key.
    /// </summary>
    public const string DEFAULT_REPO = "DEFAULT_REPO";
    /// <summary>
    /// The RTPS discovery default key.
    /// </summary>
    public const string DEFAULT_RTPS = "DEFAULT_RTPS";
    /// <summary>
    /// The static discovery default key.
    /// </summary>
    public const string DEFAULT_STATIC = "DEFAULT_STATIC";
    #endregion

    #region Fields
    private IntPtr _native;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the discovery unique key.
    /// </summary>
    public string Key => GetKey();
    #endregion

    #region Methods
    private string GetKey()
    {
        var ptr = UnsafeNativeMethods.GetKey(_native);

        var key = Marshal.PtrToStringAnsi(ptr);
        ptr.ReleaseNativePointer();

        return key;
    }

    internal IntPtr ToNative()
    {
        return _native;
    }

    internal void FromNative(IntPtr native)
    {
        _native = native;
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
[ExcludeFromCodeCoverage]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Native p/invoke calls.")]
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented", Justification = "Partial required for the source generator.")]
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "Discovery_GetKey")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetKey(IntPtr d);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Discovery_GetKey", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetKey(IntPtr d);
#endif
}