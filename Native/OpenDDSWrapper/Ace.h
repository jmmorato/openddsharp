/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "ace/Init_ACE.h"

EXTERN_METHOD_EXPORT
int Ace_Init();

EXTERN_METHOD_EXPORT
int Ace_Fini();