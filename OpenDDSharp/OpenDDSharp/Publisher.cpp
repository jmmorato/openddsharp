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
#include "Publisher.h"
#include "PublisherListener.h"
#include "DomainParticipant.h"

OpenDDSharp::DDS::Publisher::Publisher(::DDS::Publisher_ptr publisher) : OpenDDSharp::DDS::Entity(static_cast<::DDS::Entity_ptr>(publisher)) {
	impl_entity = ::DDS::Publisher::_duplicate(publisher);
};

OpenDDSharp::DDS::Publisher::!Publisher() {
    impl_entity = NULL;
}

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::Publisher::Participant::get() {
	return GetParticipant();
}

OpenDDSharp::DDS::DataWriter^ OpenDDSharp::DDS::Publisher::CreateDataWriter(OpenDDSharp::DDS::Topic^ topic) {
	return OpenDDSharp::DDS::Publisher::CreateDataWriter(topic, nullptr, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::DataWriter^ OpenDDSharp::DDS::Publisher::CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::DataWriterQos^ qos) {
	return OpenDDSharp::DDS::Publisher::CreateDataWriter(topic, qos, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::DataWriter^ OpenDDSharp::DDS::Publisher::CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener) {
	return OpenDDSharp::DDS::Publisher::CreateDataWriter(topic, nullptr, listener, StatusMask::DefaultStatusMask);
};

OpenDDSharp::DDS::DataWriter^ OpenDDSharp::DDS::Publisher::CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener, StatusMask statusMask) {
	return OpenDDSharp::DDS::Publisher::CreateDataWriter(topic, nullptr, listener, statusMask);
};

OpenDDSharp::DDS::DataWriter^ OpenDDSharp::DDS::Publisher::CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::DataWriterQos^ qos, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener) {
	return OpenDDSharp::DDS::Publisher::CreateDataWriter(topic, qos, listener, StatusMask::DefaultStatusMask);
};

OpenDDSharp::DDS::DataWriter^ OpenDDSharp::DDS::Publisher::CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::DataWriterQos^ qos, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener, StatusMask statusMask) {
	if (topic == nullptr) {
		return nullptr;
	}
	
	::DDS::DataWriterQos dwQos;
	if (qos != nullptr) {
		dwQos = qos->ToNative();
	}
	else {	
        impl_entity->get_default_datawriter_qos(dwQos);
	}

	::DDS::DataWriterListener_var lst = NULL;
	if (listener != nullptr) {
		lst = listener->impl_entity;
	}

	::DDS::DataWriter_var dw = impl_entity->create_datawriter(topic->impl_entity, dwQos, lst, (System::UInt32)statusMask);
	
	if (dw == NULL) {
		return nullptr;
	}

	OpenDDSharp::DDS::DataWriter^ w = gcnew OpenDDSharp::DDS::DataWriter(dw);
	w->_listener = listener;

	EntityManager::get_instance()->add(dw, w);
	contained_entities->Add(w);

	return w;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::DeleteDataWriter(OpenDDSharp::DDS::DataWriter^ datawriter) {
    if (datawriter == nullptr) {
        return OpenDDSharp::DDS::ReturnCode::Ok;
    }

	::DDS::ReturnCode_t ret = impl_entity->delete_datawriter(datawriter->impl_entity);
	if (ret == ::DDS::RETCODE_OK) {
		EntityManager::get_instance()->remove(datawriter->impl_entity);
		contained_entities->Remove(datawriter);            
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::DataWriter^ OpenDDSharp::DDS::Publisher::LookupDataWriter(System::String^ topicName) {
	msclr::interop::marshal_context context;

	::DDS::DataWriter_ptr dw = impl_entity->lookup_datawriter(context.marshal_as<const char *>(topicName));
	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(dw);

	return static_cast<OpenDDSharp::DDS::DataWriter^>(entity);	
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::DeleteContainedEntities() {	
	::DDS::ReturnCode_t ret = impl_entity->delete_contained_entities();
	if (ret == ::DDS::RETCODE_OK) {
		for each (Entity^ e in contained_entities) {
			EntityManager::get_instance()->remove(e->impl_entity);
		}	
		contained_entities->Clear();
	}	

	return (OpenDDSharp::DDS::ReturnCode)ret;	
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::SetQos(OpenDDSharp::DDS::PublisherQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_qos(qos->ToNative());
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::GetQos(OpenDDSharp::DDS::PublisherQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::PublisherQos nativeQos;
	::DDS::ReturnCode_t ret = impl_entity->get_qos(nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::SetListener(OpenDDSharp::DDS::PublisherListener^ listener) {
	return OpenDDSharp::DDS::Publisher::SetListener(listener, StatusMask::DefaultStatusMask);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::SetListener(OpenDDSharp::DDS::PublisherListener^ listener, OpenDDSharp::DDS::StatusMask mask) {
	m_listener = listener;
	if (m_listener != nullptr) {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(listener->impl_entity, (System::UInt32)mask);
	}
	else {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(NULL, (System::UInt32)mask);
	}
};

OpenDDSharp::DDS::PublisherListener^ OpenDDSharp::DDS::Publisher::GetListener() {
	return m_listener;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::SuspendPublications() {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->suspend_publications();
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::ResumePublications() {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->resume_publications();
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::BeginCoherentChanges() {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->begin_coherent_changes();
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::EndCoherentChanges() {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->end_coherent_changes();
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::WaitForAcknowledgments(OpenDDSharp::DDS::Duration maxWait) {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->wait_for_acknowledgments(maxWait.ToNative());
};

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::Publisher::GetParticipant() {	
	::DDS::DomainParticipant_ptr participant = impl_entity->get_participant();

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(participant);

	return static_cast<OpenDDSharp::DDS::DomainParticipant^>(entity);	
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::SetDefaultDataWriterQos(OpenDDSharp::DDS::DataWriterQos^ qos) {
	if (qos == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_default_datawriter_qos(qos->ToNative());
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Publisher::GetDefaultDataWriterQos(OpenDDSharp::DDS::DataWriterQos^ qos) {
	if (qos == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::DataWriterQos nativeQos;
	::DDS::ReturnCode_t ret = impl_entity->get_default_datawriter_qos(nativeQos);

    // OpenDDS always return OK, not neeed to check it
	qos->FromNative(nativeQos);	

	return (::OpenDDSharp::DDS::ReturnCode)ret;
};
