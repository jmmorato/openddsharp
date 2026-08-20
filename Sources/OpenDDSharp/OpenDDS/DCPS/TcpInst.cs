/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
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
/// Provides access to the configurable options for the TCP/IP transport.
/// </summary>
/// <remarks>
/// A properly configured transport provides added resilience to underlying stack disturbances.Almost all of the
/// options available to customize the connection and reconnection strategies have reasonable
/// defaults, but ultimately these values should to be chosen based upon a careful study of the
/// quality of the network and the desired QoS in the specific DDS application and target environment.
/// </remarks>
public class TcpInst : TransportInst
{
    #region Fields
    private readonly IntPtr _native;
    #endregion

    #region Properties
    /// <summary>
    /// Gets a value indicating whether the transport is reliable or not.
    /// </summary>
    public bool IsReliable => GetIsReliable();

    /// <summary>
    /// Gets or sets total send buffer size in bytes for TCP payload.
    /// </summary>
    /// <remarks>
    /// The default value is 0.
    /// This means that the OS default value will be used.
    /// </remarks>
    public int SendBufferSize
    {
        get => GetSendBufferSize();
        set => SetSendBufferSize(value);
    }

    /// <summary>
    /// Gets or sets the total receive buffer size in bytes for TCP payload.
    /// </summary>
    /// <remarks>
    /// The default value is 0.
    /// This means that the OS default value will be used.
    /// </remarks>
    public int RcvBufferSize
    {
        get => GetRcvBufferSize();
        set => SetRcvBufferSize(value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether enable or disable the Nagle’s algorithm.
    /// By default, it is disabled (false).
    /// </summary>
    /// <remarks>
    /// Enabling the Nagle’s algorithm may increase
    /// throughput at the expense of increased latency.
    /// </remarks>
    public bool EnableNagleAlgorithm
    {
        get => GetEnableNagleAlgorithm();
        set => SetEnableNagleAlgorithm(value);
    }

    /// <summary>
    /// Gets or sets the initial retry delay in milliseconds.
    /// The first connection retry will be when the loss of connection
    /// is detected.  The second try will be after this delay.
    /// The default is 500 milliseconds.
    /// </summary>
    public int ConnRetryInitialDelay
    {
        get => GetConnRetryInitialDelay();
        set => SetConnRetryInitialDelay(value);
    }

    /// <summary>
    /// Gets or sets the backoff multiplier for reconnection strategy.
    /// The third and so on reconnect will be this value * the previous delay.
    /// Hence with ConnRetryInitialDelay=500 and ConnRetryBackoffMultiplier=1.5
    /// the second reconnect attempt will be at 0.5 seconds after first retry connect
    /// fails; the third attempt will be 0.75 seconds after the second retry connect
    /// fails; the fourth attempt will be 1.125 seconds after the third retry connect
    /// fails. The default value is 2.0.
    /// </summary>
    public double ConnRetryBackoffMultiplier
    {
        get => GetConnRetryBackoffMultiplier();
        set => SetConnRetryBackoffMultiplier(value);
    }

    /// <summary>
    /// Gets or sets the  number of attempts to reconnect before giving up and calling
    /// OnPublicationLost() and OnSubscriptionLost() callbacks.
    /// The default is 3.
    /// </summary>
    public int ConnRetryAttempts
    {
        get => GetConnRetryAttempts();
        set => SetConnRetryAttempts(value);
    }

    /// <summary>
    /// Gets or sets the maximum period (in milliseconds) of not being able to send queued
    /// messages. If there are samples queued and no output for longer
    /// than this period then the connection will be closed and on_*_lost()
    /// callbacks will be called. If the value is -1, the default, then
    /// this check will not be made.
    /// </summary>
    public int MaxOutputPausePeriod
    {
        get => GetMaxOutputPausePeriod();
        set => SetMaxOutputPausePeriod(value);
    }

    /// <summary>
    /// Gets or sets the time period in milliseconds for the acceptor side
    /// of a connection to wait for the connection to be reconnected.
    /// If not reconnected within this period then
    /// OnPublicationLost() and OnSubscriptionLost() callbacks
    /// will be called. The default is 2 seconds (2000 millseconds).
    /// </summary>
    public int PassiveReconnectDuration
    {
        get => GetPassiveReconnectDuration();
        set => SetPassiveReconnectDuration(value);
    }

    /// <summary>
    /// Gets or sets the time period in milliseconds for the active side
    /// of a connection to wait for the connection to be established.
    /// If not connected within this period then this link is removed
    /// from pending and any other links are attempted.
    /// The default is 5 seconds (5000 milliseconds).
    /// </summary>
    public int ActiveConnTimeoutPeriod
    {
        get => GetActiveConnTimeoutPeriod();
        set => SetActiveConnTimeoutPeriod(value);
    }

    /// <summary>
    /// Gets or sets a value that override the address sent to peers with the confgured string.
    /// </summary>
    /// <remarks>
    /// Usually this is the same as the local address, but if
    /// a public address is explicitly specified, use that.
    /// This can be used for firewall traversal and other advanced
    /// network configurations.
    /// </remarks>
    public string PublicAddress
    {
        get => GetPublicAddress();
        set => SetPublicAddress(value);
    }

    /// <summary>
    /// Gets or sets the hostname and port of the connection acceptor. The
    /// default value is the FQDN and port 0, which means
    /// the OS will choose the port.
    /// </summary>
    /// <remarks>
    /// If only the host is specified and the port number is omitted,
    /// the ':' is still required on the host specifier.
    /// </remarks>
    public string LocalAddress
    {
        get => GetLocalAddress();
        set => SetLocalAddress(value);
    }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="TcpInst"/> class.
    /// </summary>
    /// <param name="inst">
    /// The base <see cref="TransportInst" /> object created with the <see cref="TransportRegistry" />.
    /// </param>
    public TcpInst(TransportInst inst) : base(inst?.ToNative() ?? IntPtr.Zero)
    {
        if (inst == null)
        {
            throw new ArgumentNullException(nameof(inst));
        }

        _native = UnsafeNativeMethods.TcpInstNew(inst.ToNative());
    }
    #endregion

    #region Methods
    private bool GetIsReliable()
    {
        return UnsafeNativeMethods.TcpInstGetIsReliable(_native);
    }

    private int GetSendBufferSize()
    {
        return UnsafeNativeMethods.TcpInstGetSendBufferSize(_native);
    }

    private void SetSendBufferSize(int value)
    {
        UnsafeNativeMethods.TcpInstSetSendBufferSize(_native, value);
    }

    private int GetRcvBufferSize()
    {
        return UnsafeNativeMethods.TcpInstGetRcvBufferSize(_native);
    }

    private void SetRcvBufferSize(int value)
    {
        UnsafeNativeMethods.TcpInstSetRcvBufferSize(_native, value);
    }

    private bool GetEnableNagleAlgorithm()
    {
        return UnsafeNativeMethods.GetEnableNagleAlgorithm(_native);
    }

    private void SetEnableNagleAlgorithm(bool value)
    {
        UnsafeNativeMethods.SetEnableNagleAlgorithm(_native, value);
    }

    private int GetConnRetryInitialDelay()
    {
        return UnsafeNativeMethods.GetConnRetryInitialDelay(_native);
    }

    private void SetConnRetryInitialDelay(int value)
    {
        UnsafeNativeMethods.SetConnRetryInitialDelay(_native, value);
    }

    private double GetConnRetryBackoffMultiplier()
    {
        return UnsafeNativeMethods.GetConnRetryBackoffMultiplier(_native);
    }

    private void SetConnRetryBackoffMultiplier(double value)
    {
        UnsafeNativeMethods.SetConnRetryBackoffMultiplier(_native, value);
    }

    private int GetConnRetryAttempts()
    {
        return UnsafeNativeMethods.GetConnRetryAttempts(_native);
    }

    private void SetConnRetryAttempts(int value)
    {
        UnsafeNativeMethods.SetConnRetryAttempts(_native, value);
    }

    private int GetMaxOutputPausePeriod()
    {
        return UnsafeNativeMethods.GetMaxOutputPausePeriod(_native);
    }

    private void SetMaxOutputPausePeriod(int value)
    {
        UnsafeNativeMethods.SetMaxOutputPausePeriod(_native, value);
    }

    private int GetPassiveReconnectDuration()
    {
        return UnsafeNativeMethods.GetPassiveReconnectDuration(_native);
    }

    private void SetPassiveReconnectDuration(int value)
    {
        UnsafeNativeMethods.SetPassiveReconnectDuration(_native, value);
    }

    private int GetActiveConnTimeoutPeriod()
    {
        return UnsafeNativeMethods.GetActiveConnTimeoutPeriod(_native);
    }

    private void SetActiveConnTimeoutPeriod(int value)
    {
        UnsafeNativeMethods.SetActiveConnTimeoutPeriod(_native, value);
    }

    private string GetPublicAddress()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetPublicAddress(_native));
    }

