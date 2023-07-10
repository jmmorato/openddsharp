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