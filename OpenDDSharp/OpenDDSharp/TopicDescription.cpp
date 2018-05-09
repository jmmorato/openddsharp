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
