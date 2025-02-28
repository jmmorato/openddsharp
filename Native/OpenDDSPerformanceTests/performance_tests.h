/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2025 Jose Morato

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
#pragma once

#include "latency_test.h"
#include "throughput_test.h"

EXTERN_METHOD_EXPORT
LatencyTest* latency_initialize(const CORBA::ULong total_instances, const CORBA::ULong total_samples,
  const CORBA::ULong payload_size, const DDS::DomainParticipant_ptr participant);

EXTERN_METHOD_EXPORT
void latency_run(LatencyTest* test);

EXTERN_METHOD_EXPORT
void latency_finalize(const LatencyTest* test);

EXTERN_METHOD_EXPORT
void* latency_get_latencies(const LatencyTest* test);

EXTERN_METHOD_EXPORT
DDS::DomainParticipant* global_setup(const char * config_name);

EXTERN_METHOD_EXPORT
void global_cleanup(DDS::DomainParticipant* participant);

EXTERN_METHOD_EXPORT
ThroughputTest* throughput_initialize(CORBA::Long total_samples, CORBA::ULongLong payload_size, DDS::DomainParticipant* participant);

EXTERN_METHOD_EXPORT
CORBA::ULong throughput_run(ThroughputTest* test);

EXTERN_METHOD_EXPORT
void throughput_finalize(ThroughputTest* test);

