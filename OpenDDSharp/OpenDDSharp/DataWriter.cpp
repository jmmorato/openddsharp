#include "DataWriter.h"
#include "DataWriterListener.h"
#include "Publisher.h"

OpenDDSharp::DDS::DataWriter::DataWriter(::DDS::DataWriter_ptr dataWriter) : OpenDDSharp::DDS::Entity(dataWriter) {
	impl_entity = dataWriter;	
}

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::DataWriter::Topic::get() {
	return GetTopic();
}

OpenDDSharp::DDS::Publisher^ OpenDDSharp::DDS::DataWriter::Publisher::get() {
	return GetPublisher();
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::SetQos(OpenDDSharp::DDS::DataWriterQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_qos(qos->ToNative());
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::GetQos(OpenDDSharp::DDS::DataWriterQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::DataWriterQos nativeQos;
	::DDS::ReturnCode_t ret = impl_entity->get_qos(nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::SetListener(OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener) {
	return OpenDDSharp::DDS::DataWriter::SetListener(listener, StatusMask::DefaultStatusMask);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::SetListener(OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener, OpenDDSharp::DDS::StatusMask mask) {
	_listener = listener;

	if (_listener != nullptr) {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(listener->impl_entity, (System::UInt32)mask);
	}
	else {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(NULL, (System::UInt32)mask);
	}
}

OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ OpenDDSharp::DDS::DataWriter::GetListener() {
	return _listener;
}

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::DataWriter::GetTopic() {
	::DDS::Topic_ptr topic = impl_entity->get_topic();

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(topic);
	if (entity != nullptr) {
		return static_cast<OpenDDSharp::DDS::Topic^>(entity);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::Publisher^ OpenDDSharp::DDS::DataWriter::GetPublisher() {
	::DDS::Publisher_ptr publisher = impl_entity->get_publisher();

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(publisher);
	if (entity != nullptr) {
		return static_cast<OpenDDSharp::DDS::Publisher^>(entity);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::WaitForAcknowledgments(OpenDDSharp::DDS::Duration maxWait) {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->wait_for_acknowledgments(maxWait.ToNative());
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::GetLivelinessLostStatus(OpenDDSharp::DDS::LivelinessLostStatus% status) {
	::DDS::LivelinessLostStatus s;
	::DDS::ReturnCode_t ret = impl_entity->get_liveliness_lost_status(s);
	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(s);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::GetOfferedDeadlineMissedStatus(OpenDDSharp::DDS::OfferedDeadlineMissedStatus% status) {
	::DDS::OfferedDeadlineMissedStatus s;
	::DDS::ReturnCode_t ret = impl_entity->get_offered_deadline_missed_status(s);
	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(s);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::GetOfferedIncompatibleQosStatus(OpenDDSharp::DDS::OfferedIncompatibleQosStatus% status) {
	::DDS::OfferedIncompatibleQosStatus s;
	::DDS::ReturnCode_t ret = impl_entity->get_offered_incompatible_qos_status(s);
	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(s);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::GetPublicationMatchedStatus(OpenDDSharp::DDS::PublicationMatchedStatus% status) {
	::DDS::PublicationMatchedStatus s;
	::DDS::ReturnCode_t ret = impl_entity->get_publication_matched_status(s);
	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(s);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::AssertLiveliness() {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->assert_liveliness();
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::GetMatchedSubscriptions(ICollection<OpenDDSharp::DDS::InstanceHandle>^ subscriptionHandles) {
	if (subscriptionHandles == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::InstanceHandleSeq seq;
	::DDS::ReturnCode_t ret = impl_entity->get_matched_subscriptions(seq);

	subscriptionHandles->Clear();

	if (ret == ::DDS::RETCODE_OK) {
		System::UInt32 i = 0;
		while (i < seq.length()) {
			subscriptionHandles->Add(seq[i]);
		}
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataWriter::GetMatchedSubscriptionData(OpenDDSharp::DDS::InstanceHandle subscriptionHandle, OpenDDSharp::DDS::SubscriptionBuiltinTopicData% subscriptionData) {
	::DDS::SubscriptionBuiltinTopicData data;
	::DDS::ReturnCode_t ret = impl_entity->get_matched_subscription_data(data, subscriptionHandle);

	if (ret == ::DDS::RETCODE_OK) {
		subscriptionData.FromNative(data);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}
