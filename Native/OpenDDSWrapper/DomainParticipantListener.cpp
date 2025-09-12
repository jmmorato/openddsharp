/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "DomainParticipantListener.h"

OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl_ptr DomainParticipantListener_New(void *onDataOnReaders,
                                                                                           void *onDataAvailable,
                                                                                           void *onRequestedDeadlineMissed,
                                                                                           void *onRequestedIncompatibleQos,
                                                                                           void *onSampleRejected,
                                                                                           void *onLivelinessChanged,
                                                                                           void *onSubscriptionMatched,
                                                                                           void *onSampleLost,
                                                                                           void *onOfferedDeadlineMissed,
                                                                                           void *onOfferedIncompatibleQos,
                                                                                           void *onLivelinessLost,
                                                                                           void *onPublicationMatched,
                                                                                           void *onInconsistentTopic) {
  return new OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl(onDataOnReaders,
                                                                      onDataAvailable,
                                                                      onRequestedDeadlineMissed,
                                                                      onRequestedIncompatibleQos,
                                                                      onSampleRejected,
                                                                      onLivelinessChanged,
                                                                      onSubscriptionMatched,
                                                                      onSampleLost,
                                                                      onOfferedDeadlineMissed,
                                                                      onOfferedIncompatibleQos,
                                                                      onLivelinessLost,
                                                                      onPublicationMatched,
                                                                      onInconsistentTopic);
}

void DomainParticipantListener_Dispose(OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl_ptr ptr) {
  ptr->dispose();
}