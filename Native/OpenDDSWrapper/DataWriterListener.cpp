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