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

#include "throughput_test.h"

using random_bytes_engine = std::independent_bits_engine<std::default_random_engine, CHAR_BIT, unsigned char>;

void ThroughputTest::initialize(const CORBA::ULong total_samples, const CORBA::ULong payload_size, DDS::DomainParticipant_ptr participant) {
  this->total_samples_ = total_samples;
  this->payload_size_ = payload_size;
  this->participant_ = participant;

  this->sample_.KeyField = "1";
  this->sample_.ValueField.length(payload_size);

  random_bytes_engine rbe(std::random_device{}());
  std::vector<unsigned char> data(payload_size);
  std::generate(begin(data), end(data), std::ref(rbe));

  for (CORBA::ULong i = 0; i < payload_size; ++i) {
    this->sample_.ValueField[i] = data[i];
  }

  // Initialize the Publisher entity
  DDS::PublisherQos publisher_qos;
  this->participant_->get_default_publisher_qos(publisher_qos);
  publisher_qos.entity_factory.autoenable_created_entities = false;

  this->publisher_ = this->participant_->create_publisher(publisher_qos, DDS::PublisherListener::_nil(), OpenDDS::DCPS::NO_STATUS_MASK);

  if (is_nil(this->publisher_)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_publisher failed.\n")));
    throw std::runtime_error("create_publisher failed.");
  }

  // Initialize the Subscriber entity
  DDS::SubscriberQos subscriber_qos;
  this->participant_->get_default_subscriber_qos(subscriber_qos);
  subscriber_qos.entity_factory.autoenable_created_entities = false;

  this->subscriber_ = this->participant_->create_subscriber(subscriber_qos, DDS::SubscriberListener::_nil(), OpenDDS::DCPS::NO_STATUS_MASK);

  if (is_nil(this->subscriber_)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_subscriber failed.\n")));
    throw std::runtime_error("create_subscriber failed.");
  }

  // Initialize the Topic entity
  const OpenDDSNative::KeyedOctetsTypeSupport_var ts = new OpenDDSNative::KeyedOctetsTypeSupportImpl;
  const auto type_name = ts->get_type_name();
  auto ret = ts->register_type(participant_, type_name);
  if (ret != ::DDS::RETCODE_OK) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) register_type failed.\n")));
    throw std::runtime_error("register_type failed.");
  }

  const auto topic_name = random_string(16);

  ::DDS::TopicQos topic_qos;
  this->participant_->get_default_topic_qos(topic_qos);
  topic_qos.reliability.kind = DDS::RELIABLE_RELIABILITY_QOS;
  topic_qos.history.kind = DDS::KEEP_ALL_HISTORY_QOS;

  this->topic_ = this->participant_->create_topic(topic_name.c_str(), type_name, topic_qos, DDS::TopicListener::_nil(), OpenDDS::DCPS::NO_STATUS_MASK);

  if (is_nil(this->topic_)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_topic failed.\n")));
    throw std::runtime_error("create_topic failed.");
  }

  // Initialize the DataWriter entity
  DDS::DataWriterQos dw_qos;
  this->publisher_->get_default_datawriter_qos(dw_qos);
  dw_qos.reliability.kind = DDS::RELIABLE_RELIABILITY_QOS;
  dw_qos.reliability.max_blocking_time = { DDS::DURATION_INFINITE_SEC, DDS::DURATION_INFINITE_NSEC };
  dw_qos.history.kind = DDS::KEEP_ALL_HISTORY_QOS;

  this->writer_ = this->publisher_->create_datawriter(this->topic_, dw_qos, DDS::DataWriterListener::_nil(), OpenDDS::DCPS::NO_STATUS_MASK);

  if (is_nil(this->writer_)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_datawriter failed.\n")));
    throw std::runtime_error("create_datawriter failed.");
  }

  this->data_writer_ = OpenDDSNative::KeyedOctetsDataWriter::_narrow(writer_);

  // Initialize the DataReader entity
  DDS::DataReaderQos dr_qos;
  this->subscriber_->get_default_datareader_qos(dr_qos);
  dr_qos.reliability.kind = DDS::RELIABLE_RELIABILITY_QOS;
  dr_qos.reliability.max_blocking_time = { DDS::DURATION_INFINITE_SEC, DDS::DURATION_INFINITE_NSEC };
  dr_qos.history.kind = DDS::KEEP_ALL_HISTORY_QOS;

  this->reader_ = this->subscriber_->create_datareader(this->topic_, dr_qos, DDS::DataReaderListener::_nil(), OpenDDS::DCPS::DEFAULT_STATUS_MASK);

  if (is_nil(this->reader_)) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) create_datareader failed.\n")));
    throw std::runtime_error("create_datareader failed.");
  }

  this->data_reader_ = OpenDDSNative::KeyedOctetsDataReader::_narrow(reader_);

  // Initialize waitset and status condition
  const auto status_condition = this->reader_->get_statuscondition();
  status_condition->set_enabled_statuses(DDS::DATA_AVAILABLE_STATUS);
  this->wait_set_ = new DDS::WaitSet;
  this->wait_set_->attach_condition(status_condition);

  // Enable the entities and wait for discovery
  ret = writer_->enable();
  if (ret != ::DDS::RETCODE_OK) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) writer enable failed.\n")));
    throw std::runtime_error("writer enable failed.");
  }

  ret = reader_->enable();
  if (ret != ::DDS::RETCODE_OK) {
    ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) reader enable failed.\n")));
    throw std::runtime_error("reader enable failed.");
  }

  wait_for_publications(reader_, 1, 5000);
  wait_for_subscriptions(writer_, 1, 5000);
}

