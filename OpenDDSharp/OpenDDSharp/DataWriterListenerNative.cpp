#include "DataWriterListenerNative.h"

::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::DataWriterListenerNative(std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> onOfferedDeadlineMissed,
																			     std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> onOfferedIncompatibleQos,
																				 std::function<void(::DDS::DataWriter_ptr writer, ::DDS::LivelinessLostStatus status)> onLivelinessLost,
																				 std::function<void(::DDS::DataWriter_ptr writer, ::DDS::PublicationMatchedStatus status)> onPublicationMatched,
																				 std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationDisconnectedStatus)> onPublicationDisconnected,
																				 std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationReconnectedStatus)> onPublicationReconnected,
																				 std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationLostStatus)> onPublicationLost,
																				 std::function<void(::DDS::DataWriter_ptr)> onConnectionDeleted) {
	_onOfferedDeadlineMissed = onOfferedDeadlineMissed;
	_onOfferedIncompatibleQos = onOfferedIncompatibleQos;
	_onLivelinessLost = onLivelinessLost;
	_onPublicationMatched = onPublicationMatched;
	_onPublicationDisconnected = onPublicationDisconnected;
	_onPublicationReconnected = onPublicationReconnected;
	_onPublicationLost = onPublicationLost;
	_onConnectionDeleted = onConnectionDeleted;
};


::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::~DataWriterListenerNative() {
	_onOfferedDeadlineMissed = nullptr;
	_onOfferedIncompatibleQos = nullptr;
	_onLivelinessLost = nullptr;
	_onPublicationMatched = nullptr;
	_onPublicationDisconnected = nullptr;
	_onPublicationReconnected = nullptr;
	_onPublicationLost = nullptr;
	_onConnectionDeleted = nullptr;
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status) {
	if (_onOfferedDeadlineMissed != nullptr)
		_onOfferedDeadlineMissed(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status) {
	if (_onOfferedIncompatibleQos != nullptr)
		_onOfferedIncompatibleQos(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status) {
	if (_onLivelinessLost != nullptr)
		_onLivelinessLost(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status) {
	if (_onPublicationMatched != nullptr)
		_onPublicationMatched(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_publication_disconnected(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationDisconnectedStatus & status) {
	if (_onPublicationDisconnected != nullptr)
		_onPublicationDisconnected(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_publication_reconnected(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationReconnectedStatus & status) {
	if (_onPublicationReconnected != nullptr)
		_onPublicationReconnected(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_publication_lost(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationLostStatus & status) {
	if (_onPublicationLost != nullptr)
		_onPublicationLost(writer, status);
};

void ::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative::on_connection_deleted(::DDS::DataWriter_ptr reader) {
	if (_onConnectionDeleted != nullptr)
		_onConnectionDeleted(reader);
}