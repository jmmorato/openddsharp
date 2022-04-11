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
#include "TopicBuiltinTopicDataDataReader.h"

::DDS::TopicBuiltinTopicDataDataReader_ptr TopicBuiltinTopicDataDataReader_Narrow(DDS::DataReader_ptr dr)
{
    return ::DDS::TopicBuiltinTopicDataDataReader::_narrow(dr);
}

int TopicBuiltinTopicDataDataReader_ReadNextSample(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, TopicBuiltinTopicDataWrapper& data, ::DDS::SampleInfo* sampleInfo)
{
    ::DDS::TopicBuiltinTopicData nativeData;
    ::DDS::ReturnCode_t ret = dr->read_next_sample(nativeData, *sampleInfo);

    if (ret == ::DDS::RETCODE_OK)
    {
        data = nativeData;
    }
    
    return (int)ret;
}

int TopicBuiltinTopicDataDataReader_TakeNextSample(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, TopicBuiltinTopicDataWrapper& data, ::DDS::SampleInfo* sampleInfo)
{
    ::DDS::TopicBuiltinTopicData nativeData;
    ::DDS::ReturnCode_t ret = dr->take_next_sample(nativeData, *sampleInfo);
    if (ret == ::DDS::RETCODE_OK)
    {
        data = nativeData;
    }

    return (int)ret;
}

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_Read(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;

    ::DDS::ReturnCode_t ret = dr->read(received_data, info_seq, maxSamples, sampleStates, viewStates, instanceStates);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_ReadWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;

    ::DDS::ReturnCode_t ret = dr->read_w_condition(received_data, info_seq, maxSamples, condition);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_Take(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;    
    ::DDS::ReturnCode_t ret = dr->take(received_data, info_seq, maxSamples, sampleStates, viewStates, instanceStates);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_TakeWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;

    ::DDS::ReturnCode_t ret = dr->take_w_condition(received_data, info_seq, maxSamples, condition);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

int TopicBuiltinTopicDataDataReader_LookupInstance(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, TopicBuiltinTopicDataWrapper instance) {
    ::DDS::TopicBuiltinTopicData nativeData = instance;

    return dr->lookup_instance(nativeData);
}

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_ReadInstance(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;    
    ::DDS::ReturnCode_t ret = dr->read_instance(received_data, info_seq, maxSamples, handle, sampleStates, viewStates, instanceStates);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_ReadInstanceWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;

    ::DDS::ReturnCode_t ret = dr->read_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_TakeInstance(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;    
    ::DDS::ReturnCode_t ret = dr->take_instance(received_data, info_seq, maxSamples, handle, sampleStates, viewStates, instanceStates);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_TakeInstanceWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;

    ::DDS::ReturnCode_t ret = dr->take_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_ReadNextInstance(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;    
    ::DDS::ReturnCode_t ret = dr->read_next_instance(received_data, info_seq, maxSamples, handle, sampleStates, viewStates, instanceStates);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_ReadNextInstanceWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;    
    ::DDS::ReturnCode_t ret = dr->read_next_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_TakeNextInstance(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;    
    ::DDS::ReturnCode_t ret = dr->take_next_instance(received_data, info_seq, maxSamples, handle, sampleStates, viewStates, instanceStates);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_TakeNextInstanceWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition)
{
    ::DDS::TopicBuiltinTopicDataSeq received_data;
    ::DDS::SampleInfoSeq info_seq;    
    ::DDS::ReturnCode_t ret = dr->take_next_instance_w_condition(received_data, info_seq, maxSamples, handle, condition);
    if (ret == ::DDS::RETCODE_OK)
    {
        TAO::unbounded_value_sequence<TopicBuiltinTopicDataWrapper> seq(received_data.length());
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

int TopicBuiltinTopicDataDataReader_GetKeyValue(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, TopicBuiltinTopicDataWrapper& data, int handle)
{
    ::DDS::TopicBuiltinTopicData nativeData;
    ::DDS::ReturnCode_t ret = dr->get_key_value(nativeData, handle);

    if (ret == ::DDS::RETCODE_OK)
    {
        data = nativeData;
    }

    return ret;
}