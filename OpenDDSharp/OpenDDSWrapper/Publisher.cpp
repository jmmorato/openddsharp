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
#include "Publisher.h"

::DDS::Entity_ptr Publisher_NarrowBase(::DDS::Publisher_ptr p) {
	return static_cast<::DDS::Entity_ptr>(p);
}

::DDS::DataWriter_ptr Publisher_CreateDataWriter(::DDS::Publisher_ptr pub,
												 ::DDS::Topic_ptr topic,
												 DataWriterQosWrapper* qos,
												 ::DDS::DataWriterListener_ptr a_listener,
												 ::DDS::StatusMask mask) {
	::DDS::DataWriterQos nativeQos;
	::DDS::ReturnCode_t ret = pub->get_default_datawriter_qos(nativeQos);
	nativeQos.reliability.kind = DDS::ReliabilityQosPolicyKind::RELIABLE_RELIABILITY_QOS;

    return pub->create_datawriter(topic, nativeQos, NULL, ::OpenDDS::DCPS::DEFAULT_STATUS_MASK); //DATAWRITER_QOS_DEFAULT
}