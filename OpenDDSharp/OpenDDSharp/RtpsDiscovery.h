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
#include <dds\DCPS\RTPS\RtpsDiscovery.h>
#pragma managed

#include "Discovery.h"
#include "TimeValue.h"

#include <vcclr.h>
#include <msclr/marshal.h>

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace RTPS {
            
            /// <summary>
            /// Represent a RTPS discovery.
            /// </summary>
            /// <remarks>
            /// <para>The RTPS specifcation splits up the discovery protocol into two independent protocols:</para>
            /// <para>    1. Participant Discovery Protocol</para>
            /// <para>    2. Endpoint Discovery Protocol</para>
            /// <para>A Participant Discovery Protocol (PDP) specifes how Participants discover each other in the
            /// network. Once two Participants have discovered each other, they exchange information on the
            /// Endpoints they contain using an Endpoint Discovery Protocol (EDP). Apart from this causality
            /// relationship, both protocols can be considered independent.</para>
            /// </remarks>
            public ref class RtpsDiscovery : public ::OpenDDSharp::OpenDDS::DCPS::Discovery {

            internal:
                ::OpenDDS::RTPS::RtpsDiscovery* impl_entity;

            public:
                /// <summary>
                /// The number of seconds that a process waits
                /// between the announcement of participants.
                /// The default value is 30 seconds.
                /// </summary>
                property TimeValue ResendPeriod {
                    TimeValue get();
                    void set(TimeValue value);
                }

                /// <summary>
                /// Port Base number. The default value is 7400.
                /// </summary>
                /// <remarks>
                /// This number sets the starting point for deriving port numbers used for Simple
                /// Endpoint Discovery Protocol (SEDP). This property is used in conjunction with 
                /// DG, PG, D0 (or DX), and D1 to construct the necessary Endpoints for RTPS
                /// discovery communication.
                /// </remarks>
                property System::UInt16 PB {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                /// <summary>
                /// An integer value representing the Domain Gain.
                /// The default value is 250.
                /// </summary>
                /// <remarks>
                /// This is a multiplier that assists in formulating Multicast
                /// or Unicast ports for RTPS.
                /// </remarks>
                property System::UInt16 DG {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                /// <summary>
                /// An integer value representing the Port Gain.
                /// The default value is 2.
                /// </summary>
                /// <remarks>
                /// The port gain assists in confguring SPDP Unicast
                /// ports and serves as an offset multiplier as
                /// participants are assigned addresses using the
                /// formula: PB + DG * domainId + d1 + PG * participantId
                /// </remarks>
                property System::UInt16 PG {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                /// <summary>
                /// An integer value representing the Offset Zero.
                /// The default value is 0.
                /// </summary>
                /// <remarks>
                /// The offset zero assists in providing an offset
                /// for calculating an assignable port in SPDP Multicast
                /// confgurations. The formula used is: PB + DG * domainId + d0
                /// </remarks>
                property System::UInt16 D0 {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                /// <summary>
                /// An integer value representing the Offset One.
                /// The default value is 10.
                /// </summary>
                /// <remarks>
                /// The offset one assists in providing an ofset
                /// for calculating an assignable port in SPDP Unicast
                /// confgurations. The formula used is: PB + DG * domainId + d1 + PG * participantId
                /// </remarks>
                property System::UInt16 D1 {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                /// <summary>
                /// An integer value representing the Offset X.
                /// The default value is 2.
                /// </summary>
                /// <remarks>
                /// <para>The offset X assists in providing an offset
                /// for calculating an assignable port in SEDP Multicast
                /// confgurations. The formula used is: PB + DG * domainId + dx </para>
                /// <para>This is only valid when SedpMulticast=true.</para>
                /// </remarks>
                property System::UInt16 DX {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                /// <summary>
                /// The value of the time-to-live (ttl) field of
                /// multicast datagrams sent as part of discovery. This
                /// value specifes the number of hops the datagram
                /// will traverse before being discarded by the network.
                /// The default value of 1 means that all data is
                /// restricted to the local network subnet.
                /// </summary>
                property System::Byte Ttl {
                    System::Byte get();
                    void set(System::Byte value);
                };

                /// <summary>
                /// Confgure the transport instance created and used
                /// by SEDP to bind to the specifed local address and port.
                /// </summary>
                /// <remarks>
                /// In order to leave the port unspecifed, it can
                /// be omitted from the setting but the trailing ':' must
                /// be present.
                /// </remarks>
                property System::String^ SedpLocalAddress {
                    System::String^ get();
                    void set(System::String^ value);
                };

                /// <summary>
                /// Address of a local interface (no port), which will be
                /// used by SPDP to bind to that specifc interface.
                /// </summary>
                property System::String^ SpdpLocalAddress {
                    System::String^ get();
                    void set(System::String^ value);
                };

                /// <summary>
                /// A boolean value that determines whether
                /// Multicast is used for the SEDP trafic. When set to true,
                /// Multicast is used. When set to false Unicast for
                /// SEDP is used. The default value is true.
                /// </summary>
                property System::Boolean SedpMulticast {
                    System::Boolean get();
                    void set(System::Boolean value);
                };

                /// <summary>
                /// Specifes the network interface to be used by this
                /// discovery instance. This uses a platform-specifc
                /// format that identifes the network interface.
                /// </summary>
                property System::String^ MulticastInterface {
                    System::String^ get();
                    void set(System::String^ value);
                };

                /// <summary>
                /// A network address specifying the multicast group to
                /// be used for SPDP discovery. The default value is 239.255.0.1.
                /// </summary>
                /// <remarks>
                /// This overrides the interoperability group of the specifcation. 
                /// It can be used, for example, to specify use of a routed group
                /// address to provide a larger discovery scope.
                /// </remarks>
                property System::String^ DefaultMulticastGroup {
                    System::String^ get();
                    void set(System::String^ value);
                };

                /// <summary>
                /// A list (comma or whitespace separated) of host:port
                /// pairs used as destinations for SPDP content. This
                /// can be a combination of Unicast and Multicast
                /// addresses.
                /// </summary>
                property IEnumerable<System::String^>^ SpdpSendAddrs {
                    IEnumerable<System::String^>^ get();
                };

                /// <summary>
                /// Specifes the network interface to use when
                /// determining which local MAC address should
                /// appear in a GUID generated by this node.
                /// </summary>
                property System::String^ GuidInterface {
                    System::String^ get();
                    void set(System::String^ value);
                };
                
            public:
                /// <summary>
                /// Creates a new instance of <see cref="RtpsDiscovery" />.
                /// </summary>
                /// <param name="name">The name for the RTPS discovery.</param>
                RtpsDiscovery(System::String^ name);
            };
        }
    }
}