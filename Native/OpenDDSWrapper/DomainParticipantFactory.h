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
#include "DomainParticipantListenerImpl.h"

#include "dds/DCPS/Marked_Default_Qos.h"

EXTERN_METHOD_EXPORT
DDS::DomainParticipant_ptr DomainParticipantFactory_CreateParticipant(::DDS::DomainParticipantFactory_ptr dpf, 
                                                                      ::DDS::DomainId_t domainId,
                                                                      DomainParticipantQosWrapper qos,
                                                                      OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl_ptr a_listener,
                                                                      ::DDS::StatusMask mask);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipantFactory_DeleteParticipant(::DDS::DomainParticipantFactory_ptr dpf, ::DDS::DomainParticipant_ptr dp);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipantFactory_GetDefaultDomainParticipantQos(::DDS::DomainParticipantFactory_ptr pub, DomainParticipantQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipantFactory_SetDefaultDomainParticipantQos(::DDS::DomainParticipantFactory_ptr pub, DomainParticipantQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipantFactory_GetQos(::DDS::DomainParticipantFactory_ptr dpf, DomainParticipantFactoryQosWrapper& qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t DomainParticipantFactory_SetQos(::DDS::DomainParticipantFactory_ptr dpf, DomainParticipantFactoryQosWrapper qos_wrapper);

EXTERN_METHOD_EXPORT
::DDS::Entity_ptr DomainParticipantFactory_LookupParticipant(::DDS::DomainParticipantFactory_ptr dpf, ::DDS::DomainId_t domainId);
