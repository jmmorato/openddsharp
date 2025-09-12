/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "DataWriterListener.h"

OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl_ptr DataWriterListener_New(void *onOfferedDeadlineMissed,
                                                                             void *onOfferedIncompatibleQos,
                                                                             void *onLivelinessLost,
                                                                             void *onPublicationMatched) {
  return new OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl(onOfferedDeadlineMissed,
                                                               onOfferedIncompatibleQos,
                                                               onLivelinessLost,
                                                               onPublicationMatched);
}

void DataWriterListener_Dispose(OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl_ptr ptr) {
  ptr->dispose();
}