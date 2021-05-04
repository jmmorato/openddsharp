/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

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
#include "EntityFactoryQosPolicy.h"

OpenDDSharp::DDS::EntityFactoryQosPolicy::EntityFactoryQosPolicy() {
	autoenable_created_entities = true;
};

System::Boolean OpenDDSharp::DDS::EntityFactoryQosPolicy::AutoenableCreatedEntities::get() {
	return autoenable_created_entities;	
}

void OpenDDSharp::DDS::EntityFactoryQosPolicy::AutoenableCreatedEntities::set(System::Boolean value) {	
	autoenable_created_entities = value;
}

::DDS::EntityFactoryQosPolicy OpenDDSharp::DDS::EntityFactoryQosPolicy::ToNative() {
	::DDS::EntityFactoryQosPolicy qos;

	if (autoenable_created_entities) {
		qos.autoenable_created_entities = TRUE;
	}
	else {
		qos.autoenable_created_entities = FALSE;
	}

	return qos;
};

void OpenDDSharp::DDS::EntityFactoryQosPolicy::FromNative(::DDS::EntityFactoryQosPolicy qos) {	
	if (qos.autoenable_created_entities == FALSE) {
		autoenable_created_entities = false;
	}
	else {
		autoenable_created_entities = true;
	}
};