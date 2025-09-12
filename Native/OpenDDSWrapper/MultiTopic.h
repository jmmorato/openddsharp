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

EXTERN_METHOD_EXPORT
::DDS::TopicDescription_ptr MultiTopic_NarrowTopicDescription(::DDS::MultiTopic_ptr t);

EXTERN_METHOD_EXPORT
char *MultiTopic_GetTypeName(::DDS::MultiTopic_ptr t);

EXTERN_METHOD_EXPORT
char *MultiTopic_GetName(::DDS::MultiTopic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::DomainParticipant_ptr MultiTopic_GetParticipant(::DDS::MultiTopic_ptr t);

EXTERN_METHOD_EXPORT
char *MultiTopic_GetSubscriptionExpression(::DDS::MultiTopic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t MultiTopic_GetExpressionParameters(::DDS::MultiTopic_ptr t, void *&seq);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t MultiTopic_SetExpressionParameters(::DDS::MultiTopic_ptr t, void *seq);