CORBA::ULong ThroughputTest::run() {
  this->samples_received_ = 0;

  // std::thread writer_thread(&ThroughputTest::write_thread, this);
  std::thread writer_thread([this] {
    for (int i = 1; i <= this->total_samples_; i++) {
      this->data_writer_->write(this->sample_, DDS::HANDLE_NIL);
    }
  });

  std::thread reader_thread([this] {
    CORBA::ULong samples_received = 0;
    while (true) {
      DDS::ConditionSeq active_conditions;
      DDS::Duration_t duration = { DDS::DURATION_INFINITE_SEC, DDS::DURATION_INFINITE_NSEC };
      auto ret = this->wait_set_->wait(active_conditions, duration);
      if (ret != DDS::RETCODE_OK) {
        ACE_ERROR((LM_ERROR, ACE_TEXT("(%P|%t) wait failed.\n")));
        continue;
      }

      OpenDDSNative::KeyedOctetsSeq samples;
      DDS::SampleInfoSeq infos;
      ret = this->data_reader_->take(samples, infos, DDS::LENGTH_UNLIMITED, DDS::ANY_SAMPLE_STATE, DDS::ANY_VIEW_STATE, DDS::ANY_INSTANCE_STATE);
      if (ret != DDS::RETCODE_OK) {
        ACE_ERROR((LM_ERROR, ACE_TEXT("take failed.\n")));
        continue;
      }

      samples_received += samples.length();

      this->data_reader_->return_loan(samples, infos);

      // ACE_DEBUG((LM_DEBUG, ACE_TEXT("Received %d samples.\n"), samples_received));
      if (samples_received == this->total_samples_) {
        return;
      }
    }
  });

  writer_thread.join();
  reader_thread.join();

  return this->total_samples_;
}

void ThroughputTest::finalize() const {
  this->publisher_->delete_datawriter(this->writer_);
  this->publisher_->delete_contained_entities();
  this->participant_->delete_publisher(this->publisher_);

  this->wait_set_->detach_condition(this->reader_->get_statuscondition());

  this->reader_->delete_contained_entities();
  this->subscriber_->delete_datareader(this->reader_);

  this->subscriber_->delete_contained_entities();
  this->participant_->delete_subscriber(this->subscriber_);

  this->participant_->delete_topic(this->topic_);
}
