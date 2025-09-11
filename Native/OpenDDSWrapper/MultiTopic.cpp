/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "MultiTopic.h"

::DDS::TopicDescription_ptr MultiTopic_NarrowTopicDescription(::DDS::MultiTopic_ptr t) {
  return static_cast< ::DDS::TopicDescription_ptr>(t);;
}

char *MultiTopic_GetTypeName(::DDS::MultiTopic_ptr t) {
  return t->get_type_name();
}

char *MultiTopic_GetName(::DDS::MultiTopic_ptr t) {
  return t->get_name();
}

::DDS::DomainParticipant_ptr MultiTopic_GetParticipant(::DDS::MultiTopic_ptr t) {
  return t->get_participant();
}

char *MultiTopic_GetSubscriptionExpression(::DDS::MultiTopic_ptr t) {
  return t->get_subscription_expression();
}

::DDS::ReturnCode_t MultiTopic_GetExpressionParameters(::DDS::MultiTopic_ptr t, void *&seq) {
  ::DDS::StringSeq parameters;

  ::DDS::ReturnCode_t ret = t->get_expression_parameters(parameters);

  if (ret == ::DDS::RETCODE_OK) {
    unbounded_basic_string_sequence_to_ptr(parameters, seq);
  }

  return ret;
}

::DDS::ReturnCode_t MultiTopic_SetExpressionParameters(::DDS::MultiTopic_ptr t, void *seq) {
  ::DDS::StringSeq parameters;
  ptr_to_unbounded_basic_string_sequence(seq, parameters);

  ::DDS::ReturnCode_t ret = t->set_expression_parameters(parameters);

  return ret;
}
