/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "TimeValueWrapper.h"
#include "marshal.h"

#include <dds/DCPS/RTPS/RtpsDiscovery.h>
#include <dds/DCPS/transport/rtps_udp/RtpsUdp.h>
#include <dds/DCPS/LogAddr.h>
#include <string>

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::Discovery *RtpsDiscovery_NarrowBase(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
::OpenDDS::RTPS::RtpsDiscovery *RtpsDiscovery_new(const char *key);

EXTERN_METHOD_EXPORT
TimeValueWrapper RtpsDiscovery_GetResendPeriod(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetResendPeriod(::OpenDDS::RTPS::RtpsDiscovery *d, TimeValueWrapper value);

EXTERN_METHOD_EXPORT
CORBA::UInt16 RtpsDiscovery_GetPB(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetPB(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value);

EXTERN_METHOD_EXPORT
CORBA::UInt16 RtpsDiscovery_GetDG(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetDG(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value);

EXTERN_METHOD_EXPORT
CORBA::UInt16 RtpsDiscovery_GetPG(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetPG(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value);

EXTERN_METHOD_EXPORT
CORBA::UInt16 RtpsDiscovery_GetD0(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetD0(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value);

EXTERN_METHOD_EXPORT
CORBA::UInt16 RtpsDiscovery_GetD1(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetD1(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value);

EXTERN_METHOD_EXPORT
CORBA::UInt16 RtpsDiscovery_GetDX(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetDX(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::UInt16 value);

EXTERN_METHOD_EXPORT
CORBA::Octet RtpsDiscovery_GetTtl(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetTtl(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::Octet value);

EXTERN_METHOD_EXPORT
char *RtpsDiscovery_GetSedpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetSedpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d, char *value);

EXTERN_METHOD_EXPORT
char *RtpsDiscovery_GetSpdpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetSpdpLocalAddress(::OpenDDS::RTPS::RtpsDiscovery *d, char *value);

EXTERN_METHOD_EXPORT
CORBA::Boolean RtpsDiscovery_GetSedpMulticast(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetSedpMulticast(::OpenDDS::RTPS::RtpsDiscovery *d, CORBA::Boolean value);

EXTERN_METHOD_EXPORT
char *RtpsDiscovery_GetMulticastInterface(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetMulticastInterface(::OpenDDS::RTPS::RtpsDiscovery *d, char *value);

EXTERN_METHOD_EXPORT
char *RtpsDiscovery_GetDefaultMulticastGroup(::OpenDDS::RTPS::RtpsDiscovery *d, int domain_id);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetDefaultMulticastGroup(::OpenDDS::RTPS::RtpsDiscovery *d, char *value);

EXTERN_METHOD_EXPORT
void *RtpsDiscovery_GetSpdpSendAddrs(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetSpdpSendAddrs(::OpenDDS::RTPS::RtpsDiscovery *d, char *value);

EXTERN_METHOD_EXPORT
char *RtpsDiscovery_GetGuidInterface(::OpenDDS::RTPS::RtpsDiscovery *d);

EXTERN_METHOD_EXPORT
void RtpsDiscovery_SetGuidInterface(::OpenDDS::RTPS::RtpsDiscovery *d, char *value);
