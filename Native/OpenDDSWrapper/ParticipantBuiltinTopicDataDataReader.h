/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "marshal.h"
#include "BuiltinTopicData.h"

#include <dds/DdsDcpsCoreTypeSupportC.h>

EXTERN_METHOD_EXPORT
::DDS::ParticipantBuiltinTopicDataDataReader_ptr ParticipantBuiltinTopicDataDataReader_Narrow(DDS::DataReader_ptr dr);

EXTERN_METHOD_EXPORT
int ParticipantBuiltinTopicDataDataReader_ReadNextSample(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                         ParticipantBuiltinTopicDataWrapper &data,
                                                         ::DDS::SampleInfo *sampleInfo);

EXTERN_METHOD_EXPORT
int ParticipantBuiltinTopicDataDataReader_TakeNextSample(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                         ParticipantBuiltinTopicDataWrapper &data,
                                                         ::DDS::SampleInfo *sampleInfo);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_Read(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr, void *&receivedData,
                                           void *&receivedInfo, CORBA::Long maxSamples,
                                           ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                           ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_ReadWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                        void *&receivedData, void *&receivedInfo,
                                                        CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_Take(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr, void *&receivedData,
                                           void *&receivedInfo, CORBA::Long maxSamples,
                                           ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                           ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_TakeWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                        void *&receivedData, void *&receivedInfo,
                                                        CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
int ParticipantBuiltinTopicDataDataReader_LookupInstance(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                         ParticipantBuiltinTopicDataWrapper instance);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_ReadInstance(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                   void *&receivedData, void *&receivedInfo,
                                                   ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                   ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                                   ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_ReadInstanceWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                                void *&receivedData, void *&receivedInfo,
                                                                ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                                ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_TakeInstance(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                   void *&receivedData, void *&receivedInfo,
                                                   ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                   ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                                   ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_TakeInstanceWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                                void *&receivedData, void *&receivedInfo,
                                                                ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                                ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_ReadNextInstance(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                       void *&receivedData, void *&receivedInfo,
                                                       ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                       ::DDS::SampleStateMask sampleStates,
                                                       ::DDS::ViewStateMask viewStates,
                                                       ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_ReadNextInstanceWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                                    void *&receivedData, void *&receivedInfo,
                                                                    ::DDS::InstanceHandle_t handle,
                                                                    CORBA::Long maxSamples,
                                                                    ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_TakeNextInstance(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                       void *&receivedData, void *&receivedInfo,
                                                       ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                       ::DDS::SampleStateMask sampleStates,
                                                       ::DDS::ViewStateMask viewStates,
                                                       ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
ParticipantBuiltinTopicDataDataReader_TakeNextInstanceWithCondition(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                                    void *&receivedData, void *&receivedInfo,
                                                                    ::DDS::InstanceHandle_t handle,
                                                                    CORBA::Long maxSamples,
                                                                    ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
int ParticipantBuiltinTopicDataDataReader_GetKeyValue(::DDS::ParticipantBuiltinTopicDataDataReader_ptr dr,
                                                      ParticipantBuiltinTopicDataWrapper &data, int handle);