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
#include "TransportRegistry.h"
#include <iostream>
#include <exception>

void TransportRegistry_Close() {
	::OpenDDS::DCPS::TransportRegistry::close();
}

void TransportRegistry_Release() {
	TheTransportRegistry->release();
}

bool TransportRegistry_GetReleased() {
	return TheTransportRegistry->released();
}

OpenDDS::DCPS::TransportConfig* TransportRegistry_CreateConfig(const char * name) {
	char buf[2048];
	sprintf(buf, "Create Config %s \n", name);
	OutputDebugString(buf);

	::OpenDDS::DCPS::TransportConfig_rch native = TheTransportRegistry->create_config(std::string(name));
	if (native.is_nil()) {
		return NULL;
	}	

	::OpenDDS::DCPS::TransportConfig* pointer = native.in();
	pointer->_add_ref();

	return pointer;
}

::OpenDDS::DCPS::TransportInst* TransportRegistry_CreateInst(const char * name, const char * transportType) {
	char buf[2048];
	sprintf(buf, "Create Inst name %s \n", name);
	OutputDebugString(buf);

	sprintf(buf, "Create Inst transport type %s \n", transportType);
	OutputDebugString(buf);

	::OpenDDS::DCPS::TransportInst_rch native = TheTransportRegistry->create_inst(std::string(name), transportType);
	if (native.is_nil()) {
		return NULL;
	}

	::OpenDDS::DCPS::TransportInst* pointer = native.in();
	pointer->_add_ref();

	return pointer;
}

void TransportRegistry_BindConfigName(const char * name, ::DDS::Entity_ptr entity) {
	char buf[2048];	
	sprintf(buf, "Bind Config Starts %s \n", name);
	OutputDebugString(buf);
	
	//::DDS::Entity_ptr entity = static_cast<::DDS::Entity_ptr>(ptr);
	try {
		const std::string str(name);
		
		sprintf(buf, "Bind Config InstanceHandle %d \n", entity->get_instance_handle());
		OutputDebugString(buf);
		TheTransportRegistry->bind_config(str, entity);
	}
	catch (std::exception& e) {
		buf[2048];
		sprintf(buf, "Bind Config Exception %s \n", e.what());
		OutputDebugString(buf);
	}

	sprintf(buf, "Bind Config Ends %s \n", name);
	OutputDebugString(buf);
}