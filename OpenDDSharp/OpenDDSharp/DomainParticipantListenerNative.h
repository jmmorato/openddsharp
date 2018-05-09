/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsDomainC.h>
#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#pragma managed

#include "DataReader.h"
#include "SubscriberListener.h"
#include "DataWriter.h"
#include "PublisherListener.h"
#include "Topic.h"
#include "TopicListener.h"

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			class DomainParticipantListenerNative : public virtual ::OpenDDS::DCPS::LocalObject<::DDS::DomainParticipantListener> {

			private:
				std::function<void(::DDS::Subscriber_ptr)> _onDataOnReaders;
				std::function<void(::DDS::DataReader_ptr)> _onDataAvalaible;
				std::function<void(::DDS::DataReader_ptr, ::DDS::RequestedDeadlineMissedStatus)> _onRequestedDeadlineMissed;
				std::function<void(::DDS::DataReader_ptr, ::DDS::RequestedIncompatibleQosStatus)> _onRequestedIncompatibleQos;
				std::function<void(::DDS::DataReader_ptr, ::DDS::SampleRejectedStatus)> _onSampleRejected;
				std::function<void(::DDS::DataReader_ptr, ::DDS::LivelinessChangedStatus)> _onLivelinessChanged;
				std::function<void(::DDS::DataReader_ptr, ::DDS::SubscriptionMatchedStatus)> _onSubscriptionMatched;
				std::function<void(::DDS::DataReader_ptr, ::DDS::SampleLostStatus)> _onSampleLost;
				std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionDisconnectedStatus)> _onSubscriptionDisconnected;
				std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionReconnectedStatus)> _onSubscriptionReconnected;
				std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionLostStatus)> _onSubscriptionLost;
				std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::BudgetExceededStatus)> _onBudgetExceeded;
				std::function<void(::DDS::DataReader_ptr)> _onReaderConnectionDeleted;

				std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> _onOfferedDeadlineMissed;
				std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> _onOfferedIncompatibleQos;
				std::function<void(::DDS::DataWriter_ptr writer, ::DDS::LivelinessLostStatus status)> _onLivelinessLost;
				std::function<void(::DDS::DataWriter_ptr writer, ::DDS::PublicationMatchedStatus status)> _onPublicationMatched;
				std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationDisconnectedStatus)> _onPublicationDisconnected;
				std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationReconnectedStatus)> _onPublicationReconnected;
				std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationLostStatus)> _onPublicationLost;
				std::function<void(::DDS::DataWriter_ptr)> _onWriterConnectionDeleted;

				std::function<void(::DDS::Topic_ptr topic, ::DDS::InconsistentTopicStatus status)> _onInconsistentTopic;

			public:
				DomainParticipantListenerNative(std::function<void(::DDS::Subscriber_ptr)> onDataOnReaders,
												std::function<void(::DDS::DataReader_ptr)> onDataAvalaible,
												std::function<void(::DDS::DataReader_ptr, ::DDS::RequestedDeadlineMissedStatus)> onRequestedDeadlineMissed,
												std::function<void(::DDS::DataReader_ptr, ::DDS::RequestedIncompatibleQosStatus)> onRequestedIncompatibleQos,
												std::function<void(::DDS::DataReader_ptr, ::DDS::SampleRejectedStatus)> onSampleRejected,
												std::function<void(::DDS::DataReader_ptr, ::DDS::LivelinessChangedStatus)> onLivelinessChanged,
												std::function<void(::DDS::DataReader_ptr, ::DDS::SubscriptionMatchedStatus)> onSubscriptionMatched,
												std::function<void(::DDS::DataReader_ptr, ::DDS::SampleLostStatus)> onSampleLost,
												std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionDisconnectedStatus)> onSubscriptionDisconnected,
												std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionReconnectedStatus)> onSubscriptionReconnected,
												std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::SubscriptionLostStatus)> onSubscriptionLost,
												std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::BudgetExceededStatus)> onBudgetExceeded,
												std::function<void(::DDS::DataReader_ptr)> onReaderConnectionDeleted,
												std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> onOfferedDeadlineMissed,
												std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> onOfferedIncompatibleQos,
												std::function<void(::DDS::DataWriter_ptr writer, ::DDS::LivelinessLostStatus status)> onLivelinessLost,
												std::function<void(::DDS::DataWriter_ptr writer, ::DDS::PublicationMatchedStatus status)> onPublicationMatched,
												std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationDisconnectedStatus)> onPublicationDisconnected,
												std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationReconnectedStatus)> onPublicationReconnected,
												std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationLostStatus)> onPublicationLost,
												std::function<void(::DDS::DataWriter_ptr)> onWriterConnectionDeleted,
												std::function<void(::DDS::Topic_ptr topic, ::DDS::InconsistentTopicStatus status)> onInconsistentTopic);

				virtual ~DomainParticipantListenerNative(void);

				/* Subscriber methods */
				virtual void on_data_on_readers(::DDS::Subscriber_ptr subscriber);

				virtual void on_data_available(::DDS::DataReader_ptr reader);

				virtual void on_requested_deadline_missed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus & status);

				virtual void on_requested_incompatible_qos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus & status);

				virtual void on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status);

				virtual void on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus & status);

				virtual void on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus & status);

				virtual void on_sample_lost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status);

				virtual void on_subscription_disconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionDisconnectedStatus & status);

				virtual void on_subscription_reconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionReconnectedStatus & status);

				virtual void on_subscription_lost(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionLostStatus & status);

				virtual void on_budget_exceeded(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::BudgetExceededStatus& status);

				virtual void on_connection_deleted(::DDS::DataReader_ptr);

				/* Publisher methods */
				virtual void on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status);

				virtual void on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status);

				virtual void on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status);

				virtual void on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status);

				virtual void on_publication_disconnected(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationDisconnectedStatus & status);

				virtual void on_publication_reconnected(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationReconnectedStatus & status);

				virtual void on_publication_lost(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationLostStatus & status);

				virtual void on_connection_deleted(::DDS::DataWriter_ptr);

				/* Topic methods */
				virtual void on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus & status);
			};

		};
	};
};