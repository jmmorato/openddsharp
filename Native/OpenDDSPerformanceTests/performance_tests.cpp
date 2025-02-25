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
#include "performance_tests.h"

#include <dds/DCPS/transport/framework/TransportRegistry.h>

LatencyTest* latency_initialize() {
  auto* test = new LatencyTest();

  test->initialize();

  return test;
}

CORBA::ULong latency_run(LatencyTest* test) {
  return test->run();
}

void latency_finalize(LatencyTest* test) {
  test->finalize();
}

DDS::DomainParticipant* throughput_global_setup(const char* config_name) {
  DDS::DomainParticipant* participant = TheParticipantFactory->create_participant(DOMAIN_ID,
    PARTICIPANT_QOS_DEFAULT, DDS::DomainParticipantListener::_nil(), OpenDDS::DCPS::DEFAULT_STATUS_MASK);

  if (is_nil(participant)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_participant failed.\n")));
  }
  TheTransportRegistry->bind_config(config_name, participant);

  return participant;
}

void throughput_global_cleanup(DDS::DomainParticipant* participant) {
  participant->delete_contained_entities();
  TheParticipantFactory->delete_participant(participant);
}

ThroughputTest* throughput_initialize(const CORBA::Long total_samples, const CORBA::ULongLong payload_size, DDS::DomainParticipant_ptr participant) {
  auto * test = new ThroughputTest();

  test->initialize(total_samples, payload_size, participant);

  return test;
}

CORBA::ULong throughput_run(ThroughputTest* test) {
  return test->run();
}

void throughput_finalize(ThroughputTest* test) {
  test->finalize();
}