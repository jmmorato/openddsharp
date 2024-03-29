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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#include "Utils.h"

#include <dds/DCPS/InfoRepoDiscovery/InfoRepoDiscovery.h>

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::Discovery *InfoRepoDiscovery_NarrowBase(::OpenDDS::DCPS::InfoRepoDiscovery *d);

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::InfoRepoDiscovery *InfoRepoDiscovery_new(const char *key, const char *ior);

EXTERN_METHOD_EXPORT
CORBA::Long InfoRepoDiscovery_GetBitTransportPort(::OpenDDS::DCPS::InfoRepoDiscovery *idr);

EXTERN_METHOD_EXPORT
void InfoRepoDiscovery_SetBitTransportPort(::OpenDDS::DCPS::InfoRepoDiscovery *idr, CORBA::Long port_number);

EXTERN_METHOD_EXPORT
char *InfoRepoDiscovery_GetBitTransportIp(::OpenDDS::DCPS::InfoRepoDiscovery *idr);

EXTERN_METHOD_EXPORT
void InfoRepoDiscovery_SetBitTransportIp(::OpenDDS::DCPS::InfoRepoDiscovery *idr, char *ip);