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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;
using OpenDDSharp.OpenDDS.DCPS;

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.OpenDDS.RTPS;

/// <summary>
/// Discovery Strategy class that implements RTPS discovery.
/// This class implements the Discovery interface for Rtps-based discovery.
/// </summary>
/// <remarks>
/// <para>The RTPS specification splits up the discovery protocol into two independent protocols:</para>
/// <para>    1. Participant Discovery Protocol.</para>
/// <para>    2. Endpoint Discovery Protocol.</para>
/// <para>A Participant Discovery Protocol (PDP) specifies how Participants discover each other in the
/// network. Once two Participants have discovered each other, they exchange information on the
/// Endpoints they contain using an Endpoint Discovery Protocol (EDP). Apart from this causality
/// relationship, both protocols can be considered independent.</para>
/// </remarks>
public class RtpsDiscovery : Discovery
{
    #region Fields
    private readonly IntPtr _native;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the number of seconds that a process waits
    /// between the announcement of participants.
    /// </summary>
    /// <remarks>
    /// The default value is 30 seconds.
    /// </remarks>
    public TimeValue ResendPeriod
    {
        get => GetResendPeriod();
        set => SetResendPeriod(value);
    }

    /// <summary>
    /// Gets or sets the port base number.
    /// </summary>
    /// <remarks>
    /// This number sets the starting point for deriving port numbers used for Simple
    /// Endpoint Discovery Protocol (SEDP). This property is used in conjunction with
    /// DG, PG, D0 (or DX), and D1 to construct the necessary Endpoints for RTPS
    /// discovery communication. The default value is 7400.
    /// </remarks>
    public ushort PB
    {
        get => GetPB();
        set => SetPB(value);
    }

    /// <summary>
    /// Gets or sets an integer value representing the Domain Gain.
    /// The default value is 250.
    /// </summary>
    /// <remarks>
    /// This is a multiplier that assists in formulating Multicast
    /// or Unicast ports for RTPS.
    /// </remarks>
    public ushort DG
    {
        get => GetDG();
        set => SetDG(value);
    }

    /// <summary>
    /// Gets or sets an integer value representing the Port Gain.
    /// The default value is 2.
    /// </summary>
    /// <remarks>
    /// The port gain assists in confguring SPDP Unicast
    /// ports and serves as an offset multiplier as
    /// participants are assigned addresses using the
    /// formula: PB + DG * domainId + d1 + PG * participantId.
    /// </remarks>
    public ushort PG
    {
        get => GetPG();
        set => SetPG(value);
    }

    /// <summary>
    /// Gets or sets an integer value representing the Offset Zero.
    /// The default value is 0.
    /// </summary>
    /// <remarks>
    /// The offset zero assists in providing an offset
    /// for calculating an assignable port in SPDP Multicast
    /// configurations. The formula used is: PB + DG * domainId + d0.
    /// </remarks>
    public ushort D0
    {
        get => GetD0();
        set => SetD0(value);
    }

    /// <summary>
    /// Gets or sets an integer value representing the Offset One.
    /// The default value is 10.
    /// </summary>
    /// <remarks>
    /// The offset one assists in providing an ofset
    /// for calculating an assignable port in SPDP Unicast
    /// configurations. The formula used is: PB + DG * domainId + d1 + PG * participantId.
    /// </remarks>
    public ushort D1
    {
        get => GetD1();
        set => SetD1(value);
    }

    /// <summary>
    /// Gets or sets an integer value representing the Offset X.
    /// The default value is 2.
    /// </summary>
    /// <remarks>
    /// <para>The offset X assists in providing an offset
    /// for calculating an assignable port in SEDP Multicast
    /// configurations. The formula used is: PB + DG * domainId + dx. </para>
    /// <para>This is only valid when SedpMulticast=true.</para>
    /// </remarks>
    public ushort DX
    {
        get => GetDX();
        set => SetDX(value);
    }

    /// <summary>
    /// Gets or sets the value of the time-to-live (ttl) field of
    /// multicast datagrams sent as part of discovery. This
    /// value specifies the number of hops the datagram
    /// will traverse before being discarded by the network.
    /// The default value of 1 means that all data is
    /// restricted to the local network subnet.
    /// </summary>
    public byte Ttl
    {
        get => GetTtl();
        set => SetTtl(value);
    }

    /// <summary>
    /// Gets or sets a value for configure the transport instance created and used
    /// by SEDP to bind to the specified local address and port.
    /// </summary>
    /// <remarks>
    /// In order to leave the port unspecified, it can
    /// be omitted from the setting but the trailing ':' must
    /// be present.
    /// </remarks>
    public string SedpLocalAddress
    {
        get => GetSedpLocalAddress();
        set => SetSedpLocalAddress(value);
    }

