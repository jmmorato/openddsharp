/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "Discovery.h"

char *Discovery_GetKey(::OpenDDS::DCPS::Discovery *d) {
  return CORBA::string_dup(d->key().c_str());
}