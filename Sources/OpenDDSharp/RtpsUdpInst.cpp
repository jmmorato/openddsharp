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
#include "RtpsUdpInst.h"

OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::RtpsUdpInst(TransportInst^ inst) : TransportInst(inst->impl_entity) {
    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst->impl_entity);
    ::OpenDDS::DCPS::RtpsUdpInst_rch rui = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::RtpsUdpInst>(rch);
    ::OpenDDS::DCPS::RtpsUdpInst* pointer = rui.in();
    pointer->_add_ref();
    impl_entity = pointer;
}

System::Boolean OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::IsReliable::get() {
    return impl_entity->is_reliable();
};

System::Boolean OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::RequiresCdr::get() {
    return impl_entity->requires_cdr_encapsulation();
};

System::Int32 OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::SendBufferSize::get() {
    return impl_entity->send_buffer_size_;
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::SendBufferSize::set(System::Int32 value) {
    impl_entity->send_buffer_size_ = value;
};

System::Int32 OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::RcvBufferSize::get() {
    return impl_entity->rcv_buffer_size_;
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::RcvBufferSize::set(System::Int32 value) {
    impl_entity->rcv_buffer_size_ = value;
};

size_t OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::NakDepth::get() {
    return impl_entity->nak_depth_;
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::NakDepth::set(size_t value) {
    impl_entity->nak_depth_ = value;
};

OpenDDSharp::TimeValue OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::NakResponseDelay::get() {
    return impl_entity->nak_response_delay_;
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::NakResponseDelay::set(OpenDDSharp::TimeValue value) {
    impl_entity->nak_response_delay_ = value;
};

OpenDDSharp::TimeValue OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::HeartbeatPeriod::get() {
    return impl_entity->heartbeat_period_;
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::HeartbeatPeriod::set(OpenDDSharp::TimeValue value) {
    impl_entity->heartbeat_period_ = value;
};

OpenDDSharp::TimeValue OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::HeartbeatResponseDelay::get() {
    return impl_entity->heartbeat_response_delay_;
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::HeartbeatResponseDelay::set(OpenDDSharp::TimeValue value) {
    impl_entity->heartbeat_response_delay_ = value;
};


System::Boolean OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::UseMulticast::get() {
    return impl_entity->use_multicast_;
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::UseMulticast::set(System::Boolean value) {
    impl_entity->use_multicast_ = value;
};

System::Byte OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::Ttl::get() {
    return impl_entity->ttl_;
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::Ttl::set(System::Byte value) {
    impl_entity->ttl_ = value;
};

System::String^ OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::MulticastGroupAddress::get() {
    msclr::interop::marshal_context context;

    char * buffer = new char[512];
    if (impl_entity->multicast_group_address().addr_to_string(buffer, 512) < 0) {
        return System::String::Empty;
    }
    
    const char * s = buffer;
    return context.marshal_as<System::String^>(s);
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::MulticastGroupAddress::set(System::String^ value) {
    msclr::interop::marshal_context context;
    System::String^ full = value;
    if (!full->Contains(":"))
    {
        full += ":0";
    }
    const ACE_INET_Addr addr = static_cast<const ACE_INET_Addr>(context.marshal_as<const char*>(full));
    impl_entity->multicast_group_address(addr);
};

System::String^ OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::MulticastInterface::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->multicast_interface_.c_str());
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::MulticastInterface::set(System::String^ value) {
    msclr::interop::marshal_context context;
    impl_entity->multicast_interface_ = context.marshal_as<const char *>(value);
};

System::String^ OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::LocalAddress::get() {
    msclr::interop::marshal_context context;
    char* buffer = new char[512];
    if (impl_entity->local_address().addr_to_string(buffer, 512) < 0) {
        return System::String::Empty;
    }

    const char* s = buffer;
    return context.marshal_as<System::String^>(s);
};

void OpenDDSharp::OpenDDS::DCPS::RtpsUdpInst::LocalAddress::set(System::String^ value) {
    msclr::interop::marshal_context context;
    System::String^ full = value;
    if (!full->Contains(":"))
    {
        full += ":0";
    }
    const ACE_INET_Addr addr = static_cast<const ACE_INET_Addr>(context.marshal_as<const char*>(full));
    impl_entity->local_address(addr);
};