    /// <summary>
    /// Gets or sets the address of a local interface (no port), which will be
    /// used by SPDP to bind to that specific interface.
    /// </summary>
    public string SpdpLocalAddress
    {
        get => GetSpdpLocalAddress();
        set => SetSpdpLocalAddress(value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether
    /// Multicast is used for the SEDP trafic. When set to true,
    /// Multicast is used. When set to false Unicast for
    /// SEDP is used. The default value is true.
    /// </summary>
    public bool SedpMulticast
    {
        get => GetSedpMulticast();
        set => SetSedpMulticast(value);
    }

    /// <summary>
    /// Gets or sets the specific network interface to be used by this
    /// discovery instance. This uses a platform-specific
    /// format that identifies the network interface.
    /// </summary>
    public string MulticastInterface
    {
        get => GetMulticastInterface();
        set => SetMulticastInterface(value);
    }

    /// <summary>
    /// Gets or sets a network address specifying the multicast group to
    /// be used for SPDP discovery. The default value is 239.255.0.1.
    /// </summary>
    /// <remarks>
    /// This overrides the interoperability group of the specification.
    /// It can be used, for example, to specify use of a routed group
    /// address to provide a larger discovery scope.
    /// </remarks>
    public string DefaultMulticastGroup
    {
        get => GetDefaultMulticastGroup();
        set => SetDefaultMulticastGroup(value);
    }

    /// <summary>
    /// Gets a list (comma or whitespace separated) of host:port
    /// pairs used as destinations for SPDP content. This
    /// can be a combination of Unicast and Multicast
    /// addresses.
    /// </summary>
    public IEnumerable<string> SpdpSendAddrs
    {
        get => GetSpdpSendAddrs();
    }

    /// <summary>
    /// Gets or sets the specific network interface to use when
    /// determining which local MAC address should
    /// appear in a GUID generated by this node.
    /// </summary>
    public string GuidInterface
    {
        get => GetGuidInterface();
        set => SetGuidInterface(value);
    }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="RtpsDiscovery"/> class.
    /// </summary>
    /// <param name="key">The discovery unique key.</param>
    public RtpsDiscovery(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        _native = UnsafeNativeMethods.RtpsDiscoveryNew(key);
        FromNative(UnsafeNativeMethods.NarrowBase(_native));
    }
    #endregion

    #region Methods
    private TimeValue GetResendPeriod()
    {
        return UnsafeNativeMethods.GetResendPeriod(_native);
    }

    private void SetResendPeriod(TimeValue value)
    {
        UnsafeNativeMethods.SetResendPeriod(_native, value);
    }

    private ushort GetPB()
    {
        return UnsafeNativeMethods.GetPB(_native);
    }

    private void SetPB(ushort value)
    {
        UnsafeNativeMethods.SetPB(_native, value);
    }

    private ushort GetDG()
    {
        return UnsafeNativeMethods.GetDG(_native);
    }

    private void SetDG(ushort value)
    {
        UnsafeNativeMethods.SetDG(_native, value);
    }

    private ushort GetPG()
    {
        return UnsafeNativeMethods.GetPG(_native);
    }

    private void SetPG(ushort value)
    {
        UnsafeNativeMethods.SetPG(_native, value);
    }

    private ushort GetD0()
    {
        return UnsafeNativeMethods.GetD0(_native);
    }

    private void SetD0(ushort value)
    {
        UnsafeNativeMethods.SetD0(_native, value);
    }

    private ushort GetD1()
    {
        return UnsafeNativeMethods.GetD1(_native);
    }

    private void SetD1(ushort value)
    {
        UnsafeNativeMethods.SetD1(_native, value);
    }

    private ushort GetDX()
    {
        return UnsafeNativeMethods.GetDX(_native);
    }

    private void SetDX(ushort value)
    {
        UnsafeNativeMethods.SetDX(_native, value);
    }

    private byte GetTtl()
    {
        return UnsafeNativeMethods.GetTtl(_native);
    }

    private void SetTtl(byte value)
    {
        UnsafeNativeMethods.SetTtl(_native, value);
    }

    private string GetSedpLocalAddress()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetSedpLocalAddress(_native));
    }

    private void SetSedpLocalAddress(string value)
    {
        string full = value;
        if (!full.Contains(":"))
        {
            full += ":0";
        }
        UnsafeNativeMethods.SetSedpLocalAddress(_native, full);
    }

    private string GetSpdpLocalAddress()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetSedpLocalAddress(_native));
    }

    private void SetSpdpLocalAddress(string value)
    {
        string full = value;
        if (!full.Contains(":"))
        {
            full += ":0";
        }
        UnsafeNativeMethods.SetSpdpLocalAddress(_native, full);
    }

    private bool GetSedpMulticast()
    {
        return UnsafeNativeMethods.GetSedpMulticast(_native);
    }

    private void SetSedpMulticast(bool value)
    {
        UnsafeNativeMethods.SetSedpMulticast(_native, value);
    }

    private string GetMulticastInterface()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetMulticastInterface(_native));
    }

    private void SetMulticastInterface(string value)
    {
        UnsafeNativeMethods.SetMulticastInterface(_native, value);
    }

    private string GetDefaultMulticastGroup()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetDefaultMulticastGroup(_native));
    }

    private void SetDefaultMulticastGroup(string value)
    {
        string full = value;
        if (!full.Contains(":"))
        {
            full += ":0";
        }
        UnsafeNativeMethods.SetDefaultMulticastGroup(_native, full);
    }

    private IEnumerable<string> GetSpdpSendAddrs()
    {
        IList<string> addrs = new List<string>();

        UnsafeNativeMethods.GetSpdpSendAddrs(_native).PtrToStringSequence(ref addrs, false);

        return addrs;
    }

    private string GetGuidInterface()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetGuidInterface(_native));
    }

    private void SetGuidInterface(string value)
    {
        UnsafeNativeMethods.SetGuidInterface(_native, value);
    }
    #endregion
}

