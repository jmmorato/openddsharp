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
#include "TransportConfig.h"

void TransportConfig_Insert(::OpenDDS::DCPS::TransportConfig* cfg, ::OpenDDS::DCPS::TransportInst* inst) {
	::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from< ::OpenDDS::DCPS::TransportInst>(inst);
	cfg->instances_.push_back(rch);
}

void TransportConfig_SortedInsert(::OpenDDS::DCPS::TransportConfig* cfg, ::OpenDDS::DCPS::TransportInst* inst) {
	::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from< ::OpenDDS::DCPS::TransportInst>(inst);
	cfg->sorted_insert(rch);
}

CORBA::Boolean TransportConfig_GetSwapBytes(::OpenDDS::DCPS::TransportConfig* cfg) {
	return cfg->swap_bytes_;
}

void TransportConfig_SetSwapBytes(::OpenDDS::DCPS::TransportConfig* cfg, CORBA::Boolean value) {
	cfg->swap_bytes_ = value;
}

CORBA::UInt32 TransportConfig_GetPassiveConnectDuration(::OpenDDS::DCPS::TransportConfig* cfg) {
	return cfg->passive_connect_duration_;
}

void TransportConfig_SetPassiveConnectDuration(::OpenDDS::DCPS::TransportConfig* cfg, CORBA::UInt32 value) {
	cfg->passive_connect_duration_ = value;
}

char* TransportConfig_GetName(::OpenDDS::DCPS::TransportConfig* cfg) {
	return CORBA::string_dup(cfg->name().c_str());
}

void* TransportConfig_GetTransports(::OpenDDS::DCPS::TransportConfig* cfg) {
	CORBA::ULongLong size = cfg->instances_.size();
	TAO::unbounded_value_sequence< ::OpenDDS::DCPS::TransportInst*> seq(static_cast<CORBA::ULong>(size));

	int i = 0;
	for (auto inst = cfg->instances_.begin(); inst != cfg->instances_.end(); ++inst) {		
		seq[i] = inst->in();
		i++;
	}
	
	void* ptr;
	unbounded_sequence_to_ptr(seq, ptr);

	return ptr;
}