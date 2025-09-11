/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "dds/DdsDcpsDomainC.h"

#ifndef EXTERN_METHOD_EXPORT
  #ifdef _WIN32
    #define EXTERN_METHOD_EXPORT extern "C" __declspec(dllexport)
  #else
    #define EXTERN_METHOD_EXPORT extern "C" __attribute__((visibility("default")))
  #endif
#endif

#ifndef EXTERN_STRUCT_EXPORT
  #define EXTERN_STRUCT_EXPORT extern "C" struct
#endif

EXTERN_METHOD_EXPORT void Utils_CreateOctetSeq(unsigned char bytes[], ::DDS::OctetSeq *seq);
