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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "RtpsUdpInst.h"

::OpenDDS::DCPS::RtpsUdpInst *RtpsUdpInst_new(::OpenDDS::DCPS::TransportInst *inst) {
  ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst);
  ::OpenDDS::DCPS::RtpsUdpInst_rch rtps = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::RtpsUdpInst>(rch);
  ::OpenDDS::DCPS::RtpsUdpInst *pointer = rtps.in();
  pointer->_add_ref();

  return pointer;
}

CORBA::Boolean RtpsUdpInst_GetIsReliable(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return ri->is_reliable();
}

CORBA::Boolean RtpsUdpInst_GetRequiresCdr(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return ri->requires_cdr_encapsulation();
}

CORBA::Int32 RtpsUdpInst_GetSendBufferSize(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return ri->send_buffer_size_.get();
}

void RtpsUdpInst_SetSendBufferSize(::OpenDDS::DCPS::RtpsUdpInst *ri, CORBA::Int32 value) {
  ri->send_buffer_size(value);
}

CORBA::Int32 RtpsUdpInst_GetRcvBufferSize(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return ri->rcv_buffer_size();
}

void RtpsUdpInst_SetRcvBufferSize(::OpenDDS::DCPS::RtpsUdpInst *ri, CORBA::Int32 value) {
  ri->rcv_buffer_size(value);
}

size_t RtpsUdpInst_GetNakDepth(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return ri->nak_depth();
}

void RtpsUdpInst_SetNakDepth(::OpenDDS::DCPS::RtpsUdpInst *ri, size_t value) {
  ri->nak_depth(value);
}

CORBA::Boolean RtpsUdpInst_GetUseMulticast(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return ri->use_multicast();
}

void RtpsUdpInst_SetUseMulticast(::OpenDDS::DCPS::RtpsUdpInst *ri, CORBA::Boolean value) {
  ri->use_multicast(value);
}

CORBA::Octet RtpsUdpInst_GetTtl(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return ri->ttl();
}

void RtpsUdpInst_SetTtl(::OpenDDS::DCPS::RtpsUdpInst *ri, CORBA::Octet value) {
  ri->ttl(value);
}

char *RtpsUdpInst_GetMulticastGroupAddress(::OpenDDS::DCPS::RtpsUdpInst *ri, int domain_id) {
  const std::string addr_str = ::OpenDDS::DCPS::LogAddr(ri->multicast_group_address(domain_id)).str();
  if (addr_str.empty()) {
    return CORBA::string_dup("");
  }

  return CORBA::string_dup(addr_str.c_str());
}

void RtpsUdpInst_SetMulticastGroupAddress(::OpenDDS::DCPS::RtpsUdpInst *ri, char *value) {
  const OpenDDS::DCPS::NetworkAddress addr(value);
  ri->multicast_group_address(addr);
}

char *RtpsUdpInst_GetMulticastInterface(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return CORBA::string_dup(ri->multicast_interface().c_str());
}

void RtpsUdpInst_SetMulticastInterface(::OpenDDS::DCPS::RtpsUdpInst *ri, char *value) {
  ri->multicast_interface(value);
}

char *RtpsUdpInst_GetLocalAddress(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  const std::string addr_str = ::OpenDDS::DCPS::LogAddr(ri->local_address()).str();
  if (addr_str.empty()) {
    return CORBA::string_dup("");
  }
  return CORBA::string_dup(addr_str.c_str());
}

void RtpsUdpInst_SetLocalAddress(::OpenDDS::DCPS::RtpsUdpInst *ri, char *value) {
  const OpenDDS::DCPS::NetworkAddress addr(value);
  ri->local_address(addr);
}

TimeValueWrapper RtpsUdpInst_GetNakResponseDelay(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return ri->nak_response_delay();
}

void RtpsUdpInst_SetNakResponseDelay(::OpenDDS::DCPS::RtpsUdpInst *ri, TimeValueWrapper value) {
  ri->nak_response_delay(value);
}

TimeValueWrapper RtpsUdpInst_GetHeartbeatPeriod(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return ri->heartbeat_period();
}

void RtpsUdpInst_SetHeartbeatPeriod(::OpenDDS::DCPS::RtpsUdpInst *ri, TimeValueWrapper value) {
  ri->heartbeat_period(value);
}

TimeValueWrapper RtpsUdpInst_GetReceiveAddressDuration(::OpenDDS::DCPS::RtpsUdpInst *ri) {
  return ri->receive_address_duration();
}

void RtpsUdpInst_SetReceiveAddressDuration(::OpenDDS::DCPS::RtpsUdpInst *ri, TimeValueWrapper value) {
  ri->receive_address_duration(value);
}