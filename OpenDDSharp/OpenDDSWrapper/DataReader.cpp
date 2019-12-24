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
#include "marshal.h"

::DDS::ReturnCode_t DataReader_GetMatchedPublications(::DDS::DataReader_ptr dr, void* & ptr) {
	::DDS::InstanceHandleSeq publication_handles;
	::DDS::ReturnCode_t ret = dr->get_matched_publications(publication_handles);

	if (ret == ::DDS::RETCODE_OK) {
		/*char buf[2048];
		sprintf(buf, "PUBLICATION HANDLES %d \n", publication_handles.length());
		OutputDebugStringA(buf);*/

		unbounded_sequence_to_ptr(publication_handles, ptr);
	}

	return ret;
}
