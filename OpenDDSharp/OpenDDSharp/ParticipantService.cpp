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
#include "ParticipantService.h"

OpenDDSharp::OpenDDS::DCPS::ParticipantService^ OpenDDSharp::OpenDDS::DCPS::ParticipantService::Instance::get() {
	return %_instance;
}

OpenDDSharp::OpenDDS::DCPS::ParticipantService::ParticipantService() {
	ACE::init();

	impl_entity = ::OpenDDS::DCPS::Service_Participant::instance();
};

System::String^ OpenDDSharp::OpenDDS::DCPS::ParticipantService::DefaultDiscovery::get() {
    msclr::interop::marshal_context context;

    return context.marshal_as<System::String^>(impl_entity->get_default_discovery().c_str());
}

void OpenDDSharp::OpenDDS::DCPS::ParticipantService::DefaultDiscovery::set(System::String^ value) {
    msclr::interop::marshal_context context;

    impl_entity->set_default_discovery(context.marshal_as<const char *>(value)) ;
}

OpenDDSharp::DDS::DomainParticipantFactory^  OpenDDSharp::OpenDDS::DCPS::ParticipantService::GetDomainParticipantFactory() {
	::DDS::DomainParticipantFactory_ptr factory = impl_entity->get_domain_participant_factory();
	
	return gcnew OpenDDSharp::DDS::DomainParticipantFactory(factory);
};

OpenDDSharp::DDS::DomainParticipantFactory^  OpenDDSharp::OpenDDS::DCPS::ParticipantService::GetDomainParticipantFactory(...array<System::String^>^ args) {
	msclr::interop::marshal_context context;
	int argc = args->Length + 1;
	char **argv = new char *[argc];

	// don't need the program name (can't be NULL though, else ACE_Arg_Shifter fails)
	argv[0] = "";  
	for (int i = 0; i < args->Length; i++)
		argv[i + 1] = _strdup(context.marshal_as<const char*>(args[i]));
	
	::DDS::DomainParticipantFactory_ptr factory = impl_entity->get_domain_participant_factory(argc, argv);
	
	return gcnew OpenDDSharp::DDS::DomainParticipantFactory(factory);
};

void OpenDDSharp::OpenDDS::DCPS::ParticipantService::AddDiscovery(::OpenDDSharp::OpenDDS::DCPS::Discovery^ discovery) {
    ::OpenDDS::DCPS::Discovery_rch disc = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::Discovery>(discovery->impl_entity);
    impl_entity->add_discovery(disc);
}

void OpenDDSharp::OpenDDS::DCPS::ParticipantService::Shutdown() {
	impl_entity->shutdown();
	
	ACE::fini();
};
