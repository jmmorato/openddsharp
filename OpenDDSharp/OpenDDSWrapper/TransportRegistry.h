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

#include "Utils.h"

#include <dds/DCPS/transport/framework/TransportRegistry.h>

EXTERN_METHOD_EXPORT
void TransportRegistry_Close();

EXTERN_METHOD_EXPORT
void TransportRegistry_Release();

EXTERN_METHOD_EXPORT
bool TransportRegistry_GetReleased();

EXTERN_METHOD_EXPORT
OpenDDS::DCPS::TransportConfig* TransportRegistry_CreateConfig(const char * name);

EXTERN_METHOD_EXPORT
::OpenDDS::DCPS::TransportInst* TransportRegistry_CreateInst(const char * name, const char * transportType);

EXTERN_METHOD_EXPORT
void TransportRegistry_BindConfigName(const char * name, ::DDS::Entity_ptr entity);
