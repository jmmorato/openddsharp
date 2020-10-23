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
	return static_cast<::DDS::Entity_ptr>(dw);
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
