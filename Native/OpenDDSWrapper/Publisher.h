/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "QosPolicies.h"
#include "PublisherListenerImpl.h"
#include "DataWriterListenerImpl.h"

#include "dds/DCPS/Marked_Default_Qos.h"

EXTERN_METHOD_EXPORT
::DDS::Entity_ptr Publisher_NarrowBase(::DDS::Publisher_ptr pub);

EXTERN_METHOD_EXPORT
::DDS::DataWriter_ptr Publisher_CreateDataWriter(::DDS::Publisher_ptr pub,
                                                 ::DDS::Topic_ptr topic,
                                                 DataWriterQosWrapper qos,
                                                 OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl_ptr a_listener,
                                                 ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_GetQos(::DDS::Publisher_ptr pub, PublisherQosWrapper &qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_SetQos(::DDS::Publisher_ptr pub, PublisherQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_GetDefaultDataWriterQos(::DDS::Publisher_ptr pub, DataWriterQosWrapper &qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_SetDefaultDataWriterQos(::DDS::Publisher_ptr pub, DataWriterQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_DeleteDataWriter(::DDS::Publisher_ptr pub, ::DDS::DataWriter_ptr dw);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
Publisher_SetListener(::DDS::Publisher_ptr pub, OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl_ptr listener,
                      ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::DomainParticipant_ptr Publisher_GetParticipant(::DDS::Publisher_ptr pub);

EXTERN_METHOD_EXPORT
::DDS::DataWriter_ptr Publisher_LookupDataWriter(::DDS::Publisher_ptr pub, char *topicName);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_DeleteContainedEntities(::DDS::Publisher_ptr pub);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_WaitForAcknowledgments(::DDS::Publisher_ptr pub, ::DDS::Duration_t maxWait);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_SuspendPublications(::DDS::Publisher_ptr pub);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_ResumePublications(::DDS::Publisher_ptr pub);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_BeginCoherentChanges(::DDS::Publisher_ptr pub);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_EndCoherentChanges(::DDS::Publisher_ptr pub);