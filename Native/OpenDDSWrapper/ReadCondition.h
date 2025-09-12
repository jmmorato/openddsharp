/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"

EXTERN_METHOD_EXPORT ::DDS::Condition_ptr ReadCondition_NarrowBase(::DDS::ReadCondition_ptr ptr);

EXTERN_METHOD_EXPORT void ReadCondition_Release(::DDS::ReadCondition_ptr ptr);

EXTERN_METHOD_EXPORT ::DDS::SampleStateMask ReadCondition_GetSampleStateMask(::DDS::ReadCondition_ptr ptr);

EXTERN_METHOD_EXPORT ::DDS::ViewStateMask ReadCondition_GetViewStateMask(::DDS::ReadCondition_ptr ptr);

EXTERN_METHOD_EXPORT ::DDS::InstanceStateMask ReadCondition_GetInstanceStateMask(::DDS::ReadCondition_ptr ptr);