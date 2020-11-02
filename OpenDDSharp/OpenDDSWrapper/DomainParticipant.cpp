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
#include "DomainParticipant.h"
#include <dds\DCPS\PublisherImpl.h>
#include <dds\DCPS\DomainParticipantImpl.h>

::DDS::Entity_ptr DomainParticipant_NarrowBase(::DDS::DomainParticipant_ptr dp) {
	return static_cast<::DDS::Entity_ptr>(dp);
}

CORBA::Long DomainParticipant_GetDomainId(::DDS::DomainParticipant_ptr dp) {
    return dp->get_domain_id();
}

::DDS::Publisher_ptr DomainParticipant_CreatePublisher(::DDS::DomainParticipant_ptr dp, 
                                                       PublisherQosWrapper qos, 
                                                       OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl_ptr a_listener,
                                                       ::DDS::StatusMask mask) {
    return dp->create_publisher(qos, a_listener, mask);
}

::DDS::ReturnCode_t DomainParticipant_GetDefaultPublisherQos(::DDS::DomainParticipant_ptr dp, PublisherQosWrapper& qos_wrapper) {
    ::DDS::PublisherQos qos_native;
    ::DDS::ReturnCode_t ret = dp->get_default_publisher_qos(qos_native);

    if (ret == ::DDS::RETCODE_OK) {
        qos_wrapper = qos_native;
    }

    return ret;
}

::DDS::ReturnCode_t DomainParticipant_SetDefaultPublisherQos(::DDS::DomainParticipant_ptr dp, PublisherQosWrapper qos_wrapper) {
    return dp->set_default_publisher_qos(qos_wrapper);
}

::DDS::Subscriber_ptr DomainParticipant_CreateSubscriber(::DDS::DomainParticipant_ptr dp,
                                                         SubscriberQosWrapper qos,
                                                         OpenDDSharp::OpenDDS::DDS::SubscriberListenerImpl_ptr a_listener,
                                                         ::DDS::StatusMask mask) {
    return dp->create_subscriber(qos, a_listener, mask);
}

::DDS::ReturnCode_t DomainParticipant_GetDefaultSubscriberQos(::DDS::DomainParticipant_ptr dp, SubscriberQosWrapper& qos_wrapper) {
    ::DDS::SubscriberQos qos_native;
    ::DDS::ReturnCode_t ret = dp->get_default_subscriber_qos(qos_native);

    if (ret == ::DDS::RETCODE_OK) {
        qos_wrapper = qos_native;
    }

    return ret;
}

::DDS::ReturnCode_t DomainParticipant_SetDefaultSubscriberQos(::DDS::DomainParticipant_ptr dp, SubscriberQosWrapper qos_wrapper) {
    return dp->set_default_subscriber_qos(qos_wrapper);
}

::DDS::Topic_ptr DomainParticipant_CreateTopic(::DDS::DomainParticipant_ptr dp,
                                               const char * topic_name,
                                               const char * type_name,
                                               TopicQosWrapper qos,
                                               OpenDDSharp::OpenDDS::DDS::TopicListenerImpl_ptr a_listener,
                                               ::DDS::StatusMask mask) {    
    return dp->create_topic(topic_name, type_name, qos, a_listener, mask);
}

::DDS::ReturnCode_t DomainParticipant_GetDefaultTopicQos(::DDS::DomainParticipant_ptr dp, TopicQosWrapper& qos_wrapper) {
    ::DDS::TopicQos qos_native;
    ::DDS::ReturnCode_t ret = dp->get_default_topic_qos(qos_native);

    if (ret == ::DDS::RETCODE_OK) {
        qos_wrapper = qos_native;
    }

    return ret;
}

::DDS::ReturnCode_t DomainParticipant_SetDefaultTopicQos(::DDS::DomainParticipant_ptr dp, TopicQosWrapper qos_wrapper) {
    return dp->set_default_topic_qos(qos_wrapper);
}

::DDS::ReturnCode_t DomainParticipant_GetQos(::DDS::DomainParticipant_ptr dp, DomainParticipantQosWrapper& qos_wrapper) {
    ::DDS::DomainParticipantQos qos_native;
    ::DDS::ReturnCode_t ret = dp->get_qos(qos_native);

    if (ret == ::DDS::RETCODE_OK) {
        qos_wrapper = qos_native;
    }

    return ret;
}

::DDS::ReturnCode_t DomainParticipant_SetQos(::DDS::DomainParticipant_ptr dp, DomainParticipantQosWrapper qos_wrapper) {
    return dp->set_qos(qos_wrapper);
}

::DDS::ReturnCode_t DomainParticipant_SetListener(::DDS::DomainParticipant_ptr dp, OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl_ptr listener, ::DDS::StatusMask mask) {
    return dp->set_listener(listener, mask);
}

