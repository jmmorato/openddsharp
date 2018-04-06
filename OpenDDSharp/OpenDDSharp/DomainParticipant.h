#pragma once

#pragma unmanaged 
#include <dds/DCPS/Service_Participant.h>
#include <dds/DCPS/Marked_Default_Qos.h>
#pragma managed

#include "Entity.h"
#include "Topic.h"
#include "Publisher.h"
#include "Subscriber.h"
#include "ReturnCode.h"
#include "DomainParticipantQos.h"
#include "DomainParticipantListener.h"
#include "TopicQos.h"
#include "PublisherQos.h"
#include "SubscriberQos.h"
#include "StatusMask.h"
#include "TopicListener.h"
#include "PublisherListener.h"
#include "SubscriberListener.h"
#include "InstanceHandle.h"
#include "Timestamp.h"
#include "EntityManager.h"
#include "ParticipantBuiltinTopicData.h"
#include "TopicBuiltinTopicData.h"

#include <vcclr.h>
#include <msclr/marshal.h>

#pragma make_public(::DDS::DomainParticipant)

namespace OpenDDSharp {
	namespace DDS {

		ref class ContentFilteredTopic;
		ref class MultiTopic;

		public ref class DomainParticipant : public OpenDDSharp::DDS::Entity {

		internal:
			OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ m_listener;			

		public:
			::DDS::DomainParticipant_ptr impl_entity;			
		
		internal:
			DomainParticipant(::DDS::DomainParticipant_ptr participant);

		public:
			OpenDDSharp::DDS::Topic^ CreateTopic(System::String^ topicName, System::String^ typeName);
			OpenDDSharp::DDS::Topic^ CreateTopic(System::String^ topicName, System::String^ typeName, OpenDDSharp::DDS::TopicQos^ qos);
			OpenDDSharp::DDS::Topic^ CreateTopic(System::String^ topicName, System::String^ typeName, OpenDDSharp::DDS::TopicListener^ listener);
			OpenDDSharp::DDS::Topic^ CreateTopic(System::String^ topicName, System::String^ typeName, OpenDDSharp::DDS::TopicListener^ listener, StatusMask statusMask);
			OpenDDSharp::DDS::Topic^ CreateTopic(System::String^ topicName, System::String^ typeName, OpenDDSharp::DDS::TopicQos^ qos, OpenDDSharp::DDS::TopicListener^ listener);
			OpenDDSharp::DDS::Topic^ CreateTopic(System::String^ topicName, System::String^ typeName, OpenDDSharp::DDS::TopicQos^ qos, OpenDDSharp::DDS::TopicListener^ listener, StatusMask statusMask);
			OpenDDSharp::DDS::ReturnCode DeleteTopic(OpenDDSharp::DDS::Topic^ topic);

			OpenDDSharp::DDS::Publisher^ CreatePublisher();
			OpenDDSharp::DDS::Publisher^ CreatePublisher(OpenDDSharp::DDS::PublisherQos^ qos);
			OpenDDSharp::DDS::Publisher^ CreatePublisher(OpenDDSharp::DDS::PublisherListener^ listener);
			OpenDDSharp::DDS::Publisher^ CreatePublisher(OpenDDSharp::DDS::PublisherListener^ listener, StatusMask statusMask);
			OpenDDSharp::DDS::Publisher^ CreatePublisher(OpenDDSharp::DDS::PublisherQos^ qos, OpenDDSharp::DDS::PublisherListener^ listener);
			OpenDDSharp::DDS::Publisher^ CreatePublisher(OpenDDSharp::DDS::PublisherQos^ qos, OpenDDSharp::DDS::PublisherListener^ listener, StatusMask statusMask);
			OpenDDSharp::DDS::ReturnCode GetDefaultPublisherQos(OpenDDSharp::DDS::PublisherQos^ qos);
			OpenDDSharp::DDS::ReturnCode SetDefaultPublisherQos(OpenDDSharp::DDS::PublisherQos^ qos);
			OpenDDSharp::DDS::ReturnCode DeletePublisher(OpenDDSharp::DDS::Publisher^ pub);
			
			OpenDDSharp::DDS::Subscriber^ CreateSubscriber();
			OpenDDSharp::DDS::Subscriber^ CreateSubscriber(OpenDDSharp::DDS::SubscriberQos^ qos);
			OpenDDSharp::DDS::Subscriber^ CreateSubscriber(OpenDDSharp::DDS::SubscriberListener^ listener);
			OpenDDSharp::DDS::Subscriber^ CreateSubscriber(OpenDDSharp::DDS::SubscriberListener^ listener, StatusMask statusMask);
			OpenDDSharp::DDS::Subscriber^ CreateSubscriber(OpenDDSharp::DDS::SubscriberQos^ qos, OpenDDSharp::DDS::SubscriberListener^ listener);
			OpenDDSharp::DDS::Subscriber^ CreateSubscriber(OpenDDSharp::DDS::SubscriberQos^ qos, OpenDDSharp::DDS::SubscriberListener^ listener, StatusMask statusMask);
			OpenDDSharp::DDS::Subscriber^ GetBuiltinSubscriber();
			OpenDDSharp::DDS::ReturnCode GetDefaultSubscriberQos(OpenDDSharp::DDS::SubscriberQos^ qos);
			OpenDDSharp::DDS::ReturnCode SetDefaultSubscriberQos(OpenDDSharp::DDS::SubscriberQos^ qos);
			OpenDDSharp::DDS::ReturnCode DeleteSubscriber(OpenDDSharp::DDS::Subscriber^ sub);

			System::Int32 GetDomainId();
			System::Boolean ContainsEntity(OpenDDSharp::DDS::InstanceHandle handle);
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::DomainParticipantQos^ qos);
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::DomainParticipantQos^ qos);
			OpenDDSharp::DDS::ReturnCode DeleteContainedEntities();
			OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ GetListener();
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener, OpenDDSharp::DDS::StatusMask mask);									
			OpenDDSharp::DDS::ReturnCode IgnoreParticipant(OpenDDSharp::DDS::InstanceHandle handle);
			OpenDDSharp::DDS::ReturnCode IgnoreTopic(OpenDDSharp::DDS::InstanceHandle handle);
			OpenDDSharp::DDS::ReturnCode IgnorePublication(OpenDDSharp::DDS::InstanceHandle handle);
			OpenDDSharp::DDS::ReturnCode IgnoreSubscription(OpenDDSharp::DDS::InstanceHandle handle);			
			OpenDDSharp::DDS::ReturnCode AssertLiveliness();
			OpenDDSharp::DDS::ReturnCode GetCurrentTimestamp(OpenDDSharp::DDS::Timestamp currentTime);

			OpenDDSharp::DDS::Topic^ FindTopic(System::String^ topicName, OpenDDSharp::DDS::Duration timeout);
			OpenDDSharp::DDS::ITopicDescription^ LookupTopicDescription(System::String^ name);
			OpenDDSharp::DDS::ContentFilteredTopic^ CreateContentFilteredTopic(System::String^ name, OpenDDSharp::DDS::Topic^ relatedTopic, System::String^ filterExpression, ICollection<System::String^>^ expressionParameters);
			OpenDDSharp::DDS::ReturnCode DeleteContentFilteredTopic(OpenDDSharp::DDS::ContentFilteredTopic^ contentFilteredTopic);
			OpenDDSharp::DDS::MultiTopic^ CreateMultiTopic(System::String^ name, System::String^ typeName, System::String^ subscriptionExpression,ICollection<System::String^>^ expressionParameters);
			OpenDDSharp::DDS::ReturnCode DeleteMultiTopic(OpenDDSharp::DDS::MultiTopic^ multitopic);

			OpenDDSharp::DDS::ReturnCode GetDiscoveredParticipants(ICollection<OpenDDSharp::DDS::InstanceHandle>^ participantHandles);
			OpenDDSharp::DDS::ReturnCode GetDiscoveredParticipantData(OpenDDSharp::DDS::ParticipantBuiltinTopicData% participantData, OpenDDSharp::DDS::InstanceHandle participantHandle);
			OpenDDSharp::DDS::ReturnCode GetDiscoveredTopics(ICollection<OpenDDSharp::DDS::InstanceHandle>^ topicHandles);
			OpenDDSharp::DDS::ReturnCode GetDiscoveredTopicData(OpenDDSharp::DDS::TopicBuiltinTopicData% topicData, OpenDDSharp::DDS::InstanceHandle topicHandle);

		};

	};
};