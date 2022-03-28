/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2022 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "RtpsUdpInst.h"

::OpenDDS::DCPS::RtpsUdpInst* RtpsUdpInst_new(::OpenDDS::DCPS::TransportInst* inst) {
    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from< ::OpenDDS::DCPS::TransportInst>(inst);
    ::OpenDDS::DCPS::RtpsUdpInst_rch rtps = ::OpenDDS::DCPS::static_rchandle_cast< ::OpenDDS::DCPS::RtpsUdpInst>(rch);
    ::OpenDDS::DCPS::RtpsUdpInst* pointer = rtps.in();
    pointer->_add_ref();

    return pointer;
}

CORBA::Boolean RtpsUdpInst_GetIsReliable(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->is_reliable();
}

CORBA::Boolean RtpsUdpInst_GetRequiresCdr(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->requires_cdr_encapsulation();
}

CORBA::Int32 RtpsUdpInst_GetSendBufferSize(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->send_buffer_size_;
}

void RtpsUdpInst_SetSendBufferSize(::OpenDDS::DCPS::RtpsUdpInst* ri, CORBA::Int32 value) {
    ri->send_buffer_size_ = value;
}

CORBA::Int32 RtpsUdpInst_GetRcvBufferSize(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->rcv_buffer_size_;
}

void RtpsUdpInst_SetRcvBufferSize(::OpenDDS::DCPS::RtpsUdpInst* ri, CORBA::Int32 value) {
    ri->rcv_buffer_size_ = value;
}

size_t RtpsUdpInst_GetNakDepth(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->nak_depth_;
}

void RtpsUdpInst_SetNakDepth(::OpenDDS::DCPS::RtpsUdpInst* ri, size_t value) {
    ri->nak_depth_ = value;
}

CORBA::Boolean RtpsUdpInst_GetUseMulticast(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->use_multicast_;
}

void RtpsUdpInst_SetUseMulticast(::OpenDDS::DCPS::RtpsUdpInst* ri, CORBA::Boolean value) {
    ri->use_multicast_ = value;
}

CORBA::Octet RtpsUdpInst_GetTtl(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->ttl_;
}

void RtpsUdpInst_SetTtl(::OpenDDS::DCPS::RtpsUdpInst* ri, CORBA::Octet value) {
    ri->ttl_ = value;
}

char* RtpsUdpInst_GetMulticastGroupAddress(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    char * buffer = new char[512];
    ri->multicast_group_address().addr_to_string(buffer, 512);
    
    return CORBA::string_dup(buffer);
}

void RtpsUdpInst_SetMulticastGroupAddress(::OpenDDS::DCPS::RtpsUdpInst* ri, char* value) {
    const ACE_INET_Addr addr = static_cast<const ACE_INET_Addr>(value);
    ri->multicast_group_address(addr);
}

char* RtpsUdpInst_GetMulticastInterface(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return CORBA::string_dup(ri->multicast_interface_.c_str());
}

void RtpsUdpInst_SetMulticastInterface(::OpenDDS::DCPS::RtpsUdpInst* ri, char* value) {
    ri->multicast_interface_ = value;
}

char* RtpsUdpInst_GetLocalAddress(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    char * buffer = new char[512];
    ri->local_address().addr_to_string(buffer, 512);
    
    return CORBA::string_dup(buffer);
}

void RtpsUdpInst_SetLocalAddress(::OpenDDS::DCPS::RtpsUdpInst* ri, char* value) {
    const ACE_INET_Addr addr = static_cast<const ACE_INET_Addr>(value);
    ri->local_address(addr);
}

TimeValueWrapper RtpsUdpInst_GetNakResponseDelay(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->nak_response_delay_;
}

void RtpsUdpInst_SetNakResponseDelay(::OpenDDS::DCPS::RtpsUdpInst* ri, TimeValueWrapper value) {
    ri->nak_response_delay_ = value;
}

TimeValueWrapper RtpsUdpInst_GetHeartbeatPeriod(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->heartbeat_period_;
}

void RtpsUdpInst_SetHeartbeatPeriod(::OpenDDS::DCPS::RtpsUdpInst* ri, TimeValueWrapper value) {
    ri->heartbeat_period_ = value;
}

TimeValueWrapper RtpsUdpInst_GetHeartbeatResponseDelay(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->heartbeat_response_delay_;
}

void RtpsUdpInst_SetHeartbeatResponseDelay(::OpenDDS::DCPS::RtpsUdpInst* ri, TimeValueWrapper value) {
    ri->heartbeat_response_delay_ = value;
}

TimeValueWrapper RtpsUdpInst_GetReceiveAddressDuration(::OpenDDS::DCPS::RtpsUdpInst* ri) {
    return ri->receive_address_duration_;
}

void RtpsUdpInst_SetReceiveAddressDuration(::OpenDDS::DCPS::RtpsUdpInst* ri, TimeValueWrapper value){ 
    ri->receive_address_duration_ = value;
}