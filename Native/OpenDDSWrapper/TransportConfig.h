/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "marshal.h"
#include <dds/DCPS/transport/framework/TransportConfig_rch.h>
#include <dds/DCPS/transport/framework/TransportConfig.h>
#include <dds/DCPS/transport/framework/TransportInst.h>
#include "TimeValueWrapper.h"

EXTERN_METHOD_EXPORT
void TransportConfig_Insert(::OpenDDS::DCPS::TransportConfig *cfg, ::OpenDDS::DCPS::TransportInst *inst);

EXTERN_METHOD_EXPORT
void TransportConfig_SortedInsert(::OpenDDS::DCPS::TransportConfig *cfg, ::OpenDDS::DCPS::TransportInst *inst);

EXTERN_METHOD_EXPORT
CORBA::Boolean TransportConfig_GetSwapBytes(::OpenDDS::DCPS::TransportConfig *cfg);

EXTERN_METHOD_EXPORT
void TransportConfig_SetSwapBytes(::OpenDDS::DCPS::TransportConfig *cfg, CORBA::Boolean value);

EXTERN_METHOD_EXPORT
CORBA::UInt32 TransportConfig_GetPassiveConnectDuration(::OpenDDS::DCPS::TransportConfig *cfg);

EXTERN_METHOD_EXPORT
void TransportConfig_SetPassiveConnectDuration(::OpenDDS::DCPS::TransportConfig *cfg, CORBA::UInt32 value);

EXTERN_METHOD_EXPORT
char *TransportConfig_GetName(::OpenDDS::DCPS::TransportConfig *cfg);

EXTERN_METHOD_EXPORT
void *TransportConfig_GetTransports(::OpenDDS::DCPS::TransportConfig *cfg);