/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "Topic.h"

::DDS::Entity_ptr Topic_NarrowBase(::DDS::Topic_ptr t) {
  return static_cast< ::DDS::Entity_ptr>(t);
}

::DDS::TopicDescription_ptr Topic_NarrowTopicDescription(::DDS::Topic_ptr t) {
  return static_cast< ::DDS::TopicDescription_ptr>(t);;
}

::DDS::ReturnCode_t Topic_GetQos(::DDS::Topic_ptr t, TopicQosWrapper &qos_wrapper) {
  ::DDS::TopicQos qos_native;
  ::DDS::ReturnCode_t ret = t->get_qos(qos_native);

  if (ret == ::DDS::RETCODE_OK) {
    qos_wrapper = qos_native;
  }

  return ret;
}

::DDS::ReturnCode_t Topic_SetQos(::DDS::Topic_ptr t, TopicQosWrapper qos_wrapper) {
  return t->set_qos(qos_wrapper);
}

::DDS::ReturnCode_t Topic_SetListener(::DDS::Topic_ptr t, OpenDDSharp::OpenDDS::DDS::TopicListenerImpl_ptr listener,
                                      ::DDS::StatusMask status) {
  return t->set_listener(listener, status);
}

char *Topic_GetTypeName(::DDS::Topic_ptr t) {
  return t->get_type_name();
}

char *Topic_GetName(::DDS::Topic_ptr t) {
  return t->get_name();
}

::DDS::DomainParticipant_ptr Topic_GetParticipant(::DDS::Topic_ptr t) {
  return t->get_participant();
}

::DDS::ReturnCode_t Topic_GetInconsistentTopicStatus(::DDS::Topic_ptr t, ::DDS::InconsistentTopicStatus_out status) {
  return t->get_inconsistent_topic_status(status);
}