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
#include "TransportInst.h"

char *TransportInst_GetTransportType(::OpenDDS::DCPS::TransportInst *ti) {
  return CORBA::string_dup(ti->transport_type_.c_str());
}

char *TransportInst_GetName(::OpenDDS::DCPS::TransportInst *ti) {
  return CORBA::string_dup(ti->name().c_str());
}

CORBA::ULong TransportInst_GetMaxPacketSize(::OpenDDS::DCPS::TransportInst *ti) {
  return ti->max_packet_size();
}

void TransportInst_SetMaxPacketSize(::OpenDDS::DCPS::TransportInst *ti, CORBA::ULong value) {
  ti->max_packet_size(value);
}

size_t TransportInst_GetMaxSamplesPerPacket(::OpenDDS::DCPS::TransportInst *ti) {
  return ti->max_samples_per_packet();
}

void TransportInst_SetMaxSamplesPerPacket(::OpenDDS::DCPS::TransportInst *ti, size_t value) {
  ti->max_samples_per_packet(value);
}

CORBA::ULong TransportInst_GetOptimumPacketSize(::OpenDDS::DCPS::TransportInst *ti) {
  return ti->optimum_packet_size();
}

void TransportInst_SetOptimumPacketSize(::OpenDDS::DCPS::TransportInst *ti, CORBA::ULong value) {
  ti->optimum_packet_size(value);
}

CORBA::Boolean TransportInst_GetThreadPerConnection(::OpenDDS::DCPS::TransportInst *ti) {
  return ti->thread_per_connection();
}

void TransportInst_SetThreadPerConnection(::OpenDDS::DCPS::TransportInst *ti, CORBA::Boolean value) {
  ti->thread_per_connection(value);
}

long TransportInst_GetDatalinkReleaseDelay(::OpenDDS::DCPS::TransportInst *ti) {
  return ti->datalink_release_delay();
}

void TransportInst_SetDatalinkReleaseDelay(::OpenDDS::DCPS::TransportInst *ti, long value) {
  ti->datalink_release_delay(value);
}

size_t TransportInst_GetDatalinkControlChunks(::OpenDDS::DCPS::TransportInst *ti) {
  return ti->datalink_control_chunks();
}

void TransportInst_SetDatalinkControlChunks(::OpenDDS::DCPS::TransportInst *ti, size_t value) {
  ti->datalink_control_chunks(value);
}