/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "Entity.h"

::DDS::ReturnCode_t Entity_Enable(::DDS::Entity_ptr entity) {
  return entity->enable();
}

::DDS::StatusCondition_ptr Entity_GetStatusCondition(::DDS::Entity_ptr entity) {
  return entity->get_statuscondition();
}

::DDS::StatusMask Entity_GetStatusChanges(::DDS::Entity_ptr entity) {
  return entity->get_status_changes();
}

::DDS::InstanceHandle_t Entity_GetInstanceHandle(::DDS::Entity_ptr entity) {
  return entity->get_instance_handle();
}