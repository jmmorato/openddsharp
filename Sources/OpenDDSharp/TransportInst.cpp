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
#include "TransportInst.h"

OpenDDSharp::OpenDDS::DCPS::TransportInst::TransportInst(::OpenDDS::DCPS::TransportInst* native) {
    impl_entity = native;
};

System::String^ OpenDDSharp::OpenDDS::DCPS::TransportInst::TransportType::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->transport_type_.c_str());   
};

System::String^ OpenDDSharp::OpenDDS::DCPS::TransportInst::Name::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->name().c_str());
};

size_t OpenDDSharp::OpenDDS::DCPS::TransportInst::QueueMessagesPerPool::get() {
    return impl_entity->queue_messages_per_pool_;
};

void OpenDDSharp::OpenDDS::DCPS::TransportInst::QueueMessagesPerPool::set(size_t value) {
    impl_entity->queue_messages_per_pool_ = value;
};

size_t OpenDDSharp::OpenDDS::DCPS::TransportInst::QueueInitialPools::get() {
    return impl_entity->queue_initial_pools_;
};

void OpenDDSharp::OpenDDS::DCPS::TransportInst::QueueInitialPools::set(size_t value) {
    impl_entity->queue_initial_pools_ = value;
};

System::UInt32 OpenDDSharp::OpenDDS::DCPS::TransportInst::MaxPacketSize::get() {
    return impl_entity->max_packet_size_;
};

void OpenDDSharp::OpenDDS::DCPS::TransportInst::MaxPacketSize::set(System::UInt32 value) {
    impl_entity->max_packet_size_ = value;
};

size_t OpenDDSharp::OpenDDS::DCPS::TransportInst::MaxSamplesPerPacket::get() {
    return impl_entity->max_samples_per_packet_;
};

void OpenDDSharp::OpenDDS::DCPS::TransportInst::MaxSamplesPerPacket::set(size_t value) {
    impl_entity->max_samples_per_packet_ = value;
};

System::UInt32 OpenDDSharp::OpenDDS::DCPS::TransportInst::OptimumPacketSize::get() {
    return impl_entity->optimum_packet_size_;
};

void OpenDDSharp::OpenDDS::DCPS::TransportInst::OptimumPacketSize::set(System::UInt32 value) {
    impl_entity->optimum_packet_size_ = value;
};

System::Boolean OpenDDSharp::OpenDDS::DCPS::TransportInst::ThreadPerConnection::get() {
    return impl_entity->thread_per_connection_;
};

void OpenDDSharp::OpenDDS::DCPS::TransportInst::ThreadPerConnection::set(System::Boolean value) {
    impl_entity->thread_per_connection_ = value;
};

long OpenDDSharp::OpenDDS::DCPS::TransportInst::DatalinkReleaseDelay::get() {
    return impl_entity->datalink_release_delay_;    
};

void OpenDDSharp::OpenDDS::DCPS::TransportInst::DatalinkReleaseDelay::set(long value) {
    impl_entity->datalink_release_delay_ = value;
};

size_t OpenDDSharp::OpenDDS::DCPS::TransportInst::DatalinkControlChunks::get() {
    return impl_entity->datalink_control_chunks_;
};

void OpenDDSharp::OpenDDS::DCPS::TransportInst::DatalinkControlChunks::set(size_t value) {
    impl_entity->datalink_control_chunks_ = value;
};
