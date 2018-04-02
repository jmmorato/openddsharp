#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionExtC.h>
#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#pragma managed

#include "LNK4248.h"
#include "DataReader.h"

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			class DataReaderListenerNative : public virtual ::OpenDDS::DCPS::LocalObject<::OpenDDS::DCPS::DataReaderListener> {

			private:				
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
				std::function<void(::DDS::DataReader_ptr)> _onConnectionDeleted;

			public:
				DataReaderListenerNative(std::function<void(::DDS::DataReader_ptr)> onDataAvalaible,
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
									     std::function<void(::DDS::DataReader_ptr)> onConnectionDeleted);

				virtual ~DataReaderListenerNative(void);

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
			};

		};
	};
};