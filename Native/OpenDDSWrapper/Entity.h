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
::DDS::ReturnCode_t Entity_Enable(::DDS::Entity_ptr entity);

EXTERN_METHOD_EXPORT
::DDS::StatusCondition_ptr Entity_GetStatusCondition(::DDS::Entity_ptr entity);

EXTERN_METHOD_EXPORT
::DDS::StatusMask Entity_GetStatusChanges(::DDS::Entity_ptr entity);

EXTERN_METHOD_EXPORT
::DDS::InstanceHandle_t Entity_GetInstanceHandle(::DDS::Entity_ptr entity);
