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
#include "DataWriter.h"

::DDS::ReturnCode_t DataWriter_WaitForAcknowledgments(::DDS::DataWriter_ptr dw, ::DDS::Duration_t max_wait) {	
	/*char buf_seconds[2048];
	char buf_nanoseconds[2048];
	sprintf(buf_seconds, "DURATION SECONDS %d \n", maxWait.sec);
	sprintf(buf_nanoseconds, "DURATION NANOSECONDS %d \n", maxWait.nanosec);
	OutputDebugStringA(buf_seconds);
	OutputDebugStringA(buf_nanoseconds);*/

	return dw->wait_for_acknowledgments(max_wait);
};

::DDS::ReturnCode_t DataWriter_GetPublicationMatchedStatus(::DDS::DataWriter_ptr dw, ::DDS::PublicationMatchedStatus_out status) {
	return dw->get_publication_matched_status(status);
};