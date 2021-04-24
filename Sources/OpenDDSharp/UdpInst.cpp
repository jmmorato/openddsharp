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
#include "UdpInst.h"

OpenDDSharp::OpenDDS::DCPS::UdpInst::UdpInst(TransportInst^ inst) : TransportInst(inst->impl_entity) {
    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst->impl_entity);
    ::OpenDDS::DCPS::UdpInst_rch udp = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::UdpInst>(rch);
    ::OpenDDS::DCPS::UdpInst* pointer = udp.in();
    pointer->_add_ref();
    impl_entity = pointer;
}

System::Boolean OpenDDSharp::OpenDDS::DCPS::UdpInst::IsReliable::get() {
    return impl_entity->is_reliable();
};

System::Int32 OpenDDSharp::OpenDDS::DCPS::UdpInst::SendBufferSize::get() {
    return impl_entity->send_buffer_size_;
};

void OpenDDSharp::OpenDDS::DCPS::UdpInst::SendBufferSize::set(System::Int32 value) {
    impl_entity->send_buffer_size_ = value;
};

System::Int32 OpenDDSharp::OpenDDS::DCPS::UdpInst::RcvBufferSize::get() {
    return impl_entity->rcv_buffer_size_;
};

void OpenDDSharp::OpenDDS::DCPS::UdpInst::RcvBufferSize::set(System::Int32 value) {
    impl_entity->rcv_buffer_size_ = value;
};

System::String^ OpenDDSharp::OpenDDS::DCPS::UdpInst::LocalAddress::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->local_address_string().c_str());
};

void OpenDDSharp::OpenDDS::DCPS::UdpInst::LocalAddress::set(System::String^ value) {
    msclr::interop::marshal_context context;
    impl_entity->local_address(context.marshal_as<const char *>(value));
};
