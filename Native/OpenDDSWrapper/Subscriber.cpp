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
	return static_cast<DDS::Entity_ptr>(sub);
}

::DDS::DataReader_ptr Subscriber_CreateDataReader(::DDS::Subscriber_ptr sub,
												  ::DDS::TopicDescription_ptr topicDescription,
												  DataReaderQosWrapper qos,
												  OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl_ptr a_listener,
												  ::DDS::StatusMask mask) {
    return sub->create_datareader(topicDescription, qos, a_listener, mask);
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
	return sub->set_qos(qos_wrapper);
}

::DDS::ReturnCode_t Subscriber_SetListener(::DDS::Subscriber_ptr sub, OpenDDSharp::OpenDDS::DDS::SubscriberListenerImpl_ptr listener, ::DDS::StatusMask mask) {
	return sub->set_listener(listener, mask);
}

::DDS::ReturnCode_t Subscriber_DeleteDataReader(::DDS::Subscriber_ptr sub, ::DDS::DataReader_ptr dataReader) {
	return sub->delete_datareader(dataReader);
}

::DDS::ReturnCode_t Subscriber_BeginAccess(::DDS::Subscriber_ptr sub) {
	return sub->begin_access();
}

::DDS::ReturnCode_t Subscriber_EndAccess(::DDS::Subscriber_ptr sub) {
	return sub->end_access();
}

::DDS::DomainParticipant_ptr Subscriber_GetParticipant(::DDS::Subscriber_ptr sub) {
	return sub->get_participant();
}

::DDS::Entity_ptr Subscriber_LookupDataReader(::DDS::Subscriber_ptr sub, char* topicName) {
	return static_cast<DDS::Entity_ptr>(sub->lookup_datareader(topicName));
}

::DDS::ReturnCode_t Subscriber_DeleteContainedEntities(::DDS::Subscriber_ptr sub) {
	return sub->delete_contained_entities();
}

::DDS::ReturnCode_t Subscriber_NotifyDataReaders(::DDS::Subscriber_ptr sub) {
	return sub->notify_datareaders();
}

::DDS::ReturnCode_t Subscriber_GetDataReaders(::DDS::Subscriber_ptr sub, void*& lst, ::DDS::SampleStateMask sampleState, ::DDS::ViewStateMask viewState, ::DDS::InstanceStateMask instanceState) {
	::DDS::DataReaderSeq seq;
	::DDS::ReturnCode_t ret = sub->get_datareaders(seq, sampleState, viewState, instanceState);
	if (ret == ::DDS::RETCODE_OK) {
		CORBA::ULong length = seq.length();
		TAO::unbounded_value_sequence<DDS::DataReader_ptr> readers(length);
		readers.length(length);
		for (CORBA::ULong i = 0; i < length; i++) {
			readers[i] = seq[i].in();
		}

		unbounded_sequence_to_ptr(readers, lst);
	}

	return ret;
}