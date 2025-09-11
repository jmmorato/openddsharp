/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using System;
using OpenDDSharp.DDS;

namespace OpenDDSharp.UnitTest.Listeners
{
    internal class MyParticipantListener : DomainParticipantListener
    {
        public Action<Topic, InconsistentTopicStatus> InconsistentTopic { get; set; }
        public Action<DataReader> DataAvailable { get; set; }
        public Action<Subscriber> DataOnReaders { get; set; }
        public Action<DataReader, LivelinessChangedStatus> LivelinessChanged { get; set; }
        public Action<DataReader, RequestedDeadlineMissedStatus> RequestedDeadlineMissed { get; set; }
        public Action<DataReader, RequestedIncompatibleQosStatus> RequestedIncompatibleQos { get; set; }
        public Action<DataReader, SampleLostStatus> SampleLost { get; set; }
        public Action<DataReader, SampleRejectedStatus> SampleRejected { get; set; }
        public Action<DataReader, SubscriptionMatchedStatus> SubscriptionMatched { get; set; }
        public Action<DataWriter, LivelinessLostStatus> LivelinessLost { get; set; }
        public Action<DataWriter, OfferedDeadlineMissedStatus> OfferedDeadlineMissed { get; set; }
        public Action<DataWriter, OfferedIncompatibleQosStatus> OfferedIncompatibleQos { get; set; }
        public Action<DataWriter, PublicationMatchedStatus> PublicationMatched { get; set; }

        public override void OnInconsistentTopic(Topic topic, InconsistentTopicStatus status)
        {
            InconsistentTopic?.Invoke(topic, status);
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
    }
}
