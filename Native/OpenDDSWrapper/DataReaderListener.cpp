/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#include "DataReaderListener.h"

OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl_ptr DataReaderListener_New(void *onDataAvailable,
                                                                             void *onRequestedDeadlineMissed,
                                                                             void *onRequestedIncompatibleQos,
                                                                             void *onSampleRejected,
                                                                             void *onLivelinessChanged,
                                                                             void *onSubscriptionMatched,
                                                                             void *onSampleLost) {
  return new OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl(onDataAvailable,
                                                               onRequestedDeadlineMissed,
                                                               onRequestedIncompatibleQos,
                                                               onSampleRejected,
                                                               onLivelinessChanged,
                                                               onSubscriptionMatched,
                                                               onSampleLost);
}

void DataReaderListener_Dispose(OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl_ptr ptr) {
  ptr->dispose();
}