#pragma once

#pragma unmanaged 
#include <dds/DCPS/Service_Participant.h>
#include <dds/DCPS/Marked_Default_Qos.h>
#pragma managed

#include "DomainParticipant.h"
#include "DomainParticipantQos.h"
#include "DomainParticipantListener.h"
#include "DomainParticipantFactoryQos.h"
#include "ReturnCode.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class DomainParticipantFactory {

		private:
			::DDS::DomainParticipantFactory_ptr impl_entity;

		internal:
			DomainParticipantFactory(::DDS::DomainParticipantFactory_ptr factory);			

		public:
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId);
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId, OpenDDSharp::DDS::DomainParticipantQos^ qos);
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener);
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener, StatusMask statusMask);
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId, OpenDDSharp::DDS::DomainParticipantQos^ qos, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener);
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId, OpenDDSharp::DDS::DomainParticipantQos^ qos, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener, StatusMask statusMask);
			OpenDDSharp::DDS::DomainParticipant^ LookupParticipant(int domainId);			
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::DomainParticipantFactoryQos^ qos);
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::DomainParticipantFactoryQos^ qos);
			OpenDDSharp::DDS::ReturnCode GetDefaultDomainParticipantQos(OpenDDSharp::DDS::DomainParticipantQos^ qos);
			OpenDDSharp::DDS::ReturnCode SetDefaultDomainParticipantQos(OpenDDSharp::DDS::DomainParticipantQos^ qos);
			OpenDDSharp::DDS::ReturnCode DeleteParticipant(OpenDDSharp::DDS::DomainParticipant^ participant);
		};

	};
};