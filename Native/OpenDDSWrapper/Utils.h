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
#pragma once

#include "dds/DdsDcpsDomainC.h"

#ifndef EXTERN_METHOD_EXPORT
    #ifdef _WIN32
        #define EXTERN_METHOD_EXPORT extern "C" __declspec(dllexport)
    #else
        #define EXTERN_METHOD_EXPORT extern "C" __attribute__((cdecl))
    #endif
#endif

#ifndef EXTERN_STRUCT_EXPORT
    #define EXTERN_STRUCT_EXPORT extern "C" struct
#endif

EXTERN_METHOD_EXPORT void Utils_CreateOctetSeq(unsigned char bytes[], ::DDS::OctetSeq* seq);
