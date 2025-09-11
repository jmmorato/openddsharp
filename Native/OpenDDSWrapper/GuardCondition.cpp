/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "GuardCondition.h"

::DDS::GuardCondition_ptr GuardCondition_CreateGuardCondition() {
  return new ::DDS::GuardCondition();
}

::DDS::Condition_ptr GuardCondition_NarrowBase(::DDS::GuardCondition_ptr gc) {
  return static_cast< ::DDS::Condition_ptr>(gc);
}

::CORBA::Boolean GuardCondition_GetTriggerValue(::DDS::GuardCondition_ptr gc) {
  return gc->get_trigger_value();
}

void GuardCondition_SetTriggerValue(::DDS::GuardCondition_ptr gc, ::CORBA::Boolean value) {
  gc->set_trigger_value(value);
}
