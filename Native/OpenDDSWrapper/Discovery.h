/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
//#include <objbase.h>
#include <dds/DCPS/Discovery.h>

EXTERN_METHOD_EXPORT
char *Discovery_GetKey(::OpenDDS::DCPS::Discovery *d);
