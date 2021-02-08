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
#include "DomainParticipantListenerNative.h"

::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::DomainParticipantListenerNative(std::function<void(::DDS::Subscriber_ptr)> onDataOnReaders,
																							   std::function<void(::DDS::DataReader_ptr)> onDataAvalaible,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::RequestedDeadlineMissedStatus)> onRequestedDeadlineMissed,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::RequestedIncompatibleQosStatus)> onRequestedIncompatibleQos,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::SampleRejectedStatus)> onSampleRejected,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::LivelinessChangedStatus)> onLivelinessChanged,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::SubscriptionMatchedStatus)> onSubscriptionMatched,
																							   std::function<void(::DDS::DataReader_ptr, ::DDS::SampleLostStatus)> onSampleLost,																							  
																							   std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> onOfferedDeadlineMissed,
																							   std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> onOfferedIncompatibleQos,
																							   std::function<void(::DDS::DataWriter_ptr writer, ::DDS::LivelinessLostStatus status)> onLivelinessLost,
																							   std::function<void(::DDS::DataWriter_ptr writer, ::DDS::PublicationMatchedStatus status)> onPublicationMatched,
																							   std::function<void(::DDS::Topic_ptr topic, ::DDS::InconsistentTopicStatus status)> onInconsistentTopic) {
	_onDataOnReaders = onDataOnReaders;
	_onDataAvalaible = onDataAvalaible;
	_onRequestedDeadlineMissed = onRequestedDeadlineMissed;
	_onRequestedIncompatibleQos = onRequestedIncompatibleQos;
	_onSampleRejected = onSampleRejected;
	_onLivelinessChanged = onLivelinessChanged;
	_onSubscriptionMatched = onSubscriptionMatched;
	_onSampleLost = onSampleLost;
	_onOfferedDeadlineMissed = onOfferedDeadlineMissed;
	_onOfferedIncompatibleQos = onOfferedIncompatibleQos;
	_onLivelinessLost = onLivelinessLost;
	_onPublicationMatched = onPublicationMatched;
	_onInconsistentTopic = onInconsistentTopic;
}

::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::~DomainParticipantListenerNative() {
	_onDataOnReaders = NULL;
	_onDataAvalaible = NULL;
	_onRequestedDeadlineMissed = NULL;
	_onRequestedIncompatibleQos = NULL;
	_onSampleRejected = NULL;
	_onLivelinessChanged = NULL;
	_onSubscriptionMatched = NULL;
	_onSampleLost = NULL;
	_onOfferedDeadlineMissed = NULL;
	_onOfferedIncompatibleQos = NULL;
	_onLivelinessLost = NULL;
	_onPublicationMatched = NULL;
	_onInconsistentTopic = NULL;
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_data_on_readers(::DDS::Subscriber_ptr subscriber) {
	if (_onDataOnReaders != NULL)
		_onDataOnReaders(subscriber);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_data_available(::DDS::DataReader_ptr reader) {
	if (_onDataAvalaible != NULL)
		_onDataAvalaible(reader);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_requested_deadline_missed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status) {
	if (_onRequestedDeadlineMissed != NULL)
		_onRequestedDeadlineMissed(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_requested_incompatible_qos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus & status) {
	if (_onRequestedIncompatibleQos != NULL)
		_onRequestedIncompatibleQos(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status) {
	if (_onSampleRejected != NULL)
		_onSampleRejected(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status) {
	if (_onLivelinessChanged != NULL)
		_onLivelinessChanged(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status) {
	if (_onSubscriptionMatched != NULL)
		_onSubscriptionMatched(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_sample_lost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status) {
	if (_onSampleLost != NULL)
		_onSampleLost(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status) {
	if (_onOfferedDeadlineMissed != NULL)
		_onOfferedDeadlineMissed(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status) {
	if (_onOfferedIncompatibleQos != NULL)
		_onOfferedIncompatibleQos(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status) {
	if (_onLivelinessLost != NULL)
		_onLivelinessLost(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status) {
	if (_onPublicationMatched != NULL)
		_onPublicationMatched(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative::on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus & status) {
	if (_onInconsistentTopic != NULL)
		_onInconsistentTopic(topic, status);
};
