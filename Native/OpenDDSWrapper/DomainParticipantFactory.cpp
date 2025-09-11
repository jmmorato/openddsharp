/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "DomainParticipantFactory.h"

DDS::DomainParticipant_ptr DomainParticipantFactory_CreateParticipant(::DDS::DomainParticipantFactory_ptr dpf,
                                                                      ::DDS::DomainId_t domainId,
                                                                      DomainParticipantQosWrapper qos,
                                                                      OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl_ptr a_listener,
                                                                      ::DDS::StatusMask mask) {
  return dpf->create_participant(domainId, qos, a_listener, mask);
}

::DDS::ReturnCode_t
DomainParticipantFactory_DeleteParticipant(::DDS::DomainParticipantFactory_ptr dpf, ::DDS::DomainParticipant_ptr dp) {
  return dpf->delete_participant(dp);
}

::DDS::ReturnCode_t DomainParticipantFactory_GetDefaultDomainParticipantQos(::DDS::DomainParticipantFactory_ptr dpf,
                                                                            DomainParticipantQosWrapper &qos_wrapper) {
  ::DDS::DomainParticipantQos qos_native;
  ::DDS::ReturnCode_t ret = dpf->get_default_participant_qos(qos_native);

  if (ret == ::DDS::RETCODE_OK) {
    qos_wrapper = qos_native;
  }

  return ret;
}

::DDS::ReturnCode_t DomainParticipantFactory_SetDefaultDomainParticipantQos(::DDS::DomainParticipantFactory_ptr dpf,
                                                                            DomainParticipantQosWrapper qos_wrapper) {
  return dpf->set_default_participant_qos(qos_wrapper);
}

::DDS::ReturnCode_t DomainParticipantFactory_GetQos(::DDS::DomainParticipantFactory_ptr dpf,
                                                    DomainParticipantFactoryQosWrapper &qos_wrapper) {
  ::DDS::DomainParticipantFactoryQos qos_native;
  ::DDS::ReturnCode_t ret = dpf->get_qos(qos_native);

  if (ret == ::DDS::RETCODE_OK) {
    qos_wrapper = qos_native;
  }

  return ret;
}

::DDS::ReturnCode_t DomainParticipantFactory_SetQos(::DDS::DomainParticipantFactory_ptr dpf,
                                                    DomainParticipantFactoryQosWrapper qos_wrapper) {
  return dpf->set_qos(qos_wrapper);
}

::DDS::Entity_ptr
DomainParticipantFactory_LookupParticipant(::DDS::DomainParticipantFactory_ptr dpf, ::DDS::DomainId_t domainId) {
  return static_cast< ::DDS::Entity_ptr>(dpf->lookup_participant(domainId));
}
