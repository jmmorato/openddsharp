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
                /// Enables reliable communication. This option will eventually
                /// be deprecated. The default value is: true.
                /// </summary>
                property System::Boolean Reliable {
                    System::Boolean get();
                    void set(System::Boolean value);
                }

                /// <summary>
                /// Enables IPv6 default group address selection.
                /// The default value is: false.
                /// </summary>
                property System::Boolean DefaultToIpv6 {
                    System::Boolean get();
                    void set(System::Boolean value);
                }

                /// <summary>
                /// The default port number (when <see cref="GroupAddress" /> is not set)
                /// The default value is: 49152 [IANA 2009-11-16].
                /// </summary>
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
                /// If non-empty, the address to pass to ACE which indicates the
                /// local network interface which should be used for joining the
                /// multicast group.
                /// </summary>
                property System::String^ LocalAddress {
                    System::String^ get();
                    void set(System::String^ value);
                }

                /// <summary>
                /// The exponential base used during handshake retries; smaller
                /// values yield shorter delays between attempts.
                /// The default value is: 2.0.
                /// </summary>
                property System::Double SynBackoff {
                    System::Double get();
                    void set(System::Double value);
                }

                /// <summary>
                /// The minimum number of milliseconds to wait between handshake
                /// attempts during association. The default value is: 250.
                /// </summary>
                property TimeValue SynInterval {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// The maximum number of milliseconds to wait before giving up
                /// on a handshake response during association.
                /// The default value is: 30000 (30 seconds).
                /// </summary>
                property TimeValue SynTimeout {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// The number of datagrams to retain in order to service repair
                /// requests (reliable only). The default value is: 32.
                /// </summary>
                property size_t NakDepth {
                    size_t get();
                    void set(size_t value);
                }

                /// <summary>
                /// The minimum number of milliseconds to wait between repair
                /// requests (reliable only). The default value is: 500.
                /// </summary>
                property TimeValue NakInterval {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// The number of interval's between nak's for a sample
                /// (after initial nak). The default value is: 4.
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
                /// Time-To-Live.
                /// The default value is: 1 (in same subnet)
                /// </summary>
                property System::Byte TTL {
                    System::Byte get();
                    void set(System::Byte value);
                };

                /// <summary>
                /// The size of the socket receive buffer.
                /// The default value is: ACE_DEFAULT_MAX_SOCKET_BUFSIZ if it's defined,
                /// otherwise, 0. If the value is 0, the system default value is used.
                /// </summary>
                property size_t RcvBufferSize {
                    size_t get();
                    void set(size_t value);
                }

                /// <summary>
                /// Sending using asynchronous I/O on Windows platforms that support it.
                /// The default value is: false.
                /// This parameter has no effect on non-Windows platforms and Windows platforms
                /// that don't support asynchronous I/O.
                /// </summary>
                property System::Boolean AsyncSend {
                    System::Boolean get();
                    void set(System::Boolean value);
                }

            public:
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