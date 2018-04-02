#include "SubscriberListenerNative.h"

::OpenDDSharp::DDS::SubscriberListenerNative::SubscriberListenerNative(std::function<void(::DDS::Subscriber_ptr)> onDataOnReaders,
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
																	   std::function<void(::DDS::DataReader_ptr)> onConnectionDeleted) {	
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
	_onConnectionDeleted = onConnectionDeleted;
}

::OpenDDSharp::DDS::SubscriberListenerNative::~SubscriberListenerNative() {
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
	_onConnectionDeleted = nullptr;
};

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_data_on_readers(::DDS::Subscriber_ptr subscriber) {
	if (_onDataOnReaders != nullptr)
		_onDataOnReaders(subscriber);
};

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_data_available(::DDS::DataReader_ptr reader) {
	if (_onDataAvalaible != nullptr)
		_onDataAvalaible(reader);
};

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_requested_deadline_missed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status) {
	if (_onRequestedDeadlineMissed != nullptr)
		_onRequestedDeadlineMissed(reader, status);
}

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_requested_incompatible_qos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus & status) {
	if (_onRequestedIncompatibleQos != nullptr)
		_onRequestedIncompatibleQos(reader, status);
}

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status) {
	if (_onSampleRejected != nullptr)
		_onSampleRejected(reader, status);
}

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status) {
	if (_onLivelinessChanged != nullptr)
		_onLivelinessChanged(reader, status);
}

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status) {
	if (_onSubscriptionMatched != nullptr)
		_onSubscriptionMatched(reader, status);
}

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_sample_lost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status) {
	if (_onSampleLost != nullptr)
		_onSampleLost(reader, status);
}

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_subscription_disconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionDisconnectedStatus& status) {
	if (_onSubscriptionDisconnected != nullptr)
		_onSubscriptionDisconnected(reader, status);
}

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_subscription_reconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionReconnectedStatus& status) {
	if (_onSubscriptionReconnected != nullptr)
		_onSubscriptionReconnected(reader, status);
}

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_subscription_lost(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionLostStatus& status) {
	if (_onSubscriptionLost != nullptr)
		_onSubscriptionLost(reader, status);
}

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_budget_exceeded(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::BudgetExceededStatus& status) {
	if (_onBudgetExceeded != nullptr)
		_onBudgetExceeded(reader, status);
}

void ::OpenDDSharp::DDS::SubscriberListenerNative::on_connection_deleted(::DDS::DataReader_ptr reader) {
	if (_onConnectionDeleted != nullptr)
		_onConnectionDeleted(reader);
}