#region UnsafeNativeMethods
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
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_NarrowBase")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr NarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_new", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr RtpsDiscoveryNew(string key);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetResendPeriod", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Struct)]
    public static extern TimeValue GetResendPeriod(IntPtr d);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetResendPeriod", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetResendPeriod(IntPtr d, [MarshalAs(UnmanagedType.Struct)] TimeValue value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetPB")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ushort GetPB(IntPtr d);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetPB")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetPB(IntPtr d, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetDG")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ushort GetDG(IntPtr d);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetDG")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetDG(IntPtr d, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetPG")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ushort GetPG(IntPtr d);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetPG")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetPG(IntPtr mi, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetD0")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ushort GetD0(IntPtr mi);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetD0")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetD0(IntPtr mi, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetD1")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ushort GetD1(IntPtr mi);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetD1")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetD1(IntPtr mi, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetDX")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial ushort GetDX(IntPtr mi);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetDX")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetDX(IntPtr mi, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetTtl")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial byte GetTtl(IntPtr mi);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetTtl")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetTtl(IntPtr mi, byte value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetSedpLocalAddress")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetSedpLocalAddress(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetSedpLocalAddress", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetSedpLocalAddress(IntPtr ird, string ip);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetSpdpLocalAddress")]
    public static partial IntPtr GetSpdpLocalAddress(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetSpdpLocalAddress", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetSpdpLocalAddress(IntPtr ird, string ip);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetSedpMulticast")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool GetSedpMulticast(IntPtr mi);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetSedpMulticast")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetSedpMulticast(IntPtr mi, [MarshalAs(UnmanagedType.I1)] bool value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetMulticastInterface")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetMulticastInterface(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetMulticastInterface", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetMulticastInterface(IntPtr ird, string ip);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetDefaultMulticastGroup")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetDefaultMulticastGroup(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetDefaultMulticastGroup", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetDefaultMulticastGroup(IntPtr ird, string ip);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetSpdpSendAddrs")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetSpdpSendAddrs(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetGuidInterface")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetGuidInterface(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetGuidInterface", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetGuidInterface(IntPtr ird, string ip);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr NarrowBase(IntPtr ptr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_new", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr RtpsDiscoveryNew(string key);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetResendPeriod", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Struct)]
    public static extern TimeValue GetResendPeriod(IntPtr d);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetResendPeriod", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetResendPeriod(IntPtr d, [MarshalAs(UnmanagedType.Struct)] TimeValue value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetPB", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort GetPB(IntPtr d);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetPB", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetPB(IntPtr d, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetDG", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort GetDG(IntPtr d);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetDG", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetDG(IntPtr d, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetPG", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort GetPG(IntPtr d);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetPG", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetPG(IntPtr mi, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetD0", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort GetD0(IntPtr mi);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetD0", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetD0(IntPtr mi, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetD1", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort GetD1(IntPtr mi);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetD1", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetD1(IntPtr mi, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetDX", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort GetDX(IntPtr mi);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetDX", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetDX(IntPtr mi, ushort value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetTtl", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte GetTtl(IntPtr mi);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetTtl", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetTtl(IntPtr mi, byte value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetSedpLocalAddress", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetSedpLocalAddress(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetSedpLocalAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern void SetSedpLocalAddress(IntPtr ird, string ip);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetSpdpLocalAddress", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetSpdpLocalAddress(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetSpdpLocalAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern void SetSpdpLocalAddress(IntPtr ird, string ip);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetSedpMulticast", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool GetSedpMulticast(IntPtr mi);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetSedpMulticast", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetSedpMulticast(IntPtr mi, [MarshalAs(UnmanagedType.I1)] bool value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetMulticastInterface", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetMulticastInterface(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetMulticastInterface", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern void SetMulticastInterface(IntPtr ird, string ip);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetDefaultMulticastGroup", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetDefaultMulticastGroup(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetDefaultMulticastGroup", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern void SetDefaultMulticastGroup(IntPtr ird, string ip);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetSpdpSendAddrs", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetSpdpSendAddrs(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_GetGuidInterface", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetGuidInterface(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "RtpsDiscovery_SetGuidInterface", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern void SetGuidInterface(IntPtr ird, string ip);
#endif
}
#endregion