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
#include "ShmemInst.h"

OpenDDSharp::OpenDDS::DCPS::ShmemInst::ShmemInst(TransportInst^ inst) : TransportInst(inst->impl_entity) {
    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst->impl_entity);
    ::OpenDDS::DCPS::ShmemInst_rch shmemi = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::ShmemInst>(rch);
    ::OpenDDS::DCPS::ShmemInst* pointer = shmemi.in();
    pointer->_add_ref();
    impl_entity = pointer;
}

System::Boolean OpenDDSharp::OpenDDS::DCPS::ShmemInst::IsReliable::get() {
    return impl_entity->is_reliable();
};

size_t OpenDDSharp::OpenDDS::DCPS::ShmemInst::PoolSize::get() {
    return impl_entity->pool_size_;
};

void OpenDDSharp::OpenDDS::DCPS::ShmemInst::PoolSize::set(size_t value) {
    impl_entity->pool_size_ = value;
};

size_t OpenDDSharp::OpenDDS::DCPS::ShmemInst::DatalinkControlSize::get() {
    return impl_entity->datalink_control_size_;    
};

void OpenDDSharp::OpenDDS::DCPS::ShmemInst::DatalinkControlSize::set(size_t value) {
    impl_entity->datalink_control_size_ = value;
};

System::String^ OpenDDSharp::OpenDDS::DCPS::ShmemInst::HostName::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->hostname().c_str());
};

System::String^ OpenDDSharp::OpenDDS::DCPS::ShmemInst::PoolName::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->poolname().c_str());
};
