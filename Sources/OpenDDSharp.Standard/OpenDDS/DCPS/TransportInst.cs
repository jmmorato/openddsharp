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
using OpenDDSharp.Helpers;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenDDSharp.OpenDDS.DCPS
{
    /// <summary>
    /// Base class to hold configuration settings for TransportImpls.
    /// </summary>
    /// <remarks>
    /// Each transport implementation will need to define a concrete
    /// subclass of the TransportInst class.The base
    /// class (TransportInst) contains configuration settings that
    /// are common to all (or most) concrete transport implementations.
    /// The concrete transport implementation defines any configuration
    /// settings that it requires within its concrete subclass of this
    /// TransportInst base class.
    /// </remarks>
    public class TransportInst
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the type of the transport; tcp, udp, multicast,
        /// shmem, and rtps_udp are included with OpenDDSharp.
        /// </summary>
        public string TransportType => GetTransportType();

        /// <summary>
        /// Gets the instance's name.
        /// </summary>
        public string Name => GetName();

        /// <summary>
        /// Gets or sets the number of pre-created link (list) objects per pool for the
        /// "send queue" of each DataLink. The default value is 10.
        /// </summary>
        /// <remarks>
        /// When backpressure is detected, messages to be
        /// sent are queued.When the message queue must
        /// grow, it grows by this number.
        /// </remarks>
        public ulong QueueMessagesPerPool
        {
            get => GetQueueMessagesPerPool();
            set => SetQueueMessagesPerPool(value);
        }

        /// <summary>
        /// Gets or sets the initial number of pools for the backpressure
        /// queue. The default value is 5.
        /// </summary>
        /// <remarks>
        /// The default settings of the two backpressure queue values
        /// preallocate space for 50 messages(5 pools of 10 messages).
        /// </remarks>
        public ulong QueueInitialPools
        {
            get => GetQueueInitialPools();
            set => SetQueueInitialPools(value);
        }

        /// <summary>
        /// Gets or sets the maximum size of a transport packet, including
        /// its transport header, sample header, and sample data.
        /// The default value is 2147481599.
        /// </summary>
        public uint MaxPacketSize
        {
            get => GetMaxPacketSize();
            set => SetMaxPacketSize(value);
        }

        /// <summary>
        /// Gets or sets maximum number of samples in a transport packet.
        /// The default value is 10.
        /// </summary>
        public ulong MaxSamplesPerPacket
        {
            get => GetMaxSamplesPerPacket();
            set => SetMaxSamplesPerPacket(value);
        }

        /// <summary>
        /// Gets or sets the optimum size (in bytes) of a packet (packet header + sample(s)).
        /// The default value is 4096.
        /// </summary>
        /// <remarks>
        /// Transport packets greater than this size will be sent over the wire even if there are still queued
        /// samples to be sent. This value may impact performance depending on your network
        /// confguration and application nature.
        /// </remarks>
        public uint OptimumPacketSize
        {
            get => GetOptimumPacketSize();
            set => SetOptimumPacketSize(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether enable or disable the thread per connection send
        /// strategy. By default, this option is disabled (false).
        /// </summary>
        /// <remarks>
        /// Enabling the ThreadPerConnection option will increase performance when writing to
        /// multiple data readers on diferent process as long as the overhead of thread context
        /// switching does not outweigh the benefts of parallel writes.This balance of network
        /// performance to context switching overhead is best determined by experimenting. If a
        /// machine has multiple network cards, it may improve performance by creating a transport
        /// for each network card.
        /// </remarks>
        public bool ThreadPerConnection
        {
            get => GetThreadPerConnection();
            set => SetThreadPerConnection(value);
        }

        /// <summary>
        /// Gets or sets delay in milliseconds that the datalink should be released after all
        /// associations are removed. The default value is 10 seconds.
        /// </summary>
        /// <remarks>
        /// The DatalinkReleaseDelay is the delay for datalink release after no associations.
        /// Increasing this value may reduce the overhead of re-establishment when reader/writer
        /// associations are added and removed frequently.
        /// </remarks>
        public long DatalinkReleaseDelay
        {
            get => GetDatalinkReleaseDelay();
            set => SetDatalinkReleaseDelay(value);
        }

        /// <summary>
        /// Gets or sets the number of chunks used to size allocators for transport control
        /// samples. The default value is 32.
        /// </summary>
        public ulong DatalinkControlChunks
        {
            get => GetDatalinkControlChunks();
            set => SetDatalinkControlChunks(value);
        }
        #endregion

        #region Constructors
        internal TransportInst(IntPtr native)
        {
            _native = native;
        }
        #endregion

        #region Methods
        private string GetTransportType()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetTransportType(_native));
        }

        private string GetName()
        {
            return Marshal.PtrToStringAnsi(UnsafeNativeMethods.GetName(_native));
        }

        private ulong GetQueueMessagesPerPool()
        {
            return UnsafeNativeMethods.GetQueueMessagesPerPool(_native).ToUInt64();
        }

        private void SetQueueMessagesPerPool(ulong value)
        {
            UnsafeNativeMethods.SetQueueMessagesPerPool(_native, new UIntPtr(value));
        }

        private ulong GetQueueInitialPools()
        {
            return UnsafeNativeMethods.GetQueueInitialPools(_native).ToUInt64();
        }

        private void SetQueueInitialPools(ulong value)
        {
            UnsafeNativeMethods.SetQueueInitialPools(_native, new UIntPtr(value));
        }

        private uint GetMaxPacketSize()
        {
            return UnsafeNativeMethods.GetMaxPacketSize(_native);
        }

        private void SetMaxPacketSize(uint value)
        {
            UnsafeNativeMethods.SetMaxPacketSize(_native, value);
        }

        private ulong GetMaxSamplesPerPacket()
        {
            return UnsafeNativeMethods.GetMaxSamplesPerPacket(_native).ToUInt64();
        }

        private void SetMaxSamplesPerPacket(ulong value)
        {
            UnsafeNativeMethods.SetMaxSamplesPerPacket(_native, new UIntPtr(value));
        }

        private uint GetOptimumPacketSize()
        {
            return UnsafeNativeMethods.GetOptimumPacketSize(_native);
        }

        private void SetOptimumPacketSize(uint value)
        {
            UnsafeNativeMethods.SetOptimumPacketSize(_native, value);
        }

        private bool GetThreadPerConnection()
        {
            return UnsafeNativeMethods.GetThreadPerConnection(_native);
        }

        private void SetThreadPerConnection(bool value)
        {
            UnsafeNativeMethods.SetThreadPerConnection(_native, value);
        }

        private long GetDatalinkReleaseDelay()
        {
            return UnsafeNativeMethods.GetDatalinkReleaseDelay(_native);
        }

        private void SetDatalinkReleaseDelay(long value)
        {
            UnsafeNativeMethods.SetDatalinkReleaseDelay(_native, value);
        }

        private ulong GetDatalinkControlChunks()
        {
            return UnsafeNativeMethods.GetDatalinkControlChunks(_native).ToUInt64();
        }

        private void SetDatalinkControlChunks(ulong value)
        {
            UnsafeNativeMethods.SetDatalinkControlChunks(_native, new UIntPtr(value));
        }

        internal IntPtr ToNative()
        {
            return _native;
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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_GetTransportType", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetTransportType(IntPtr ti);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_GetName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr GetName(IntPtr ti);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_GetQueueMessagesPerPool", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetQueueMessagesPerPool(IntPtr ti);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_SetQueueMessagesPerPool", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetQueueMessagesPerPool(IntPtr ti, UIntPtr value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_GetQueueInitialPools", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetQueueInitialPools(IntPtr ti);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_SetQueueInitialPools", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetQueueInitialPools(IntPtr ti, UIntPtr value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_GetMaxPacketSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint GetMaxPacketSize(IntPtr ti);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_SetMaxPacketSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetMaxPacketSize(IntPtr ti, uint value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_GetMaxSamplesPerPacket", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetMaxSamplesPerPacket(IntPtr ti);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_SetMaxSamplesPerPacket", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetMaxSamplesPerPacket(IntPtr ti, UIntPtr value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_GetOptimumPacketSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint GetOptimumPacketSize(IntPtr ti);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_SetOptimumPacketSize", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetOptimumPacketSize(IntPtr ti, uint value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_GetThreadPerConnection", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool GetThreadPerConnection(IntPtr mi);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_SetThreadPerConnection", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetThreadPerConnection(IntPtr mi, [MarshalAs(UnmanagedType.I1)] bool value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_GetDatalinkReleaseDelay", CallingConvention = CallingConvention.Cdecl)]
            public static extern long GetDatalinkReleaseDelay(IntPtr ti);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_SetDatalinkReleaseDelay", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetDatalinkReleaseDelay(IntPtr ti, long value);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_GetDatalinkControlChunks", CallingConvention = CallingConvention.Cdecl)]
            public static extern UIntPtr GetDatalinkControlChunks(IntPtr ti);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "TransportInst_SetDatalinkControlChunks", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetDatalinkControlChunks(IntPtr ti, UIntPtr value);
        }
        #endregion
    }
}
