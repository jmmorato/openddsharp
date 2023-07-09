/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "WaitSet.h"

::DDS::WaitSet_ptr WaitSet_New() {
  return new ::DDS::WaitSet();
}

::DDS::ReturnCode_t WaitSet_Wait(::DDS::WaitSet_ptr ws, void *&sequence, ::DDS::Duration_t duration) {
  ::DDS::ConditionSeq seq;
  ::DDS::ReturnCode_t ret = ws->wait(seq, duration);

  if (ret == ::DDS::RETCODE_OK) {
    CORBA::ULong length = seq.length();
    TAO::unbounded_value_sequence<::DDS::Condition_ptr> conditions(length);
    conditions.length(length);
    for (CORBA::ULong i = 0; i < length; i++) {
      conditions[i] = seq[i].in();
    }
    unbounded_sequence_to_ptr(conditions, sequence);
  }

  return ret;
}

::DDS::ReturnCode_t WaitSet_AttachCondition(::DDS::WaitSet_ptr ws, ::DDS::Condition_ptr condition) {
  return ws->attach_condition(condition);
}

::DDS::ReturnCode_t WaitSet_DetachCondition(::DDS::WaitSet_ptr ws, ::DDS::Condition_ptr condition) {
  return ws->detach_condition(condition);
}

::DDS::ReturnCode_t WaitSet_GetConditions(::DDS::WaitSet_ptr ws, void *&sequence) {
  ::DDS::ConditionSeq seq;
  ::DDS::ReturnCode_t ret = ws->get_conditions(seq);

  if (ret == ::DDS::RETCODE_OK) {
    CORBA::ULong length = seq.length();
    TAO::unbounded_value_sequence<::DDS::Condition_ptr> conditions(length);
    conditions.length(length);
    for (CORBA::ULong i = 0; i < length; i++) {
      conditions[i] = seq[i].in();
    }
    unbounded_sequence_to_ptr(conditions, sequence);
  }

  return ret;
}

::DDS::ReturnCode_t WaitSet_DetachConditions(::DDS::WaitSet_ptr ws, void *sequence) {
  ::TAO::unbounded_value_sequence<::DDS::Condition_ptr> seq;
  ptr_to_unbounded_sequence(sequence, seq);

  ::CORBA::ULong length = seq.length();
  ::DDS::ConditionSeq conditions(length);
  conditions.length(length);
  for (CORBA::ULong i = 0; i < length; i++) {
    conditions[i] = seq[i];
  }

  return ws->detach_conditions(conditions);
}
