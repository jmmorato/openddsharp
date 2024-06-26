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
#include "TimeValueWrapper.h"
#include <dds/DCPS/transport/rtps_udp/RtpsUdp.h>
#include <dds/DCPS/transport/rtps_udp/RtpsUdpInst.h>
#include <dds/DCPS/transport/rtps_udp/RtpsUdpInst_rch.h>
#include <dds/DCPS/transport/framework/TransportInst.h>
#include <dds/DCPS/transport/framework/TransportInst_rch.h>
#include <dds/DCPS/LogAddr.h>

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::RtpsUdpInst *RtpsUdpInst_new(::OpenDDS::DCPS::TransportInst *inst);

EXTERN_METHOD_EXPORT
CORBA::Boolean RtpsUdpInst_GetIsReliable(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
CORBA::Boolean RtpsUdpInst_GetRequiresCdr(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
CORBA::Int32 RtpsUdpInst_GetSendBufferSize(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetSendBufferSize(::OpenDDS::DCPS::RtpsUdpInst *ri, CORBA::Int32 value);

EXTERN_METHOD_EXPORT
CORBA::Int32 RtpsUdpInst_GetRcvBufferSize(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetRcvBufferSize(::OpenDDS::DCPS::RtpsUdpInst *ri, CORBA::Int32 value);

EXTERN_METHOD_EXPORT
size_t RtpsUdpInst_GetNakDepth(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetNakDepth(::OpenDDS::DCPS::RtpsUdpInst *ri, size_t value);

EXTERN_METHOD_EXPORT
CORBA::Boolean RtpsUdpInst_GetUseMulticast(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetUseMulticast(::OpenDDS::DCPS::RtpsUdpInst *ri, CORBA::Boolean value);

EXTERN_METHOD_EXPORT
CORBA::Octet RtpsUdpInst_GetTtl(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetTtl(::OpenDDS::DCPS::RtpsUdpInst *ri, CORBA::Octet value);

EXTERN_METHOD_EXPORT
char *RtpsUdpInst_GetMulticastGroupAddress(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetMulticastGroupAddress(::OpenDDS::DCPS::RtpsUdpInst *ri, char *value);

EXTERN_METHOD_EXPORT
char *RtpsUdpInst_GetMulticastInterface(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetMulticastInterface(::OpenDDS::DCPS::RtpsUdpInst *ri, char *value);

EXTERN_METHOD_EXPORT
char *RtpsUdpInst_GetLocalAddress(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetLocalAddress(::OpenDDS::DCPS::RtpsUdpInst *ri, char *value);

EXTERN_METHOD_EXPORT
TimeValueWrapper RtpsUdpInst_GetNakResponseDelay(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetNakResponseDelay(::OpenDDS::DCPS::RtpsUdpInst *ri, TimeValueWrapper value);

EXTERN_METHOD_EXPORT
TimeValueWrapper RtpsUdpInst_GetHeartbeatPeriod(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetHeartbeatPeriod(::OpenDDS::DCPS::RtpsUdpInst *ri, TimeValueWrapper value);

EXTERN_METHOD_EXPORT
TimeValueWrapper RtpsUdpInst_GetReceiveAddressDuration(::OpenDDS::DCPS::RtpsUdpInst *ri);

EXTERN_METHOD_EXPORT
void RtpsUdpInst_SetReceiveAddressDuration(::OpenDDS::DCPS::RtpsUdpInst *ri, TimeValueWrapper value);
