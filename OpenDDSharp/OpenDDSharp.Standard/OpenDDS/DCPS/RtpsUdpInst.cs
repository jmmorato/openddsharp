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

namespace OpenDDSharp.OpenDDS.DCPS
{
    /// <summary>
    /// Provides access to the configurable options for the RTPS UDP transport.
    /// </summary>
    /// <remarks>
    /// The RTPS UDP transport is one of the pluggable transports available to a developer and is necessary
    /// for interoperable communication between implementations.
    /// </remarks>
    public class RtpsUdpInst : TransportInst
    {
        #region Properties
        /// <summary>
        /// Gets a value indicating whether the transport is reliable or not.
        /// </summary>
        public bool IsReliable => GetIsReliable();

        /// <summary>
        ///  Gets a value indicating whether the transport requires CDR serialization or not.
        /// </summary>
        public bool RequiresCdr => GetRequiresCdr();

        /// <summary>
        /// Gets or sets the total send bufer size in bytes for UDP payload.
        /// The default value is the platform value of
        /// ACE_DEFAULT_MAX_SOCKET_BUFSIZ.
        /// </summary>
        public int SendBufferSize
        {
            get => GetSendBufferSize();
            set => SetSendBufferSize(value);
        }

        /// <summary>
        /// Gets or sets the total receive bufer size in bytes for UDP payload.
        /// The default value is the platform value of
        /// ACE_DEFAULT_MAX_SOCKET_BUFSIZ.
        /// </summary>
        public int RcvBufferSize
        {
            get => GetRcvBufferSize();
            set => SetRcvBufferSize(value);
        }

