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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
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
::DDS::ReturnCode_t Topic_GetQos(::DDS::Topic_ptr t, TopicQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Topic_SetQos(::DDS::Topic_ptr t, TopicQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Topic_SetListener(::DDS::Topic_ptr t, OpenDDSharp::OpenDDS::DDS::TopicListenerImpl_ptr listener, ::DDS::StatusMask status);

EXTERN_METHOD_EXPORT
char* Topic_GetTypeName(::DDS::Topic_ptr t);

EXTERN_METHOD_EXPORT
char* Topic_GetName(::DDS::Topic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::DomainParticipant_ptr Topic_GetParticipant(::DDS::Topic_ptr t);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Topic_GetInconsistentTopicStatus(::DDS::Topic_ptr t, ::DDS::InconsistentTopicStatus_out status);