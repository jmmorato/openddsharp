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
			public ref class ParticipantService {

			private:
				static ParticipantService _instance;
				::OpenDDS::DCPS::Service_Participant* impl_entity;

			public:
				static property ParticipantService^ Instance { ParticipantService^ get(); }

			private:
				ParticipantService();
		
			public:
				OpenDDSharp::DDS::DomainParticipantFactory^ GetDomainParticipantFactory();
				OpenDDSharp::DDS::DomainParticipantFactory^ GetDomainParticipantFactory(array<System::String^>^ argv);
				void Shutdown();
			};
		};
	};
};