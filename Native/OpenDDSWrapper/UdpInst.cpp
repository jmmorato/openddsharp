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
#include "UdpInst.h"

::OpenDDS::DCPS::UdpInst *UdpInst_new(::OpenDDS::DCPS::TransportInst *inst) {
  ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst);
  ::OpenDDS::DCPS::UdpInst_rch udp = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::UdpInst>(rch);
  ::OpenDDS::DCPS::UdpInst *pointer = udp.in();
  pointer->_add_ref();

  return pointer;
}

CORBA::Boolean UdpInst_GetIsReliable(::OpenDDS::DCPS::UdpInst *ui) {
  return ui->is_reliable();
}

CORBA::Int32 UdpInst_GetSendBufferSize(::OpenDDS::DCPS::UdpInst *ui) {
  return ui->send_buffer_size_.get();
}

void UdpInst_SetSendBufferSize(::OpenDDS::DCPS::UdpInst *ui, CORBA::Int32 value) {
  ui->send_buffer_size_ = value;
}

CORBA::Int32 UdpInst_GetRcvBufferSize(::OpenDDS::DCPS::UdpInst *ui) {
  return ui->rcv_buffer_size_.get();
}

void UdpInst_SetRcvBufferSize(::OpenDDS::DCPS::UdpInst *ui, CORBA::Int32 value) {
  ui->rcv_buffer_size_ = value;
}

char *UdpInst_GetLocalAddress(::OpenDDS::DCPS::UdpInst *ui) {
  const char* addr_str = CORBA::string_dup(ui->local_address().c_str());

  return CORBA::string_dup(addr_str);
}

void UdpInst_SetLocalAddress(::OpenDDS::DCPS::UdpInst *ui, char *value) {
  const ACE_INET_Addr addr = static_cast<const ACE_INET_Addr>(value);
  ui->local_address(addr);
}
