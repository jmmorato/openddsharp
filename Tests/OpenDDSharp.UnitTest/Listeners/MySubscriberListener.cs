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
    internal class MySubscriberListener : SubscriberListener
    {
        public Action<DataReader> DataAvailable { get; set; }
        public Action<Subscriber> DataOnReaders { get; set; }
        public Action<DataReader, LivelinessChangedStatus> LivelinessChanged { get; set; }
        public Action<DataReader, RequestedDeadlineMissedStatus> RequestedDeadlineMissed { get; set; }
        public Action<DataReader, RequestedIncompatibleQosStatus> RequestedIncompatibleQos { get; set; }
        public Action<DataReader, SampleLostStatus> SampleLost { get; set; }
        public Action<DataReader, SampleRejectedStatus> SampleRejected { get; set; }
        public Action<DataReader, SubscriptionMatchedStatus> SubscriptionMatched { get; set; }

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
    }
}
