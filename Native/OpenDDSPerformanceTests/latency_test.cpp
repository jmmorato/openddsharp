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

void LatencyTest::initialize(const CORBA::ULong total_instances, const CORBA::ULong total_samples,
  const CORBA::ULong payload_size, DDS::DomainParticipant_ptr participant) {

  // Initialize the test parameters
  this->total_instances_ = total_instances;
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

  // Initialize the DataReader entity
  this->reader_ = create_data_reader(this->subscriber_, this->topic_);
  this->data_reader_ = OpenDDSNative::KeyedOctetsDataReader::_narrow(reader_);

  // Initialize waitset and status condition
  this->wait_set_ = create_wait_set(this->reader_);

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

void LatencyTest::run() {
  this->samples_received_ = 0;

  if (!this->latencies_.empty()) {
    this->latencies_.clear();
  }

  std::thread writer_thread([this] {
    this->notified_ = false;

    for (int i = 1; i <= this->total_samples_; i++) {
      for (int j = 1; j <= this->total_instances_; j++) {
        this->sample_.KeyField = std::to_string(j).c_str();

        auto t_start = std::chrono::high_resolution_clock::now();

        const auto ret = this->data_writer_->write(this->sample_, DDS::HANDLE_NIL);
        if (ret != ::DDS::RETCODE_OK) {
          std::cout << "Error writing sample " << ret << ": " << i << std::endl;
          continue;
        }

        std::unique_lock<std::mutex> u_lock(this->mtx_);
        this->cv_.wait(u_lock, [this] { return this->notified_; });
        this->notified_ = false;

        const auto t_end = std::chrono::high_resolution_clock::now();
        auto duration = std::chrono::duration<double, std::milli>(t_end - t_start);

        this->latencies_.push_back(duration.count());
      }
    }
  });

  std::thread reader_thread([this] {
    const CORBA::ULong total = this->total_samples_ * this->total_instances_;
    while (true) {
      DDS::ConditionSeq active_conditions;
      DDS::Duration_t duration = { 5, 0 };

      auto ret = this->wait_set_->wait(active_conditions, duration);
      if (ret != DDS::RETCODE_OK) {
        std::cout << "Error waiting for samples" << ret << ": " << this->samples_received_ << std::endl;
        this->notified_ = true;
        this->cv_.notify_all();
        continue;
      }

      OpenDDSNative::KeyedOctetsSeq samples;
      DDS::SampleInfoSeq infos;

      ret = this->data_reader_->take(samples, infos, DDS::LENGTH_UNLIMITED,
        DDS::ANY_SAMPLE_STATE, DDS::ANY_VIEW_STATE, DDS::ANY_INSTANCE_STATE);

      if (ret != DDS::RETCODE_OK) {
        std::cout << "Error taking samples " << ret << ": " << this->samples_received_ << std::endl;
        this->notified_ = true;
        this->cv_.notify_all();
        continue;
      }

      if (samples.length() > 1) {
        throw std::runtime_error("Received more than one sample");
      }

      this->samples_received_ += samples.length();
      this->data_reader_->return_loan(samples, infos);

      this->notified_ = true;
      this->cv_.notify_all();

      if (this->samples_received_ == total) {
        return;
      }
    }
  });

  reader_thread.join();
  writer_thread.join();
}

void LatencyTest::finalize() const {
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

void* LatencyTest::get_latencies() const {
  return serialize_latencies(this->latencies_);
}
