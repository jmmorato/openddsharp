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
#include "utils.h"

std::string random_string(const std::size_t length) {
  const std::string CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

  std::random_device random_device;
  std::mt19937 generator(random_device());
  std::uniform_int_distribution<> distribution(0, static_cast<int>(CHARACTERS.size()) - 1);

  std::string random_string;

  for (std::size_t i = 0; i < length; ++i)
  {
    random_string += CHARACTERS[distribution(generator)];
  }

  return random_string;
}

std::vector<unsigned char> random_bytes(const std::size_t length) {
  const std::vector<unsigned short> NUMBERS = {1, 2, 3, 4, 5, 6, 7, 8, 9, 0};
  std::random_device random_device;
  std::mt19937 generator(random_device());
  std::uniform_int_distribution<> distribution(0, static_cast<int>(NUMBERS.size()) - 1);

  std::vector<unsigned char> random_bytes;
  for (std::size_t i = 0; i < length; ++i)
  {
    random_bytes.push_back(NUMBERS[distribution(generator)] & 0xFF);
  }

  return random_bytes;
}

bool wait_for_publications(::DDS::DataReader_ptr reader, int publications_count, int milliseconds) {
  DDS::InstanceHandleSeq handles;
  reader->get_matched_publications(handles);

  int count = milliseconds / 100;
  while (handles.length() != publications_count && count > 0) {
    std::this_thread::sleep_for(std::chrono::milliseconds(100));

    handles.length(0);
    reader->get_matched_publications(handles);

    count--;
  }

  return count != 0 || handles.length() == publications_count;
}

bool wait_for_subscriptions(DDS::DataWriter_ptr writer, int subscriptions_count, int milliseconds) {
  DDS::InstanceHandleSeq handles;
  writer->get_matched_subscriptions(handles);

  int count = milliseconds / 100;
  while (handles.length() != subscriptions_count && count > 0) {
    std::this_thread::sleep_for(::std::chrono::milliseconds(100));

    handles.length(0);
    writer->get_matched_subscriptions(handles);

    count--;
  }

  return count != 0 || handles.length() == subscriptions_count;
}

DDS::Publisher_ptr create_publisher(DDS::DomainParticipant_ptr participant) {
  DDS::PublisherQos publisher_qos;
  participant->get_default_publisher_qos(publisher_qos);
  publisher_qos.entity_factory.autoenable_created_entities = false;

  DDS::Publisher_ptr publisher = participant->create_publisher(publisher_qos, DDS::PublisherListener::_nil(), OpenDDS::DCPS::NO_STATUS_MASK);

  if (is_nil(publisher)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_publisher failed.\n")));
    throw std::runtime_error("create_publisher failed.");
  }

  return publisher;
}

void* serialize_latencies(const std::vector<double>& vec) {
  ACE_UINT32 length = vec.size();
  const size_t struct_size = sizeof(double);
  const size_t buffer_size = (length * struct_size) + sizeof length;
  char *bytes = new char[buffer_size];
  ACE_OS::memcpy(bytes, &length, sizeof length);

  for (ACE_UINT32 i = 0; i < length; i++) {
    ACE_OS::memcpy(&bytes[(i * struct_size) + sizeof length], &vec[i], struct_size);
  }

  // Alloc memory for the pointer
  void* ptr = ACE_OS::malloc(buffer_size);

  // Copy the bytes in the pointer
  ACE_OS::memcpy(ptr, bytes, buffer_size);

  // Free temporally allocated memory
  delete[] bytes;

  return ptr;
}

DDS::Subscriber_ptr create_subscriber(DDS::DomainParticipant_ptr participant) {
  DDS::SubscriberQos subscriber_qos;
  participant->get_default_subscriber_qos(subscriber_qos);
  subscriber_qos.entity_factory.autoenable_created_entities = false;

  DDS::Subscriber_ptr subscriber = participant->create_subscriber(subscriber_qos, DDS::SubscriberListener::_nil(), OpenDDS::DCPS::NO_STATUS_MASK);

  if (is_nil(subscriber)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_subscriber failed.\n")));
    throw std::runtime_error("create_subscriber failed.");
  }

  return subscriber;
}

DDS::Topic_ptr create_topic(DDS::DomainParticipant_ptr participant) {
  const OpenDDSNative::KeyedOctetsTypeSupport_var ts = new OpenDDSNative::KeyedOctetsTypeSupportImpl;
  const auto type_name = ts->get_type_name();
  auto ret = ts->register_type(participant, type_name);
  if (ret != ::DDS::RETCODE_OK) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) register_type failed.\n")));
    throw std::runtime_error("register_type failed.");
  }

  const auto topic_name = random_string(16);

  ::DDS::TopicQos topic_qos;
  participant->get_default_topic_qos(topic_qos);
  topic_qos.reliability.kind = DDS::RELIABLE_RELIABILITY_QOS;
  topic_qos.history.kind = DDS::KEEP_ALL_HISTORY_QOS;

  DDS::Topic_ptr topic = participant->create_topic(topic_name.c_str(), type_name, topic_qos, DDS::TopicListener::_nil(), OpenDDS::DCPS::NO_STATUS_MASK);

  if (is_nil(topic)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_topic failed.\n")));
    throw std::runtime_error("create_topic failed.");
  }

  return topic;
}

DDS::DataWriter_ptr create_data_writer(DDS::Publisher_ptr publisher, DDS::Topic_ptr topic) {
  DDS::DataWriterQos dw_qos;
  publisher->get_default_datawriter_qos(dw_qos);
  dw_qos.reliability.kind = DDS::RELIABLE_RELIABILITY_QOS;
  dw_qos.reliability.max_blocking_time = {DDS::DURATION_INFINITE_SEC, DDS::DURATION_INFINITE_NSEC};
  dw_qos.history.kind = DDS::KEEP_ALL_HISTORY_QOS;

  DDS::DataWriter_ptr writer = publisher->create_datawriter(topic, dw_qos, DDS::DataWriterListener::_nil(), OpenDDS::DCPS::NO_STATUS_MASK);

  if (is_nil(writer)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_datawriter failed.\n")));
    throw std::runtime_error("create_datawriter failed.");
  }

  return DDS::DataWriter::_duplicate(writer);
}

DDS::DataReader_ptr create_data_reader(DDS::Subscriber_ptr subscriber, DDS::Topic_ptr topic) {
  DDS::DataReaderQos dr_qos;
  subscriber->get_default_datareader_qos(dr_qos);
  dr_qos.reliability.kind = DDS::RELIABLE_RELIABILITY_QOS;
  dr_qos.reliability.max_blocking_time = {DDS::DURATION_INFINITE_SEC, DDS::DURATION_INFINITE_NSEC};
  dr_qos.history.kind = DDS::KEEP_ALL_HISTORY_QOS;

  DDS::DataReader_ptr reader = subscriber->create_datareader(topic, dr_qos, DDS::DataReaderListener::_nil(), OpenDDS::DCPS::DEFAULT_STATUS_MASK);

  if (is_nil(reader)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_datareader failed.\n")));
    throw std::runtime_error("create_datareader failed.");
  }

  return DDS::DataReader::_duplicate(reader);
}