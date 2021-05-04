/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

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
using System;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.UnitTest.Listeners
{
    public class MyDataWriterListener : DataWriterListener
    {
        public Action<DataWriter> ConnectionDeleted;
        public Action<DataWriter, LivelinessLostStatus> LivelinessLost;
        public Action<DataWriter, OfferedDeadlineMissedStatus> OfferedDeadlineMissed;
        public Action<DataWriter, OfferedIncompatibleQosStatus> OfferedIncompatibleQos;
        public Action<DataWriter, PublicationDisconnectedStatus> PublicationDisconnected;
        public Action<DataWriter, PublicationLostStatus> PublicationLost;
        public Action<DataWriter, PublicationMatchedStatus> PublicationMatched;
        public Action<DataWriter, PublicationReconnectedStatus> PublicationReconnected;

        public override void OnLivelinessLost(DataWriter writer, LivelinessLostStatus status)
        {
            LivelinessLost?.Invoke(writer, status);
        }

        public override void OnOfferedDeadlineMissed(DataWriter writer, OfferedDeadlineMissedStatus status)
        {
            OfferedDeadlineMissed?.Invoke(writer, status);
        }

        public override void OnOfferedIncompatibleQos(DataWriter writer, OfferedIncompatibleQosStatus status)
        {
            OfferedIncompatibleQos?.Invoke(writer, status);
        }

        public override void OnPublicationDisconnected(DataWriter writer, PublicationDisconnectedStatus status)
        {
            PublicationDisconnected?.Invoke(writer, status);
        }

        public override void OnPublicationLost(DataWriter writer, PublicationLostStatus status)
        {
            PublicationLost?.Invoke(writer, status);
        }

        public override void OnPublicationMatched(DataWriter writer, PublicationMatchedStatus status)
        {
            PublicationMatched?.Invoke(writer, status);
        }

        public override void OnPublicationReconnected(DataWriter writer, PublicationReconnectedStatus status)
        {
            PublicationReconnected?.Invoke(writer, status);
        }
    }
}
