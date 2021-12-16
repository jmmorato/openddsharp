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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "DataReader.h"

::DDS::Entity_ptr DataReader_NarrowBase(::DDS::DataReader_ptr dw) {
	return static_cast<DDS::Entity_ptr>(dw);
}

::DDS::ReturnCode_t DataReader_GetMatchedPublications(::DDS::DataReader_ptr dr, void* & ptr) {
	::DDS::InstanceHandleSeq publication_handles;
	::DDS::ReturnCode_t ret = dr->get_matched_publications(publication_handles);

	if (ret == ::DDS::RETCODE_OK) {
		unbounded_sequence_to_ptr(publication_handles, ptr);
	}

	return ret;
}

::DDS::ReturnCode_t DataReader_WaitForHistoricalData(::DDS::DataReader_ptr dr, ::DDS::Duration_t max_wait) {
	return dr->wait_for_historical_data(max_wait);
}

::DDS::ReturnCode_t DataReader_GetQos(::DDS::DataReader_ptr dr, DataReaderQosWrapper& qos_wrapper) {
	::DDS::DataReaderQos qos_native;
	::DDS::ReturnCode_t ret = dr->get_qos(qos_native);

	if (ret == ::DDS::RETCODE_OK) {
		qos_wrapper = qos_native;
	}

	return ret;
}

::DDS::ReturnCode_t DataReader_SetQos(::DDS::DataReader_ptr dr, DataReaderQosWrapper qos_wrapper) {
	return dr->set_qos(qos_wrapper);
}

::DDS::ReturnCode_t DataReader_SetListener(::DDS::DataReader_ptr dr, OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl_ptr listener, ::DDS::StatusMask mask) {
	return dr->set_listener(listener, mask);
}

::DDS::Subscriber_ptr DataReader_GetSubscriber(::DDS::DataReader_ptr dr) {
	return dr->get_subscriber();
}

::DDS::TopicDescription_ptr DataReader_GetTopicDescription(::DDS::DataReader_ptr dr) {
	return dr->get_topicdescription();
}

::DDS::ReadCondition_ptr DataReader_CreateReadCondition(::DDS::DataReader_ptr dr, ::DDS::SampleStateMask sampleMask, ::DDS::ViewStateMask viewMask, ::DDS::InstanceStateMask instanceMask) {
	return dr->create_readcondition(sampleMask, viewMask, instanceMask);
}

::DDS::QueryCondition_ptr DataReader_CreateQueryCondition(::DDS::DataReader_ptr dr, ::DDS::SampleStateMask sampleMask, ::DDS::ViewStateMask viewMask, ::DDS::InstanceStateMask instanceMask, char* expr, void* parameters) {
	::DDS::StringSeq seq;
	ptr_to_unbounded_basic_string_sequence(parameters, seq);

	return dr->create_querycondition(sampleMask, viewMask, instanceMask, expr, seq);
}

::DDS::ReturnCode_t DataReader_DeleteReadCondition(::DDS::DataReader_ptr dr, ::DDS::ReadCondition_ptr rc) {
	return dr->delete_readcondition(rc);
}

::DDS::ReturnCode_t DataReader_DeleteContainedEntities(::DDS::DataReader_ptr dr) {
	return dr->delete_contained_entities();
}

::DDS::ReturnCode_t DataReader_GetSampleRejectedStatus(::DDS::DataReader_ptr dr, ::DDS::SampleRejectedStatus_out status) {
	return dr->get_sample_rejected_status(status);
}

::DDS::ReturnCode_t DataReader_GetLivelinessChangedStatus(::DDS::DataReader_ptr dr, ::DDS::LivelinessChangedStatus_out status) {
	return dr->get_liveliness_changed_status(status);
}

::DDS::ReturnCode_t DataReader_GetRequestedDeadlineMissedStatus(::DDS::DataReader_ptr dr, ::DDS::RequestedDeadlineMissedStatus_out status) {
	return dr->get_requested_deadline_missed_status(status);
}

::DDS::ReturnCode_t DataReader_GetRequestedIncompatibleQosStatus(::DDS::DataReader_ptr dr, RequestedIncompatibleQosStatusWrapper& status) {
	::DDS::RequestedIncompatibleQosStatus s;
	::DDS::ReturnCode_t ret = dr->get_requested_incompatible_qos_status(s);

	if (ret == ::DDS::RETCODE_OK) {
		status = s;
	}

	return ret;
}

::DDS::ReturnCode_t DataReader_GetSubscriptionMatchedStatus(::DDS::DataReader_ptr dr, ::DDS::SubscriptionMatchedStatus_out status) {
	return dr->get_subscription_matched_status(status);
}

::DDS::ReturnCode_t DataReader_GetSampleLostStatus(::DDS::DataReader_ptr dr, ::DDS::SampleLostStatus_out status) {
	return dr->get_sample_lost_status(status);
}

::DDS::ReturnCode_t DataReader_GetMatchedPublicationData(::DDS::DataReader_ptr dr, PublicationBuiltinTopicDataWrapper& data, ::DDS::InstanceHandle_t handle) {
	::DDS::PublicationBuiltinTopicData d;
	DDS::ReturnCode_t ret = dr->get_matched_publication_data(d, handle);

	if (ret == ::DDS::RETCODE_OK) {
		data = d;
	}

	return ret;
}