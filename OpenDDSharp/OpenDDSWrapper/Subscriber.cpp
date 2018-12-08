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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "Subscriber.h"

::DDS::DataReader_ptr Subscriber_CreateDataReader(
    ::DDS::Subscriber_ptr sub,
    ::DDS::Topic_ptr topic,
    DataReaderQosWrapper* qos,
    ::DDS::DataReaderListener_ptr a_listener,
    ::DDS::StatusMask mask)
{
    return sub->create_datareader(topic, DATAREADER_QOS_DEFAULT, NULL, ::OpenDDS::DCPS::DEFAULT_STATUS_MASK);
}