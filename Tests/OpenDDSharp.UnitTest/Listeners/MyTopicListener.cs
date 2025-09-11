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
    internal class MyTopicListener : TopicListener
    {
        public Action<Topic, InconsistentTopicStatus> InconsistentTopic { get; set; }

        public override void OnInconsistentTopic(Topic topic, InconsistentTopicStatus status)
        {
            InconsistentTopic?.Invoke(topic, status);
        }
    }
}
