#include "DomainParticipantListenerNative.h"

::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::DomainParticipantListenerNative(std::function<void(::DDS::Subscriber_ptr)> onDataOnReaders,
																							   std::function<void(::DDS::DataReader_ptr)> onDataAvalaible,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::RequestedDeadlineMissedStatus)> onRequestedDeadlineMissed,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::RequestedIncompatibleQosStatus)> onRequestedIncompatibleQos,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::SampleRejectedStatus)> onSampleRejected,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::LivelinessChangedStatus)> onLivelinessChanged,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::SubscriptionMatchedStatus)> onSubscriptionMatched,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::SampleLostStatus)> onSampleLost,
																							   std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionDisconnectedStatus)> onSubscriptionDisconnected,
																							   std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionReconnectedStatus)> onSubscriptionReconnected,
																							   std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionLostStatus)> onSubscriptionLost,
																							   std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::BudgetExceededStatus)> onBudgetExceeded,
																							   std::function<void(::DDS::DataReader_ptr)> onReaderConnectionDeleted,
																							   std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> onOfferedDeadlineMissed,
																							   std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> onOfferedIncompatibleQos,
																							   std::function<void(::DDS::DataWriter_ptr writer, ::DDS::LivelinessLostStatus status)> onLivelinessLost,
																							   std::function<void(::DDS::DataWriter_ptr writer, ::DDS::PublicationMatchedStatus status)> onPublicationMatched,
																							   std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationDisconnectedStatus)> onPublicationDisconnected,
																							   std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationReconnectedStatus)> onPublicationReconnected,
																							   std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationLostStatus)> onPublicationLost,
																							   std::function<void(::DDS::DataWriter_ptr)> onWriterConnectionDeleted,
																							   std::function<void(::DDS::Topic_ptr topic, ::DDS::InconsistentTopicStatus status)> onInconsistentTopic) {
	_onDataOnReaders = onDataOnReaders;
	_onDataAvalaible = onDataAvalaible;
	_onRequestedDeadlineMissed = onRequestedDeadlineMissed;
	_onRequestedIncompatibleQos = onRequestedIncompatibleQos;
	_onSampleRejected = onSampleRejected;
	_onLivelinessChanged = onLivelinessChanged;
	_onSubscriptionMatched = onSubscriptionMatched;
	_onSampleLost = onSampleLost;
	_onSubscriptionDisconnected = onSubscriptionDisconnected;
	_onSubscriptionReconnected = onSubscriptionReconnected;
	_onSubscriptionLost = onSubscriptionLost;
	_onBudgetExceeded = onBudgetExceeded;
	_onReaderConnectionDeleted = onReaderConnectionDeleted;
	_onOfferedDeadlineMissed = onOfferedDeadlineMissed;
	_onOfferedIncompatibleQos = onOfferedIncompatibleQos;
	_onLivelinessLost = onLivelinessLost;
	_onPublicationMatched = onPublicationMatched;
	_onPublicationDisconnected = onPublicationDisconnected;
	_onPublicationReconnected = onPublicationReconnected;
	_onPublicationLost = onPublicationLost;
	_onWriterConnectionDeleted = onWriterConnectionDeleted;
	_onInconsistentTopic = onInconsistentTopic;
}

::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::~DomainParticipantListenerNative() {
	_onDataOnReaders = nullptr;
	_onDataAvalaible = nullptr;
	_onRequestedDeadlineMissed = nullptr;
	_onRequestedIncompatibleQos = nullptr;
	_onSampleRejected = nullptr;
	_onLivelinessChanged = nullptr;
	_onSubscriptionMatched = nullptr;
	_onSampleLost = nullptr;
	_onSubscriptionDisconnected = nullptr;
	_onSubscriptionReconnected = nullptr;
	_onSubscriptionLost = nullptr;
	_onBudgetExceeded = nullptr;
	_onReaderConnectionDeleted = nullptr;
	_onOfferedDeadlineMissed = nullptr;
	_onOfferedIncompatibleQos = nullptr;
	_onLivelinessLost = nullptr;
	_onPublicationMatched = nullptr;
	_onPublicationDisconnected = nullptr;
	_onPublicationReconnected = nullptr;
	_onPublicationLost = nullptr;
	_onWriterConnectionDeleted = nullptr;
	_onInconsistentTopic = nullptr;
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_data_on_readers(::DDS::Subscriber_ptr subscriber) {
	if (_onDataOnReaders != nullptr)
		_onDataOnReaders(subscriber);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_data_available(::DDS::DataReader_ptr reader) {
	if (_onDataAvalaible != nullptr)
		_onDataAvalaible(reader);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_requested_deadline_missed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status) {
	if (_onRequestedDeadlineMissed != nullptr)
		_onRequestedDeadlineMissed(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_requested_incompatible_qos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus & status) {
	if (_onRequestedIncompatibleQos != nullptr)
		_onRequestedIncompatibleQos(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status) {
	if (_onSampleRejected != nullptr)
		_onSampleRejected(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status) {
	if (_onLivelinessChanged != nullptr)
		_onLivelinessChanged(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status) {
	if (_onSubscriptionMatched != nullptr)
		_onSubscriptionMatched(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_sample_lost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status) {
	if (_onSampleLost != nullptr)
		_onSampleLost(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_subscription_disconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionDisconnectedStatus& status) {
	if (_onSubscriptionDisconnected != nullptr)
		_onSubscriptionDisconnected(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_subscription_reconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionReconnectedStatus& status) {
	if (_onSubscriptionReconnected != nullptr)
		_onSubscriptionReconnected(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_subscription_lost(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionLostStatus& status) {
	if (_onSubscriptionLost != nullptr)
		_onSubscriptionLost(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_budget_exceeded(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::BudgetExceededStatus& status) {
	if (_onBudgetExceeded != nullptr)
		_onBudgetExceeded(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_connection_deleted(::DDS::DataReader_ptr reader) {
	if (_onReaderConnectionDeleted != nullptr)
		_onReaderConnectionDeleted(reader);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status) {
	if (_onOfferedDeadlineMissed != nullptr)
		_onOfferedDeadlineMissed(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status) {
	if (_onOfferedIncompatibleQos != nullptr)
		_onOfferedIncompatibleQos(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status) {
	if (_onLivelinessLost != nullptr)
		_onLivelinessLost(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status) {
	if (_onPublicationMatched != nullptr)
		_onPublicationMatched(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_publication_disconnected(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationDisconnectedStatus & status) {
	if (_onPublicationDisconnected != nullptr)
		_onPublicationDisconnected(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_publication_reconnected(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationReconnectedStatus & status) {
	if (_onPublicationReconnected != nullptr)
		_onPublicationReconnected(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_publication_lost(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationLostStatus & status) {
	if (_onPublicationLost != nullptr)
		_onPublicationLost(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_connection_deleted(::DDS::DataWriter_ptr writer) {
	if (_onWriterConnectionDeleted != nullptr)
		_onWriterConnectionDeleted(writer);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus & status) {
	if (_onInconsistentTopic != nullptr)
		_onInconsistentTopic(topic, status);
};
