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
#include <dds/DCPS/transport/udp/Udp.h>
#include <dds/DCPS/transport/udp/UdpInst.h>
#include <dds/DCPS/transport/udp/UdpInst_rch.h>
#include <dds/DCPS/transport/framework/TransportInst.h>
#include <dds/DCPS/transport/framework/TransportInst_rch.h>

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::UdpInst *UdpInst_new(::OpenDDS::DCPS::TransportInst *inst);

EXTERN_METHOD_EXPORT
CORBA::Boolean UdpInst_GetIsReliable(::OpenDDS::DCPS::UdpInst *ui);

EXTERN_METHOD_EXPORT
CORBA::Int32 UdpInst_GetSendBufferSize(::OpenDDS::DCPS::UdpInst *ui);

EXTERN_METHOD_EXPORT
void UdpInst_SetSendBufferSize(::OpenDDS::DCPS::UdpInst *ui, CORBA::Int32 value);

EXTERN_METHOD_EXPORT
CORBA::Int32 UdpInst_GetRcvBufferSize(::OpenDDS::DCPS::UdpInst *ui);

EXTERN_METHOD_EXPORT
void UdpInst_SetRcvBufferSize(::OpenDDS::DCPS::UdpInst *ui, CORBA::Int32 value);

EXTERN_METHOD_EXPORT
char *UdpInst_GetLocalAddress(::OpenDDS::DCPS::UdpInst *ui);

EXTERN_METHOD_EXPORT
void UdpInst_SetLocalAddress(::OpenDDS::DCPS::UdpInst *ui, char *value);