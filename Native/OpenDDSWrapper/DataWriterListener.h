/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "DataWriterListenerImpl.h"
#include "ListenerDelegates.h"

EXTERN_METHOD_EXPORT
OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl_ptr DataWriterListener_New(void *onOfferedDeadlineMissed,
                                                                             void *onOfferedIncompatibleQos,
                                                                             void *onLivelinessLost,
                                                                             void *onPublicationMatched);

EXTERN_METHOD_EXPORT
void DataWriterListener_Dispose(OpenDDSharp::OpenDDS::DDS::DataWriterListenerImpl_ptr ptr);
