/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"

#include <dds/DCPS/GuardCondition.h>

EXTERN_METHOD_EXPORT
::DDS::GuardCondition_ptr GuardCondition_CreateGuardCondition();

EXTERN_METHOD_EXPORT
::DDS::Condition_ptr GuardCondition_NarrowBase(::DDS::GuardCondition_ptr gc);

EXTERN_METHOD_EXPORT
::CORBA::Boolean GuardCondition_GetTriggerValue(::DDS::GuardCondition_ptr gc);

EXTERN_METHOD_EXPORT
void GuardCondition_SetTriggerValue(::DDS::GuardCondition_ptr gc, ::CORBA::Boolean value);
