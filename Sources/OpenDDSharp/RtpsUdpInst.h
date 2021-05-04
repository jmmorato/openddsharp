/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#pragma unmanaged 
#include <dds/DCPS/transport/rtps_udp/RtpsUdp.h>
#include <dds/DCPS/transport/rtps_udp/RtpsUdpInst.h>
#include <dds/DCPS/transport/rtps_udp/RtpsUdpInst_rch.h>
#pragma managed

#include "TransportInst.h"
#include "TimeValue.h"
#include "TransportRegistry.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {
            
            /// <summary>
            /// Provides access to the configurable options for the RTPS UDP transport.
            /// </summary>
            /// <remarks>
            /// The RTPS UDP transport is one of the pluggable transports available to a developer and is necessary 
            /// for interoperable communication between implementations.
            /// </remarks>
            public ref class RtpsUdpInst : public TransportInst {
                
            internal:
                ::OpenDDS::DCPS::RtpsUdpInst* impl_entity;

            public:
                /// <summary>
                /// Indicates whether the transport is reliable or not.
                /// </summary>
                property System::Boolean IsReliable {
                    System::Boolean get();
                };

                /// <summary>
                /// Indicates whether the transport requires CDR serialization or not.
                /// </summary>
                property System::Boolean RequiresCdr {
                    System::Boolean get();
                };

                /// <summary>
                /// Total send bufer size in bytes for UDP payload.
                /// The default value is the platform value of
                /// ACE_DEFAULT_MAX_SOCKET_BUFSIZ.
                /// </summary>
                property System::Int32 SendBufferSize {
                    System::Int32 get();
                    void set(System::Int32 value);
                };

                /// <summary>
                /// Total receive bufer size in bytes for UDP payload.
                /// The default value is the platform value of
                /// ACE_DEFAULT_MAX_SOCKET_BUFSIZ.
                /// </summary>
                property System::Int32 RcvBufferSize {
                    System::Int32 get();
                    void set(System::Int32 value);
                };

                /// <summary>
                /// The number of datagrams to retain in order to
                /// service repair requests (reliable only).
                /// The default value is 0.
                /// </summary>
                property size_t NakDepth {
                    size_t get();
                    void set(size_t value);
                }

                /// <summary>
                /// The RTPS UDP transport can use Unicast or
                /// Multicast. When set to false the transport uses
                /// Unicast, otherwise a value of true will use Multicast.
                /// The default value is true.
                /// </summary>
                property System::Boolean UseMulticast {
                    System::Boolean get();
                    void set(System::Boolean value);
                };

                /// <summary>
                /// The value of the time-to-live (ttl) field of any
                /// multicast datagrams sent. This value specifes the
                /// number of hops the datagram will traverse before
                /// being discarded by the network. The default value
                /// of 1 means that all data is restricted to the local
                /// network subnet.
                /// </summary>
                property System::Byte Ttl {
                    System::Byte get();
                    void set(System::Byte value);
                };

                /// <summary>
                /// When the transport is set to multicast, this is the
                /// multicast network address that should be used. If
                /// no port is specifed for the network address, port
                /// 7401 will be used. The default value is 239.255.0.2:7401.
                /// </summary>
                property System::String^ MulticastGroupAddress {
                    System::String^ get();
                    void set(System::String^ value);
                }

                /// <summary>
                /// Specifes the network interface to be used by this
                /// transport instance. This uses a platform-specifc
                /// format that identifes the network interface.
                /// </summary>
                property System::String^ MulticastInterface {
                    System::String^ get();
                    void set(System::String^ value);
                }

                /// <summary>
                /// Bind the socket to the given address and port.
                /// Port can be omitted but the trailing ':' is required.
                /// </summary>
                property System::String^ LocalAddress {
                    System::String^ get();
                    void set(System::String^ value);
                }

                /// <summary>
                /// Protocol tuning parameter that allows the RTPS
                /// Writer to delay the response (expressed in
                /// milliseconds) to a request for data from a negative
                /// acknowledgment. The default value is 200.
                /// </summary>
                property TimeValue NakResponseDelay {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// Protocol tuning parameter that specifes in
                /// milliseconds how often an RTPS Writer announces
                /// the availability of data. The default value is 1000.
                /// </summary>
                property TimeValue HeartbeatPeriod {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// Protocol tuning parameter in milliseconds that
                /// allows the RTPS Reader to delay the sending of a
                /// positive or negative acknowledgment. This
                /// parameter is used to reduce the occurrences of
                /// network storms.
                /// </summary>
                property TimeValue HeartbeatResponseDelay {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// The maximum number of milliseconds to wait
                /// before giving up on a handshake response during
                /// association. The default is 30000 (30 seconds).
                /// </summary>
                property TimeValue HandshakeTimeout {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// The durable data timeout.
                /// The default value is 60 seconds.
                /// </summary>
                property TimeValue DurableDataTimeout {
                    TimeValue get();
                    void set(TimeValue value);
                }

            public:
                /// <summary>
                /// Creates a new instance of <see cref="RtpsUdpInst" />.
                /// </summary>
                /// <param name="inst">The base <see cref="TransportInst" /> object created with the <see cref="TransportRegistry" />.</param>
                RtpsUdpInst(TransportInst^ inst);
            
            /*public:
                /// <summary>
                /// Implicit conversion operator from <see cref="TransportInst" /> to <see cref="RtpsUdpInst" />.
                /// </summary>
                /// <param name="value">The value to transform.</param>
                /// <returns>The <see cref="RtpsUdpInst" /> value.</returns>
                static operator RtpsUdpInst^ (TransportInst^ value) {
                    RtpsUdpInst^ rui = gcnew RtpsUdpInst(value);
                    return rui;
                }*/

            };
        }
    }
}
