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

::DDS::Entity_ptr DomainParticipant_NarrowBase(::DDS::DomainParticipant_ptr dp) {
	return static_cast<::DDS::Entity_ptr>(dp);
}

::DDS::Publisher_ptr DomainParticipant_CreatePublisher(
    ::DDS::DomainParticipant_ptr dp, 
    PublisherQosWrapper* qos, 
    ::DDS::PublisherListener_ptr a_listener, 
    ::DDS::StatusMask mask)
{
    return dp->create_publisher(PUBLISHER_QOS_DEFAULT, NULL, ::OpenDDS::DCPS::DEFAULT_STATUS_MASK);
}

::DDS::Subscriber_ptr DomainParticipant_CreateSubscriber(
    ::DDS::DomainParticipant_ptr dp,
    SubscriberQosWrapper qos,
    ::DDS::SubscriberListener_ptr a_listener,
    ::DDS::StatusMask mask)
{
    return dp->create_subscriber(qos, NULL, ::OpenDDS::DCPS::DEFAULT_STATUS_MASK);
}

::DDS::Topic_ptr DomainParticipant_CreateTopic(
    ::DDS::DomainParticipant_ptr dp,
    const char * topic_name,
    const char * type_name,
    TopicQosWrapper* qos,
    ::DDS::TopicListener_ptr a_listener,
    ::DDS::StatusMask mask)
{
    return dp->create_topic(topic_name, type_name, TOPIC_QOS_DEFAULT, NULL, ::OpenDDS::DCPS::DEFAULT_STATUS_MASK);
}

::DDS::ReturnCode_t DomainParticipant_DeleteContainedEntities(::DDS::DomainParticipant_ptr dp)
{
	return dp->delete_contained_entities();
}
