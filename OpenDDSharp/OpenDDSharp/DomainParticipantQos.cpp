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
#include "DomainParticipantQos.h"

OpenDDSharp::DDS::DomainParticipantQos::DomainParticipantQos() {
	user_data = gcnew OpenDDSharp::DDS::UserDataQosPolicy();
	entity_factory = gcnew OpenDDSharp::DDS::EntityFactoryQosPolicy();	
};

OpenDDSharp::DDS::UserDataQosPolicy^ OpenDDSharp::DDS::DomainParticipantQos::UserData::get() {
	return user_data;
};

OpenDDSharp::DDS::EntityFactoryQosPolicy^ OpenDDSharp::DDS::DomainParticipantQos::EntityFactory::get() {
	return entity_factory;
};

::DDS::DomainParticipantQos OpenDDSharp::DDS::DomainParticipantQos::ToNative() {
	::DDS::DomainParticipantQos qos;

	qos.entity_factory = entity_factory->ToNative();
	qos.user_data = user_data->ToNative();

	return qos;
};

void OpenDDSharp::DDS::DomainParticipantQos::FromNative(::DDS::DomainParticipantQos qos) {
	entity_factory->FromNative(qos.entity_factory);
	user_data->FromNative(qos.user_data);
};
