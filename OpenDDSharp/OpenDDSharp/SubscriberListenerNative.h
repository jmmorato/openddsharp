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

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionExtC.h>
#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#pragma managed

#include "LNK4248.h"
#include "Subscriber.h"

namespace OpenDDSharp {
	namespace DDS {		

		class SubscriberListenerNative : public virtual ::OpenDDS::DCPS::LocalObject<::DDS::SubscriberListener> {

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

		public:
			SubscriberListenerNative(std::function<void(::DDS::Subscriber_ptr)> onDataOnReaders,
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
									 std::function<void(::DDS::DataReader_ptr, ::OpenDDS::DCPS::BudgetExceededStatus)> onBudgetExceeded);
			
			virtual ~SubscriberListenerNative(void);

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
			
		};
	};
};
