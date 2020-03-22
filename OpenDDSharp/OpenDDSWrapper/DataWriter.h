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

EXTERN_METHOD_EXPORT
::DDS::Entity_ptr DataWriter_NarrowBase(::DDS::DataWriter_ptr dp);

EXTERN_METHOD_EXPORT 
::DDS::ReturnCode_t DataWriter_WaitForAcknowledgments(::DDS::DataWriter_ptr dw, ::DDS::Duration_t max_wait);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataWriter_GetPublicationMatchedStatus(::DDS::DataWriter_ptr dw, ::DDS::PublicationMatchedStatus_out status);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataWriter_GetQos(::DDS::DataWriter_ptr dw, DataWriterQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DataWriter_SetQos(::DDS::DataWriter_ptr dw, DataWriterQosWrapper qos_wrapper);

