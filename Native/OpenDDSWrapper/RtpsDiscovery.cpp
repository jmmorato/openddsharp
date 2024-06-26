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
#include "RtpsDiscovery.h"

::OpenDDS::DCPS::Discovery *RtpsDiscovery_NarrowBase(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return dynamic_cast< ::OpenDDS::RTPS::RtpsDiscovery *>(d);
}

::OpenDDS::RTPS::RtpsDiscovery *RtpsDiscovery_new(const char *key) {
  return new ::OpenDDS::RTPS::RtpsDiscovery(key);
}

TimeValueWrapper RtpsDiscovery_GetResendPeriod(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return d->resend_period();
}

void RtpsDiscovery_SetResendPeriod(::OpenDDS::RTPS::RtpsDiscovery *d, TimeValueWrapper value) {
  d->resend_period(value);
}

CORBA::UInt16 RtpsDiscovery_GetPB(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return d->pb();
}

void RtpsDiscovery_SetPB(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value) {
  d->pb(value);
}

CORBA::UInt16 RtpsDiscovery_GetDG(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return d->dg();
}

void RtpsDiscovery_SetDG(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value) {
  d->dg(value);
}

CORBA::UInt16 RtpsDiscovery_GetPG(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return d->pg();
}

void RtpsDiscovery_SetPG(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value) {
  d->pg(value);
}

CORBA::UInt16 RtpsDiscovery_GetD0(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return d->d0();
}

void RtpsDiscovery_SetD0(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value) {
  d->d0(value);
}

CORBA::UInt16 RtpsDiscovery_GetD1(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return d->d1();
}

void RtpsDiscovery_SetD1(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value) {
  d->d1(value);
}

CORBA::UInt16 RtpsDiscovery_GetDX(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return d->dx();
}

void RtpsDiscovery_SetDX(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value) {
  d->dx(value);
}

CORBA::Octet RtpsDiscovery_GetTtl(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return d->ttl();
}

void RtpsDiscovery_SetTtl(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::Octet value) {
  d->ttl(value);
}

char *RtpsDiscovery_GetSedpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d) {
  const char* addr_str = ::OpenDDS::DCPS::LogAddr(d->sedp_local_address()).c_str();
  if (addr_str == NULL) {
    return CORBA::string_dup("");
  }

  return CORBA::string_dup(addr_str);
}

void RtpsDiscovery_SetSedpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d, char *value) {
  const ::OpenDDS::DCPS::NetworkAddress addr = static_cast<const ::OpenDDS::DCPS::NetworkAddress>(value);
  d->sedp_local_address(addr);
}

char *RtpsDiscovery_GetSpdpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d) {
  const char* addr_str = ::OpenDDS::DCPS::LogAddr(d->spdp_local_address()).c_str();
  if (addr_str == NULL) {
    return CORBA::string_dup("");
  }

  return CORBA::string_dup(addr_str);
}

void RtpsDiscovery_SetSpdpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d, char *value) {
  const ::OpenDDS::DCPS::NetworkAddress addr = static_cast<const ::OpenDDS::DCPS::NetworkAddress>(value);
  d->spdp_local_address(addr);
}

CORBA::Boolean RtpsDiscovery_GetSedpMulticast(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return d->sedp_multicast();
}

void RtpsDiscovery_SetSedpMulticast(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::Boolean value) {
  d->sedp_multicast(value);
}

char *RtpsDiscovery_GetMulticastInterface(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return CORBA::string_dup(d->multicast_interface().c_str());
}

void RtpsDiscovery_SetMulticastInterface(::OpenDDS::RTPS::RtpsDiscovery *d, char *value) {
  d->multicast_interface(value);
}

char *RtpsDiscovery_GetDefaultMulticastGroup(::OpenDDS::RTPS::RtpsDiscovery *d, int domain_id) {
  const char* addr_str = ::OpenDDS::DCPS::LogAddr(d->default_multicast_group(domain_id)).c_str();
  if (addr_str == NULL) {
    return CORBA::string_dup("");
  }

  return CORBA::string_dup(addr_str);
}

void RtpsDiscovery_SetDefaultMulticastGroup(::OpenDDS::RTPS::RtpsDiscovery *d, char *value) {
  const  ::OpenDDS::DCPS::NetworkAddress addr = static_cast<const  ::OpenDDS::DCPS::NetworkAddress>(value);
  d->default_multicast_group(addr);
}

void *RtpsDiscovery_GetSpdpSendAddrs(::OpenDDS::RTPS::RtpsDiscovery *d) {
  ::OpenDDS::DCPS::NetworkAddressSet addrs = d->spdp_send_addrs();

  size_t size = addrs.size();
  TAO::unbounded_basic_string_sequence<char> seq(static_cast<CORBA::ULong>(size));

  int i = 0;
  for (auto inst = addrs.begin(); inst != addrs.end(); ++inst) {
    seq[i] = ::OpenDDS::DCPS::LogAddr(inst->to_addr()).c_str();
    i++;
  }

  void *ptr;
  unbounded_basic_string_sequence_to_ptr(seq, ptr);

  return ptr;
}

char *RtpsDiscovery_GetGuidInterface(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return CORBA::string_dup(d->guid_interface().c_str());
}

void RtpsDiscovery_SetGuidInterface(::OpenDDS::RTPS::RtpsDiscovery *d, char *value) {
  d->guid_interface(value);
}