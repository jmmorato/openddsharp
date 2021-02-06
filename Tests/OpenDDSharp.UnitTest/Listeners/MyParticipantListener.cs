﻿/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
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
using System;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.UnitTest.Listeners
{
    public class MyParticipantListener : DomainParticipantListener
    {
        public Action<Topic, InconsistentTopicStatus> InconsistentTopic;        
        public Action<DataReader> ConnectionDataReaderDeleted;
        public Action<DataReader> DataAvailable;
        public Action<Subscriber> DataOnReaders;
        public Action<DataReader, LivelinessChangedStatus> LivelinessChanged;
        public Action<DataReader, RequestedDeadlineMissedStatus> RequestedDeadlineMissed;
        public Action<DataReader, RequestedIncompatibleQosStatus> RequestedIncompatibleQos;
        public Action<DataReader, SampleLostStatus> SampleLost;
        public Action<DataReader, SampleRejectedStatus> SampleRejected;
        public Action<DataReader, SubscriptionMatchedStatus> SubscriptionMatched;        
        public Action<DataWriter, LivelinessLostStatus> LivelinessLost;
        public Action<DataWriter, OfferedDeadlineMissedStatus> OfferedDeadlineMissed;
        public Action<DataWriter, OfferedIncompatibleQosStatus> OfferedIncompatibleQos;
        public Action<DataWriter, PublicationMatchedStatus> PublicationMatched;        
        public Action<DataWriter> ConnectionDataWriterDeleted;

        public override void OnInconsistentTopic(Topic topic, InconsistentTopicStatus status)
        {
            InconsistentTopic?.Invoke(topic, status);
        }        

        public override void OnConnectionDeleted(DataReader reader)
        {
            ConnectionDataReaderDeleted?.Invoke(reader);
        }

        public override void OnDataAvailable(DataReader reader)
        {
            DataAvailable?.Invoke(reader);
        }

        public override void OnDataOnReaders(Subscriber subscriber)
        {
            DataOnReaders?.Invoke(subscriber);
        }

        public override void OnLivelinessChanged(DataReader reader, LivelinessChangedStatus status)
        {
            LivelinessChanged?.Invoke(reader, status);
        }

        public override void OnRequestedDeadlineMissed(DataReader reader, RequestedDeadlineMissedStatus status)
        {
            RequestedDeadlineMissed?.Invoke(reader, status);
        }

        public override void OnRequestedIncompatibleQos(DataReader reader, RequestedIncompatibleQosStatus status)
        {
            RequestedIncompatibleQos?.Invoke(reader, status);
        }

        public override void OnSampleLost(DataReader reader, SampleLostStatus status)
        {
            SampleLost?.Invoke(reader, status);
        }

        public override void OnSampleRejected(DataReader reader, SampleRejectedStatus status)
        {
            SampleRejected?.Invoke(reader, status);
        }

        public override void OnSubscriptionMatched(DataReader reader, SubscriptionMatchedStatus status)
        {
            SubscriptionMatched?.Invoke(reader, status);
        }   

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

        public override void OnPublicationMatched(DataWriter writer, PublicationMatchedStatus status)
        {
            PublicationMatched?.Invoke(writer, status);
        }        

        public override void OnConnectionDeleted(DataWriter writer)
        {
            ConnectionDataWriterDeleted?.Invoke(writer);
        }
    }
}
