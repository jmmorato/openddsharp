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
	/*char buf_seconds[2048];
	char buf_nanoseconds[2048];
	sprintf(buf_seconds, "DURATION SECONDS %d \n", max_wait.sec);
	sprintf(buf_nanoseconds, "DURATION NANOSECONDS %d \n", max_wait.nanosec);
	OutputDebugStringA(buf_seconds);
	OutputDebugStringA(buf_nanoseconds);*/

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