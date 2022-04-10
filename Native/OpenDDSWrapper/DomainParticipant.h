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
#include "QosPolicies.h"
#include "DomainParticipantListenerImpl.h"
#include "TopicListenerImpl.h"
#include "SubscriberListenerImpl.h"
#include "PublisherListenerImpl.h"
#include "BuiltinTopicData.h"

#include "dds/DCPS/Marked_Default_Qos.h"
#include "dds/DCPS/PublisherImpl.h"
#include "dds/DCPS/DomainParticipantImpl.h"

EXTERN_METHOD_EXPORT
::DDS::Entity_ptr DomainParticipant_NarrowBase(::DDS::DomainParticipant_ptr dp);

EXTERN_METHOD_EXPORT
CORBA::Long DomainParticipant_GetDomainId(::DDS::DomainParticipant_ptr dp);

EXTERN_METHOD_EXPORT 
::DDS::Publisher_ptr DomainParticipant_CreatePublisher(::DDS::DomainParticipant_ptr dp, 
                                                       PublisherQosWrapper qos, 
                                                       OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl_ptr a_listener,
                                                       ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_GetDefaultPublisherQos(::DDS::DomainParticipant_ptr dp, PublisherQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_SetDefaultPublisherQos(::DDS::DomainParticipant_ptr dp, PublisherQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT 
::DDS::Subscriber_ptr DomainParticipant_CreateSubscriber(::DDS::DomainParticipant_ptr dp, 
                                                         SubscriberQosWrapper qos, 
                                                         OpenDDSharp::OpenDDS::DDS::SubscriberListenerImpl_ptr a_listener,
                                                         ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_GetDefaultSubscriberQos(::DDS::DomainParticipant_ptr dp, SubscriberQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_SetDefaultSubscriberQos(::DDS::DomainParticipant_ptr dp, SubscriberQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT 
::DDS::Topic_ptr DomainParticipant_CreateTopic(::DDS::DomainParticipant_ptr dp,
                                               const char * topic_name,
                                               const char * type_name,
                                               TopicQosWrapper qos,
                                               OpenDDSharp::OpenDDS::DDS::TopicListenerImpl_ptr a_listener,
                                               ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_GetDefaultTopicQos(::DDS::DomainParticipant_ptr dp, TopicQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_SetDefaultTopicQos(::DDS::DomainParticipant_ptr dp, TopicQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_GetQos(::DDS::DomainParticipant_ptr dp, DomainParticipantQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_SetQos(::DDS::DomainParticipant_ptr dp, DomainParticipantQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_SetListener(::DDS::DomainParticipant_ptr dp, OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl_ptr listener, ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_DeleteContainedEntities(::DDS::DomainParticipant_ptr dp);

EXTERN_METHOD_EXPORT
CORBA::Boolean DomainParticipant_ContainsEntity(::DDS::DomainParticipant_ptr dp, ::DDS::InstanceHandle_t handle);

EXTERN_METHOD_EXPORT
::DDS::Topic_ptr DomainParticipant_FindTopic(::DDS::DomainParticipant_ptr dp, char* topicName, ::DDS::Duration_t duration);

EXTERN_METHOD_EXPORT
::DDS::TopicDescription_ptr DomainParticipant_LookupTopicDescription(::DDS::DomainParticipant_ptr dp, char* name);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_DeleteTopic(::DDS::DomainParticipant_ptr dp, ::DDS::Topic_ptr topic);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_DeletePublisher(::DDS::DomainParticipant_ptr dp, ::DDS::Publisher_ptr pub);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_DeleteSubscriber(::DDS::DomainParticipant_ptr dp, ::DDS::Subscriber_ptr sub);

EXTERN_METHOD_EXPORT
::DDS::Subscriber_ptr DomainParticipant_GetBuiltinSubscriber(::DDS::DomainParticipant_ptr dp);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_IgnoreParticipant(::DDS::DomainParticipant_ptr dp, ::DDS::InstanceHandle_t handle);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_IgnoreTopic(::DDS::DomainParticipant_ptr dp, ::DDS::InstanceHandle_t handle);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_IgnorePublication(::DDS::DomainParticipant_ptr dp, ::DDS::InstanceHandle_t handle);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_IgnoreSubscription(::DDS::DomainParticipant_ptr dp, ::DDS::InstanceHandle_t handle);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_AssertLiveliness(::DDS::DomainParticipant_ptr dp);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_GetCurrentTimestamp(::DDS::DomainParticipant_ptr dp, ::DDS::Time_t& time);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_GetDiscoveredParticipants(::DDS::DomainParticipant_ptr dp, void*& ptr);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_GetDiscoveredTopics(::DDS::DomainParticipant_ptr dp, void*& ptr);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_GetDiscoveredParticipantData(::DDS::DomainParticipant_ptr dp, ParticipantBuiltinTopicDataWrapper& data, ::DDS::InstanceHandle_t handle);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_GetDiscoveredTopicData(::DDS::DomainParticipant_ptr dp, TopicBuiltinTopicDataWrapper& data, ::DDS::InstanceHandle_t handle);

EXTERN_METHOD_EXPORT
::DDS::ContentFilteredTopic_ptr DomainParticipant_CreateContentFilteredTopic(::DDS::DomainParticipant_ptr dp, char* name, ::DDS::Topic_ptr relatedTopic, char* filterExpression, void* seq);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_DeleteContentFilteredTopic(::DDS::DomainParticipant_ptr dp, ::DDS::ContentFilteredTopic_ptr cft);

EXTERN_METHOD_EXPORT
::DDS::MultiTopic_ptr DomainParticipant_CreateMultiTopic(::DDS::DomainParticipant_ptr dp, char* name, char* type_name, char* expression, void* seq);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipant_DeleteMultiTopic(::DDS::DomainParticipant_ptr dp, ::DDS::MultiTopic_ptr cft);