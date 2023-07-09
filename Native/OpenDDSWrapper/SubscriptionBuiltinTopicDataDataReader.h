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
::DDS::SubscriptionBuiltinTopicDataDataReader_ptr SubscriptionBuiltinTopicDataDataReader_Narrow(DDS::DataReader_ptr dr);

EXTERN_METHOD_EXPORT
int SubscriptionBuiltinTopicDataDataReader_ReadNextSample(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                          SubscriptionBuiltinTopicDataWrapper &data,
                                                          ::DDS::SampleInfo *sampleInfo);

EXTERN_METHOD_EXPORT
int SubscriptionBuiltinTopicDataDataReader_TakeNextSample(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                          SubscriptionBuiltinTopicDataWrapper &data,
                                                          ::DDS::SampleInfo *sampleInfo);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
SubscriptionBuiltinTopicDataDataReader_Read(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr, void *&receivedData,
                                            void *&receivedInfo, CORBA::Long maxSamples,
                                            ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                            ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
SubscriptionBuiltinTopicDataDataReader_ReadWithCondition(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                         void *&receivedData, void *&receivedInfo,
                                                         CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
SubscriptionBuiltinTopicDataDataReader_Take(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr, void *&receivedData,
                                            void *&receivedInfo, CORBA::Long maxSamples,
                                            ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                            ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
SubscriptionBuiltinTopicDataDataReader_TakeWithCondition(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                         void *&receivedData, void *&receivedInfo,
                                                         CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
int SubscriptionBuiltinTopicDataDataReader_LookupInstance(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                          SubscriptionBuiltinTopicDataWrapper instance);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
SubscriptionBuiltinTopicDataDataReader_ReadInstance(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                    void *&receivedData, void *&receivedInfo,
                                                    ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                    ::DDS::SampleStateMask sampleStates,
                                                    ::DDS::ViewStateMask viewStates,
                                                    ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
SubscriptionBuiltinTopicDataDataReader_ReadInstanceWithCondition(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                                 void *&receivedData, void *&receivedInfo,
                                                                 ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                                 ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
SubscriptionBuiltinTopicDataDataReader_TakeInstance(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                    void *&receivedData, void *&receivedInfo,
                                                    ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                    ::DDS::SampleStateMask sampleStates,
                                                    ::DDS::ViewStateMask viewStates,
                                                    ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
SubscriptionBuiltinTopicDataDataReader_TakeInstanceWithCondition(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                                 void *&receivedData, void *&receivedInfo,
                                                                 ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                                 ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
SubscriptionBuiltinTopicDataDataReader_ReadNextInstance(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                        void *&receivedData, void *&receivedInfo,
                                                        ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                        ::DDS::SampleStateMask sampleStates,
                                                        ::DDS::ViewStateMask viewStates,
                                                        ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t SubscriptionBuiltinTopicDataDataReader_ReadNextInstanceWithCondition(
    ::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr, void *&receivedData, void *&receivedInfo,
    ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
SubscriptionBuiltinTopicDataDataReader_TakeNextInstance(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                        void *&receivedData, void *&receivedInfo,
                                                        ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                        ::DDS::SampleStateMask sampleStates,
                                                        ::DDS::ViewStateMask viewStates,
                                                        ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t SubscriptionBuiltinTopicDataDataReader_TakeNextInstanceWithCondition(
    ::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr, void *&receivedData, void *&receivedInfo,
    ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
int SubscriptionBuiltinTopicDataDataReader_GetKeyValue(::DDS::SubscriptionBuiltinTopicDataDataReader_ptr dr,
                                                       SubscriptionBuiltinTopicDataWrapper &data, int handle);