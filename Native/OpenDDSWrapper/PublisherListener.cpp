/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "PublisherListener.h"

OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl_ptr PublisherListener_New(void *onOfferedDeadlineMissed,
                                                                           void *onOfferedIncompatibleQos,
                                                                           void *onLivelinessLost,
                                                                           void *onPublicationMatched) {
  return new OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl(onOfferedDeadlineMissed,
                                                              onOfferedIncompatibleQos,
                                                              onLivelinessLost,
                                                              onPublicationMatched);
}

void PublisherListener_Dispose(OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl_ptr ptr) {
  ptr->dispose();
}