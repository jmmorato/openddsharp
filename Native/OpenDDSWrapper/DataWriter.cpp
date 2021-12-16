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
#include "DataWriter.h"

::DDS::Entity_ptr DataWriter_NarrowBase(::DDS::DataWriter_ptr dw) {
	return static_cast<::DDS::Entity_ptr>(dw);
}

::DDS::ReturnCode_t DataWriter_WaitForAcknowledgments(::DDS::DataWriter_ptr dw, ::DDS::Duration_t max_wait) {
	return dw->wait_for_acknowledgments(max_wait);
};

::DDS::ReturnCode_t DataWriter_GetPublicationMatchedStatus(::DDS::DataWriter_ptr dw, ::DDS::PublicationMatchedStatus_out status) {
	return dw->get_publication_matched_status(status);
};

::DDS::ReturnCode_t DataWriter_GetQos(::DDS::DataWriter_ptr dw, DataWriterQosWrapper& qos_wrapper) {
	::DDS::DataWriterQos qos_native;
	::DDS::ReturnCode_t ret = dw->get_qos(qos_native);

	if (ret == ::DDS::RETCODE_OK) {
		qos_wrapper = qos_native;
	}

	return ret;
}

::DDS::ReturnCode_t DataWriter_SetQos(::DDS::DataWriter_ptr dw, DataWriterQosWrapper qos_wrapper) {
	return dw->set_qos(qos_wrapper);
}

::DDS::ReturnCode_t DataWriter_AssertLiveliness(::DDS::DataWriter_ptr dw) {
	return dw->assert_liveliness();
}

::DDS::ReturnCode_t DataWriter_SetListener(::DDS::DataWriter_ptr dw, OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl_ptr listener, ::DDS::StatusMask mask) {
	return dw->set_listener(listener, mask);
}

::DDS::Publisher_ptr DataWriter_GetPublisher(::DDS::DataWriter_ptr dw) {
	return dw->get_publisher();
}

::DDS::Topic_ptr DataWriter_GetTopic(::DDS::DataWriter_ptr dw) {
	return dw->get_topic();
}

::DDS::ReturnCode_t DataWriter_GetLivelinessLostStatus(::DDS::DataWriter_ptr dw, ::DDS::LivelinessLostStatus_out status) {
	return dw->get_liveliness_lost_status(status);
}

::DDS::ReturnCode_t DataWriter_GetOfferedDeadlineMissedStatus(::DDS::DataWriter_ptr dw, ::DDS::OfferedDeadlineMissedStatus_out status) {
	return dw->get_offered_deadline_missed_status(status);
}

::DDS::ReturnCode_t DataWriter_GetOfferedIncompatibleQosStatus(::DDS::DataWriter_ptr dw, OfferedIncompatibleQosStatusWrapper& status) {
	::DDS::OfferedIncompatibleQosStatus s;
	::DDS::ReturnCode_t ret = dw->get_offered_incompatible_qos_status(s);

	if (ret == ::DDS::RETCODE_OK) {
		status = s;
	}

	return ret;
}

::DDS::ReturnCode_t DataWriter_GetMatchedSubscriptions(::DDS::DataWriter_ptr dw, void*& ptr) {
	::DDS::InstanceHandleSeq seq;
	::DDS::ReturnCode_t ret = dw->get_matched_subscriptions(seq);

	if (ret == ::DDS::RETCODE_OK) {
		unbounded_sequence_to_ptr(seq, ptr);
	}

	return ret;
}

::DDS::ReturnCode_t DataWriter_GetMatchedSubscriptionData(::DDS::DataWriter_ptr dw, SubscriptionBuiltinTopicDataWrapper& data, ::DDS::InstanceHandle_t handle) {
	::DDS::SubscriptionBuiltinTopicData d;
	DDS::ReturnCode_t ret = dw->get_matched_subscription_data(d, handle);

	if (ret == ::DDS::RETCODE_OK) {
		data = d;
	}

	return ret;
}