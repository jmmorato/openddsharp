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
#include "Publisher.h"

::DDS::Entity_ptr Publisher_NarrowBase(::DDS::Publisher_ptr pub) {
  return static_cast< ::DDS::Entity_ptr>(pub);
}

::DDS::DataWriter_ptr Publisher_CreateDataWriter(::DDS::Publisher_ptr pub,
                                                 ::DDS::Topic_ptr topic,
                                                 DataWriterQosWrapper qos,
                                                 OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl_ptr a_listener,
                                                 ::DDS::StatusMask mask) {
  return pub->create_datawriter(topic, qos, a_listener, mask);
}

::DDS::ReturnCode_t Publisher_GetDefaultDataWriterQos(::DDS::Publisher_ptr pub, DataWriterQosWrapper &qos_wrapper) {
  ::DDS::DataWriterQos qos_native;
  ::DDS::ReturnCode_t ret = pub->get_default_datawriter_qos(qos_native);

  if (ret == ::DDS::RETCODE_OK) {
    qos_wrapper = qos_native;
  }

  return ret;
}

::DDS::ReturnCode_t Publisher_SetDefaultDataWriterQos(::DDS::Publisher_ptr pub, DataWriterQosWrapper qos_wrapper) {
  return pub->set_default_datawriter_qos(qos_wrapper);
}

::DDS::ReturnCode_t Publisher_GetQos(::DDS::Publisher_ptr pub, PublisherQosWrapper &qos_wrapper) {
  ::DDS::PublisherQos qos_native;
  ::DDS::ReturnCode_t ret = pub->get_qos(qos_native);

  if (ret == ::DDS::RETCODE_OK) {
    qos_wrapper = qos_native;
  }

  return ret;
}

::DDS::ReturnCode_t Publisher_SetQos(::DDS::Publisher_ptr pub, PublisherQosWrapper qos_wrapper) {
  return pub->set_qos(qos_wrapper);
}

::DDS::ReturnCode_t Publisher_DeleteDataWriter(::DDS::Publisher_ptr pub, ::DDS::DataWriter_ptr dw) {
  return pub->delete_datawriter(dw);
}

::DDS::ReturnCode_t
Publisher_SetListener(::DDS::Publisher_ptr pub, OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl_ptr listener,
                      ::DDS::StatusMask mask) {
  return pub->set_listener(listener, mask);
}

::DDS::DomainParticipant_ptr Publisher_GetParticipant(::DDS::Publisher_ptr pub) {
  return pub->get_participant();
}

::DDS::DataWriter_ptr Publisher_LookupDataWriter(::DDS::Publisher_ptr pub, char *topicName) {
  return pub->lookup_datawriter(topicName);
}

::DDS::ReturnCode_t Publisher_DeleteContainedEntities(::DDS::Publisher_ptr pub) {
  return pub->delete_contained_entities();
}

::DDS::ReturnCode_t Publisher_WaitForAcknowledgments(::DDS::Publisher_ptr pub, ::DDS::Duration_t maxWait) {
  return pub->wait_for_acknowledgments(maxWait);
}

::DDS::ReturnCode_t Publisher_SuspendPublications(::DDS::Publisher_ptr pub) {
  return pub->suspend_publications();
}

::DDS::ReturnCode_t Publisher_ResumePublications(::DDS::Publisher_ptr pub) {
  return pub->resume_publications();
}

::DDS::ReturnCode_t Publisher_BeginCoherentChanges(::DDS::Publisher_ptr pub) {
  return pub->begin_coherent_changes();
}

::DDS::ReturnCode_t Publisher_EndCoherentChanges(::DDS::Publisher_ptr pub) {
  return pub->end_coherent_changes();
}