/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#pragma unmanaged 
#include <dds/DCPS/Service_Participant.h>
#include <dds/DCPS/Discovery.h>
#include <dds/DCPS/RTPS/RtpsDiscovery.h>
#include "dds/DCPS/Message_Block_Ptr.h"

#include <dds/DCPS/RTPS/RtpsDiscovery.h>
#include <dds/DCPS/transport/framework/TransportRegistry.h>

#include "dds/DCPS/StaticIncludes.h"
#include <dds/DCPS/transport/rtps_udp/RtpsUdp.h>

#include <dds/DCPS/transport/rtps_udp/RtpsUdpInst.h>
#include <dds/DCPS/transport/rtps_udp/RtpsUdpInst_rch.h>

#pragma managed

#pragma make_public(DDS::SampleInfo)

#include "DomainParticipantFactory.h"

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			/// <summary>
			/// Singleton object to obtain the <see cref="OpenDDSharp::DDS::DomainParticipantFactory" />.
			/// </summary>
			public ref class ParticipantService {

			private:
				static ParticipantService _instance;
				::OpenDDS::DCPS::Service_Participant* impl_entity;

			public:
				/// <summary>
				/// Gets the singleton instance
				/// </summary>
				static property ParticipantService^ Instance { ParticipantService^ get(); }

			private:
				ParticipantService();
		
			public:
				/// <summary>
				/// Initialize the DDS client environment and get the <see cref="OpenDDSharp::DDS::DomainParticipantFactory" />.
				/// </summary>
				/// <returns> The <see cref="OpenDDSharp::DDS::DomainParticipantFactory" />.</returns>
				OpenDDSharp::DDS::DomainParticipantFactory^ GetDomainParticipantFactory();

				/// <summary>
				/// Initialize the DDS client environment and get the <see cref="OpenDDSharp::DDS::DomainParticipantFactory" />.
				/// This method consumes -DCPS* and -ORB* options and their arguments.
				/// </summary>
				/// <param name="argv">The array of parameters to be consumed (i.e. -DCPS* and -ORB* options).</param>
				/// <returns> The <see cref="OpenDDSharp::DDS::DomainParticipantFactory" />.</returns>
				OpenDDSharp::DDS::DomainParticipantFactory^ GetDomainParticipantFactory(array<System::String^>^ argv);

				/// <summary>
				/// Stop being a participant in the service.
				/// </summary>
				/// <remarks>
				/// Required Precondition: All DomainParticipants have been deleted.
				/// </remarks>
				void Shutdown();
			};
		};
	};
};