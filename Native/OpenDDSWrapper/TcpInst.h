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
#include <dds/DCPS/transport/tcp/Tcp.h>
#include <dds/DCPS/transport/tcp/TcpInst.h>
#include <dds/DCPS/transport/framework/TransportInst.h>
#include <dds/DCPS/transport/framework/TransportInst_rch.h>

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::TcpInst *TcpInst_new(::OpenDDS::DCPS::TransportInst *inst);

EXTERN_METHOD_EXPORT
CORBA::Boolean TcpInst_GetIsReliable(::OpenDDS::DCPS::TcpInst *ti);

EXTERN_METHOD_EXPORT
CORBA::Boolean TcpInst_GetEnableNagleAlgorithm(::OpenDDS::DCPS::TcpInst *ti);

EXTERN_METHOD_EXPORT
void TcpInst_SetEnableNagleAlgorithm(::OpenDDS::DCPS::TcpInst *ti, CORBA::Boolean value);

EXTERN_METHOD_EXPORT
CORBA::Long TcpInst_GetConnRetryInitialDelay(::OpenDDS::DCPS::TcpInst *ti);

EXTERN_METHOD_EXPORT
void TcpInst_SetConnRetryInitialDelay(::OpenDDS::DCPS::TcpInst *ti, CORBA::Long value);

EXTERN_METHOD_EXPORT
CORBA::Double TcpInst_GetConnRetryBackoffMultiplier(::OpenDDS::DCPS::TcpInst *ti);

EXTERN_METHOD_EXPORT
void TcpInst_SetConnRetryBackoffMultiplier(::OpenDDS::DCPS::TcpInst *ti, CORBA::Double value);

EXTERN_METHOD_EXPORT
CORBA::Long TcpInst_GetConnRetryAttempts(::OpenDDS::DCPS::TcpInst *ti);

EXTERN_METHOD_EXPORT
void TcpInst_SetConnRetryAttempts(::OpenDDS::DCPS::TcpInst *ti, CORBA::Long value);

EXTERN_METHOD_EXPORT
CORBA::Long TcpInst_GetMaxOutputPausePeriod(::OpenDDS::DCPS::TcpInst *ti);

EXTERN_METHOD_EXPORT
void TcpInst_SetMaxOutputPausePeriod(::OpenDDS::DCPS::TcpInst *ti, CORBA::Long value);

EXTERN_METHOD_EXPORT
CORBA::Long TcpInst_GetPassiveReconnectDuration(::OpenDDS::DCPS::TcpInst *ti);

EXTERN_METHOD_EXPORT
void TcpInst_SetPassiveReconnectDuration(::OpenDDS::DCPS::TcpInst *ti, CORBA::Long value);

EXTERN_METHOD_EXPORT
char *TcpInst_GetPublicAddress(::OpenDDS::DCPS::TcpInst *ti);

EXTERN_METHOD_EXPORT
void TcpInst_SetPublicAddress(::OpenDDS::DCPS::TcpInst *ti, char *value);

EXTERN_METHOD_EXPORT
char *TcpInst_GetLocalAddress(::OpenDDS::DCPS::TcpInst *ti);

EXTERN_METHOD_EXPORT
void TcpInst_SetLocalAddress(::OpenDDS::DCPS::TcpInst *ti, char *value);
