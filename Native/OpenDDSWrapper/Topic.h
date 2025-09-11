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
#include "TopicListenerImpl.h"

EXTERN_METHOD_EXPORT
::DDS::Entity_ptr Topic_NarrowBase(::DDS::Topic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::TopicDescription_ptr Topic_NarrowTopicDescription(::DDS::Topic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Topic_GetQos(::DDS::Topic_ptr t, TopicQosWrapper &qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Topic_SetQos(::DDS::Topic_ptr t, TopicQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Topic_SetListener(::DDS::Topic_ptr t, OpenDDSharp::OpenDDS::DDS::TopicListenerImpl_ptr listener,
                                      ::DDS::StatusMask status);

EXTERN_METHOD_EXPORT
char *Topic_GetTypeName(::DDS::Topic_ptr t);

EXTERN_METHOD_EXPORT
char *Topic_GetName(::DDS::Topic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::DomainParticipant_ptr Topic_GetParticipant(::DDS::Topic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Topic_GetInconsistentTopicStatus(::DDS::Topic_ptr t, ::DDS::InconsistentTopicStatus_out status);