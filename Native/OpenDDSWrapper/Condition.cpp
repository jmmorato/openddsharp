/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "Condition.h"

CORBA::Boolean Condition_GetTriggerValue(::DDS::Condition_ptr condition) {
  return condition->get_trigger_value();
}