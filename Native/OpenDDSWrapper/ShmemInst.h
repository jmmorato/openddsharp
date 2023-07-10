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
#include <dds/DCPS/transport/shmem/Shmem.h>
#include <dds/DCPS/transport/shmem/ShmemInst.h>
#include <dds/DCPS/transport/shmem/ShmemInst_rch.h>
#include <dds/DCPS/transport/framework/TransportInst.h>
#include <dds/DCPS/transport/framework/TransportInst_rch.h>

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::ShmemInst *ShmemInst_new(::OpenDDS::DCPS::TransportInst *inst);

EXTERN_METHOD_EXPORT
CORBA::Boolean ShmemInst_GetIsReliable(::OpenDDS::DCPS::ShmemInst *si);

EXTERN_METHOD_EXPORT
size_t ShmemInst_GetPoolSize(::OpenDDS::DCPS::ShmemInst *si);

EXTERN_METHOD_EXPORT
void ShmemInst_SetPoolSize(::OpenDDS::DCPS::ShmemInst *si, size_t value);

EXTERN_METHOD_EXPORT
size_t ShmemInst_GetDatalinkControlSize(::OpenDDS::DCPS::ShmemInst *si);

EXTERN_METHOD_EXPORT
void ShmemInst_SetDatalinkControlSize(::OpenDDS::DCPS::ShmemInst *si, size_t value);

EXTERN_METHOD_EXPORT
char *ShmemInst_GetHostName(::OpenDDS::DCPS::ShmemInst *si);

EXTERN_METHOD_EXPORT
char *ShmemInst_GetPoolName(::OpenDDS::DCPS::ShmemInst *si);