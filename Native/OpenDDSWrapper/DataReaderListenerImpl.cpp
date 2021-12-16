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
#include "DataReaderListenerImpl.h"

::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::DataReaderListenerImpl(std::function<void(::DDS::Entity_ptr)> onDataAvalaible,
																			std::function<void(::DDS::Entity_ptr, ::DDS::RequestedDeadlineMissedStatus)> onRequestedDeadlineMissed,
																			std::function<void(::DDS::Entity_ptr, ::DDS::RequestedIncompatibleQosStatus)> onRequestedIncompatibleQos,
																			std::function<void(::DDS::Entity_ptr, ::DDS::SampleRejectedStatus)> onSampleRejected,
																			std::function<void(::DDS::Entity_ptr, ::DDS::LivelinessChangedStatus)> onLivelinessChanged,
																			std::function<void(::DDS::Entity_ptr, ::DDS::SubscriptionMatchedStatus)> onSubscriptionMatched,
																			std::function<void(::DDS::Entity_ptr, ::DDS::SampleLostStatus)> onSampleLost) {	
	_onDataAvalaible = onDataAvalaible;
	_onRequestedDeadlineMissed = onRequestedDeadlineMissed;
	_onRequestedIncompatibleQos = onRequestedIncompatibleQos;
	_onSampleRejected = onSampleRejected;
	_onLivelinessChanged = onLivelinessChanged;
	_onSubscriptionMatched = onSubscriptionMatched;
	_onSampleLost = onSampleLost;
}

::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::~DataReaderListenerImpl() {	
	_onDataAvalaible = NULL;
	_onRequestedDeadlineMissed = NULL;
	_onRequestedIncompatibleQos = NULL;
	_onSampleRejected = NULL;
	_onLivelinessChanged = NULL;
	_onSubscriptionMatched = NULL;
	_onSampleLost = NULL;
};

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_data_available(::DDS::DataReader_ptr reader) {
	if (_onDataAvalaible != NULL) {
		_onDataAvalaible(static_cast< ::DDS::Entity_ptr>(reader));
	}
};

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_requested_deadline_missed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status) {
	if (_onRequestedDeadlineMissed != NULL) {
		_onRequestedDeadlineMissed(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_requested_incompatible_qos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status) {
	if (_onRequestedIncompatibleQos != NULL) {
		_onRequestedIncompatibleQos(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status) {
	if (_onSampleRejected != NULL) {
		_onSampleRejected(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status) {
	if (_onLivelinessChanged != NULL) {
		_onLivelinessChanged(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status) {
	if (_onSubscriptionMatched != NULL) {
		_onSubscriptionMatched(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_sample_lost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status) {
	if (_onSampleLost != NULL) {
		_onSampleLost(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}
