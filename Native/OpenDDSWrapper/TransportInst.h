/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include <dds/DCPS/transport/framework/TransportInst.h>
#include <dds/DCPS/transport/framework/TransportInst_rch.h>

EXTERN_METHOD_EXPORT
char *TransportInst_GetTransportType(::OpenDDS::DCPS::TransportInst *ti);

EXTERN_METHOD_EXPORT
char *TransportInst_GetName(::OpenDDS::DCPS::TransportInst *ti);

EXTERN_METHOD_EXPORT
CORBA::ULong TransportInst_GetMaxPacketSize(::OpenDDS::DCPS::TransportInst *ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetMaxPacketSize(::OpenDDS::DCPS::TransportInst *ti, CORBA::ULong value);

EXTERN_METHOD_EXPORT
size_t TransportInst_GetMaxSamplesPerPacket(::OpenDDS::DCPS::TransportInst *ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetMaxSamplesPerPacket(::OpenDDS::DCPS::TransportInst *ti, size_t value);

EXTERN_METHOD_EXPORT
CORBA::ULong TransportInst_GetOptimumPacketSize(::OpenDDS::DCPS::TransportInst *ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetOptimumPacketSize(::OpenDDS::DCPS::TransportInst *ti, CORBA::ULong value);

EXTERN_METHOD_EXPORT
CORBA::Boolean TransportInst_GetThreadPerConnection(::OpenDDS::DCPS::TransportInst *ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetThreadPerConnection(::OpenDDS::DCPS::TransportInst *ti, CORBA::Boolean value);

EXTERN_METHOD_EXPORT
long TransportInst_GetDatalinkReleaseDelay(::OpenDDS::DCPS::TransportInst *ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetDatalinkReleaseDelay(::OpenDDS::DCPS::TransportInst *ti, long value);

EXTERN_METHOD_EXPORT
size_t TransportInst_GetDatalinkControlChunks(::OpenDDS::DCPS::TransportInst *ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetDatalinkControlChunks(::OpenDDS::DCPS::TransportInst *ti, size_t value);

EXTERN_METHOD_EXPORT
int TransportInst_GetEventDispatcherThreads(::OpenDDS::DCPS::TransportInst *ti);

EXTERN_METHOD_EXPORT
void TransportInst_SetEventDispatcherThreads(::OpenDDS::DCPS::TransportInst *ti, int value);