        /// <summary>
        /// Gets or sets the number of datagrams to retain in order to
        /// service repair requests (reliable only).
        /// The default value is 32.
        /// </summary>
        public ulong NakDepth
        {
            get => GetNakDepth();
            set => SetNakDepth(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the RTPS UDP transport can use Unicast or
        /// Multicast. When set to false the transport uses
        /// Unicast, otherwise a value of true will use Multicast.
        /// The default value is true.
        /// </summary>
        public bool UseMulticast
        {
            get => GetUseMulticast();
            set => SetUseMulticast(value);
        }

        /// <summary>
        /// Gets or sets the value of the time-to-live (ttl) field of any
        /// multicast datagrams sent. This value specifes the
        /// number of hops the datagram will traverse before
        /// being discarded by the network. The default value
        /// of 1 means that all data is restricted to the local
        /// network subnet.
        /// </summary>
        public byte Ttl
        {
            get => GetTtl();
            set => SetTtl(value);
        }

        /// <summary>
        /// Gets or sets the multicast group address.
        /// When the transport is set to multicast, this is the
        /// multicast network address that should be used. If
        /// no port is specifed for the network address, port
        /// 7401 will be used. The default value is 239.255.0.2:7401.
        /// </summary>
        public string MulticastGroupAddress
        {
            get => GetMulticastGroupAddress();
            set => SetMulticastGroupAddress(value);
        }

        /// <summary>
        /// Gets or sets the network interface to be used by this
        /// transport instance. This uses a platform-specifc
        /// format that identifes the network interface.
        /// </summary>
        public string MulticastInterface
        {
            get => GetMulticastInterface();
            set => SetMulticastInterface(value);
        }

        /// <summary>
        /// Gets or sets the address and port to bind the socket.
        /// Port can be omitted but the trailing ':' is required.
        /// </summary>
        public string LocalAddress
        {
            get => GetLocalAddress();
            set => SetLocalAddress(value);
        }

        /// <summary>
        /// Gets or sets the protocol tuning parameter that allows the RTPS
        /// Writer to delay the response (expressed in
        /// milliseconds) to a request for data from a negative
        /// acknowledgment. The default value is 200.
        /// </summary>
        public TimeValue NakResponseDelay
        {
            get => GetNakResponseDelay();
            set => SetNakResponseDelay(value);
        }

        /// <summary>
        /// Gets or sets the protocol tuning parameter that specifes in
        /// milliseconds how often an RTPS Writer announces
        /// the availability of data. The default value is 1000.
        /// </summary>
        public TimeValue HeartbeatPeriod
        {
            get => GetHeartbeatPeriod();
            set => SetHeartbeatPeriod(value);
        }

        /// <summary>
        /// Gets or sets the protocol tuning parameter in milliseconds that
        /// allows the RTPS Reader to delay the sending of a
        /// positive or negative acknowledgment. This
        /// parameter is used to reduce the occurrences of
        /// network storms.
        /// </summary>
        public TimeValue HeartbeatResponseDelay
        {
            get => GetHeartbeatResponseDelay();
            set => SetHeartbeatResponseDelay(value);
        }

        /// <summary>
        /// Gets or sets the maximum number of milliseconds to wait
        /// before giving up on a handshake response during
        /// association. The default is 30000 (30 seconds).
        /// </summary>
        public TimeValue HandshakeTimeout
        {
            get => GetHandshakeTimeout();
            set => SetHandshakeTimeout(value);
        }

        /// <summary>
        /// Gets or sets the durable data timeout.
        /// The default value is 60 seconds.
        /// </summary>
        public TimeValue DurableDataTimeout
        {
            get => GetDurableDataTimeout();
            set => SetDurableDataTimeout(value);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RtpsUdpInst"/> class.
        /// </summary>
        /// <param name="inst">The base <see cref="TransportInst" /> object created with the <see cref="TransportRegistry" />.</param>
        public RtpsUdpInst(TransportInst inst) : base(inst != null ? inst.ToNative() : IntPtr.Zero)
        {
        }
        #endregion

        #region Methods
        private bool GetIsReliable()
        {
            throw new NotImplementedException();
        }

        private bool GetRequiresCdr()
        {
            throw new NotImplementedException();
        }

        private int GetSendBufferSize()
        {
            throw new NotImplementedException();
        }

        private void SetSendBufferSize(int value)
        {
            throw new NotImplementedException();
        }

        private int GetRcvBufferSize()
        {
            throw new NotImplementedException();
        }

        private void SetRcvBufferSize(int value)
        {
            throw new NotImplementedException();
        }

        private ulong GetNakDepth()
        {
            throw new NotImplementedException();
        }

        private void SetNakDepth(ulong value)
        {
            throw new NotImplementedException();
        }

        private bool GetUseMulticast()
        {
            throw new NotImplementedException();
        }

        private void SetUseMulticast(bool value)
        {
            throw new NotImplementedException();
        }

        private byte GetTtl()
        {
            throw new NotImplementedException();
        }

        private void SetTtl(byte value)
        {
            throw new NotImplementedException();
        }

        private string GetMulticastGroupAddress()
        {
            throw new NotImplementedException();
        }

        private void SetMulticastGroupAddress(string value)
        {
            throw new NotImplementedException();
        }

        private string GetMulticastInterface()
        {
            throw new NotImplementedException();
        }

        private void SetMulticastInterface(string value)
        {
            throw new NotImplementedException();
        }

        private string GetLocalAddress()
        {
            throw new NotImplementedException();
        }

        private void SetLocalAddress(string value)
        {
            throw new NotImplementedException();
        }

        private TimeValue GetNakResponseDelay()
        {
            throw new NotImplementedException();
        }

        private void SetNakResponseDelay(TimeValue value)
        {
            throw new NotImplementedException();
        }

        private TimeValue GetHeartbeatPeriod()
        {
            throw new NotImplementedException();
        }

        private void SetHeartbeatPeriod(TimeValue value)
        {
            throw new NotImplementedException();
        }

        private TimeValue GetHeartbeatResponseDelay()
        {
            throw new NotImplementedException();
        }

        private void SetHeartbeatResponseDelay(TimeValue value)
        {
            throw new NotImplementedException();
        }

        private TimeValue GetHandshakeTimeout()
        {
            throw new NotImplementedException();
        }

        private void SetHandshakeTimeout(TimeValue value)
        {
            throw new NotImplementedException();
        }

        private TimeValue GetDurableDataTimeout()
        {
            throw new NotImplementedException();
        }

        private void SetDurableDataTimeout(TimeValue value)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
