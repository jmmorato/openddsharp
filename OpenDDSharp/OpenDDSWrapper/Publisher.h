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
#pragma once

#include "Utils.h"
#include "QosPolicies.h"
#include "dds\DCPS\Marked_Default_Qos.h"
#include "PublisherListenerImpl.h"

EXTERN_METHOD_EXPORT
::DDS::Entity_ptr Publisher_NarrowBase(::DDS::Publisher_ptr pub);

EXTERN_METHOD_EXPORT 
::DDS::DataWriter_ptr Publisher_CreateDataWriter(::DDS::Publisher_ptr pub,
												 ::DDS::Topic_ptr topic,
												 DataWriterQosWrapper qos,
												 ::DDS::DataWriterListener_ptr a_listener,
												 ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_GetQos(::DDS::Publisher_ptr pub, PublisherQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_SetQos(::DDS::Publisher_ptr pub, PublisherQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_GetDefaultDataWriterQos(::DDS::Publisher_ptr pub, DataWriterQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_SetDefaultDataWriterQos(::DDS::Publisher_ptr pub, DataWriterQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_DeleteDataWriter(::DDS::Publisher_ptr pub, ::DDS::DataWriter_ptr dw);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t Publisher_SetListener(::DDS::Publisher_ptr pub, OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl_ptr listener, ::DDS::StatusMask mask);
