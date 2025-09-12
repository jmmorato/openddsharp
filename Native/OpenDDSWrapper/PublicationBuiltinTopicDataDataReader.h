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
::DDS::PublicationBuiltinTopicDataDataReader_ptr PublicationBuiltinTopicDataDataReader_Narrow(DDS::DataReader_ptr dr);

EXTERN_METHOD_EXPORT
int PublicationBuiltinTopicDataDataReader_ReadNextSample(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                         PublicationBuiltinTopicDataWrapper &data,
                                                         ::DDS::SampleInfo *sampleInfo);

EXTERN_METHOD_EXPORT
int PublicationBuiltinTopicDataDataReader_TakeNextSample(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                         PublicationBuiltinTopicDataWrapper &data,
                                                         ::DDS::SampleInfo *sampleInfo);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_Read(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr, void *&receivedData,
                                           void *&receivedInfo, CORBA::Long maxSamples,
                                           ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                           ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_ReadWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                        void *&receivedData, void *&receivedInfo,
                                                        CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_Take(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr, void *&receivedData,
                                           void *&receivedInfo, CORBA::Long maxSamples,
                                           ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                           ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_TakeWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                        void *&receivedData, void *&receivedInfo,
                                                        CORBA::Long maxSamples, ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
int PublicationBuiltinTopicDataDataReader_LookupInstance(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                         PublicationBuiltinTopicDataWrapper instance);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_ReadInstance(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                   void *&receivedData, void *&receivedInfo,
                                                   ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                   ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                                   ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_ReadInstanceWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                                void *&receivedData, void *&receivedInfo,
                                                                ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                                ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_TakeInstance(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                   void *&receivedData, void *&receivedInfo,
                                                   ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                   ::DDS::SampleStateMask sampleStates, ::DDS::ViewStateMask viewStates,
                                                   ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_TakeInstanceWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                                void *&receivedData, void *&receivedInfo,
                                                                ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                                ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_ReadNextInstance(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                       void *&receivedData, void *&receivedInfo,
                                                       ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                       ::DDS::SampleStateMask sampleStates,
                                                       ::DDS::ViewStateMask viewStates,
                                                       ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_ReadNextInstanceWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                                    void *&receivedData, void *&receivedInfo,
                                                                    ::DDS::InstanceHandle_t handle,
                                                                    CORBA::Long maxSamples,
                                                                    ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_TakeNextInstance(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                       void *&receivedData, void *&receivedInfo,
                                                       ::DDS::InstanceHandle_t handle, CORBA::Long maxSamples,
                                                       ::DDS::SampleStateMask sampleStates,
                                                       ::DDS::ViewStateMask viewStates,
                                                       ::DDS::InstanceStateMask instanceStates);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
PublicationBuiltinTopicDataDataReader_TakeNextInstanceWithCondition(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                                    void *&receivedData, void *&receivedInfo,
                                                                    ::DDS::InstanceHandle_t handle,
                                                                    CORBA::Long maxSamples,
                                                                    ::DDS::ReadCondition_ptr condition);

EXTERN_METHOD_EXPORT
int PublicationBuiltinTopicDataDataReader_GetKeyValue(::DDS::PublicationBuiltinTopicDataDataReader_ptr dr,
                                                      PublicationBuiltinTopicDataWrapper &data, int handle);