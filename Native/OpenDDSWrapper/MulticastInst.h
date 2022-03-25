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
#include <dds/DCPS/transport/multicast/Multicast.h>
#include <dds/DCPS/transport/multicast/MulticastInst.h>
#include <dds/DCPS/transport/multicast/MulticastInst_rch.h>
#include <dds/DCPS/transport/framework/TransportInst.h>
#include <dds/DCPS/transport/framework/TransportInst_rch.h>

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::MulticastInst* MulticastInst_new(::OpenDDS::DCPS::TransportInst* inst);

EXTERN_METHOD_EXPORT
CORBA::Boolean MulticastInst_GetIsReliable(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
CORBA::Boolean MulticastInst_GetReliable(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetReliable(::OpenDDS::DCPS::MulticastInst* mi, CORBA::Boolean value);

EXTERN_METHOD_EXPORT
CORBA::Boolean MulticastInst_GetDefaultToIpv6(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetDefaultToIpv6(::OpenDDS::DCPS::MulticastInst* mi, CORBA::Boolean value);

EXTERN_METHOD_EXPORT
CORBA::UShort MulticastInst_GetPortOffset(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetPortOffset(::OpenDDS::DCPS::MulticastInst* mi, CORBA::UShort value);

EXTERN_METHOD_EXPORT
char* MulticastInst_GetGroupAddress(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetGroupAddress(::OpenDDS::DCPS::MulticastInst* mi, char* value);

EXTERN_METHOD_EXPORT
char* MulticastInst_GetLocalAddress(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetLocalAddress(::OpenDDS::DCPS::MulticastInst* mi, char* value);

EXTERN_METHOD_EXPORT
CORBA::Double MulticastInst_GetSynBackoff(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetSynBackoff(::OpenDDS::DCPS::MulticastInst* mi, CORBA::Double value);

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::TimeDuration MulticastInst_GetSynInterval(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetSynInterval(::OpenDDS::DCPS::MulticastInst* mi, ::OpenDDS::DCPS::TimeDuration value);

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::TimeDuration MulticastInst_GetSynTimeout(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetSynTimeout(::OpenDDS::DCPS::MulticastInst* mi, ::OpenDDS::DCPS::TimeDuration value);

EXTERN_METHOD_EXPORT
size_t MulticastInst_GetNakDepth(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetNakDepth(::OpenDDS::DCPS::MulticastInst* mi, size_t value);

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::TimeDuration MulticastInst_GetNakInterval(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetNakInterval(::OpenDDS::DCPS::MulticastInst* mi, ::OpenDDS::DCPS::TimeDuration value);

EXTERN_METHOD_EXPORT
size_t MulticastInst_GetNakDelayIntervals(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetNakDelayIntervals(::OpenDDS::DCPS::MulticastInst* mi, size_t value);

EXTERN_METHOD_EXPORT
size_t MulticastInst_GetNakMax(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetNakMax(::OpenDDS::DCPS::MulticastInst* mi, size_t value);

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::TimeDuration MulticastInst_GetNakTimeout(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetNakTimeout(::OpenDDS::DCPS::MulticastInst* mi, ::OpenDDS::DCPS::TimeDuration value);

EXTERN_METHOD_EXPORT
CORBA::Octet MulticastInst_GetTtl(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetTtl(::OpenDDS::DCPS::MulticastInst* mi, CORBA::Octet value);

EXTERN_METHOD_EXPORT
size_t MulticastInst_GetRcvBufferSize(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetRcvBufferSize(::OpenDDS::DCPS::MulticastInst* mi, size_t value);

EXTERN_METHOD_EXPORT
CORBA::Boolean MulticastInst_GetAsyncSend(::OpenDDS::DCPS::MulticastInst* mi);

EXTERN_METHOD_EXPORT
void MulticastInst_SetAsyncSend(::OpenDDS::DCPS::MulticastInst* mi, CORBA::Boolean value);
