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

#include <random>
#include <thread>

#include "dds/DCPS/DomainParticipantImpl.h"
#include "dds/DCPS/Marked_Default_Qos.h"
#include "dds/DdsDcpsInfrastructureC.h"
#include "TestDataC.h"
#include "TestDataTypeSupportC.h"
#include "TestDataTypeSupportImpl.h"

#ifndef CLASS_EXPORT_FLAG
  #ifdef _WIN32
    #define CLASS_EXPORT_FLAG __declspec (dllexport)
  #else
    #define CLASS_EXPORT_FLAG __attribute__ ((visibility("default")))
  #endif
#endif

#ifndef EXTERN_METHOD_EXPORT
  #ifdef _WIN32
    #define EXTERN_METHOD_EXPORT extern "C" __declspec(dllexport)
  #else
    #define EXTERN_METHOD_EXPORT extern "C"
  #endif
#endif

#ifndef DOMAIN_ID
  #define DOMAIN_ID 45
#endif

inline std::string random_string(const std::size_t length)
{
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

inline std::vector<unsigned char> random_bytes(const std::size_t length)
{

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

inline bool wait_for_publications(::DDS::DataReader_ptr reader, int publications_count, int milliseconds)
{
  DDS::InstanceHandleSeq handles;
  reader->get_matched_publications(handles);

  int count = milliseconds / 100;
  while (handles.length() != publications_count && count > 0)
  {
    std::this_thread::sleep_for(std::chrono::milliseconds(100));
    reader->get_matched_publications(handles);
    count--;
  }

  return count != 0 || handles.length() == publications_count;
}

inline bool wait_for_subscriptions(DDS::DataWriter_ptr writer, int subscriptions_count, int milliseconds)
{
  DDS::InstanceHandleSeq handles;
  writer->get_matched_subscriptions(handles);

  int count = milliseconds / 100;
  while (handles.length() != subscriptions_count && count > 0)
  {
    std::this_thread::sleep_for(::std::chrono::milliseconds(100));
    writer->get_matched_subscriptions(handles);
    count--;
  }

  return count != 0 || handles.length() == subscriptions_count;
}
