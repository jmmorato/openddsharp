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

void ThroughputTest::initialize(const CORBA::ULong total_samples, const CORBA::ULong payload_size, DDS::DomainParticipant_ptr participant) {
  // Initialize the test parameters
  this->total_samples_ = total_samples;
  this->payload_size_ = payload_size;
  this->participant_ = participant;

  this->sample_.KeyField = "1";
  this->sample_.ValueField.length(payload_size);

  const auto data = random_bytes(payload_size);
  for (CORBA::ULong i = 0; i < payload_size; ++i) {
    this->sample_.ValueField[i] = data[i];
  }

  // Initialize the Publisher entity
  this->publisher_ = create_publisher(this->participant_);

  // Initialize the Subscriber entity
  this->subscriber_ = create_subscriber(this->participant_);

  // Initialize the Topic entity
  this->topic_ = create_topic(this->participant_);

  // Initialize the DataWriter entity
  this->writer_ = create_data_writer(this->publisher_, this->topic_);
  this->data_writer_ = OpenDDSNative::KeyedOctetsDataWriter::_narrow(writer_);
  this->reader_ = create_data_reader(this->subscriber_, this->topic_);
  this->data_reader_ = OpenDDSNative::KeyedOctetsDataReader::_narrow(reader_);

  // Initialize waitset and status condition
  // Initialize waitset and status condition
  this->status_condition_ = this->data_reader_->get_statuscondition();
  this->status_condition_->set_enabled_statuses(DDS::DATA_AVAILABLE_STATUS);
  this->wait_set_ = new DDS::WaitSet;
  const auto result = this->wait_set_->attach_condition(this->status_condition_);

  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("attach_condition failed.");
  }

  // Enable the entities and wait for discovery
  auto ret = writer_->enable();
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
    this->samples_received_ = 0;

    while (true) {
      DDS::ConditionSeq active_conditions;
      DDS::Duration_t duration = { DDS::DURATION_INFINITE_SEC, DDS::DURATION_INFINITE_NSEC };
      auto ret = this->wait_set_->wait(active_conditions, duration);
      if (ret != DDS::RETCODE_OK) {
        continue;
      }

      OpenDDSNative::KeyedOctetsSeq samples;
      DDS::SampleInfoSeq infos;

      ret = this->data_reader_->take(samples, infos, DDS::LENGTH_UNLIMITED,
        DDS::ANY_SAMPLE_STATE, DDS::ANY_VIEW_STATE, DDS::ANY_INSTANCE_STATE);

      if (ret != DDS::RETCODE_OK) {
        continue;
      }

      this->samples_received_ += samples.length();
      this->data_reader_->return_loan(samples, infos);

      if (this->samples_received_ == this->total_samples_) {
        return;
      }
    }
  });

  writer_thread.join();
  reader_thread.join();

  return this->samples_received_;
}

void ThroughputTest::finalize() const {
  DDS::ReturnCode_t result = this->publisher_->delete_datawriter(this->writer_);
  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("delete_datawriter failed.");
  }

  result = this->publisher_->delete_contained_entities();
  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("delete_contained_entities failed.");
  }

  result = this->participant_->delete_publisher(this->publisher_);
  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("delete_publisher failed.");
  }

  result = this->status_condition_->set_enabled_statuses(OpenDDS::DCPS::NO_STATUS_MASK);
  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("set_enabled_statuses failed.");
  }

  result = this->wait_set_->detach_condition(this->status_condition_);
  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("detach_condition failed.");
  }

  result = this->reader_->delete_contained_entities();
  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("delete_contained_entities failed.");
  }

  result = this->subscriber_->delete_datareader(this->reader_);
  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("delete_datareader failed.");
  }

  result = this->subscriber_->delete_contained_entities();
  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("delete_contained_entities failed.");
  }

  result = this->participant_->delete_subscriber(this->subscriber_);
  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("delete_subscriber failed.");
  }

  result = this->participant_->delete_topic(this->topic_);
  if (result != DDS::RETCODE_OK) {
    throw std::runtime_error("delete_topic failed.");
  }
}
