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
#include "DomainParticipantFactory.h"

OpenDDSharp::DDS::DomainParticipantFactory::DomainParticipantFactory(::DDS::DomainParticipantFactory_ptr factory) {
	impl_entity = ::DDS::DomainParticipantFactory::_duplicate(factory);
};

OpenDDSharp::DDS::DomainParticipantFactory::!DomainParticipantFactory() {
    impl_entity = NULL;
}

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(System::Int32 domainId) {
	return OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(domainId, nullptr, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(System::Int32 domainId, OpenDDSharp::DDS::DomainParticipantQos^ qos) {
	return OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(domainId, qos, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(System::Int32 domainId, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener) {
	return OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(domainId, nullptr, listener, StatusMask::DefaultStatusMask);
};

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(System::Int32 domainId, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener, StatusMask statusMask) {
	return OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(domainId, nullptr, listener, statusMask);
};

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(System::Int32 domainId, OpenDDSharp::DDS::DomainParticipantQos^ qos, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener) {
	return OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(domainId, qos, listener, StatusMask::DefaultStatusMask);
};

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::DomainParticipantFactory::CreateParticipant(System::Int32 domainId, OpenDDSharp::DDS::DomainParticipantQos^ qos, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener, StatusMask statusMask) {
	if (domainId < 0 || domainId > 231) {
		return nullptr;
	}

	::DDS::DomainParticipantQos dpQos;
	if (qos != nullptr) {
		dpQos = qos->ToNative();
	}
	else {
		if (impl_entity->get_default_participant_qos(dpQos) != ::DDS::RETCODE_OK) {
			dpQos = ::PARTICIPANT_QOS_DEFAULT;
		}
	}

	::DDS::DomainParticipantListener_var lst = NULL;
	if (listener != nullptr) {
		lst = listener->impl_entity;
	}

	::DDS::DomainParticipant_ptr participant = this->impl_entity->create_participant(domainId, dpQos, lst, (System::UInt32)statusMask);

	if (participant != NULL) {
		char id_string[32];
		sprintf(id_string, "%d", ++counter);
		char config_name[64] = "openddsharp_rtps_interop_";
		char inst_name[64] = "internal_openddsharp_rtps_transport_";
		strcat(config_name, id_string);
		strcat(inst_name, id_string);

		::OpenDDS::DCPS::TransportConfig_rch config = ::OpenDDS::DCPS::TransportRegistry::instance()->create_config(config_name);
		::OpenDDS::DCPS::TransportInst_rch inst = ::OpenDDS::DCPS::TransportRegistry::instance()->create_inst(inst_name, "rtps_udp");
		::OpenDDS::DCPS::RtpsUdpInst_rch rui = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::RtpsUdpInst>(inst);
		rui->handshake_timeout_ = 1;
		config->instances_.push_back(inst);		

		::OpenDDS::DCPS::TransportRegistry::instance()->bind_config(config_name, participant);		

		OpenDDSharp::DDS::DomainParticipant^ p = gcnew OpenDDSharp::DDS::DomainParticipant(participant);
		p->m_listener = listener;

		EntityManager::get_instance()->add(participant, p);

		return p;
	}
	else {
		return nullptr;
	}
};

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::DomainParticipantFactory::LookupParticipant(System::Int32 domainId) {
	::DDS::DomainParticipant_ptr dp = impl_entity->lookup_participant(domainId);	

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(dp);
	if (entity != nullptr) {
		return static_cast<OpenDDSharp::DDS::DomainParticipant^>(entity);
	}
	else {
		return nullptr;
	}
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipantFactory::GetQos(OpenDDSharp::DDS::DomainParticipantFactoryQos^ qos) {
	::DDS::DomainParticipantFactoryQos* nativeQos = new ::DDS::DomainParticipantFactoryQos();
	::DDS::ReturnCode_t ret = impl_entity->get_qos(*nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(*nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipantFactory::SetQos(OpenDDSharp::DDS::DomainParticipantFactoryQos^ qos) {
	::DDS::DomainParticipantFactoryQos nativeQos = qos->ToNative();
	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_qos(nativeQos);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipantFactory::GetDefaultDomainParticipantQos(OpenDDSharp::DDS::DomainParticipantQos^ qos) {
	::DDS::DomainParticipantQos* nativeQos = new ::DDS::DomainParticipantQos();
	::DDS::ReturnCode_t ret = impl_entity->get_default_participant_qos(*nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(*nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipantFactory::SetDefaultDomainParticipantQos(OpenDDSharp::DDS::DomainParticipantQos^ qos) {
	::DDS::DomainParticipantQos nativeQos = qos->ToNative();
	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_default_participant_qos(nativeQos);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipantFactory::DeleteParticipant(OpenDDSharp::DDS::DomainParticipant^ participant) {
	if (participant == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	::DDS::ReturnCode_t ret = impl_entity->delete_participant(participant->impl_entity);
	if (ret == ::DDS::RETCODE_OK) {
		EntityManager::get_instance()->remove(participant->impl_entity);
        participant->impl_entity = NULL;        
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;	
}
