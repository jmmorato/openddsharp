/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"

#include <dds/DCPS/Service_Participant.h>

EXTERN_METHOD_EXPORT
::DDS::DomainParticipantFactory_ptr ParticipantService_GetDomainParticipantFactory();

EXTERN_METHOD_EXPORT
::DDS::DomainParticipantFactory_ptr ParticipantService_GetDomainParticipantFactoryParameters(int argc, char *argv[]);

EXTERN_METHOD_EXPORT
void ParticipantService_AddDiscovery(::OpenDDS::DCPS::Discovery *discovery);

EXTERN_METHOD_EXPORT
char *ParticipantService_GetDefaultDiscovery();

EXTERN_METHOD_EXPORT
void ParticipantService_SetDefaultDiscovery(char *defaultDiscovery);

EXTERN_METHOD_EXPORT
void ParticipantService_SetRepoDomain(int domain, char *repo, bool attach_participant);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t ParticipantService_Shutdown();

EXTERN_METHOD_EXPORT
bool ParticipantService_GetIsShutdown();

EXTERN_METHOD_EXPORT
long ParticipantService_GetScheduler();

EXTERN_METHOD_EXPORT
void ParticipantService_Scheduler(long scheduler);