/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include <dds/DCPS/transport/udp/Udp.h>
#include <dds/DCPS/transport/udp/UdpInst.h>
#include <dds/DCPS/transport/udp/UdpInst_rch.h>
#include <dds/DCPS/transport/framework/TransportInst.h>
#include <dds/DCPS/transport/framework/TransportInst_rch.h>

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::UdpInst *UdpInst_new(::OpenDDS::DCPS::TransportInst *inst);

EXTERN_METHOD_EXPORT
CORBA::Boolean UdpInst_GetIsReliable(::OpenDDS::DCPS::UdpInst *ui);

EXTERN_METHOD_EXPORT
CORBA::Int32 UdpInst_GetSendBufferSize(::OpenDDS::DCPS::UdpInst *ui);

EXTERN_METHOD_EXPORT
void UdpInst_SetSendBufferSize(::OpenDDS::DCPS::UdpInst *ui, CORBA::Int32 value);

EXTERN_METHOD_EXPORT
CORBA::Int32 UdpInst_GetRcvBufferSize(::OpenDDS::DCPS::UdpInst *ui);

EXTERN_METHOD_EXPORT
void UdpInst_SetRcvBufferSize(::OpenDDS::DCPS::UdpInst *ui, CORBA::Int32 value);

EXTERN_METHOD_EXPORT
char *UdpInst_GetLocalAddress(::OpenDDS::DCPS::UdpInst *ui);

EXTERN_METHOD_EXPORT
void UdpInst_SetLocalAddress(::OpenDDS::DCPS::UdpInst *ui, char *value);