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
#include "Subscriber.h"
#include "SubscriberListener.h"
#include "DomainParticipant.h"

OpenDDSharp::DDS::Subscriber::Subscriber(::DDS::Subscriber_ptr subscriber) : OpenDDSharp::DDS::Entity(subscriber) {
	impl_entity = subscriber;
};

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::Subscriber::Participant::get() {
	return GetParticipant();
}

OpenDDSharp::DDS::DataReader^ OpenDDSharp::DDS::Subscriber::CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription) {
	return OpenDDSharp::DDS::Subscriber::CreateDataReader(topicDescription, nullptr, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::DataReader^ OpenDDSharp::DDS::Subscriber::CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription, ::OpenDDSharp::DDS::DataReaderQos^ qos) {
	return OpenDDSharp::DDS::Subscriber::CreateDataReader(topicDescription, qos, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::DataReader^ OpenDDSharp::DDS::Subscriber::CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener) {
	return OpenDDSharp::DDS::Subscriber::CreateDataReader(topicDescription, nullptr, listener, StatusMask::DefaultStatusMask);
};

OpenDDSharp::DDS::DataReader^ OpenDDSharp::DDS::Subscriber::CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener, OpenDDSharp::DDS::StatusMask statusMask) {
	return OpenDDSharp::DDS::Subscriber::CreateDataReader(topicDescription, nullptr, listener, statusMask);
};

OpenDDSharp::DDS::DataReader^ OpenDDSharp::DDS::Subscriber::CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription, ::OpenDDSharp::DDS::DataReaderQos^ qos, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener) {
	return OpenDDSharp::DDS::Subscriber::CreateDataReader(topicDescription, qos, listener, StatusMask::DefaultStatusMask);
};

OpenDDSharp::DDS::DataReader^ OpenDDSharp::DDS::Subscriber::CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription, ::OpenDDSharp::DDS::DataReaderQos^ qos, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener, OpenDDSharp::DDS::StatusMask statusMask) {
	if (topicDescription == nullptr) {
		return nullptr;
	}

	::DDS::DataReaderQos drQos;
	if (qos != nullptr) {
		drQos = qos->ToNative();
	}
	else {		
		if (impl_entity->get_default_datareader_qos(drQos) != ::DDS::RETCODE_OK) {
			drQos = ::DATAREADER_QOS_DEFAULT;
		}
	}

	::DDS::DataReaderListener_var lst = NULL;
	if (listener != nullptr) {
		lst = listener->impl_entity;
	}
	
	::DDS::DataReader_ptr dr = impl_entity->create_datareader(topicDescription->ToNative(), drQos, lst.in(), (System::UInt32)statusMask);

	if (dr != NULL) {
		OpenDDSharp::DDS::DataReader^ r = gcnew OpenDDSharp::DDS::DataReader(dr);
		r->_listener = listener;

		EntityManager::get_instance()->add(dr, r);
		contained_entities->Add(r);

		return r;
	}
	else {
		return nullptr;
	}
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::DeleteDataReader(OpenDDSharp::DDS::DataReader^ datareader) {
	if (datareader == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	
	::DDS::ReturnCode_t ret = impl_entity->delete_datareader(datareader->impl_entity);
	if (ret == ::DDS::RETCODE_OK) {
		EntityManager::get_instance()->remove(datareader->impl_entity);
		contained_entities->Remove(datareader);
	}	

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::DeleteContainedEntities() {	
	::DDS::ReturnCode_t ret = impl_entity->delete_contained_entities();
	if (ret != ::DDS::RETCODE_OK) {
		for each (Entity^ e in contained_entities) {
			EntityManager::get_instance()->remove(e->impl_entity);
		}
		contained_entities->Clear();
	}
	
	return (OpenDDSharp::DDS::ReturnCode)ret;	
};

OpenDDSharp::DDS::DataReader^ OpenDDSharp::DDS::Subscriber::LookupDataReader(System::String^ topicName) {
	msclr::interop::marshal_context context;

	::DDS::DataReader_ptr dr = impl_entity->lookup_datareader(context.marshal_as<const char *>(topicName));
	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(dr);

	if (entity != nullptr) {
		return static_cast<OpenDDSharp::DDS::DataReader^>(entity);
	}
	else {
		return nullptr;
	}
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::GetDataReaders(IList<OpenDDSharp::DDS::DataReader^>^ readers) {
	return GetDataReaders(readers, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::GetDataReaders(IList<OpenDDSharp::DDS::DataReader^>^ readers, OpenDDSharp::DDS::SampleStateMask sampleStates, OpenDDSharp::DDS::ViewStateMask viewStates, OpenDDSharp::DDS::InstanceStateMask instanceStates) {
	if (readers == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	readers->Clear();

	::DDS::DataReaderSeq seq;
	::DDS::ReturnCode_t ret = impl_entity->get_datareaders(seq, sampleStates, viewStates, instanceStates);
	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < seq.length(); i++) {
			OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(seq[i]);
			readers->Add(static_cast<OpenDDSharp::DDS::DataReader^>(entity));
		}
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::NotifyDataReaders() {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->notify_datareaders();
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::SetQos(OpenDDSharp::DDS::SubscriberQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_qos(qos->ToNative());
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::GetQos(OpenDDSharp::DDS::SubscriberQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::SubscriberQos nativeQos;
	::DDS::ReturnCode_t ret = impl_entity->get_qos(nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::SetListener(OpenDDSharp::DDS::SubscriberListener^ listener) {
	return OpenDDSharp::DDS::Subscriber::SetListener(listener, StatusMask::DefaultStatusMask);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::SetListener(OpenDDSharp::DDS::SubscriberListener^ listener, OpenDDSharp::DDS::StatusMask mask) {
	m_listener = listener;

	if (m_listener != nullptr) {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(listener->impl_entity, (System::UInt32)mask);
	}
	else {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(NULL, (System::UInt32)mask);
	}
};

OpenDDSharp::DDS::SubscriberListener^ OpenDDSharp::DDS::Subscriber::GetListener() {
	return m_listener;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::BeginAccess() {	
	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->begin_access();
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::EndAccess() {
	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->end_access();
};

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::Subscriber::GetParticipant() {
	::DDS::DomainParticipant_ptr participant = impl_entity->get_participant();

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(participant);
	if (entity != nullptr) {
		return static_cast<OpenDDSharp::DDS::DomainParticipant^>(entity);
	}
	else {
		return nullptr;
	}
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::SetDefaultDataReaderQos(OpenDDSharp::DDS::DataReaderQos^ qos) {
	if (qos == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_default_datareader_qos(qos->ToNative());
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Subscriber::GetDefaultDataReaderQos(OpenDDSharp::DDS::DataReaderQos^ qos) {
	if (qos == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::DataReaderQos nativeQos;
	::DDS::ReturnCode_t ret = impl_entity->get_default_datareader_qos(nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
};
