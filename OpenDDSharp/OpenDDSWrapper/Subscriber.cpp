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
#include "Subscriber.h"

::DDS::Entity_ptr Subscriber_NarrowBase(::DDS::Subscriber_ptr p) {
	return static_cast<::DDS::Entity_ptr>(p);
}

::DDS::DataReader_ptr Subscriber_CreateDataReader(::DDS::Subscriber_ptr sub,
												  ::DDS::Topic_ptr topic,
												  DataReaderQosWrapper qos,
												  ::DDS::DataReaderListener_ptr a_listener,
												  ::DDS::StatusMask mask) {
	/*::DDS::DataReaderQos nativeQos;
	::DDS::ReturnCode_t ret = sub->get_default_datareader_qos(nativeQos);
	nativeQos.reliability.kind = DDS::ReliabilityQosPolicyKind::RELIABLE_RELIABILITY_QOS;*/
	char buf[2048];
	sprintf(buf, "Subscriber_CreateDataReader DEADLINE PERIOD: %d \n", qos.deadline.period.sec);
	OutputDebugString(buf);
	::DDS::DataReaderQos nativeQos = qos;

	sprintf(buf, "Subscriber_CreateDataReader NATIVE DEADLINE PERIOD: %d \n", nativeQos.deadline.period.sec);
	OutputDebugString(buf);

    return sub->create_datareader(topic, qos, NULL, ::OpenDDS::DCPS::DEFAULT_STATUS_MASK);
}