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
#include <dds/DCPS/transport/rtps_udp/RtpsUdp.h>
#include <dds/DCPS/transport/rtps_udp/RtpsUdpInst.h>
#include <dds/DCPS/transport/rtps_udp/RtpsUdpInst_rch.h>
#pragma managed

#include "TransportInst.h"
#include "TimeValue.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {
            
            /// <summary>
            /// RTPS UDP transport implementation
            /// </summary>
            public ref class RtpsUdpInst : public TransportInst {
                
            internal:
                ::OpenDDS::DCPS::RtpsUdpInst* impl_entity;

            public:
                property System::Boolean IsReliable {
                    System::Boolean get();
                };

                property System::Boolean RequiresCdr {
                    System::Boolean get();
                };

                property System::Int32 SendBufferSize {
                    System::Int32 get();
                    void set(System::Int32 value);
                };

                property System::Int32 RcvBufferSize {
                    System::Int32 get();
                    void set(System::Int32 value);
                };

                property size_t NakDepth {
                    size_t get();
                    void set(size_t value);
                }

                /// <summary>
                /// Flag that indicates if the transport use multicast
                /// </summary>
                property System::Boolean UseMulticast {
                    System::Boolean get();
                    void set(System::Boolean value);
                };

                property System::Byte TTL {
                    System::Byte get();
                    void set(System::Byte value);
                };

                property System::String^ MulticastGroupAddress {
                    System::String^ get();
                    void set(System::String^ value);
                }

                property System::String^ MulticastInterface {
                    System::String^ get();
                    void set(System::String^ value);
                }

                property System::String^ LocalAddress {
                    System::String^ get();
                    void set(System::String^ value);
                }

                property TimeValue NakResponseDelay {
                    TimeValue get();
                    void set(TimeValue value);
                }

                property TimeValue HeartbeatPeriod {
                    TimeValue get();
                    void set(TimeValue value);
                }

                property TimeValue HeartbeatResponseDelay {
                    TimeValue get();
                    void set(TimeValue value);
                }

                property TimeValue HandshakeTimeout {
                    TimeValue get();
                    void set(TimeValue value);
                }

                property TimeValue DurableDataTimeout {
                    TimeValue get();
                    void set(TimeValue value);
                }

            public:
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
