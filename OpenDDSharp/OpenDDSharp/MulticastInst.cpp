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
#include "MulticastInst.h"

OpenDDSharp::OpenDDS::DCPS::MulticastInst::MulticastInst(TransportInst^ inst) : TransportInst(inst->impl_entity) {
    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst->impl_entity);
    ::OpenDDS::DCPS::MulticastInst_rch udp = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::MulticastInst>(rch);
    ::OpenDDS::DCPS::MulticastInst* pointer = udp.in();
    pointer->_add_ref();
    impl_entity = pointer;
}

System::Boolean OpenDDSharp::OpenDDS::DCPS::MulticastInst::IsReliable::get() {
    return impl_entity->is_reliable();
};

System::Boolean OpenDDSharp::OpenDDS::DCPS::MulticastInst::Reliable::get() {
    return impl_entity->reliable_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::Reliable::set(System::Boolean value) {
    impl_entity->reliable_ = value;
};

System::Boolean OpenDDSharp::OpenDDS::DCPS::MulticastInst::DefaultToIpv6::get() {
    return impl_entity->default_to_ipv6_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::DefaultToIpv6::set(System::Boolean value) {
    impl_entity->default_to_ipv6_ = value;
};

System::UInt16 OpenDDSharp::OpenDDS::DCPS::MulticastInst::PortOffset::get() {
    return impl_entity->port_offset_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::PortOffset::set(System::UInt16 value) {
    impl_entity->port_offset_ = value;
};

System::String^ OpenDDSharp::OpenDDS::DCPS::MulticastInst::GroupAddress::get() {
    msclr::interop::marshal_context context;

    char * buffer = new char[512];
    if (impl_entity->group_address_.addr_to_string(buffer, 512) < 0) {
        return System::String::Empty;
    }

    const char * s = buffer;
    return context.marshal_as<System::String^>(s);
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::GroupAddress::set(System::String^ value) {
    msclr::interop::marshal_context context;
    impl_entity->group_address_.set(context.marshal_as<const char *>(value));
};

System::String^ OpenDDSharp::OpenDDS::DCPS::MulticastInst::LocalAddress::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->local_address_.c_str());
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::LocalAddress::set(System::String^ value) {
    msclr::interop::marshal_context context;
    impl_entity->local_address_ = context.marshal_as<const char *>(value);
};

System::Double OpenDDSharp::OpenDDS::DCPS::MulticastInst::SynBackoff::get() {
    return impl_entity->syn_backoff_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::SynBackoff::set(System::Double value) {
    impl_entity->syn_backoff_ = value;
};

OpenDDSharp::TimeValue OpenDDSharp::OpenDDS::DCPS::MulticastInst::SynInterval::get() {
    return impl_entity->syn_interval_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::SynInterval::set(OpenDDSharp::TimeValue value) {
    impl_entity->syn_interval_ = value;
};

OpenDDSharp::TimeValue OpenDDSharp::OpenDDS::DCPS::MulticastInst::SynTimeout::get() {
    return impl_entity->syn_timeout_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::SynTimeout::set(OpenDDSharp::TimeValue value) {
    impl_entity->syn_timeout_ = value;
};

size_t OpenDDSharp::OpenDDS::DCPS::MulticastInst::NakDepth::get() {
    return impl_entity->nak_depth_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::NakDepth::set(size_t value) {
    impl_entity->nak_depth_ = value;
};

OpenDDSharp::TimeValue OpenDDSharp::OpenDDS::DCPS::MulticastInst::NakInterval::get() {
    return impl_entity->nak_interval_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::NakInterval::set(OpenDDSharp::TimeValue value) {
    impl_entity->nak_interval_ = value;
};

size_t OpenDDSharp::OpenDDS::DCPS::MulticastInst::NakDelayIntervals::get() {
    return impl_entity->nak_delay_intervals_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::NakDelayIntervals::set(size_t value) {
    impl_entity->nak_delay_intervals_ = value;
};

size_t OpenDDSharp::OpenDDS::DCPS::MulticastInst::NakMax::get() {
    return impl_entity->nak_max_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::NakMax::set(size_t value) {
    impl_entity->nak_max_ = value;
};

OpenDDSharp::TimeValue OpenDDSharp::OpenDDS::DCPS::MulticastInst::NakTimeout::get() {
    return impl_entity->nak_timeout_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::NakTimeout::set(OpenDDSharp::TimeValue value) {
    impl_entity->nak_timeout_ = value;
};

System::Byte OpenDDSharp::OpenDDS::DCPS::MulticastInst::TTL::get() {
    return impl_entity->ttl_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::TTL::set(System::Byte value) {
    impl_entity->ttl_ = value;
};

size_t OpenDDSharp::OpenDDS::DCPS::MulticastInst::RcvBufferSize::get() {
    return impl_entity->rcv_buffer_size_;
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::RcvBufferSize::set(size_t value) {
    impl_entity->rcv_buffer_size_ = value;
};

System::Boolean OpenDDSharp::OpenDDS::DCPS::MulticastInst::AsyncSend::get() {
    return impl_entity->async_send();
};

void OpenDDSharp::OpenDDS::DCPS::MulticastInst::AsyncSend::set(System::Boolean value) {
    impl_entity->async_send_ = value;
};
