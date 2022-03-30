/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2022 Jose Morato

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
#include <dds/DCPS/transport/framework/TransportInst.h>
#include <dds/DCPS/transport/framework/TransportInst_rch.h>

EXTERN_METHOD_EXPORT
char* TransportInst_GetTransportType(::OpenDDS::DCPS::TransportInst* ti);

EXTERN_METHOD_EXPORT
char* TransportInst_GetName(::OpenDDS::DCPS::TransportInst* ti);

EXTERN_METHOD_EXPORT
size_t TransportInst_GetQueueMessagesPerPool(::OpenDDS::DCPS::TransportInst* ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetQueueMessagesPerPool(::OpenDDS::DCPS::TransportInst* ti, size_t value);

EXTERN_METHOD_EXPORT
size_t TransportInst_GetQueueInitialPools(::OpenDDS::DCPS::TransportInst* ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetQueueInitialPools(::OpenDDS::DCPS::TransportInst* ti, size_t value);

EXTERN_METHOD_EXPORT
CORBA::ULong TransportInst_GetMaxPacketSize(::OpenDDS::DCPS::TransportInst* ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetMaxPacketSize(::OpenDDS::DCPS::TransportInst* ti, CORBA::ULong value);

EXTERN_METHOD_EXPORT
size_t TransportInst_GetMaxSamplesPerPacket(::OpenDDS::DCPS::TransportInst* ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetMaxSamplesPerPacket(::OpenDDS::DCPS::TransportInst* ti, size_t value);

EXTERN_METHOD_EXPORT
CORBA::ULong TransportInst_GetOptimumPacketSize(::OpenDDS::DCPS::TransportInst* ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetOptimumPacketSize(::OpenDDS::DCPS::TransportInst* ti, CORBA::ULong value);

EXTERN_METHOD_EXPORT
CORBA::Boolean TransportInst_GetThreadPerConnection(::OpenDDS::DCPS::TransportInst* ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetThreadPerConnection(::OpenDDS::DCPS::TransportInst* ti, CORBA::Boolean value);

EXTERN_METHOD_EXPORT
CORBA::UInt64 TransportInst_GetDatalinkReleaseDelay(::OpenDDS::DCPS::TransportInst* ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetDatalinkReleaseDelay(::OpenDDS::DCPS::TransportInst* ti, CORBA::UInt64 value);

EXTERN_METHOD_EXPORT
size_t TransportInst_GetDatalinkControlChunks(::OpenDDS::DCPS::TransportInst* ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetDatalinkControlChunks(::OpenDDS::DCPS::TransportInst* ti, size_t value);