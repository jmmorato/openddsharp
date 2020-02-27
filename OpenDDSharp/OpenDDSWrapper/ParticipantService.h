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
#include <dds/DCPS/Service_Participant.h>

//EXTERN_METHOD_EXPORT 
//void ParticipantService_new();

EXTERN_METHOD_EXPORT
::DDS::DomainParticipantFactory_ptr ParticipantService_GetDomainParticipantFactory();

EXTERN_METHOD_EXPORT
::DDS::DomainParticipantFactory_ptr ParticipantService_GetDomainParticipantFactoryParameters(int argc, char *argv[]);

EXTERN_METHOD_EXPORT
void ParticipantService_AddDiscovery(::OpenDDS::DCPS::Discovery* discovery);

EXTERN_METHOD_EXPORT
char * ParticipantService_GetDefaultDiscovery();

EXTERN_METHOD_EXPORT
void ParticipantService_SetDefaultDiscovery(char * defaultDiscovery);

EXTERN_METHOD_EXPORT
void ParticipantService_SetRepoDomain(int domain, char * repo, bool attach_participant);

EXTERN_METHOD_EXPORT
void ParticipantService_Shutdown();

EXTERN_METHOD_EXPORT
bool ParticipantService_GetIsShutdown();