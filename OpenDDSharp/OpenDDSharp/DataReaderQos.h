#pragma once

#include "DurabilityQosPolicy.h"
#include "DeadlineQosPolicy.h"
#include "LatencyBudgetQosPolicy.h"
#include "LivelinessQosPolicy.h"
#include "ReliabilityQosPolicy.h"
#include "DestinationOrderQosPolicy.h"
#include "HistoryQosPolicy.h"
#include "ResourceLimitsQosPolicy.h"
#include "UserDataQosPolicy.h"
#include "OwnershipQosPolicy.h"
#include "TimeBasedFilterQosPolicy.h"
#include "ReaderDataLifecycleQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class DataReaderQos {

		private:
			OpenDDSharp::DDS::DurabilityQosPolicy^ durability;
			OpenDDSharp::DDS::DeadlineQosPolicy^ deadline;
			OpenDDSharp::DDS::LatencyBudgetQosPolicy^ latency_budget;
			OpenDDSharp::DDS::LivelinessQosPolicy^ liveliness;
			OpenDDSharp::DDS::ReliabilityQosPolicy^ reliability;
			OpenDDSharp::DDS::DestinationOrderQosPolicy^ destination_order;
			OpenDDSharp::DDS::HistoryQosPolicy^ history;
			OpenDDSharp::DDS::ResourceLimitsQosPolicy^ resource_limits;
			OpenDDSharp::DDS::UserDataQosPolicy^ user_data;
			OpenDDSharp::DDS::OwnershipQosPolicy^ ownership;
			OpenDDSharp::DDS::TimeBasedFilterQosPolicy^ time_based_filter;
			OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy^ reader_data_lifecycle;

		public:
			property OpenDDSharp::DDS::DurabilityQosPolicy^ Durability {
				OpenDDSharp::DDS::DurabilityQosPolicy^ get();
			};

			property OpenDDSharp::DDS::DeadlineQosPolicy^ Deadline {
				OpenDDSharp::DDS::DeadlineQosPolicy^ get();
			};

			property OpenDDSharp::DDS::LatencyBudgetQosPolicy^ LatencyBudget {
				OpenDDSharp::DDS::LatencyBudgetQosPolicy^ get();
			};

			property OpenDDSharp::DDS::LivelinessQosPolicy^ Liveliness {
				OpenDDSharp::DDS::LivelinessQosPolicy^ get();
			};

			property OpenDDSharp::DDS::ReliabilityQosPolicy^ Reliability {
				OpenDDSharp::DDS::ReliabilityQosPolicy^ get();
			};

			property OpenDDSharp::DDS::DestinationOrderQosPolicy^ DestinationOrder {
				OpenDDSharp::DDS::DestinationOrderQosPolicy^ get();
			};

			property OpenDDSharp::DDS::HistoryQosPolicy^ History {
				OpenDDSharp::DDS::HistoryQosPolicy^ get();
			};

			property OpenDDSharp::DDS::ResourceLimitsQosPolicy^ ResourceLimits {
				OpenDDSharp::DDS::ResourceLimitsQosPolicy^ get();
			};

			property OpenDDSharp::DDS::UserDataQosPolicy^ UserData {
				OpenDDSharp::DDS::UserDataQosPolicy^ get();
			};

			property OpenDDSharp::DDS::OwnershipQosPolicy^ Ownership {
				OpenDDSharp::DDS::OwnershipQosPolicy^ get();
			};

			property OpenDDSharp::DDS::TimeBasedFilterQosPolicy^ TimeBasedFilter {
				OpenDDSharp::DDS::TimeBasedFilterQosPolicy^ get();
			};

			property OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy^ ReaderDataLifecycle {
				OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy^ get();
			};	

		public:
			DataReaderQos();			

		internal:
			::DDS::DataReaderQos ToNative();
			void FromNative(::DDS::DataReaderQos qos);
		};

	};
};