::DDS::ReturnCode_t DomainParticipant_DeleteContainedEntities(::DDS::DomainParticipant_ptr dp)
{
	return dp->delete_contained_entities();
}

CORBA::Boolean DomainParticipant_ContainsEntity(::DDS::DomainParticipant_ptr dp, ::DDS::InstanceHandle_t handle) {
    return dp->contains_entity(handle);
}

::DDS::Entity_ptr DomainParticipant_FindTopic(::DDS::DomainParticipant_ptr dp, char* topicName, ::DDS::Duration_t duration) {
    return DDS::Entity::_narrow(dp->find_topic(topicName, duration));
}

::DDS::Entity_ptr DomainParticipant_LookupTopicDescription(::DDS::DomainParticipant_ptr dp, char* name) {
    return DDS::Entity::_narrow(dp->lookup_topicdescription(name));
}

::DDS::ReturnCode_t DomainParticipant_DeleteTopic(::DDS::DomainParticipant_ptr dp, ::DDS::Topic_ptr topic) {
    return dp->delete_topic(topic);
}

::DDS::ReturnCode_t DomainParticipant_DeletePublisher(::DDS::DomainParticipant_ptr dp, ::DDS::Publisher_ptr pub) {
    OpenDDS::DCPS::PublisherImpl* the_servant = dynamic_cast<OpenDDS::DCPS::PublisherImpl*>(pub);
    return dp->delete_publisher(pub);
}

::DDS::ReturnCode_t DomainParticipant_DeleteSubscriber(::DDS::DomainParticipant_ptr dp, ::DDS::Subscriber_ptr sub) {    
    return dp->delete_subscriber(sub);
}

::DDS::Subscriber_ptr DomainParticipant_GetBuiltinSubscriber(::DDS::DomainParticipant_ptr dp) {
    return dp->get_builtin_subscriber();
}

::DDS::ReturnCode_t DomainParticipant_IgnoreParticipant(::DDS::DomainParticipant_ptr dp, ::DDS::InstanceHandle_t handle) {
    return dp->ignore_participant(handle);
}

::DDS::ReturnCode_t DomainParticipant_IgnoreTopic(::DDS::DomainParticipant_ptr dp, ::DDS::InstanceHandle_t handle) {
    return dp->ignore_topic(handle);
}

::DDS::ReturnCode_t DomainParticipant_IgnorePublication(::DDS::DomainParticipant_ptr dp, ::DDS::InstanceHandle_t handle) {
    return dp->ignore_publication(handle);
}

::DDS::ReturnCode_t DomainParticipant_IgnoreSubscription(::DDS::DomainParticipant_ptr dp, ::DDS::InstanceHandle_t handle) {
    return dp->ignore_subscription(handle);
}

::DDS::ReturnCode_t DomainParticipant_AssertLiveliness(::DDS::DomainParticipant_ptr dp) {
    return dp->assert_liveliness();
}

::DDS::ReturnCode_t DomainParticipant_GetCurrentTimestamp(::DDS::DomainParticipant_ptr dp, ::DDS::Time_t& time) {
    return dp->get_current_time(time);
}

::DDS::ReturnCode_t DomainParticipant_GetDiscoveredParticipants(::DDS::DomainParticipant_ptr dp, void* & ptr) {
    ::DDS::InstanceHandleSeq seq;    
    ::DDS::ReturnCode_t ret = dp->get_discovered_participants(seq);

    if (ret == ::DDS::RETCODE_OK) {
        unbounded_sequence_to_ptr(seq, ptr);
    }

    return ret;
}

::DDS::ReturnCode_t DomainParticipant_GetDiscoveredTopics(::DDS::DomainParticipant_ptr dp, void*& ptr) {
    ::DDS::InstanceHandleSeq seq;
    ::DDS::ReturnCode_t ret = dp->get_discovered_topics(seq);

    if (ret == ::DDS::RETCODE_OK) {
        unbounded_sequence_to_ptr(seq, ptr);
    }

    return ret;
}

::DDS::ReturnCode_t DomainParticipant_GetDiscoveredParticipantData(::DDS::DomainParticipant_ptr dp, ParticipantBuiltinTopicDataWrapper& data, ::DDS::InstanceHandle_t handle) {
    ::DDS::ParticipantBuiltinTopicData d;
    DDS::ReturnCode_t ret = dp->get_discovered_participant_data(d, handle);

    if (ret == ::DDS::RETCODE_OK) {
        data = d;
    }

    return ret;
}

::DDS::ReturnCode_t DomainParticipant_GetDiscoveredTopicData(::DDS::DomainParticipant_ptr dp, TopicBuiltinTopicDataWrapper& data, ::DDS::InstanceHandle_t handle) {
    ::DDS::TopicBuiltinTopicData d;
    DDS::ReturnCode_t ret = dp->get_discovered_topic_data(d, handle);

    if (ret == ::DDS::RETCODE_OK) {
        data = d;
    }

    return ret;
}