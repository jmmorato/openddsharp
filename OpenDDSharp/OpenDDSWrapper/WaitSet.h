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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once
#include "Utils.h"
#include <dds/DCPS/WaitSet.h>
#include "marshal.h"

EXTERN_METHOD_EXPORT
::DDS::WaitSet_ptr WaitSet_New();

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t WaitSet_Wait(::DDS::WaitSet_ptr ws, void*& sequence, ::DDS::Duration_t duration);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t WaitSet_AttachCondition(::DDS::WaitSet_ptr ws, ::DDS::Condition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t WaitSet_DetachCondition(::DDS::WaitSet_ptr ws, ::DDS::Condition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t WaitSet_GetConditions(::DDS::WaitSet_ptr ws, void*& sequence);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t WaitSet_DetachConditions(::DDS::WaitSet_ptr ws, void* sequence);
