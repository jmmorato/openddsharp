/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"

#include <dds/DCPS/InfoRepoDiscovery/InfoRepoDiscovery.h>

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::Discovery *InfoRepoDiscovery_NarrowBase(::OpenDDS::DCPS::InfoRepoDiscovery *d);

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::InfoRepoDiscovery *InfoRepoDiscovery_new(const char *key, const char *ior);

EXTERN_METHOD_EXPORT
CORBA::Long InfoRepoDiscovery_GetBitTransportPort(::OpenDDS::DCPS::InfoRepoDiscovery *idr);

EXTERN_METHOD_EXPORT
void InfoRepoDiscovery_SetBitTransportPort(::OpenDDS::DCPS::InfoRepoDiscovery *idr, CORBA::Long port_number);

EXTERN_METHOD_EXPORT
char *InfoRepoDiscovery_GetBitTransportIp(::OpenDDS::DCPS::InfoRepoDiscovery *idr);

EXTERN_METHOD_EXPORT
void InfoRepoDiscovery_SetBitTransportIp(::OpenDDS::DCPS::InfoRepoDiscovery *idr, char *ip);