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
#include "ParticipantService.h"

//void ParticipantService_new() {
//
//}

::DDS::DomainParticipantFactory_ptr ParticipantService_GetDomainParticipantFactory() {
    return TheParticipantFactory;
}

::DDS::DomainParticipantFactory_ptr ParticipantService_GetDomainParticipantFactoryParameters(int argc, char *argv[]) {
    return ::DDS::DomainParticipantFactory::_duplicate(TheParticipantFactoryWithArgs(argc, argv));
}

void ParticipantService_AddDiscovery(::OpenDDS::DCPS::Discovery* discovery) {
	::OpenDDS::DCPS::Discovery_rch disc = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::Discovery>(discovery);
	TheServiceParticipant->add_discovery(disc);
}

char * ParticipantService_GetDefaultDiscovery() {
	return CORBA::string_dup(TheServiceParticipant->get_default_discovery().c_str());
}

void ParticipantService_SetDefaultDiscovery(char * defaultDiscovery) {
	TheServiceParticipant->set_default_discovery(defaultDiscovery);
}

void ParticipantService_SetRepoDomain(int domain, char * repo, bool attach_participant) {
	TheServiceParticipant->set_repo_domain(domain, std::string(repo), attach_participant);
}

void ParticipantService_Shutdown() {
	TheServiceParticipant->shutdown();
}

bool ParticipantService_GetIsShutdown() {
	return TheServiceParticipant->is_shut_down();
}