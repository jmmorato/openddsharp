/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "DomainParticipant.h"
#include "ContentFilteredTopic.h"
#include "MultiTopic.h"

OpenDDSharp::DDS::DomainParticipant::DomainParticipant(::DDS::DomainParticipant_ptr participant) : OpenDDSharp::DDS::Entity(participant) {
	impl_entity = participant;	
};

System::Int32 OpenDDSharp::DDS::DomainParticipant::DomainId::get() {
	return GetDomainId();
}

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::DomainParticipant::CreateTopic(System::String^ topicName, System::String^ typeName) {
	return OpenDDSharp::DDS::DomainParticipant::CreateTopic(topicName, typeName, nullptr, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::DomainParticipant::CreateTopic(System::String^ topicName, System::String^ typeName, OpenDDSharp::DDS::TopicQos^ qos) {
	return OpenDDSharp::DDS::DomainParticipant::CreateTopic(topicName, typeName, qos, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::DomainParticipant::CreateTopic(System::String^ topicName, System::String^ typeName, OpenDDSharp::DDS::TopicListener^ listener) {
	return OpenDDSharp::DDS::DomainParticipant::CreateTopic(topicName, typeName, nullptr, listener, StatusMask::DefaultStatusMask);
};

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::DomainParticipant::CreateTopic(System::String^ topicName, System::String^ typeName, OpenDDSharp::DDS::TopicListener^ listener, StatusMask statusMask) {
	return OpenDDSharp::DDS::DomainParticipant::CreateTopic(topicName, typeName, nullptr, listener, statusMask);
};

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::DomainParticipant::CreateTopic(System::String^ topicName, System::String^ typeName, OpenDDSharp::DDS::TopicQos^ qos, OpenDDSharp::DDS::TopicListener^ listener) {
	return OpenDDSharp::DDS::DomainParticipant::CreateTopic(topicName, typeName, qos, listener, StatusMask::DefaultStatusMask);
};

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::DomainParticipant::CreateTopic(System::String^ topicName, System::String^ typeName, OpenDDSharp::DDS::TopicQos^ qos, OpenDDSharp::DDS::TopicListener^ listener, StatusMask statusMask) {
	msclr::interop::marshal_context context;

	if (System::String::IsNullOrWhiteSpace(topicName)) {
		return nullptr;
	}

	if (System::String::IsNullOrWhiteSpace(typeName)) {
		return nullptr;
	}

	::DDS::TopicQos tQos;
	if (qos != nullptr) {
		tQos = qos->ToNative();
	}
	else {
		if (impl_entity->get_default_topic_qos(tQos) != ::DDS::RETCODE_OK) {
			tQos = ::TOPIC_QOS_DEFAULT;
		}
	}

	::DDS::TopicListener_var lst = NULL;
	if (listener != nullptr) {
		lst = listener->impl_entity;
	}
	
	const char* topic_name = context.marshal_as<const char*>(topicName);
	const char* type_name = context.marshal_as<const char*>(typeName);
	::DDS::Topic_ptr topic = impl_entity->create_topic(topic_name, type_name, tQos, lst, (System::UInt32)statusMask);

	if (topic != NULL) {
		OpenDDSharp::DDS::Topic^ t = gcnew OpenDDSharp::DDS::Topic(topic);
		t->_listener = listener;
		
		EntityManager::get_instance()->add(topic, t);
		contained_entities->Add(t);
		
		return t;
	}
	else {
		return nullptr;
	}
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::GetDefaultTopicQos(OpenDDSharp::DDS::TopicQos^ qos) {
	::DDS::TopicQos nativeQos;
	::DDS::ReturnCode_t ret = impl_entity->get_default_topic_qos(nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::SetDefaultTopicQos(OpenDDSharp::DDS::TopicQos^ qos) {
	if (qos == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::TopicQos nativeQos = qos->ToNative();
	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_default_topic_qos(nativeQos);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::DeleteTopic(OpenDDSharp::DDS::Topic^ topic) {
	if (topic == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
			
	::DDS::ReturnCode_t ret = impl_entity->delete_topic(topic->impl_entity);
	if (ret == ::DDS::RETCODE_OK) {
		EntityManager::get_instance()->remove(topic->impl_entity);
		contained_entities->Remove(topic);
	}
	
	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::Publisher^ OpenDDSharp::DDS::DomainParticipant::CreatePublisher() {
	return  OpenDDSharp::DDS::DomainParticipant::CreatePublisher(nullptr, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::Publisher^ OpenDDSharp::DDS::DomainParticipant::CreatePublisher(OpenDDSharp::DDS::PublisherQos^ qos) {
	return  OpenDDSharp::DDS::DomainParticipant::CreatePublisher(qos, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::Publisher^ OpenDDSharp::DDS::DomainParticipant::CreatePublisher(OpenDDSharp::DDS::PublisherListener^ listener) {
	return  OpenDDSharp::DDS::DomainParticipant::CreatePublisher(nullptr, listener, StatusMask::DefaultStatusMask);
}

OpenDDSharp::DDS::Publisher^ OpenDDSharp::DDS::DomainParticipant::CreatePublisher(OpenDDSharp::DDS::PublisherListener^ listener, StatusMask statusMask) {
	return  OpenDDSharp::DDS::DomainParticipant::CreatePublisher(nullptr, listener, statusMask);
}

OpenDDSharp::DDS::Publisher^ OpenDDSharp::DDS::DomainParticipant::CreatePublisher(OpenDDSharp::DDS::PublisherQos^ qos, OpenDDSharp::DDS::PublisherListener^ listener) {
	return  OpenDDSharp::DDS::DomainParticipant::CreatePublisher(qos, listener, StatusMask::DefaultStatusMask);
}

OpenDDSharp::DDS::Publisher^ OpenDDSharp::DDS::DomainParticipant::CreatePublisher(OpenDDSharp::DDS::PublisherQos^ qos, OpenDDSharp::DDS::PublisherListener^ listener, StatusMask statusMask) {
	::DDS::PublisherQos pQos;
	if (qos != nullptr) {
		pQos = qos->ToNative();
	}
	else {
		if (impl_entity->get_default_publisher_qos(pQos) != ::DDS::RETCODE_OK) {
			pQos = ::PUBLISHER_QOS_DEFAULT;
		}
	}

	::DDS::PublisherListener_var lst = NULL;
	if (listener != nullptr) {
		lst = listener->impl_entity;
	}

	::DDS::Publisher_ptr publisher = impl_entity->create_publisher(pQos, lst, (System::UInt32)statusMask);

	if (publisher != NULL) {
		OpenDDSharp::DDS::Publisher^ p = gcnew OpenDDSharp::DDS::Publisher(publisher);
		p->m_listener = listener;

		EntityManager::get_instance()->add(publisher, p);
		contained_entities->Add(p);

		return p;
	}
	else {
		return nullptr;
	}
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::GetDefaultPublisherQos(OpenDDSharp::DDS::PublisherQos^ qos) {
	::DDS::PublisherQos nativeQos;
	::DDS::ReturnCode_t ret = impl_entity->get_default_publisher_qos(nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::SetDefaultPublisherQos(OpenDDSharp::DDS::PublisherQos^ qos) {
	if (qos == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::PublisherQos nativeQos = qos->ToNative();
	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_default_publisher_qos(nativeQos);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::DeletePublisher(OpenDDSharp::DDS::Publisher^ pub) {
	if (pub == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	
	::DDS::ReturnCode_t ret = impl_entity->delete_publisher(pub->impl_entity);
	if (ret == ::DDS::RETCODE_OK) {
		EntityManager::get_instance()->remove(pub->impl_entity);
		contained_entities->Remove(pub);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::Subscriber^ OpenDDSharp::DDS::DomainParticipant::CreateSubscriber() {
	return OpenDDSharp::DDS::DomainParticipant::CreateSubscriber(nullptr, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::Subscriber^ OpenDDSharp::DDS::DomainParticipant::CreateSubscriber(OpenDDSharp::DDS::SubscriberQos^ qos) {
	return OpenDDSharp::DDS::DomainParticipant::CreateSubscriber(qos, nullptr, StatusMask::NoStatusMask);
};

OpenDDSharp::DDS::Subscriber^ OpenDDSharp::DDS::DomainParticipant::CreateSubscriber(OpenDDSharp::DDS::SubscriberListener^ listener) {
	return OpenDDSharp::DDS::DomainParticipant::CreateSubscriber(nullptr, listener, StatusMask::DefaultStatusMask);
}

OpenDDSharp::DDS::Subscriber^ OpenDDSharp::DDS::DomainParticipant::CreateSubscriber(OpenDDSharp::DDS::SubscriberListener^ listener, StatusMask statusMask) {
	return OpenDDSharp::DDS::DomainParticipant::CreateSubscriber(nullptr, listener, statusMask);
}

OpenDDSharp::DDS::Subscriber^ OpenDDSharp::DDS::DomainParticipant::CreateSubscriber(OpenDDSharp::DDS::SubscriberQos^ qos, OpenDDSharp::DDS::SubscriberListener^ listener) {
	return OpenDDSharp::DDS::DomainParticipant::CreateSubscriber(qos, listener, StatusMask::DefaultStatusMask);
}

OpenDDSharp::DDS::Subscriber^ OpenDDSharp::DDS::DomainParticipant::CreateSubscriber(OpenDDSharp::DDS::SubscriberQos^ qos, OpenDDSharp::DDS::SubscriberListener^ listener, StatusMask statusMask) {
	::DDS::SubscriberQos sQos;
	if (qos != nullptr) {
		sQos = qos->ToNative();
	}
	else {
		if (impl_entity->get_default_subscriber_qos(sQos) != ::DDS::RETCODE_OK) {
			sQos = ::SUBSCRIBER_QOS_DEFAULT;
		}
	}

	::DDS::SubscriberListener_var lst = NULL;
	if (listener != nullptr) {
		lst = listener->impl_entity;
	}

	::DDS::Subscriber_ptr subscriber = impl_entity->create_subscriber(sQos, lst, (System::UInt32)statusMask);

	if (subscriber != NULL) {
		OpenDDSharp::DDS::Subscriber^ s = gcnew OpenDDSharp::DDS::Subscriber(subscriber);
		s->m_listener = listener;

		EntityManager::get_instance()->add(subscriber, s);
		contained_entities->Add(s);

		return s;
	}
	else {
		return nullptr;
	}
};

OpenDDSharp::DDS::Subscriber^ OpenDDSharp::DDS::DomainParticipant::GetBuiltinSubscriber() {
	::DDS::Subscriber_ptr s = impl_entity->get_builtin_subscriber();
	if (s != NULL) {
		return gcnew ::OpenDDSharp::DDS::Subscriber(s);
	}
	else {
		return nullptr;
	}
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::GetDefaultSubscriberQos(OpenDDSharp::DDS::SubscriberQos^ qos) {
	::DDS::SubscriberQos nativeQos;
	::DDS::ReturnCode_t ret = impl_entity->get_default_subscriber_qos(nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::SetDefaultSubscriberQos(OpenDDSharp::DDS::SubscriberQos^ qos) {
	if (qos == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::SubscriberQos nativeQos = qos->ToNative();
	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_default_subscriber_qos(nativeQos);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::DeleteSubscriber(OpenDDSharp::DDS::Subscriber^ sub) {
	if (sub == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
		
	::DDS::ReturnCode_t ret = impl_entity->delete_subscriber(sub->impl_entity);
	if (ret == ::DDS::RETCODE_OK) {
		EntityManager::get_instance()->remove(sub->impl_entity);
		contained_entities->Remove(sub);
	}	

	return (OpenDDSharp::DDS::ReturnCode)ret;	
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::GetQos(OpenDDSharp::DDS::DomainParticipantQos^ qos) {
	::DDS::DomainParticipantQos* nativeQos = new ::DDS::DomainParticipantQos();
	::DDS::ReturnCode_t ret = impl_entity->get_qos(*nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(*nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::SetQos(OpenDDSharp::DDS::DomainParticipantQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::DomainParticipantQos nativeQos = qos->ToNative();
	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_qos(nativeQos);	
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::DeleteContainedEntities() {
	ICollection<Entity^>^ entities = this->GetContainedEntities();

	::DDS::ReturnCode_t ret = impl_entity->delete_contained_entities();
	if (ret == ::DDS::RETCODE_OK) {
		for each (Entity^ e in entities) {
			EntityManager::get_instance()->remove(e->impl_entity);
			e->contained_entities->Clear();
		}		
		contained_entities->Clear();		
	}
	
	return (OpenDDSharp::DDS::ReturnCode)ret;
};

System::Boolean OpenDDSharp::DDS::DomainParticipant::ContainsEntity(OpenDDSharp::DDS::InstanceHandle handle) {
	return impl_entity->contains_entity(handle);
};

OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ OpenDDSharp::DDS::DomainParticipant::GetListener() {	
	return m_listener;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::SetListener(OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener) {
	return  OpenDDSharp::DDS::DomainParticipant::SetListener(listener, StatusMask::DefaultStatusMask);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::SetListener(OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener, OpenDDSharp::DDS::StatusMask mask) {
	m_listener = listener;
	if (m_listener != nullptr) {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(listener->impl_entity, (System::UInt32)mask);
	}
	else {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(NULL, (System::UInt32)mask);
	}
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::IgnoreParticipant(OpenDDSharp::DDS::InstanceHandle handle) {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->ignore_participant(handle);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::IgnoreTopic(OpenDDSharp::DDS::InstanceHandle handle) {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->ignore_topic(handle);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::IgnorePublication(OpenDDSharp::DDS::InstanceHandle handle) {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->ignore_publication(handle);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::IgnoreSubscription(OpenDDSharp::DDS::InstanceHandle handle) {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->ignore_subscription(handle);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::AssertLiveliness() {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->assert_liveliness();
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::GetCurrentTimestamp(OpenDDSharp::DDS::Timestamp% currentTime) {
	::DDS::Time_t time;
	::DDS::ReturnCode_t ret = impl_entity->get_current_time(time);

	if (ret == ::DDS::RETCODE_OK) {
		currentTime.FromNative(time);		
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

System::Int32 OpenDDSharp::DDS::DomainParticipant::GetDomainId() {
	return impl_entity->get_domain_id();
};

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::DomainParticipant::FindTopic(System::String^ topicName, OpenDDSharp::DDS::Duration timeout) {
	msclr::interop::marshal_context context;

	::DDS::Topic_ptr topic = impl_entity->find_topic(context.marshal_as<const char *>(topicName), timeout.ToNative());

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(topic);
	if (entity != nullptr) {
		return static_cast<OpenDDSharp::DDS::Topic^>(entity);
	}
	else {
		return nullptr;
	}
};

OpenDDSharp::DDS::ITopicDescription^ OpenDDSharp::DDS::DomainParticipant::LookupTopicDescription(System::String^ name) {
	msclr::interop::marshal_context context;

	::DDS::TopicDescription_ptr desc = impl_entity->lookup_topicdescription(context.marshal_as<const char *>(name));
	if (desc != NULL) {
		return gcnew OpenDDSharp::DDS::TopicDescription(desc);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::ContentFilteredTopic^ OpenDDSharp::DDS::DomainParticipant::CreateContentFilteredTopic(System::String^ name, OpenDDSharp::DDS::Topic^ relatedTopic, System::String^ filterExpression, ... array<System::String^>^ expressionParameters) {
	if (System::String::IsNullOrWhiteSpace(name)) {
		return nullptr;
	}

	if (relatedTopic == nullptr) {
		return nullptr;
	}

	if (System::String::IsNullOrWhiteSpace(filterExpression)) {
		return nullptr;
	}

	msclr::interop::marshal_context context;

	::DDS::StringSeq seq;
	if (expressionParameters != nullptr) {
		seq.length(expressionParameters->Length);

		int i = 0;
		for each (System::String^ s in expressionParameters)
		{
			seq[i] = context.marshal_as<const char *>(s);
			i++;
		}
	}
	else {
		seq.length(0);
	}

	::DDS::ContentFilteredTopic_ptr cft = impl_entity->create_contentfilteredtopic(context.marshal_as<const char*>(name), relatedTopic->impl_entity, context.marshal_as<const char*>(filterExpression), seq);

	if (cft != NULL) {
		return gcnew OpenDDSharp::DDS::ContentFilteredTopic(cft);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::DeleteContentFilteredTopic(OpenDDSharp::DDS::ContentFilteredTopic^ contentFilteredTopic) {
	if (contentFilteredTopic == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	return (OpenDDSharp::DDS::ReturnCode)impl_entity->delete_contentfilteredtopic(contentFilteredTopic->impl_entity);
}

OpenDDSharp::DDS::MultiTopic^ OpenDDSharp::DDS::DomainParticipant::CreateMultiTopic(System::String^ name, System::String^ typeName, System::String^ subscriptionExpression, ... array<System::String^>^ expressionParameters) {
	if (System::String::IsNullOrWhiteSpace(name)) {
		return nullptr;
	}

	if (System::String::IsNullOrWhiteSpace(typeName)) {
		return nullptr;
	}

	if (System::String::IsNullOrWhiteSpace(subscriptionExpression)) {
		return nullptr;
	}

	msclr::interop::marshal_context context;

	::DDS::StringSeq seq;
	if (expressionParameters != nullptr) {
		seq.length(expressionParameters->Length);

		int i = 0;
		for each (System::String^ s in expressionParameters)
		{
			seq[i] = context.marshal_as<const char *>(s);
			i++;
		}
	}
	else {
		seq.length(0);
	}

	::DDS::MultiTopic_ptr mt = impl_entity->create_multitopic(context.marshal_as<const char*>(name), context.marshal_as<const char*>(typeName), context.marshal_as<const char*>(subscriptionExpression), seq);

	if (mt != NULL) {
		return gcnew OpenDDSharp::DDS::MultiTopic(mt);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::DeleteMultiTopic(OpenDDSharp::DDS::MultiTopic^ multitopic) {
	if (multitopic == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	return (OpenDDSharp::DDS::ReturnCode)impl_entity->delete_multitopic(multitopic->impl_entity);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::GetDiscoveredParticipants(ICollection<OpenDDSharp::DDS::InstanceHandle>^ participantHandles) {
	if (participantHandles == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	participantHandles->Clear();

	::DDS::InstanceHandleSeq seq;
	::DDS::ReturnCode_t ret = impl_entity->get_discovered_participants(seq);
	
	if (ret == ::DDS::RETCODE_OK) {
		System::UInt32 i = 0;
		while (i < seq.length()) {
			participantHandles->Add(seq[i]);
		}
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::GetDiscoveredParticipantData(OpenDDSharp::DDS::ParticipantBuiltinTopicData% participantData, OpenDDSharp::DDS::InstanceHandle participantHandle) {
	::DDS::ParticipantBuiltinTopicData data;
	::DDS::ReturnCode_t ret = impl_entity->get_discovered_participant_data(data, participantHandle);

	if (ret == ::DDS::RETCODE_OK) {
		participantData.FromNative(data);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::GetDiscoveredTopics(ICollection<OpenDDSharp::DDS::InstanceHandle>^ topicHandles) {
	if (topicHandles == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	topicHandles->Clear();

	::DDS::InstanceHandleSeq seq;
	::DDS::ReturnCode_t ret = impl_entity->get_discovered_topics(seq);

	if (ret == ::DDS::RETCODE_OK) {
		System::UInt32 i = 0;
		while (i < seq.length()) {
			topicHandles->Add(seq[i]);
		}
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DomainParticipant::GetDiscoveredTopicData(OpenDDSharp::DDS::TopicBuiltinTopicData% topicData, OpenDDSharp::DDS::InstanceHandle topicHandle) {
	::DDS::TopicBuiltinTopicData data;
	::DDS::ReturnCode_t ret = impl_entity->get_discovered_topic_data(data, topicHandle);

	if (ret == ::DDS::RETCODE_OK) {
		topicData.FromNative(data);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
};