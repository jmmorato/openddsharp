#include "Topic.h"
#include "TopicListener.h"
#include "DomainParticipant.h"

OpenDDSharp::DDS::Topic::Topic(::DDS::Topic_ptr topic) : OpenDDSharp::DDS::Entity(topic) {
	impl_entity = topic;	
}

System::String^ OpenDDSharp::DDS::Topic::GetTypeName() {
	msclr::interop::marshal_context context;

	const char * typeName = impl_entity->get_type_name();
	if (typeName != NULL) {
		return context.marshal_as<System::String^>(typeName);
	}
	else {
		return nullptr;
	}
}

System::String^ OpenDDSharp::DDS::Topic::GetName() {
	msclr::interop::marshal_context context;

	const char * name = impl_entity->get_name();
	if (name != NULL) {
		return context.marshal_as<System::String^>(name);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::Topic::GetParticipant() {
	::DDS::DomainParticipant_ptr participant = impl_entity->get_participant();

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(participant);
	if (entity != nullptr) {
		return static_cast<OpenDDSharp::DDS::DomainParticipant^>(entity);
	}
	else {
		return nullptr;
	}
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
