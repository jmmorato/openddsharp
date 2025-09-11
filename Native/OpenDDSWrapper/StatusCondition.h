/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"

EXTERN_METHOD_EXPORT
::DDS::Condition_ptr StatusCondition_NarrowBase(::DDS::StatusCondition_ptr status_condition);

EXTERN_METHOD_EXPORT
::DDS::StatusMask StatusCondition_GetEnabledStatuses(::DDS::StatusCondition_ptr status_condition);

EXTERN_METHOD_EXPORT
void StatusCondition_SetEnabledStatuses(::DDS::StatusCondition_ptr status_condition, ::DDS::StatusMask value);
