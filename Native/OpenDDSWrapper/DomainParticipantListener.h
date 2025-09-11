/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "DomainParticipantListenerImpl.h"
#include "ListenerDelegates.h"

EXTERN_METHOD_EXPORT
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
                                                                                           void *onInconsistentTopic);

EXTERN_METHOD_EXPORT
void DomainParticipantListener_Dispose(OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl_ptr ptr);