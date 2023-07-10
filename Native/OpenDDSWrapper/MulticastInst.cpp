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
#include "MulticastInst.h"

::OpenDDS::DCPS::MulticastInst *MulticastInst_new(::OpenDDS::DCPS::TransportInst *inst) {
  ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst);
  ::OpenDDS::DCPS::MulticastInst_rch udp = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::MulticastInst>(rch);
  ::OpenDDS::DCPS::MulticastInst *pointer = udp.in();
  pointer->_add_ref();

  return pointer;
}

CORBA::Boolean MulticastInst_GetIsReliable(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->is_reliable();
}

CORBA::Boolean MulticastInst_GetReliable(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->reliable_;
}

void MulticastInst_SetReliable(::OpenDDS::DCPS::MulticastInst *mi, CORBA::Boolean value) {
  mi->reliable_ = value;
}

CORBA::Boolean MulticastInst_GetDefaultToIpv6(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->default_to_ipv6_;
}

void MulticastInst_SetDefaultToIpv6(::OpenDDS::DCPS::MulticastInst *mi, CORBA::Boolean value) {
  mi->default_to_ipv6_ = value;
}

CORBA::UShort MulticastInst_GetPortOffset(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->port_offset_;
}

void MulticastInst_SetPortOffset(::OpenDDS::DCPS::MulticastInst *mi, CORBA::UShort value) {
  mi->port_offset_ = value;
}

char *MulticastInst_GetGroupAddress(::OpenDDS::DCPS::MulticastInst *mi) {
  char *buffer = new char[512];
  mi->group_address_.addr_to_string(buffer, 512);

  return CORBA::string_dup(buffer);
}

void MulticastInst_SetGroupAddress(::OpenDDS::DCPS::MulticastInst *mi, char *value) {
  mi->group_address_.set(value);
}

char *MulticastInst_GetLocalAddress(::OpenDDS::DCPS::MulticastInst *mi) {
  return CORBA::string_dup(mi->local_address_.c_str());
}

void MulticastInst_SetLocalAddress(::OpenDDS::DCPS::MulticastInst *mi, char *value) {
  mi->local_address_ = value;
}

CORBA::Double MulticastInst_GetSynBackoff(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->syn_backoff_;
}

void MulticastInst_SetSynBackoff(::OpenDDS::DCPS::MulticastInst *mi, CORBA::Double value) {
  mi->syn_backoff_ = value;
}

TimeValueWrapper MulticastInst_GetSynInterval(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->syn_interval_;
}

void MulticastInst_SetSynInterval(::OpenDDS::DCPS::MulticastInst *mi, TimeValueWrapper value) {
  mi->syn_interval_ = value;
}

TimeValueWrapper MulticastInst_GetSynTimeout(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->syn_timeout_;
}

void MulticastInst_SetSynTimeout(::OpenDDS::DCPS::MulticastInst *mi, TimeValueWrapper value) {
  mi->syn_timeout_ = value;
}

size_t MulticastInst_GetNakDepth(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->nak_depth_;
}

void MulticastInst_SetNakDepth(::OpenDDS::DCPS::MulticastInst *mi, size_t value) {
  mi->nak_depth_ = value;
}

TimeValueWrapper MulticastInst_GetNakInterval(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->nak_interval_;
}

void MulticastInst_SetNakInterval(::OpenDDS::DCPS::MulticastInst *mi, TimeValueWrapper value) {
  mi->nak_interval_ = value;
}

size_t MulticastInst_GetNakDelayIntervals(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->nak_delay_intervals_;
}

void MulticastInst_SetNakDelayIntervals(::OpenDDS::DCPS::MulticastInst *mi, size_t value) {
  mi->nak_delay_intervals_ = value;
}

size_t MulticastInst_GetNakMax(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->nak_max_;
}

void MulticastInst_SetNakMax(::OpenDDS::DCPS::MulticastInst *mi, size_t value) {
  mi->nak_max_ = value;
}

TimeValueWrapper MulticastInst_GetNakTimeout(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->nak_timeout_;
}

void MulticastInst_SetNakTimeout(::OpenDDS::DCPS::MulticastInst *mi, TimeValueWrapper value) {
  mi->nak_timeout_ = value;
}

CORBA::Octet MulticastInst_GetTtl(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->ttl_;
}

void MulticastInst_SetTtl(::OpenDDS::DCPS::MulticastInst *mi, CORBA::Octet value) {
  mi->ttl_ = value;
}

size_t MulticastInst_GetRcvBufferSize(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->rcv_buffer_size_;
}

void MulticastInst_SetRcvBufferSize(::OpenDDS::DCPS::MulticastInst *mi, size_t value) {
  mi->rcv_buffer_size_ = value;
}

CORBA::Boolean MulticastInst_GetAsyncSend(::OpenDDS::DCPS::MulticastInst *mi) {
  return mi->async_send();
}

void MulticastInst_SetAsyncSend(::OpenDDS::DCPS::MulticastInst *mi, CORBA::Boolean value) {
  mi->async_send_ = value;
}
