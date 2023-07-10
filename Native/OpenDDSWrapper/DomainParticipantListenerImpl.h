#pragma once
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
#pragma once

#include <dds/DdsDcpsDomainC.h>
#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#include "ListenerDelegates.h"

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DDS {

            class DomainParticipantListenerImpl
                : public virtual ::OpenDDS::DCPS::LocalObject<::DDS::DomainParticipantListener> {
            private:
                ACE_Thread_Mutex _lock;
                bool _disposed = false;

                void *_onDataOnReaders;
                void *_onDataAvailable;
                void *_onRequestedDeadlineMissed;
                void *_onRequestedIncompatibleQos;
                void *_onSampleRejected;
                void *_onLivelinessChanged;
                void *_onSubscriptionMatched;
                void *_onSampleLost;

                void *_onOfferedDeadlineMissed;
                void *_onOfferedIncompatibleQos;
                void *_onLivelinessLost;
                void *_onPublicationMatched;

                void *_onInconsistentTopic;

            public:
                DomainParticipantListenerImpl(void *onDataOnReaders,
                                              void *onDataAvailable,
                                              void *onRequestedDeadlineMissed,
                                              void *onRequestedIncompatibleQos,
                                              void *onSampleRejected,
                                              void *onLivelinessChanged,
                                              void *onSubscriptionMatched,
                                              void *onSampleLost,
                                              void *onOfferedDeadlineMissed,
                                              void *onOfferedIncompatibleQos,
                                              void *onLivelinessLost,
                                              void *onPublicationMatched,
                                              void *onInconsistentTopic);

            protected:
                virtual ~DomainParticipantListenerImpl();

            public:
                /* Subscriber methods */
                virtual void on_data_on_readers(::DDS::Subscriber_ptr subscriber);

                virtual void on_data_available(::DDS::DataReader_ptr reader);

                virtual void on_requested_deadline_missed(::DDS::DataReader_ptr reader,
                                                          const ::DDS::RequestedDeadlineMissedStatus &status);

                virtual void on_requested_incompatible_qos(::DDS::DataReader_ptr reader,
                                                           const ::DDS::RequestedIncompatibleQosStatus &status);

                virtual void
                on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus &status);

                virtual void
                on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus &status);

                virtual void
                on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus &status);

                virtual void on_sample_lost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus &status);

                /* Publisher methods */
                virtual void on_offered_deadline_missed(::DDS::DataWriter_ptr writer,
                                                        const ::DDS::OfferedDeadlineMissedStatus &status);

                virtual void on_offered_incompatible_qos(::DDS::DataWriter_ptr writer,
                                                         const ::DDS::OfferedIncompatibleQosStatus &status);

                virtual void
                on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus &status);

                virtual void
                on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus &status);

                /* Topic methods */
                virtual void
                on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus &status);

                void dispose();
            };

            typedef OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl *DomainParticipantListenerImpl_ptr;

        };
    };
};
