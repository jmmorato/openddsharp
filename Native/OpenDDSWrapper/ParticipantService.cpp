/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "ParticipantService.h"

::DDS::DomainParticipantFactory_ptr ParticipantService_GetDomainParticipantFactory() {
  return TheParticipantFactory;
}

::DDS::DomainParticipantFactory_ptr ParticipantService_GetDomainParticipantFactoryParameters(int argc, char *argv[]) {
  return ::DDS::DomainParticipantFactory::_duplicate(TheParticipantFactoryWithArgs(argc, argv));
}

void ParticipantService_AddDiscovery(::OpenDDS::DCPS::Discovery *discovery) {
  ::OpenDDS::DCPS::Discovery_rch disc = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::Discovery>(discovery);
  TheServiceParticipant->add_discovery(disc);
}

char *ParticipantService_GetDefaultDiscovery() {
  return CORBA::string_dup(TheServiceParticipant->get_default_discovery().c_str());
}

void ParticipantService_SetDefaultDiscovery(char *defaultDiscovery) {
  TheServiceParticipant->set_default_discovery(defaultDiscovery);
}

void ParticipantService_SetRepoDomain(int domain, char *repo, bool attach_participant) {
  TheServiceParticipant->set_repo_domain(domain, std::string(repo), attach_participant);
}

::DDS::ReturnCode_t ParticipantService_Shutdown() {
  return TheServiceParticipant->shutdown();
}

bool ParticipantService_GetIsShutdown() {
  return TheServiceParticipant->is_shut_down();
}

int ParticipantService_GetScheduler() {
  return static_cast<int>(TheServiceParticipant->scheduler());
}

void ParticipantService_Scheduler(int scheduler) {
  TheServiceParticipant->scheduler(scheduler);
}