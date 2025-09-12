/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "DataReaderListenerImpl.h"
#include "ListenerDelegates.h"

EXTERN_METHOD_EXPORT
OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl_ptr DataReaderListener_New(void *onDataAvailable,
                                                                             void *onRequestedDeadlineMissed,
                                                                             void *onRequestedIncompatibleQos,
                                                                             void *onSampleRejected,
                                                                             void *onLivelinessChanged,
                                                                             void *onSubscriptionMatched,
                                                                             void *onSampleLost);

EXTERN_METHOD_EXPORT
void DataReaderListener_Dispose(OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl_ptr ptr);