/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
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
  const std::string addr_str = ::OpenDDS::DCPS::LogAddr(d->sedp_local_address()).str();
  if (addr_str.empty()) {
    return CORBA::string_dup("");
  }

  return CORBA::string_dup(addr_str.c_str());
}

void RtpsDiscovery_SetSedpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d, char *value) {
  const ::OpenDDS::DCPS::NetworkAddress addr(value);
  d->sedp_local_address(addr);
}

char *RtpsDiscovery_GetSpdpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d) {
  const std::string addr_str = ::OpenDDS::DCPS::LogAddr(d->spdp_local_address()).str();
  if (addr_str.empty()) {
    return CORBA::string_dup("");
  }

  return CORBA::string_dup(addr_str.c_str());
}

void RtpsDiscovery_SetSpdpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d, char *value) {
  const ::OpenDDS::DCPS::NetworkAddress addr(value);
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
  const std::string addr_str = ::OpenDDS::DCPS::LogAddr(d->default_multicast_group(domain_id)).str();
  if (addr_str.empty()) {
    return CORBA::string_dup("");
  }

  return CORBA::string_dup(addr_str.c_str());
}

void RtpsDiscovery_SetDefaultMulticastGroup(::OpenDDS::RTPS::RtpsDiscovery *d, char *value) {
  const ::OpenDDS::DCPS::NetworkAddress addr(value);
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

void RtpsDiscovery_SetSpdpSendAddrs(::OpenDDS::RTPS::RtpsDiscovery *d, char *value) {
  ::OpenDDS::DCPS::NetworkAddressSet addrs;

  TAO::unbounded_basic_string_sequence<char> seq;

  // Split by comma the value string into a sequence of strings
  std::string value_str(value);
  std::istringstream ss(value_str);
  std::string token;
  while (std::getline(ss, token, ',')) {
    seq.length(seq.length() + 1);
    seq[seq.length() - 1] = CORBA::string_dup(token.c_str());
  }

  for (CORBA::ULong i = 0; i < seq.length(); ++i) {
    const ::OpenDDS::DCPS::NetworkAddress addr(seq[i].in());
    addrs.insert(addr);
  }

  d->spdp_send_addrs(addrs);
}

char *RtpsDiscovery_GetGuidInterface(::OpenDDS::RTPS::RtpsDiscovery *d) {
  return CORBA::string_dup(d->guid_interface().c_str());
}

void RtpsDiscovery_SetGuidInterface(::OpenDDS::RTPS::RtpsDiscovery *d, char *value) {
  d->guid_interface(value);
}

