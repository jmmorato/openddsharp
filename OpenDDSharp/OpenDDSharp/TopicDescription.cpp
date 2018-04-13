#include "TopicDescription.h"

OpenDDSharp::DDS::TopicDescription::TopicDescription(::DDS::TopicDescription_ptr topicDescription) {
	impl_entity = topicDescription;
}

System::String^ OpenDDSharp::DDS::TopicDescription::TypeName::get() {
	return GetTypeName();
}

System::String^ OpenDDSharp::DDS::TopicDescription::Name::get() {
	return GetName();
}

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::TopicDescription::Participant::get() {
	return GetParticipant();
}

System::String^ OpenDDSharp::DDS::TopicDescription::GetTypeName() {
	msclr::interop::marshal_context context;

	const char * typeName = impl_entity->get_type_name();
	if (typeName != NULL) {
		return context.marshal_as<System::String^>(typeName);
	}
	else {
		return nullptr;
	}
}

System::String^ OpenDDSharp::DDS::TopicDescription::GetName() {
	msclr::interop::marshal_context context;

	const char * name = impl_entity->get_name();
	if (name != NULL) {
		return context.marshal_as<System::String^>(name);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::DomainParticipant^ OpenDDSharp::DDS::TopicDescription::GetParticipant() {
	::DDS::DomainParticipant_ptr participant = impl_entity->get_participant();

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(participant);
	if (entity != nullptr) {
		return static_cast<OpenDDSharp::DDS::DomainParticipant^>(entity);
	}
	else {
		return nullptr;
	}
}

::DDS::TopicDescription_ptr OpenDDSharp::DDS::TopicDescription::ToNative() {
	return impl_entity;
}
