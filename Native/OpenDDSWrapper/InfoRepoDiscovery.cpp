/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "InfoRepoDiscovery.h"

::OpenDDS::DCPS::Discovery *InfoRepoDiscovery_NarrowBase(::OpenDDS::DCPS::InfoRepoDiscovery *d) {
  return dynamic_cast< ::OpenDDS::DCPS::InfoRepoDiscovery *>(d);
}

::OpenDDS::DCPS::InfoRepoDiscovery *InfoRepoDiscovery_new(const char *key, const char *ior) {
  ::OpenDDS::DCPS::InfoRepoDiscovery* infoRepo = new ::OpenDDS::DCPS::InfoRepoDiscovery(key);
  TheServiceParticipant->set_repo_ior(ior, key, false);
  return infoRepo;
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