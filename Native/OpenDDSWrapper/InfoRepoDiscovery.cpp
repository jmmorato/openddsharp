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
#include "InfoRepoDiscovery.h"

::OpenDDS::DCPS::Discovery *InfoRepoDiscovery_NarrowBase(::OpenDDS::DCPS::InfoRepoDiscovery *d) {
  return dynamic_cast< ::OpenDDS::DCPS::InfoRepoDiscovery *>(d);
}

::OpenDDS::DCPS::InfoRepoDiscovery *InfoRepoDiscovery_new(const char *key, const char *ior) {
  return new ::OpenDDS::DCPS::InfoRepoDiscovery(key, ior);
}

CORBA::Long InfoRepoDiscovery_GetBitTransportPort(::OpenDDS::DCPS::InfoRepoDiscovery *idr) {
  return idr->bit_transport_port();
}

void InfoRepoDiscovery_SetBitTransportPort(::OpenDDS::DCPS::InfoRepoDiscovery *idr, CORBA::Long port_number) {
  idr->bit_transport_port(port_number);
}

char *InfoRepoDiscovery_GetBitTransportIp(::OpenDDS::DCPS::InfoRepoDiscovery *idr) {
  return CORBA::string_dup(idr->bit_transport_ip().c_str());
}

void InfoRepoDiscovery_SetBitTransportIp(::OpenDDS::DCPS::InfoRepoDiscovery *idr, char *ip) {
  idr->bit_transport_ip(ip);
}