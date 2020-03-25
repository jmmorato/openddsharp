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

::DDS::Entity_ptr Subscriber_NarrowBase(::DDS::Subscriber_ptr sub) {
	return static_cast<::DDS::Entity_ptr>(sub);
}

::DDS::DataReader_ptr Subscriber_CreateDataReader(::DDS::Subscriber_ptr sub,
												  ::DDS::TopicDescription_ptr topicDescription,
												  DataReaderQosWrapper qos,
												  ::DDS::DataReaderListener_ptr a_listener,
												  ::DDS::StatusMask mask) {
	/*::DDS::DataReaderQos nativeQos;
	::DDS::ReturnCode_t ret = sub->get_default_datareader_qos(nativeQos);
	nativeQos.reliability.kind = DDS::ReliabilityQosPolicyKind::RELIABLE_RELIABILITY_QOS;*/
	/*char buf[2048];
	sprintf(buf, "Subscriber_CreateDataReader DEADLINE PERIOD: %d \n", qos.deadline.period.sec);
	OutputDebugString(buf);
	::DDS::DataReaderQos nativeQos = qos;

	sprintf(buf, "Subscriber_CreateDataReader NATIVE DEADLINE PERIOD: %d \n", nativeQos.deadline.period.sec);
	OutputDebugString(buf);*/

    return sub->create_datareader(topicDescription, qos, NULL, ::OpenDDS::DCPS::DEFAULT_STATUS_MASK);
}

::DDS::ReturnCode_t Subscriber_GetDefaultDataReaderQos(::DDS::Subscriber_ptr sub, DataReaderQosWrapper& qos_wrapper) {
	::DDS::DataReaderQos qos_native;
	::DDS::ReturnCode_t ret = sub->get_default_datareader_qos(qos_native);

	if (ret == ::DDS::RETCODE_OK) {
		qos_wrapper = qos_native;
	}

	return ret;
}

::DDS::ReturnCode_t Subscriber_SetDefaultDataReaderQos(::DDS::Subscriber_ptr sub, DataReaderQosWrapper qos_wrapper) {
	return sub->set_default_datareader_qos(qos_wrapper);
}

::DDS::ReturnCode_t Subscriber_GetQos(::DDS::Subscriber_ptr sub, SubscriberQosWrapper& qos_wrapper) {
	::DDS::SubscriberQos qos_native;
	::DDS::ReturnCode_t ret = sub->get_qos(qos_native);

	if (ret == ::DDS::RETCODE_OK) {
		qos_wrapper = qos_native;
	}

	return ret;
}

::DDS::ReturnCode_t Subscriber_SetQos(::DDS::Subscriber_ptr sub, SubscriberQosWrapper qos_wrapper) {
	char buf[2048];
	sprintf(buf, "Subscriber_SetQos autoenable_created_entities: %s \n", qos_wrapper.entity_factory.autoenable_created_entities ? "true" : "false");
	OutputDebugString(buf);
	return sub->set_qos(qos_wrapper);
}