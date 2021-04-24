/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

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
#include "InfoRepoDiscovery.h"

OpenDDSharp::OpenDDS::DCPS::InfoRepoDiscovery::InfoRepoDiscovery(System::String^ key, System::String^ ior) : Discovery() {
    if (System::String::IsNullOrWhiteSpace(key)) {
        throw gcnew System::ArgumentNullException("key", "The key cannot be null or an empty string");
    }

    if (System::String::IsNullOrWhiteSpace(ior)) {
        throw gcnew System::ArgumentNullException("ior", "The ior cannot be null or an empty string");
    }

    msclr::interop::marshal_context context;

    impl_entity = new ::OpenDDS::DCPS::InfoRepoDiscovery(context.marshal_as<const char*>(key), context.marshal_as<const char*>(ior));
    Discovery::impl_entity = dynamic_cast<::OpenDDS::DCPS::InfoRepoDiscovery*>(impl_entity);
}

System::Int32 OpenDDSharp::OpenDDS::DCPS::InfoRepoDiscovery::BitTransportPort::get() {    
    return impl_entity->bit_transport_port();
};

void OpenDDSharp::OpenDDS::DCPS::InfoRepoDiscovery::BitTransportPort::set(System::Int32 value) {    
    impl_entity->bit_transport_port(value);    
};

System::String^ OpenDDSharp::OpenDDS::DCPS::InfoRepoDiscovery::BitTransportIp::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->bit_transport_ip().c_str());
};

void OpenDDSharp::OpenDDS::DCPS::InfoRepoDiscovery::BitTransportIp::set(System::String^ value) {
    msclr::interop::marshal_context context;
    impl_entity->bit_transport_ip(context.marshal_as<const char*>(value));
};