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
#pragma once

#include "Utils.h"
#include "QosPolicies.h"
#include "SubscriberListenerImpl.h"
#include "DataReaderListenerImpl.h"
#include "marshal.h"

#pragma warning(push, 0)

#include "dds/DCPS/Marked_Default_Qos.h"

#pragma warning(pop)

EXTERN_METHOD_EXPORT
::DDS::Entity_ptr Subscriber_NarrowBase(::DDS::Subscriber_ptr sub);

EXTERN_METHOD_EXPORT
::DDS::DataReader_ptr Subscriber_CreateDataReader(::DDS::Subscriber_ptr sub,
                                                  ::DDS::TopicDescription_ptr topicDescription,
                                                  DataReaderQosWrapper qos,
                                                  OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl_ptr a_listener,
                                                  ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Subscriber_GetDefaultDataReaderQos(::DDS::Subscriber_ptr sub, DataReaderQosWrapper &qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Subscriber_SetDefaultDataReaderQos(::DDS::Subscriber_ptr sub, DataReaderQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Subscriber_GetQos(::DDS::Subscriber_ptr sub, SubscriberQosWrapper &qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Subscriber_SetQos(::DDS::Subscriber_ptr sub, SubscriberQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t
Subscriber_SetListener(::DDS::Subscriber_ptr sub, OpenDDSharp::OpenDDS::DDS::SubscriberListenerImpl_ptr listener,
                       ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Subscriber_DeleteDataReader(::DDS::Subscriber_ptr sub, ::DDS::DataReader_ptr dataReader);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Subscriber_BeginAccess(::DDS::Subscriber_ptr sub);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Subscriber_EndAccess(::DDS::Subscriber_ptr sub);

EXTERN_METHOD_EXPORT
::DDS::DomainParticipant_ptr Subscriber_GetParticipant(::DDS::Subscriber_ptr sub);

EXTERN_METHOD_EXPORT
::DDS::DataReader_ptr Subscriber_LookupDataReader(::DDS::Subscriber_ptr sub, char *topicName);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Subscriber_DeleteContainedEntities(::DDS::Subscriber_ptr sub);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Subscriber_NotifyDataReaders(::DDS::Subscriber_ptr sub);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Subscriber_GetDataReaders(::DDS::Subscriber_ptr sub, void *&lst, ::DDS::SampleStateMask sampleState,
                                              ::DDS::ViewStateMask viewState, ::DDS::InstanceStateMask instanceState);

