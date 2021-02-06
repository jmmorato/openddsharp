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
#include "DataWriterListenerNative.h"

::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::DataWriterListenerNative(std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> onOfferedDeadlineMissed,
																			     std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> onOfferedIncompatibleQos,
																				 std::function<void(::DDS::DataWriter_ptr writer, ::DDS::LivelinessLostStatus status)> onLivelinessLost,
																				 std::function<void(::DDS::DataWriter_ptr writer, ::DDS::PublicationMatchedStatus status)> onPublicationMatched,
																				 std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationDisconnectedStatus)> onPublicationDisconnected,
																				 std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationReconnectedStatus)> onPublicationReconnected,
																				 std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationLostStatus)> onPublicationLost) {
	_onOfferedDeadlineMissed = onOfferedDeadlineMissed;
	_onOfferedIncompatibleQos = onOfferedIncompatibleQos;
	_onLivelinessLost = onLivelinessLost;
	_onPublicationMatched = onPublicationMatched;
	_onPublicationDisconnected = onPublicationDisconnected;
	_onPublicationReconnected = onPublicationReconnected;
	_onPublicationLost = onPublicationLost;	
};


::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::~DataWriterListenerNative() {
	_onOfferedDeadlineMissed = NULL;
	_onOfferedIncompatibleQos = NULL;
	_onLivelinessLost = NULL;
	_onPublicationMatched = NULL;
	_onPublicationDisconnected = NULL;
	_onPublicationReconnected = NULL;
	_onPublicationLost = NULL;
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status) {
	if (_onOfferedDeadlineMissed != NULL)
		_onOfferedDeadlineMissed(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status) {
	if (_onOfferedIncompatibleQos != NULL)
		_onOfferedIncompatibleQos(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status) {
	if (_onLivelinessLost != NULL)
		_onLivelinessLost(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status) {
	if (_onPublicationMatched != NULL)
		_onPublicationMatched(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_publication_disconnected(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationDisconnectedStatus & status) {
	if (_onPublicationDisconnected != NULL)
		_onPublicationDisconnected(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_publication_reconnected(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationReconnectedStatus & status) {
	if (_onPublicationReconnected != NULL)
		_onPublicationReconnected(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_publication_lost(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationLostStatus & status) {
	if (_onPublicationLost != NULL)
		_onPublicationLost(writer, status);
};