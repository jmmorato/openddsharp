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
#include "marshal.h"
#include "QosPolicies.h"
#include "DataReaderListenerImpl.h"
#include "Statuses.h"
#include "BuiltinTopicData.h"

EXTERN_METHOD_EXPORT
::DDS::Entity_ptr DataReader_NarrowBase(::DDS::DataReader_ptr dp);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_GetMatchedPublications(::DDS::DataReader_ptr dr, void* & ptr);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_WaitForHistoricalData(::DDS::DataReader_ptr dr, ::DDS::Duration_t max_wait);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_GetQos(::DDS::DataReader_ptr dr, DataReaderQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_SetQos(::DDS::DataReader_ptr dr, DataReaderQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_SetListener(::DDS::DataReader_ptr dr, OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl_ptr listener, ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::Subscriber_ptr DataReader_GetSubscriber(::DDS::DataReader_ptr dr);

EXTERN_METHOD_EXPORT
::DDS::TopicDescription_ptr DataReader_GetTopicDescription(::DDS::DataReader_ptr dr);

EXTERN_METHOD_EXPORT
::DDS::ReadCondition_ptr DataReader_CreateReadCondition(::DDS::DataReader_ptr dr, ::DDS::SampleStateMask sampleMask, ::DDS::ViewStateMask viewMask, ::DDS::InstanceStateMask instanceMask);

EXTERN_METHOD_EXPORT
::DDS::QueryCondition_ptr DataReader_CreateQueryCondition(::DDS::DataReader_ptr dr, ::DDS::SampleStateMask sampleMask, ::DDS::ViewStateMask viewMask, ::DDS::InstanceStateMask instanceMask, char* expr, void* parameters);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_DeleteReadCondition(::DDS::DataReader_ptr dr, ::DDS::ReadCondition_ptr rc);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_DeleteContainedEntities(::DDS::DataReader_ptr dr);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_GetSampleRejectedStatus(::DDS::DataReader_ptr dr, ::DDS::SampleRejectedStatus_out status);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_GetLivelinessChangedStatus(::DDS::DataReader_ptr dr, ::DDS::LivelinessChangedStatus_out status);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_GetRequestedDeadlineMissedStatus(::DDS::DataReader_ptr dr, ::DDS::RequestedDeadlineMissedStatus_out status);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_GetRequestedIncompatibleQosStatus(::DDS::DataReader_ptr dr, RequestedIncompatibleQosStatusWrapper& status);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_GetSubscriptionMatchedStatus(::DDS::DataReader_ptr dr, ::DDS::SubscriptionMatchedStatus_out status);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_GetSampleLostStatus(::DDS::DataReader_ptr dr, ::DDS::SampleLostStatus_out status);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataReader_GetMatchedPublicationData(::DDS::DataReader_ptr dr, PublicationBuiltinTopicDataWrapper& data, ::DDS::InstanceHandle_t handle);