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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp;

/// <summary>
/// The ACE library static class used for initialization and finalization.
/// </summary>
public static class Ace
{
    #region Methods
    /// <summary>
    /// This method initializes the ACE library services and initializes
    /// ACE's internal resources. Applications should not instantiate
    /// ACE classes or call methods on objects of these classes until a
    /// ACE.Init() returns successfully.
    /// </summary>
    /// <returns>
    /// Returns 0 on success, -1 on failure, and 1 if it had already been called.
    /// </returns>
    public static int Init()
    {
        return UnsafeNativeMethods.NativeInit();
    }

    /// <summary>
    /// Finalize the ACE library services and releases ACE's internal
    /// resources. In general, do not instantiate ACE classes or call
    /// methods on objects of these classes after a Ace.Fini() has been
    /// called.
    /// </summary>
    /// <returns>
    /// Returns 0 on success, -1 on failure, and 1 if it had already been called.
    /// </returns>
    public static int Fini()
    {
        return UnsafeNativeMethods.NativeFini();
    }
    #endregion
}

/// <summary>
/// This class suppresses stack walks for unmanaged code permission.
/// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full
/// security review to make sure that the usage is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Native p/invoke calls.")]
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented", Justification = "Partial required for the source generator.")]
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "Ace_Init")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int NativeInit();

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "Ace_Fini")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int NativeFini();
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Ace_Init", CallingConvention = CallingConvention.Cdecl)]
    public static extern int NativeInit();

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "Ace_Fini", CallingConvention = CallingConvention.Cdecl)]
    public static extern int NativeFini();
#endif
}