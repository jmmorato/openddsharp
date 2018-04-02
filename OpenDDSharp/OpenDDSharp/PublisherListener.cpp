#include "PublisherListener.h"

::OpenDDSharp::DDS::PublisherListener::PublisherListener() : ::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::DataWriterListener() {
	impl_entity = new OpenDDSharp::DDS::PublisherListenerNative(onOfferedDeadlineMissedFunctionCpp,
																onOfferedIncompatibleQosFunctionCpp,
																onLivelinessLostFunctionCpp,
																onPublicationMatchedFunctionCpp,
																onPublicationDisconnectedFunctionCpp,
																onPublicationReconnectedFunctionCpp,
																onPublicationLostFunctionCpp,
																onConnectionDeletedFunctionCpp);
}