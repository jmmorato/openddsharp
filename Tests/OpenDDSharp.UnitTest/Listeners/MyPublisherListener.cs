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
    internal class MyPublisherListener : PublisherListener
    {
        public Action<DataWriter> ConnectionDeleted { get; set; }
        public Action<DataWriter, LivelinessLostStatus> LivelinessLost { get; set; }
        public Action<DataWriter, OfferedDeadlineMissedStatus> OfferedDeadlineMissed { get; set; }
        public Action<DataWriter, OfferedIncompatibleQosStatus> OfferedIncompatibleQos { get; set; }
        public Action<DataWriter, PublicationMatchedStatus> PublicationMatched { get; set; }

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
