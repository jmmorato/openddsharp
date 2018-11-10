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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#pragma unmanaged 
#include <dds/DCPS/transport/multicast/Multicast.h>
#include <dds/DCPS/transport/multicast/MulticastInst.h>
#include <dds/DCPS/transport/multicast/MulticastInst_rch.h>
#pragma managed

#include "TransportInst.h"
#include "TimeValue.h"

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {

            /// <summary>
            /// Provides access to the confgurable options for the IP Multicast transport.
            /// </summary>
            /// <remarks>
            /// <para>
            /// The multicast transport provides unifed support for best-efort and reliable delivery based
            /// on a transport confguration parameter.
            /// </para>
            /// <para>
            /// Best-efort delivery imposes the least amount of overhead as data is exchanged between
            /// peers, however it does not provide any guarantee of delivery. Data may be lost due to
            /// unresponsive or unreachable peers or received in duplicate.
            /// </para>
            /// <para>
            /// Reliable delivery provides for guaranteed delivery of data to associated peers with no
            /// duplication at the cost of additional processing and bandwidth. Reliable delivery is achieved
            /// through two primary mechanisms: 2-way peer handshaking and negative acknowledgment of missing data.
            /// </para>
            /// </remarks>
            public ref class MulticastInst : public TransportInst {

            internal:
                ::OpenDDS::DCPS::MulticastInst* impl_entity;

            public:
                /// <summary>
                /// Indicates whether the transport is reliable or not.
                /// </summary>
                property System::Boolean IsReliable {
                    System::Boolean get();
                }

                /// <summary>
                /// Enables reliable communication. The default value is true.
                /// </summary>
                /// <remarks>
                /// This option will eventually be deprecated.
                /// </remarks>
                property System::Boolean Reliable {
                    System::Boolean get();
                    void set(System::Boolean value);
                }

                /// <summary>
                /// Enables IPv6 default group address selection.
                /// By default, this option is disabled (false).
                /// </summary>
                property System::Boolean DefaultToIpv6 {
                    System::Boolean get();
                    void set(System::Boolean value);
                }

                /// <summary>
                /// The default port number (when <see cref="GroupAddress" /> is not set)
                /// The default value is 49152 [IANA 2009-11-16].
                /// </summary>
                /// <remarks>
                /// When a group address is specifed, the port number within it is used. 
                /// If no group address is specifed, the port offset is used as a port number.
                /// This value should not be set less than 49152.
                /// </remarks>
                property System::UInt16 PortOffset {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                /// <summary>
                /// The multicast group to join to send/receive data.
                /// The default value is:
                ///   224.0.0.128:<port> [IANA 2009-11-17], or
                ///    [FF01::80]:<port> [IANA 2009-08-28]
                /// </summary>
                property System::String^ GroupAddress {
                    System::String^ get();
                    void set(System::String^ value);
                }

                /// <summary>
                /// If non-empty, address of a local network interface which 
                /// is used to join the multicast group.
                /// </summary>
                property System::String^ LocalAddress {
                    System::String^ get();
                    void set(System::String^ value);
                }

                /// <summary>
                /// The exponential base used during handshake retries; smaller
                /// values yield shorter delays between attempts.
                /// The default value is 2.0.
                /// </summary>
                property System::Double SynBackoff {
                    System::Double get();
                    void set(System::Double value);
                }

                /// <summary>
                /// The minimum number of milliseconds to wait between handshake
                /// attempts during association. The default value is 250.
                /// </summary>
                property TimeValue SynInterval {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// The maximum number of milliseconds to wait before giving up
                /// on a handshake response during association.
                /// The default value is 30000 (30 seconds).
                /// </summary>
                property TimeValue SynTimeout {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// The number of datagrams to retain in order to service repair
                /// requests (reliable only). The default value is 32.
                /// </summary>
                property size_t NakDepth {
                    size_t get();
                    void set(size_t value);
                }

                /// <summary>
                /// The minimum number of milliseconds to wait between repair
                /// requests (reliable only). The default value is 500.
                /// </summary>
                property TimeValue NakInterval {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// The number of intervals between nak's for a sample
                /// after initial nak. The default value is 4.
                /// </summary>
                property size_t NakDelayIntervals {
                    size_t get();
                    void set(size_t value);
                }

                /// <summary>
                /// The maximum number of a missing sample will be nak'ed.
                /// The default value is: 3.
                /// </summary>
                property size_t NakMax {
                    size_t get();
                    void set(size_t value);
                }

                /// <summary>
                /// The maximum number of milliseconds to wait before giving up
                /// on a repair response (reliable only).
                /// The default value is: 30000 (30 seconds).
                /// </summary>
                property TimeValue NakTimeout {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// The value of the time-to-live (ttl) feld of any
                /// datagrams sent. The default value of one means
                /// that all data is restricted to the local network.                
                /// </summary>
                property System::Byte Ttl {
                    System::Byte get();
                    void set(System::Byte value);
                };

                /// <summary>
                /// The size of the socket receive buffer.
                /// The default value is ACE_DEFAULT_MAX_SOCKET_BUFSIZ if it's defined,
                /// otherwise, 0. If the value is 0, the system default value is used.
                /// </summary>
                property size_t RcvBufferSize {
                    size_t get();
                    void set(size_t value);
                }

                /// <summary>
                /// Sending using asynchronous I/O on Windows platforms that support it.
                /// The default value is false.
                /// </summary>
                /// <remarks>
                /// This parameter has no effect on non-Windows platforms and Windows platforms
                /// that don't support asynchronous I/O.
                /// </remarks>
                property System::Boolean AsyncSend {
                    System::Boolean get();
                    void set(System::Boolean value);
                }

            public:
                /// <summary>
                /// Creates a new instance of <see cref="MulticastInst" />.
                /// </summary>
                /// <param name="inst">The base <see cref="TransportInst" /> object created with the <see cref="TransportRegistry" />.</param>
                MulticastInst(TransportInst^ inst);

                /*public:
                    /// <summary>
                    /// Implicit conversion operator from <see cref="TransportInst" /> to <see cref="MulticastInst" />.
                    /// </summary>
                    /// <param name="value">The value to transform.</param>
                    /// <returns>The <see cref="MulticastInst" /> value.</returns>
                    static operator MulticastInst^(TransportInst^ value) {
                        MulticastInst^ mi = gcnew MulticastInst(value);
                        return mi;
                    }*/
            };
        }
    }
}