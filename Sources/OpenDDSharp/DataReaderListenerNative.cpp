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
#include "DataReaderListenerNative.h"

::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::DataReaderListenerNative(std::function<void(::DDS::DataReader_ptr)> onDataAvalaible,
																		         std::function<void(::DDS::DataReader_ptr, ::DDS::RequestedDeadlineMissedStatus)> onRequestedDeadlineMissed,
																		         std::function<void(::DDS::DataReader_ptr, ::DDS::RequestedIncompatibleQosStatus)> onRequestedIncompatibleQos,
																				 std::function<void(::DDS::DataReader_ptr, ::DDS::SampleRejectedStatus)> onSampleRejected,
																				 std::function<void(::DDS::DataReader_ptr, ::DDS::LivelinessChangedStatus)> onLivelinessChanged,
																				 std::function<void(::DDS::DataReader_ptr, ::DDS::SubscriptionMatchedStatus)> onSubscriptionMatched,
																				 std::function<void(::DDS::DataReader_ptr, ::DDS::SampleLostStatus)> onSampleLost,
																				 std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionDisconnectedStatus)> onSubscriptionDisconnected,
																				 std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionReconnectedStatus)> onSubscriptionReconnected,
																				 std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionLostStatus)> onSubscriptionLost,
																				 std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::BudgetExceededStatus)> onBudgetExceeded) {
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
}

::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::~DataReaderListenerNative() {
	_onDataAvalaible = NULL;
	_onRequestedDeadlineMissed = NULL;
	_onRequestedIncompatibleQos = NULL;
	_onSampleRejected = NULL;
	_onLivelinessChanged = NULL;
	_onSubscriptionMatched = NULL;
	_onSampleLost = NULL;
	_onSubscriptionDisconnected = NULL;
	_onSubscriptionReconnected = NULL;
	_onSubscriptionLost = NULL;
	_onBudgetExceeded = NULL;
};

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_data_available(::DDS::DataReader_ptr reader) {
	if (_onDataAvalaible != NULL)
		_onDataAvalaible(reader);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_requested_deadline_missed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status) {
	if (_onRequestedDeadlineMissed != NULL)
		_onRequestedDeadlineMissed(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_requested_incompatible_qos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus & status) {
	if (_onRequestedIncompatibleQos != NULL)
		_onRequestedIncompatibleQos(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status) {
	if (_onSampleRejected != NULL)
		_onSampleRejected(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status) {
	if (_onLivelinessChanged != NULL)
		_onLivelinessChanged(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status) {
	if (_onSubscriptionMatched != NULL)
		_onSubscriptionMatched(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_sample_lost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status) {
	if (_onSampleLost != NULL)
		_onSampleLost(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_subscription_disconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionDisconnectedStatus& status) {
	if (_onSubscriptionDisconnected != NULL)
		_onSubscriptionDisconnected(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_subscription_reconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionReconnectedStatus& status) {
	if (_onSubscriptionReconnected != NULL)
		_onSubscriptionReconnected(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_subscription_lost(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionLostStatus& status) {
	if (_onSubscriptionLost != NULL)
		_onSubscriptionLost(reader, status);
}

void ::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative::on_budget_exceeded(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::BudgetExceededStatus& status) {
	if (_onBudgetExceeded != NULL)
		_onBudgetExceeded(reader, status);
}
