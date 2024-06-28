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
#include "ShmemInst.h"

::OpenDDS::DCPS::ShmemInst *ShmemInst_new(::OpenDDS::DCPS::TransportInst *inst) {
  ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst);
  ::OpenDDS::DCPS::ShmemInst_rch shmem = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::ShmemInst>(rch);
  ::OpenDDS::DCPS::ShmemInst *pointer = shmem.in();
  pointer->_add_ref();

  return pointer;
}

CORBA::Boolean ShmemInst_GetIsReliable(::OpenDDS::DCPS::ShmemInst *si) {
  return si->is_reliable();
}

size_t ShmemInst_GetPoolSize(::OpenDDS::DCPS::ShmemInst *si) {
  return si->pool_size();
}

void ShmemInst_SetPoolSize(::OpenDDS::DCPS::ShmemInst *si, size_t value) {
  si->pool_size(value);
}

size_t ShmemInst_GetDatalinkControlSize(::OpenDDS::DCPS::ShmemInst *si) {
  return si->datalink_control_size();
}

void ShmemInst_SetDatalinkControlSize(::OpenDDS::DCPS::ShmemInst *si, size_t value) {
  si->datalink_control_size(value);
}

char *ShmemInst_GetHostName(::OpenDDS::DCPS::ShmemInst *si) {
  return CORBA::string_dup(si->hostname().c_str());
}

char *ShmemInst_GetPoolName(::OpenDDS::DCPS::ShmemInst *si) {
  return CORBA::string_dup(si->poolname().c_str());
}