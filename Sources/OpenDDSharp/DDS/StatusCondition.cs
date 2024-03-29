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

namespace OpenDDSharp.DDS;

/// <summary>
/// A StatusCondition object is a specific <see cref="Condition" /> that is associated with each <see cref="DDS.Entity" />.
/// The <see cref="Condition.TriggerValue" /> of the StatusCondition depends on the communication status of
/// that entity (e.g., arrival of data, loss of information, etc.), 'filtered' by the set of
/// <see cref="EnabledStatuses" /> on the StatusCondition.
/// </summary>
public class StatusCondition : Condition
{
    #region Fields
    private readonly IntPtr _native;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the <see cref="DDS.Entity" /> associated with the <see cref="StatusCondition" />.
    /// </summary>
    /// <remarks>
    /// Note that there is exactly one <see cref="DDS.Entity" /> associated with each <see cref="StatusCondition" />.
    /// </remarks>
    public Entity Entity { get; }

    /// <summary>
    /// Gets or sets the <see cref="StatusMask" /> that is taken into account to determine the
    /// <see cref="Condition.TriggerValue" /> of the <see cref="StatusCondition" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Set a new value for the property may change the <see cref="Condition.TriggerValue" /> of the
    /// <see cref="StatusCondition" />.
    /// </para>
    /// <para>
    /// <see cref="WaitSet" /> objects behavior depend on the changes of the <see cref="Condition.TriggerValue" /> of
    /// their attached conditions. Therefore, any <see cref="WaitSet" /> to which the <see cref="StatusCondition" />
    /// is attached is potentially affected by this operation.
    /// </para>
    /// <para>If the setter is not invoked, the default mask of enabled statuses includes all the statuses.</para>
    /// </remarks>
    public StatusMask EnabledStatuses
    {
        get => GetEnabledStatuses();
        set => SetEnabledStatuses(value);
    }
    #endregion

    #region Constructors
    internal StatusCondition(IntPtr native, Entity entity) : base(UnsafeNativeMethods.StatusConditionNarrowBase(native))
    {
        _native = native;
        Entity = entity;
    }
    #endregion

    #region Methods
    private StatusMask GetEnabledStatuses()
    {
        return UnsafeNativeMethods.GetEnabledStatuses(_native);
    }

    private void SetEnabledStatuses(StatusMask value)
    {
        UnsafeNativeMethods.SetEnabledStatuses(_native, value);
    }
    #endregion
}

#region UnsafeNativeMethods
/// <summary>
/// This class suppresses stack walks for unmanaged code permission.
/// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full
/// security review to make sure that the usage is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "StatusCondition_NarrowBase")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr StatusConditionNarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "StatusCondition_GetEnabledStatuses")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial StatusMask GetEnabledStatuses(IntPtr sc);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "StatusCondition_SetEnabledStatuses")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial StatusMask SetEnabledStatuses(IntPtr sc, uint value);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "StatusCondition_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr StatusConditionNarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "StatusCondition_GetEnabledStatuses", CallingConvention = CallingConvention.Cdecl)]
    public static extern StatusMask GetEnabledStatuses(IntPtr sc);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "StatusCondition_SetEnabledStatuses", CallingConvention = CallingConvention.Cdecl)]
    public static extern StatusMask SetEnabledStatuses(IntPtr sc, uint value);
#endif
}
#endregion