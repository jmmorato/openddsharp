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
#include "Topic.h"
#include "TopicListener.h"
#include "DomainParticipant.h"

OpenDDSharp::DDS::Topic::Topic(::DDS::Topic_ptr topic) : OpenDDSharp::DDS::Entity(static_cast<::DDS::Entity_ptr>(topic)) {
	impl_entity = ::DDS::Topic::_duplicate(topic);
}

OpenDDSharp::DDS::Topic::!Topic() {
    impl_entity = NULL;
}

System::String^ OpenDDSharp::DDS::Topic::TypeName::get() {
	return GetTypeName();
}

System::String^ OpenDDSharp::DDS::Topic::Name::get() {
	return GetName();
}

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::Topic::Participant::get() {
	return GetParticipant();
}

System::String^ OpenDDSharp::DDS::Topic::GetTypeName() {
	msclr::interop::marshal_context context;

	const char * typeName = impl_entity->get_type_name();
	return context.marshal_as<System::String^>(typeName);	
}

System::String^ OpenDDSharp::DDS::Topic::GetName() {
	msclr::interop::marshal_context context;

	const char * name = impl_entity->get_name();
	return context.marshal_as<System::String^>(name);	
}

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::Topic::GetParticipant() {
	::DDS::DomainParticipant_ptr participant = impl_entity->get_participant();

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(participant);
	return static_cast<OpenDDSharp::DDS::DomainParticipant^>(entity);	
}

::DDS::TopicDescription_ptr OpenDDSharp::DDS::Topic::ToNative() {
	return impl_entity;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Topic::SetQos(OpenDDSharp::DDS::TopicQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	
	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_qos(qos->ToNative());
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Topic::GetQos(OpenDDSharp::DDS::TopicQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::TopicQos nativeQos;
	::DDS::ReturnCode_t ret = impl_entity->get_qos(nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Topic::SetListener(OpenDDSharp::DDS::TopicListener^ listener) {
	return  OpenDDSharp::DDS::Topic::SetListener(listener, StatusMask::DefaultStatusMask);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Topic::SetListener(OpenDDSharp::DDS::TopicListener^ listener, StatusMask mask) {
	_listener = listener;
	if (_listener != nullptr) {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(listener->impl_entity, (System::UInt32)mask);
	}
	else {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(NULL, (System::UInt32)mask);
	}
}

OpenDDSharp::DDS::TopicListener^ OpenDDSharp::DDS::Topic::GetListener() {
	return _listener;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::Topic::GetInconsistentTopicStatus(OpenDDSharp::DDS::InconsistentTopicStatus% status) {
	::DDS::InconsistentTopicStatus native;
	::DDS::ReturnCode_t ret = impl_entity->get_inconsistent_topic_status(native);

	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(native);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
}
