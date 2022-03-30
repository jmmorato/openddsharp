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

::OpenDDS::DCPS::TransportInst* TransportRegistry_CreateInst(const char * name, const char * transportType) {
	::OpenDDS::DCPS::TransportInst_rch native = TheTransportRegistry->create_inst(std::string(name), transportType);
	if (native.is_nil()) {
		return NULL;
	}

	::OpenDDS::DCPS::TransportInst* pointer = native.in();
	pointer->_add_ref();

	return pointer;
}

OpenDDS::DCPS::TransportInst* TransportRegistry_GetInst(const char * name) {
	::OpenDDS::DCPS::TransportInst_rch native = TheTransportRegistry->get_inst(name);

    if (native.is_nil()) {
        return NULL;
    }

	::OpenDDS::DCPS::TransportInst* pointer = native.in();
    pointer->_add_ref();

    return pointer;
}

void TransportRegistry_RemoveInst(::OpenDDS::DCPS::TransportInst* inst) {
	if (inst == NULL) {
        return;
    }

    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from< ::OpenDDS::DCPS::TransportInst>(inst);
	TheTransportRegistry->remove_inst(rch);
}

OpenDDS::DCPS::TransportConfig* TransportRegistry_CreateConfig(const char * name) {
	::OpenDDS::DCPS::TransportConfig_rch native = TheTransportRegistry->create_config(name);

	if (native.is_nil()) {
		return NULL;
	}	

	::OpenDDS::DCPS::TransportConfig* pointer = native.in();
	pointer->_add_ref();

	return pointer;
}

OpenDDS::DCPS::TransportConfig* TransportRegistry_GetConfig(const char * name) {
	::OpenDDS::DCPS::TransportConfig_rch native = TheTransportRegistry->get_config(name);

    if (native.is_nil()) {
        return NULL;
    }

	::OpenDDS::DCPS::TransportConfig* pointer = native.in();
    pointer->_add_ref();

    return pointer;
}

void TransportRegistry_RemoveConfig(OpenDDS::DCPS::TransportConfig* cfg) {
	::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from< ::OpenDDS::DCPS::TransportConfig>(cfg);
	TheTransportRegistry->remove_config(rch);
}

OpenDDS::DCPS::TransportConfig* TransportRegistry_GetDomainDefaultConfig(CORBA::Int32 domainId) {
	::OpenDDS::DCPS::TransportConfig_rch native = TheTransportRegistry->domain_default_config(domainId);

    if (native.is_nil()) {
        return nullptr;
    }

	::OpenDDS::DCPS::TransportConfig* pointer = native.in();
	pointer->_add_ref();

    return pointer;
}

void TransportRegistry_SetDomainDefaultConfig(CORBA::Int32 domainId, OpenDDS::DCPS::TransportConfig* cfg) {
	::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from< ::OpenDDS::DCPS::TransportConfig>(cfg);
	TheTransportRegistry->domain_default_config(domainId, rch);
}

void TransportRegistry_BindConfigName(const char * name, ::DDS::Entity_ptr entity) {
	const std::string str(name);
	TheTransportRegistry->bind_config(str, entity);
}

void TransportRegistry_BindConfigTransport(OpenDDS::DCPS::TransportConfig* cfg, ::DDS::Entity_ptr entity) {
	::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from< ::OpenDDS::DCPS::TransportConfig>(cfg);
	TheTransportRegistry->bind_config(rch, entity);
}

OpenDDS::DCPS::TransportConfig* TransportRegistry_GetGlobalConfig() {
	::OpenDDS::DCPS::TransportConfig_rch native = TheTransportRegistry->global_config();
	
    if (native.is_nil()) {
        return NULL;
    }

	::OpenDDS::DCPS::TransportConfig* pointer = native.in();
	pointer->_add_ref();

    return pointer;
}

void TransportRegistry_SetGlobalConfig(OpenDDS::DCPS::TransportConfig* cfg) {
	::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from< ::OpenDDS::DCPS::TransportConfig>(cfg);
	TheTransportRegistry->global_config(rch);
}
