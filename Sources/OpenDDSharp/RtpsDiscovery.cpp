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
#include "RtpsDiscovery.h"

OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::RtpsDiscovery(System::String^ name) : Discovery() {
    if (System::String::IsNullOrWhiteSpace(name)) {
        throw gcnew System::ArgumentNullException("name", "The discovery name cannot be null or an empty string");
    }
    msclr::interop::marshal_context context;

    impl_entity = new ::OpenDDS::RTPS::RtpsDiscovery(context.marshal_as<const char*>(name));
    Discovery::impl_entity = dynamic_cast<::OpenDDS::RTPS::RtpsDiscovery*>(impl_entity);   
}

OpenDDSharp::TimeValue OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::ResendPeriod::get() {
    return impl_entity->resend_period();
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::ResendPeriod::set(OpenDDSharp::TimeValue value) {
    impl_entity->resend_period(value);
};

System::UInt16 OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::PB::get() {
    return impl_entity->pb();
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::PB::set(System::UInt16 value) {
    impl_entity->pb(value);
};

System::UInt16 OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::DG::get() {
    return impl_entity->dg();
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::DG::set(System::UInt16 value) {
    impl_entity->dg(value);
};

System::UInt16 OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::PG::get() {
    return impl_entity->pg();
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::PG::set(System::UInt16 value) {
    impl_entity->pg(value);
};

System::UInt16 OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::D0::get() {
    return impl_entity->d0();
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::D0::set(System::UInt16 value) {
    impl_entity->d0(value);
};

System::UInt16 OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::D1::get() {
    return impl_entity->d1();
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::D1::set(System::UInt16 value) {
    impl_entity->d1(value);
};

System::UInt16 OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::DX::get() {
    return impl_entity->dx();
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::DX::set(System::UInt16 value) {
    impl_entity->dx(value);
};

System::Byte OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::Ttl::get() {
    return impl_entity->ttl();
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::Ttl::set(System::Byte value) {
    impl_entity->ttl(value);
};

System::String^ OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::SedpLocalAddress::get() {
    msclr::interop::marshal_context context;
    char* buffer = new char[512];
    if (impl_entity->sedp_local_address().addr_to_string(buffer, 512) < 0) {
        return System::String::Empty;
    }

    const char* s = buffer;
    return context.marshal_as<System::String^>(s);
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::SedpLocalAddress::set(System::String^ value) {
    msclr::interop::marshal_context context;
    System::String^ full = value;
    if (!full->Contains(":"))
    {
        full += ":0";
    }
    const ACE_INET_Addr addr = static_cast<const ACE_INET_Addr>(context.marshal_as<const char*>(full));
    impl_entity->sedp_local_address(addr);    
};

System::String^ OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::SpdpLocalAddress::get() {
    msclr::interop::marshal_context context;
    char* buffer = new char[512];
    if (impl_entity->spdp_local_address().addr_to_string(buffer, 512) < 0) {
        return System::String::Empty;
    }

    const char* s = buffer;
    return context.marshal_as<System::String^>(s);
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::SpdpLocalAddress::set(System::String^ value) {
    msclr::interop::marshal_context context;
    System::String^ full = value;
    if (!full->Contains(":"))
    {
        full += ":0";
    }
    const ACE_INET_Addr addr = static_cast<const ACE_INET_Addr>(context.marshal_as<const char*>(full));
    impl_entity->spdp_local_address(addr);
};

System::Boolean OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::SedpMulticast::get() {    
    return impl_entity->sedp_multicast();
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::SedpMulticast::set(System::Boolean value) {    
    impl_entity->sedp_multicast(value);
};

System::String^ OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::MulticastInterface::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->multicast_interface().c_str());
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::MulticastInterface::set(System::String^ value) {
    msclr::interop::marshal_context context;
    impl_entity->multicast_interface(context.marshal_as<const char*>(value));
};

System::String^ OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::DefaultMulticastGroup::get() {
    msclr::interop::marshal_context context;
    char* buffer = new char[512];
    if (impl_entity->default_multicast_group().addr_to_string(buffer, 512) < 0) {
        return System::String::Empty;
    }

    const char* s = buffer;
    return context.marshal_as<System::String^>(s);
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::DefaultMulticastGroup::set(System::String^ value) {
    msclr::interop::marshal_context context;
    System::String^ full = value;
    if (!full->Contains(":"))
    {
        full += ":0";
    }
    const ACE_INET_Addr addr = static_cast<const ACE_INET_Addr>(context.marshal_as<const char*>(full));
    impl_entity->default_multicast_group(addr);
};

IEnumerable<System::String^>^ OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::SpdpSendAddrs::get() {
    msclr::interop::marshal_context context;
    std::vector<std::string> addrs = impl_entity->spdp_send_addrs();

    List<System::String^>^ ret = gcnew List<System::String^>();
    for (auto addr = addrs.begin(); addr != addrs.end(); ++addr)
    {
        ret->Add(context.marshal_as<System::String^>(addr->c_str()));
    }

    return ret;
};

System::String^ OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::GuidInterface::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->guid_interface().c_str());
};

void OpenDDSharp::OpenDDS::RTPS::RtpsDiscovery::GuidInterface::set(System::String^ value) {
    msclr::interop::marshal_context context;
    impl_entity->guid_interface(context.marshal_as<const char*>(value));
};
