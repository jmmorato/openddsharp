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
#include "TcpInst.h"

::OpenDDS::DCPS::TcpInst *TcpInst_new(::OpenDDS::DCPS::TransportInst *inst) {
  ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst);
  ::OpenDDS::DCPS::TcpInst_rch tcp = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::TcpInst>(rch);
  ::OpenDDS::DCPS::TcpInst *pointer = tcp.in();
  pointer->_add_ref();

  return pointer;
}

CORBA::Boolean TcpInst_GetIsReliable(::OpenDDS::DCPS::TcpInst *ti) {
  return ti->is_reliable();
}

CORBA::Boolean TcpInst_GetEnableNagleAlgorithm(::OpenDDS::DCPS::TcpInst *ti) {
  return ti->enable_nagle_algorithm_;
}

void TcpInst_SetEnableNagleAlgorithm(::OpenDDS::DCPS::TcpInst *ti, CORBA::Boolean value) {
  ti->enable_nagle_algorithm_ = value;
}

CORBA::Long TcpInst_GetConnRetryInitialDelay(::OpenDDS::DCPS::TcpInst *ti) {
  return ti->conn_retry_initial_delay_;
}

void TcpInst_SetConnRetryInitialDelay(::OpenDDS::DCPS::TcpInst *ti, CORBA::Long value) {
  ti->conn_retry_initial_delay_ = value;
}

CORBA::Double TcpInst_GetConnRetryBackoffMultiplier(::OpenDDS::DCPS::TcpInst *ti) {
  return ti->conn_retry_backoff_multiplier_;
}

void TcpInst_SetConnRetryBackoffMultiplier(::OpenDDS::DCPS::TcpInst *ti, CORBA::Double value) {
  ti->conn_retry_backoff_multiplier_ = value;
}

CORBA::Long TcpInst_GetConnRetryAttempts(::OpenDDS::DCPS::TcpInst *ti) {
  return ti->conn_retry_attempts_;
}

void TcpInst_SetConnRetryAttempts(::OpenDDS::DCPS::TcpInst *ti, CORBA::Long value) {
  ti->conn_retry_attempts_ = value;
}

CORBA::Long TcpInst_GetMaxOutputPausePeriod(::OpenDDS::DCPS::TcpInst *ti) {
  return ti->max_output_pause_period_;
}

void TcpInst_SetMaxOutputPausePeriod(::OpenDDS::DCPS::TcpInst *ti, CORBA::Long value) {
  ti->max_output_pause_period_ = value;
}

CORBA::Long TcpInst_GetPassiveReconnectDuration(::OpenDDS::DCPS::TcpInst *ti) {
  return ti->passive_reconnect_duration_;
}

void TcpInst_SetPassiveReconnectDuration(::OpenDDS::DCPS::TcpInst *ti, CORBA::Long value) {
  ti->passive_reconnect_duration_ = value;
}

char *TcpInst_GetPublicAddress(::OpenDDS::DCPS::TcpInst *ti) {
  return CORBA::string_dup(ti->pub_address_str().c_str());
}

void TcpInst_SetPublicAddress(::OpenDDS::DCPS::TcpInst *ti, char *value) {
  ti->pub_address_str_ = value;
}

char *TcpInst_GetLocalAddress(::OpenDDS::DCPS::TcpInst *ti) {
  return CORBA::string_dup(ti->local_address().c_str());
}

void TcpInst_SetLocalAddress(::OpenDDS::DCPS::TcpInst *ti, char *value) {
  ti->local_address(value);
}