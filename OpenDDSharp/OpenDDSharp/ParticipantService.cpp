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

	::OpenDDS::DCPS::TransportConfig_rch config = ::OpenDDS::DCPS::TransportRegistry::instance()->create_config("openddsharp_rtps_interop");
	::OpenDDS::DCPS::TransportInst_rch inst = ::OpenDDS::DCPS::TransportRegistry::instance()->create_inst("internal_openddsharp_rtps_transport", "rtps_udp");
	::OpenDDS::DCPS::RtpsUdpInst_rch rui = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::RtpsUdpInst>(inst);
	rui->handshake_timeout_ = 1;

	config->instances_.push_back(inst);
	::OpenDDS::DCPS::TransportRegistry::instance()->global_config(config);
	::OpenDDS::RTPS::RtpsDiscovery_rch disc = ::OpenDDS::DCPS::make_rch<::OpenDDS::RTPS::RtpsDiscovery>("RtpsDiscovery");
	disc->resend_period(ACE_Time_Value(1));
	disc->sedp_multicast(true);
	impl_entity->add_discovery(::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::Discovery>(disc));
	impl_entity->set_default_discovery("RtpsDiscovery");
	::OpenDDS::DCPS::Serializer::set_use_rti_serialization(true);	
};

OpenDDSharp::DDS::DomainParticipantFactory^  OpenDDSharp::OpenDDS::DCPS::ParticipantService::GetDomainParticipantFactory() {
	::DDS::DomainParticipantFactory_ptr factory = impl_entity->get_domain_participant_factory();
	
	return gcnew OpenDDSharp::DDS::DomainParticipantFactory(factory);
};

OpenDDSharp::DDS::DomainParticipantFactory^  OpenDDSharp::OpenDDS::DCPS::ParticipantService::GetDomainParticipantFactory(array<System::String^>^ args) {
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

void OpenDDSharp::OpenDDS::DCPS::ParticipantService::Shutdown() {
	impl_entity->shutdown();
	
	ACE::fini();
};
