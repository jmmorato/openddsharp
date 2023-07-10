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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "QueryCondition.h"

::DDS::ReadCondition_ptr QueryCondition_NarrowBase(::DDS::QueryCondition_ptr ptr) {
  return static_cast< ::DDS::ReadCondition_ptr>(ptr);
}

char *QueryCondition_GetQueryExpresion(::DDS::QueryCondition_ptr ptr) {
  return ptr->get_query_expression();
}

::DDS::ReturnCode_t QueryCondition_GetQueryParameters(::DDS::QueryCondition_ptr ptr, void *&seq) {
  ::DDS::StringSeq parameters;

  ::DDS::ReturnCode_t ret = ptr->get_query_parameters(parameters);

  if (ret == ::DDS::RETCODE_OK) {
    unbounded_basic_string_sequence_to_ptr(parameters, seq);
  }

  return ret;
}

::DDS::ReturnCode_t QueryCondition_SetQueryParameters(::DDS::QueryCondition_ptr ptr, void *seq) {
  ::DDS::StringSeq parameters;
  ptr_to_unbounded_basic_string_sequence(seq, parameters);

  ::DDS::ReturnCode_t ret = ptr->set_query_parameters(parameters);

  return ret;
}
