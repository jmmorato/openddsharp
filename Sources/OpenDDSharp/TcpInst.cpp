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
#include "TcpInst.h"

OpenDDSharp::OpenDDS::DCPS::TcpInst::TcpInst(TransportInst^ inst) : TransportInst(inst->impl_entity) {
    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst->impl_entity);
    ::OpenDDS::DCPS::TcpInst_rch tcpi = ::OpenDDS::DCPS::static_rchandle_cast<::OpenDDS::DCPS::TcpInst>(rch);
    ::OpenDDS::DCPS::TcpInst* pointer = tcpi.in();
    pointer->_add_ref();
    impl_entity = pointer;
}

System::Boolean OpenDDSharp::OpenDDS::DCPS::TcpInst::IsReliable::get() {
    return impl_entity->is_reliable();
};

System::Boolean OpenDDSharp::OpenDDS::DCPS::TcpInst::EnableNagleAlgorithm::get() {
    return impl_entity->enable_nagle_algorithm_;
};

void OpenDDSharp::OpenDDS::DCPS::TcpInst::EnableNagleAlgorithm::set(System::Boolean value) {
    impl_entity->enable_nagle_algorithm_ = value;
};

System::Int32 OpenDDSharp::OpenDDS::DCPS::TcpInst::ConnRetryInitialDelay::get() {
    return impl_entity->conn_retry_initial_delay_;
};

void OpenDDSharp::OpenDDS::DCPS::TcpInst::ConnRetryInitialDelay::set(System::Int32 value) {
    impl_entity->conn_retry_initial_delay_ = value;
};

System::Double OpenDDSharp::OpenDDS::DCPS::TcpInst::ConnRetryBackoffMultiplier::get() {
    return impl_entity->conn_retry_backoff_multiplier_;
};

void OpenDDSharp::OpenDDS::DCPS::TcpInst::ConnRetryBackoffMultiplier::set(System::Double value) {
    impl_entity->conn_retry_backoff_multiplier_ = value;
};

System::Int32 OpenDDSharp::OpenDDS::DCPS::TcpInst::ConnRetryAttempts::get() {
    return impl_entity->conn_retry_attempts_;
};

void OpenDDSharp::OpenDDS::DCPS::TcpInst::ConnRetryAttempts::set(System::Int32 value) {
    impl_entity->conn_retry_attempts_ = value;
};

System::Int32 OpenDDSharp::OpenDDS::DCPS::TcpInst::MaxOutputPausePeriod::get() {
    return impl_entity->max_output_pause_period_;
};

void OpenDDSharp::OpenDDS::DCPS::TcpInst::MaxOutputPausePeriod::set(System::Int32 value) {
    impl_entity->max_output_pause_period_ = value;
};

System::Int32 OpenDDSharp::OpenDDS::DCPS::TcpInst::PassiveReconnectDuration::get() {
    return impl_entity->passive_reconnect_duration_;
};

void OpenDDSharp::OpenDDS::DCPS::TcpInst::PassiveReconnectDuration::set(System::Int32 value) {
    impl_entity->passive_reconnect_duration_ = value;
};

System::String^ OpenDDSharp::OpenDDS::DCPS::TcpInst::PublicAddress::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->get_public_address().c_str());
};

void OpenDDSharp::OpenDDS::DCPS::TcpInst::PublicAddress::set(System::String^ value) {
    msclr::interop::marshal_context context;
    impl_entity->pub_address_str_ = context.marshal_as<const char *>(value);
};

System::String^ OpenDDSharp::OpenDDS::DCPS::TcpInst::LocalAddress::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->local_address_string().c_str());
};

void OpenDDSharp::OpenDDS::DCPS::TcpInst::LocalAddress::set(System::String^ value) {
    msclr::interop::marshal_context context;
    impl_entity->local_address(context.marshal_as<const char *>(value));
};