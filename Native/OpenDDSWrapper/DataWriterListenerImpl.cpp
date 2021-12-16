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
#include "DataWriterListenerImpl.h"

::OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl::DataWriterListenerImpl(std::function<void(::DDS::Entity_ptr, ::DDS::OfferedDeadlineMissedStatus status)> onOfferedDeadlineMissed,
																		    std::function<void(::DDS::Entity_ptr, ::DDS::OfferedIncompatibleQosStatus status)> onOfferedIncompatibleQos,
																		    std::function<void(::DDS::Entity_ptr, ::DDS::LivelinessLostStatus status)> onLivelinessLost,
																		    std::function<void(::DDS::Entity_ptr, ::DDS::PublicationMatchedStatus status)> onPublicationMatched) {
	_onOfferedDeadlineMissed = onOfferedDeadlineMissed;
	_onOfferedIncompatibleQos = onOfferedIncompatibleQos;
	_onLivelinessLost = onLivelinessLost;
	_onPublicationMatched = onPublicationMatched;
}

::OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl::~DataWriterListenerImpl() {
	_onOfferedDeadlineMissed = NULL;
	_onOfferedIncompatibleQos = NULL;
	_onLivelinessLost = NULL;
	_onPublicationMatched = NULL;
};

void ::OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl::on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status) {
	if (_onOfferedDeadlineMissed) {
		_onOfferedDeadlineMissed(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};

void ::OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl::on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus& status) {
	if (_onOfferedIncompatibleQos) {
		_onOfferedIncompatibleQos(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};

void ::OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl::on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus& status) {
	if (_onLivelinessLost) {
		_onLivelinessLost(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};

void ::OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl::on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus& status) {
	if (_onPublicationMatched) {
		_onPublicationMatched(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};