    private void SetPublicAddress(string value)
    {
        UnsafeNativeMethods.SetPublicAddress(_native, value);
    }

    private string GetLocalAddress()
    {
        return Marshal.PtrToStringAnsi(UnsafeNativeMethods.TcpInstGetLocalAddress(_native));
    }

    private void SetLocalAddress(string value)
    {
        UnsafeNativeMethods.TcpInstSetLocalAddress(_native, value);
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
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_new")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr TcpInstNew(IntPtr inst);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetIsReliable")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool TcpInstGetIsReliable(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetSendBufferSize")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TcpInstGetSendBufferSize(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetSendBufferSize")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void TcpInstSetSendBufferSize(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetRcvBufferSize")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TcpInstGetRcvBufferSize(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetRcvBufferSize")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void TcpInstSetRcvBufferSize(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetEnableNagleAlgorithm")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool GetEnableNagleAlgorithm(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetEnableNagleAlgorithm")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetEnableNagleAlgorithm(IntPtr ti, [MarshalAs(UnmanagedType.I1)] bool value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetConnRetryInitialDelay")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int GetConnRetryInitialDelay(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetConnRetryInitialDelay")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetConnRetryInitialDelay(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetConnRetryBackoffMultiplier")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial double GetConnRetryBackoffMultiplier(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetConnRetryBackoffMultiplier")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetConnRetryBackoffMultiplier(IntPtr ti, double value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetConnRetryAttempts")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int GetConnRetryAttempts(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetConnRetryAttempts")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetConnRetryAttempts(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetMaxOutputPausePeriod")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int GetMaxOutputPausePeriod(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetMaxOutputPausePeriod")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetMaxOutputPausePeriod(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetPassiveReconnectDuration")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int GetPassiveReconnectDuration(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetPassiveReconnectDuration")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetPassiveReconnectDuration(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetActiveConnTimeoutPeriod")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int GetActiveConnTimeoutPeriod(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetActiveConnTimeoutPeriod")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetActiveConnTimeoutPeriod(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetPublicAddress")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetPublicAddress(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetPublicAddress", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetPublicAddress(IntPtr ird, string ip);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetLocalAddress")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr TcpInstGetLocalAddress(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetLocalAddress", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void TcpInstSetLocalAddress(IntPtr ird, string ip);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_new", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr TcpInstNew(IntPtr inst);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetIsReliable", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool TcpInstGetIsReliable(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetSendBufferSize", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TcpInstGetSendBufferSize(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetSendBufferSize", CallingConvention = CallingConvention.Cdecl)]
    public static extern void TcpInstSetSendBufferSize(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetRcvBufferSize", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TcpInstGetRcvBufferSize(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetRcvBufferSize", CallingConvention = CallingConvention.Cdecl)]
    public static extern void TcpInstSetRcvBufferSize(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetEnableNagleAlgorithm", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool GetEnableNagleAlgorithm(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetEnableNagleAlgorithm", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetEnableNagleAlgorithm(IntPtr ti, [MarshalAs(UnmanagedType.I1)] bool value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetConnRetryInitialDelay", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetConnRetryInitialDelay(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetConnRetryInitialDelay", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetConnRetryInitialDelay(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetConnRetryBackoffMultiplier", CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetConnRetryBackoffMultiplier(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetConnRetryBackoffMultiplier", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetConnRetryBackoffMultiplier(IntPtr ti, double value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetConnRetryAttempts", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetConnRetryAttempts(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetConnRetryAttempts", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetConnRetryAttempts(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetMaxOutputPausePeriod", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetMaxOutputPausePeriod(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetMaxOutputPausePeriod", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetMaxOutputPausePeriod(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetPassiveReconnectDuration", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetPassiveReconnectDuration(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetPassiveReconnectDuration", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetPassiveReconnectDuration(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetActiveConnTimeoutPeriod", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetActiveConnTimeoutPeriod(IntPtr ti);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetActiveConnTimeoutPeriod", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetActiveConnTimeoutPeriod(IntPtr ti, int value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetPublicAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr GetPublicAddress(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetPublicAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern void SetPublicAddress(IntPtr ird, string ip);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_GetLocalAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr TcpInstGetLocalAddress(IntPtr ird);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TcpInst_SetLocalAddress", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern void TcpInstSetLocalAddress(IntPtr ird, string ip);
#endif
}