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
	::OpenDDS::DCPS::TransportConfig_rch native = TheTransportRegistry->create_config(std::string(name));
	if (native.is_nil()) {
		return NULL;
	}	

	::OpenDDS::DCPS::TransportConfig* pointer = native.in();
	pointer->_add_ref();

	return pointer;
}

::OpenDDS::DCPS::TransportInst* TransportRegistry_CreateInst(const char * name, const char * transportType) {
	::OpenDDS::DCPS::TransportInst_rch native = TheTransportRegistry->create_inst(std::string(name), transportType);
	if (native.is_nil()) {
		return NULL;
	}

	::OpenDDS::DCPS::TransportInst* pointer = native.in();
	pointer->_add_ref();

	return pointer;
}

void TransportRegistry_BindConfigName(const char * name, ::DDS::Entity_ptr entity) {
	const std::string str(name);
	TheTransportRegistry->bind_config(str, entity);
}