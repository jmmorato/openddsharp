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
#include "ParticipantBuiltinTopicDataDataReader.h"

::DDS::ParticipantBuiltinTopicDataDataReader_ptr ParticipantBuiltinTopicDataDataReader_Narrow(DDS::DataReader_ptr dr) {
  return ::DDS::ParticipantBuiltinTopicDataDataReader::_narrow(dr);
}

int ParticipantBuiltinTopicDataDataReader_ReadNextSample(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                         ParticipantBuiltinTopicDataWrapper &data,
                                                         ::DDS::SampleInfo *sampleInfo) {
  ::DDS::ParticipantBuiltinTopicData nativeData;
  ::DDS::ReturnCode_t ret = dr->read_next_sample(nativeData, *sampleInfo);

  if (ret == ::DDS::RETCODE_OK) {
    data = nativeData;
  }

  return (int) ret;
}

int ParticipantBuiltinTopicDataDataReader_TakeNextSample(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                         ParticipantBuiltinTopicDataWrapper &data,
                                                         ::DDS::SampleInfo *sampleInfo) {
  ::DDS::ParticipantBuiltinTopicData nativeData;
  ::DDS::ReturnCode_t ret = dr->take_next_sample(nativeData, *sampleInfo);
  if (ret == ::DDS::RETCODE_OK) {
    data = nativeData;
  }

  return (int) ret;
}

::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_Read(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr, void *&receivedData,
                                           void *&receivedInfo, CORBA::Long maxSamples,
                                           ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                           ::DDS::InstanceStateMask instanceStates) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;

  ::DDS::ReturnCode_t ret = dr->read(received_data, info_seq, maxSamples, sampleStates, viewStates, instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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
ParticipantBuiltinTopicDataDataReader_ReadWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                        void *&receivedData, void *&receivedInfo,
                                                        CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;

  ::DDS::ReturnCode_t ret = dr->read_w_condition(received_data, info_seq, maxSamples, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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
ParticipantBuiltinTopicDataDataReader_Take(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr, void *&receivedData,
                                           void *&receivedInfo, CORBA::Long maxSamples,
                                           ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                           ::DDS::InstanceStateMask instanceStates) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->take(received_data, info_seq, maxSamples, sampleStates, viewStates, instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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
ParticipantBuiltinTopicDataDataReader_TakeWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                        void *&receivedData, void *&receivedInfo,
                                                        CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;

  ::DDS::ReturnCode_t ret = dr->take_w_condition(received_data, info_seq, maxSamples, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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

int ParticipantBuiltinTopicDataDataReader_LookupInstance(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                         ParticipantBuiltinTopicDataWrapper instance) {
  ::DDS::ParticipantBuiltinTopicData nativeData = instance;

  return dr->lookup_instance(nativeData);
}

::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_ReadInstance(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                   void *&receivedData, void *&receivedInfo,
                                                   ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                   ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                                   ::DDS::InstanceStateMask instanceStates) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->read_instance(received_data, info_seq, maxSamples, handle, sampleStates, viewStates,
                                              instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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
ParticipantBuiltinTopicDataDataReader_ReadInstanceWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                                void *&receivedData, void *&receivedInfo,
                                                                ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                                ::DDS::ReadCondition_ptr condition) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;

  ::DDS::ReturnCode_t ret = dr->read_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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
ParticipantBuiltinTopicDataDataReader_TakeInstance(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                   void *&receivedData, void *&receivedInfo,
                                                   ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                   ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                                   ::DDS::InstanceStateMask instanceStates) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->take_instance(received_data, info_seq, maxSamples, handle, sampleStates, viewStates,
                                              instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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
ParticipantBuiltinTopicDataDataReader_TakeInstanceWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                                void *&receivedData, void *&receivedInfo,
                                                                ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                                ::DDS::ReadCondition_ptr condition) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;

  ::DDS::ReturnCode_t ret = dr->take_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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
ParticipantBuiltinTopicDataDataReader_ReadNextInstance(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                       void *&receivedData, void *&receivedInfo,
                                                       ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                       ::DDS::SampleStateMask sampleStates,
                                                       ::DDS::ViewStateMask viewStates,
                                                       ::DDS::InstanceStateMask instanceStates) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->read_next_instance(received_data, info_seq, maxSamples, handle, sampleStates,
                                                   viewStates, instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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
ParticipantBuiltinTopicDataDataReader_ReadNextInstanceWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                                    void *&receivedData, void *&receivedInfo,
                                                                    ::DDS::InstanceHandle_t handle,
                                                                    CORBA::Long maxSamples,
                                                                    ::DDS::ReadCondition_ptr condition) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->read_next_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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
ParticipantBuiltinTopicDataDataReader_TakeNextInstance(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                       void *&receivedData, void *&receivedInfo,
                                                       ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                       ::DDS::SampleStateMask sampleStates,
                                                       ::DDS::ViewStateMask viewStates,
                                                       ::DDS::InstanceStateMask instanceStates) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->take_next_instance(received_data, info_seq, maxSamples, handle, sampleStates,
                                                   viewStates, instanceStates);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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
ParticipantBuiltinTopicDataDataReader_TakeNextInstanceWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                                    void *&receivedData, void *&receivedInfo,
                                                                    ::DDS::InstanceHandle_t handle,
                                                                    CORBA::Long maxSamples,
                                                                    ::DDS::ReadCondition_ptr condition) {
  ::DDS::ParticipantBuiltinTopicDataSeq received_data;
  ::DDS::SampleInfoSeq info_seq;
  ::DDS::ReturnCode_t ret = dr->take_next_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
  if (ret == ::DDS::RETCODE_OK) {
    TAO::unbounded_value_sequence<ParticipantBuiltinTopicDataWrapper> seq(received_data.length());
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

int ParticipantBuiltinTopicDataDataReader_GetKeyValue(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                      ParticipantBuiltinTopicDataWrapper &data, int handle) {
  ::DDS::ParticipantBuiltinTopicData nativeData;
  ::DDS::ReturnCode_t ret = dr->get_key_value(nativeData, handle);

  if (ret == ::DDS::RETCODE_OK) {
    data = nativeData;
  }

  return ret;
}