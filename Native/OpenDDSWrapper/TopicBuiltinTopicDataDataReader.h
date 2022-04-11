/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "marshal.h"
#include "BuiltinTopicData.h"

#include <dds/DdsDcpsCoreTypeSupportC.h>

EXTERN_METHOD_EXPORT
::DDS::TopicBuiltinTopicDataDataReader_ptr TopicBuiltinTopicDataDataReader_Narrow(DDS::DataReader_ptr dr);

EXTERN_METHOD_EXPORT
int TopicBuiltinTopicDataDataReader_ReadNextSample(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, TopicBuiltinTopicDataWrapper& data, ::DDS::SampleInfo* sampleInfo);

EXTERN_METHOD_EXPORT
int TopicBuiltinTopicDataDataReader_TakeNextSample(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, TopicBuiltinTopicDataWrapper& data, ::DDS::SampleInfo* sampleInfo);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_Read(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_ReadWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_Take(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_TakeWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
int TopicBuiltinTopicDataDataReader_LookupInstance(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, TopicBuiltinTopicDataWrapper instance);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_ReadInstance(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_ReadInstanceWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_TakeInstance(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_TakeInstanceWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_ReadNextInstance(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_ReadNextInstanceWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_TakeNextInstance(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates, ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t TopicBuiltinTopicDataDataReader_TakeNextInstanceWithCondition(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, void*& receivedData, void*& receivedInfo, ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
int TopicBuiltinTopicDataDataReader_GetKeyValue(::DDS::TopicBuiltinTopicDataDataReader_ptr dr, TopicBuiltinTopicDataWrapper& data, int handle);