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

#include <dds/DCPS/WaitSet.h>

EXTERN_METHOD_EXPORT
::DDS::WaitSet_ptr WaitSet_New();

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t WaitSet_Wait(::DDS::WaitSet_ptr ws, void *&sequence, ::DDS::Duration_t duration);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t WaitSet_AttachCondition(::DDS::WaitSet_ptr ws, ::DDS::Condition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t WaitSet_DetachCondition(::DDS::WaitSet_ptr ws, ::DDS::Condition_ptr condition);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t WaitSet_GetConditions(::DDS::WaitSet_ptr ws, void *&sequence);

EXTERN_METHOD_EXPORT
::DDS::ReturnCode_t WaitSet_DetachConditions(::DDS::WaitSet_ptr ws, void *sequence);
