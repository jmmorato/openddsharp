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
#include "latency_test.h"

void LatencyTest::initialize() {
  this->dpf = TheServiceParticipant->get_domain_participant_factory();

  this->participant = this->dpf->create_participant(0, PARTICIPANT_QOS_DEFAULT, 0, OpenDDS::DCPS::DEFAULT_STATUS_MASK);
  if (!this->participant) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_participant failed.\n")));
    throw std::runtime_error("create_participant failed.");
  }

  this->publisher = this->participant->create_publisher(PUBLISHER_QOS_DEFAULT, 0, OpenDDS::DCPS::DEFAULT_STATUS_MASK);
  if (!this->publisher) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_publisher failed.\n")));
    throw std::runtime_error("create_publisher failed.");
  }

  this->subscriber = this->participant->create_subscriber(SUBSCRIBER_QOS_DEFAULT, 0, OpenDDS::DCPS::DEFAULT_STATUS_MASK);
  if (!this->subscriber) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_subscriber failed.\n")));
    throw std::runtime_error("create_subscriber failed.");
  }

  this->topic = this->participant->create_topic("ThroughputTest", "Throughput", TOPIC_QOS_DEFAULT, 0, OpenDDS::DCPS::DEFAULT_STATUS_MASK);
  if (!this->topic) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_topic failed.\n")));
    throw std::runtime_error("create_topic failed.");
  }

  this->writer = this->publisher->create_datawriter(this->topic, DATAWRITER_QOS_DEFAULT, 0, OpenDDS::DCPS::DEFAULT_STATUS_MASK);
  if (!this->writer) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_datawriter failed.\n")));
    throw std::runtime_error("create_datawriter failed.");
  }

  this->reader = this->subscriber->create_datareader(this->topic, DATAREADER_QOS_DEFAULT, 0, OpenDDS::DCPS::DEFAULT_STATUS_MASK);
  if (!this->reader) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_datareader failed.\n")));
    throw std::runtime_error("create_datareader failed.");
  }
}

int32_t LatencyTest::run() {
  return 0;
}

void LatencyTest::finalize() {
  this->publisher->delete_datawriter(this->writer);

  this->reader->delete_contained_entities();
  this->subscriber->delete_datareader(this->reader);

  this->participant->delete_publisher(this->publisher);
  this->participant->delete_subscriber(this->subscriber);

  this->participant->delete_topic(this->topic);

  this->participant->delete_contained_entities();
  this->dpf->delete_participant(this->participant);
}


