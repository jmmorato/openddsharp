/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#include "ListenerDelegates.h"

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DDS {

            class TopicListenerImpl : public virtual ::OpenDDS::DCPS::LocalObject<::DDS::TopicListener> {
            private:
                ACE_Thread_Mutex _lock;
                bool _disposed = false;

                void *_onInconsistentTopic;

            public:
                TopicListenerImpl(void *onInconsistentTopic);

            protected:
                virtual ~TopicListenerImpl();

            public:
                virtual void
                on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus &status);

                void dispose();
            };

            typedef OpenDDSharp::OpenDDS::DDS::TopicListenerImpl *TopicListenerImpl_ptr;

        };
    };
};
