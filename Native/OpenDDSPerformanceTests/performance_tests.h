/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "latency_test.h"
#include "throughput_test.h"

EXTERN_METHOD_EXPORT
LatencyTest* latency_initialize(CORBA::Long total_instances, CORBA::Long total_samples, CORBA::ULongLong payload_size,
  DDS::DomainParticipant_ptr participant);

EXTERN_METHOD_EXPORT
void latency_run(LatencyTest* test);

EXTERN_METHOD_EXPORT
void latency_finalize(LatencyTest* test);

EXTERN_METHOD_EXPORT
void* latency_get_latencies(const LatencyTest* test);

EXTERN_METHOD_EXPORT
DDS::DomainParticipant* global_setup(const char * config_name);

EXTERN_METHOD_EXPORT
void global_cleanup(DDS::DomainParticipant* participant);

EXTERN_METHOD_EXPORT
ThroughputTest* throughput_initialize(CORBA::Long total_samples, CORBA::ULongLong payload_size,
  DDS::DomainParticipant* participant);

EXTERN_METHOD_EXPORT
CORBA::ULong throughput_run(ThroughputTest* test);

EXTERN_METHOD_EXPORT
void throughput_finalize(ThroughputTest* test);

