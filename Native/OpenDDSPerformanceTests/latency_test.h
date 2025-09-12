/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include <mutex>
#include <condition_variable>
#include "utils.h"


class CLASS_EXPORT_FLAG LatencyTest {

  DDS::DomainParticipant_ptr participant_ = DDS::DomainParticipant::_nil();
  DDS::Publisher_ptr publisher_ = DDS::Publisher::_nil();
  DDS::Subscriber_ptr subscriber_ = DDS::Subscriber::_nil();
  DDS::Topic_ptr topic_ = DDS::Topic::_nil();
  DDS::WaitSet_ptr wait_set_ = nullptr;
  DDS::StatusCondition_ptr status_condition_ = nullptr;
  DDS::DataWriter_ptr writer_ = DDS::DataWriter::_nil();
  DDS::DataReader_ptr reader_ = DDS::DataReader::_nil();
  OpenDDSNative::KeyedOctetsDataWriter_ptr data_writer_ = OpenDDSNative::KeyedOctetsDataWriter::_nil();
  OpenDDSNative::KeyedOctetsDataReader_ptr data_reader_ = OpenDDSNative::KeyedOctetsDataReader::_nil();
  OpenDDSNative::KeyedOctets sample_;
  std::vector<double> latencies_;

  CORBA::ULong total_instances_ = 0;
  CORBA::ULong total_samples_ = 0;
  CORBA::ULong payload_size_ = 0;
  CORBA::ULong samples_received_ = 0;

  std::mutex mtx_;
  std::condition_variable cv_;
  bool notified_ = false;

  public:
    void initialize(CORBA::ULong total_instances, CORBA::ULong total_samples, CORBA::ULong payload_size, DDS::DomainParticipant_ptr participant);
    void run();
    void finalize() const;
    void* get_latencies() const;
};
