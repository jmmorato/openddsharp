/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2022 Jose Morato

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
#include "PublicationBuiltinTopicDataDataReader.h"

::DDS::PublicationBuiltinTopicDataDataReader_ptr PublicationBuiltinTopicDataDataReader_Narrow(DDS::DataReader_ptr dr) {
  return ::DDS::PublicationBuiltinTopicDataDataReader::_narrow(dr);
}

int PublicationBuiltinTopicDataDataReader_ReadNextSample(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                         PublicationBuiltinTopicDataWrapper &data,
                                                         ::DDS::SampleInfo *sampleInfo) {
  ::DDS::PublicationBuiltinTopicData nativeData;
  ::DDS::ReturnCode_t ret = dr->read_next_sample(nativeData, *sampleInfo);

  if (ret == ::DDS::RETCODE_OK) {
    data = nativeData;
  }

  return (int) ret;
}

int PublicationBuiltinTopicDataDataReader_TakeNextSample(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                         PublicationBuiltinTopicDataWrapper &data,
                                                         ::DDS::SampleInfo *sampleInfo) {
  ::DDS::PublicationBuiltinTopicData nativeData;
  ::DDS::ReturnCode_t ret = dr->take_next_sample(nativeData, *sampleInfo);
  if (ret == ::DDS::RETCODE_OK) {
    data = nativeData;
  }

  return (int) ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_Read(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr, void *&receivedData,
                                           void *&receivedInfo, CORBA::Long maxSamples,
                                           ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                           ::DDS::InstanceStateMask instanceStates) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;

  ::DDS::ReturnCode_t ret = dr->read(received_data, info_seq, maxSamples, sampleStates, viewStates, instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_ReadWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                        void *&receivedData, void *&receivedInfo,
                                                        CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;

  ::DDS::ReturnCode_t ret = dr->read_w_condition(received_data, info_seq, maxSamples, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_Take(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr, void *&receivedData,
                                           void *&receivedInfo, CORBA::Long maxSamples,
                                           ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                           ::DDS::InstanceStateMask instanceStates) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->take(received_data, info_seq, maxSamples, sampleStates, viewStates, instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_TakeWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                        void *&receivedData, void *&receivedInfo,
                                                        CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;

  ::DDS::ReturnCode_t ret = dr->take_w_condition(received_data, info_seq, maxSamples, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

int PublicationBuiltinTopicDataDataReader_LookupInstance(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                         PublicationBuiltinTopicDataWrapper instance) {
  ::DDS::PublicationBuiltinTopicData nativeData = instance;

  return dr->lookup_instance(nativeData);
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_ReadInstance(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                   void *&receivedData, void *&receivedInfo,
                                                   ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                   ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                                   ::DDS::InstanceStateMask instanceStates) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->read_instance(received_data, info_seq, maxSamples, handle, sampleStates, viewStates,
                                              instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_ReadInstanceWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                                void *&receivedData, void *&receivedInfo,
                                                                ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                                ::DDS::ReadCondition_ptr condition) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;

  ::DDS::ReturnCode_t ret = dr->read_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_TakeInstance(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                   void *&receivedData, void *&receivedInfo,
                                                   ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                   ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                                   ::DDS::InstanceStateMask instanceStates) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->take_instance(received_data, info_seq, maxSamples, handle, sampleStates, viewStates,
                                              instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_TakeInstanceWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                                void *&receivedData, void *&receivedInfo,
                                                                ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                                ::DDS::ReadCondition_ptr condition) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;

  ::DDS::ReturnCode_t ret = dr->take_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_ReadNextInstance(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                       void *&receivedData, void *&receivedInfo,
                                                       ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                       ::DDS::SampleStateMask sampleStates,
                                                       ::DDS::ViewStateMask viewStates,
                                                       ::DDS::InstanceStateMask instanceStates) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->read_next_instance(received_data, info_seq, maxSamples, handle, sampleStates,
                                                   viewStates, instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_ReadNextInstanceWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                                    void *&receivedData, void *&receivedInfo,
                                                                    ::DDS::InstanceHandle_t handle,
                                                                    CORBA::Long maxSamples,
                                                                    ::DDS::ReadCondition_ptr condition) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->read_next_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_TakeNextInstance(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                       void *&receivedData, void *&receivedInfo,
                                                       ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                       ::DDS::SampleStateMask sampleStates,
                                                       ::DDS::ViewStateMask viewStates,
                                                       ::DDS::InstanceStateMask instanceStates) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->take_next_instance(received_data, info_seq, maxSamples, handle, sampleStates,
                                                   viewStates, instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_TakeNextInstanceWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                                    void *&receivedData, void *&receivedInfo,
                                                                    ::DDS::InstanceHandle_t handle,
                                                                    CORBA::Long maxSamples,
                                                                    ::DDS::ReadCondition_ptr condition) {
  ::DDS::PublicationBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->take_next_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<PublicationBuiltinTopicDataWrapper> seq(received_data.length());
    seq.length(received_data.length());
    for (unsigned int i = 0; i < received_data.length(); i++) {
      seq[i] = received_data[i];
    }

    unbounded_sequence_to_ptr(seq, receivedData);
    unbounded_sequence_to_ptr(info_seq, receivedInfo);
  }

  dr->return_loan(received_data, info_seq);

  return ret;
}

int PublicationBuiltinTopicDataDataReader_GetKeyValue(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                      PublicationBuiltinTopicDataWrapper &data, int handle) {
  ::DDS::PublicationBuiltinTopicData nativeData;
  ::DDS::ReturnCode_t ret = dr->get_key_value(nativeData, handle);

  if (ret == ::DDS::RETCODE_OK) {
    data = nativeData;
  }

  return ret;
}