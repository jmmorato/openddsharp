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
#pragma once

#include "Utils.h"
#include "marshal.h"
#include <dds/DCPS/transport/framework/TransportConfig_rch.h>
#include <dds/DCPS/transport/framework/TransportConfig.h>
#include <dds/DCPS/transport/framework/TransportInst.h>

EXTERN_METHOD_EXPORT
void TransportConfig_Insert(::OpenDDS::DCPS::TransportConfig *cfg, ::OpenDDS::DCPS::TransportInst *inst);

EXTERN_METHOD_EXPORT
void TransportConfig_SortedInsert(::OpenDDS::DCPS::TransportConfig *cfg, ::OpenDDS::DCPS::TransportInst *inst);

EXTERN_METHOD_EXPORT
CORBA::Boolean TransportConfig_GetSwapBytes(::OpenDDS::DCPS::TransportConfig *cfg);

EXTERN_METHOD_EXPORT
void TransportConfig_SetSwapBytes(::OpenDDS::DCPS::TransportConfig *cfg, CORBA::Boolean value);

EXTERN_METHOD_EXPORT
CORBA::UInt32 TransportConfig_GetPassiveConnectDuration(::OpenDDS::DCPS::TransportConfig *cfg);

EXTERN_METHOD_EXPORT
void TransportConfig_SetPassiveConnectDuration(::OpenDDS::DCPS::TransportConfig *cfg, CORBA::UInt32 value);

EXTERN_METHOD_EXPORT
char *TransportConfig_GetName(::OpenDDS::DCPS::TransportConfig *cfg);

EXTERN_METHOD_EXPORT
void *TransportConfig_GetTransports(::OpenDDS::DCPS::TransportConfig *cfg);