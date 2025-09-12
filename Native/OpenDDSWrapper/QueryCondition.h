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

EXTERN_METHOD_EXPORT ::DDS::ReadCondition_ptr QueryCondition_NarrowBase(::DDS::QueryCondition_ptr ptr);

EXTERN_METHOD_EXPORT char *QueryCondition_GetQueryExpresion(::DDS::QueryCondition_ptr ptr);

EXTERN_METHOD_EXPORT ::DDS::ReturnCode_t QueryCondition_GetQueryParameters(::DDS::QueryCondition_ptr ptr, void *&seq);

EXTERN_METHOD_EXPORT ::DDS::ReturnCode_t QueryCondition_SetQueryParameters(::DDS::QueryCondition_ptr ptr, void *seq);
