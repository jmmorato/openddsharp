#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "BuiltinTopicKey.h"
#include "DurabilityQosPolicy.h"
#include "DurabilityServiceQosPolicy.h"
#include "DeadlineQosPolicy.h"
#include "LatencyBudgetQosPolicy.h"
#include "LivelinessQosPolicy.h"
#include "ReliabilityQosPolicy.h"
#include "TransportPriorityQosPolicy.h"
#include "LifespanQosPolicy.h"
#include "DestinationOrderQosPolicy.h"
#include "HistoryQosPolicy.h"
#include "ResourceLimitsQosPolicy.h"
#include "OwnershipQosPolicy.h"
#include "TopicDataQosPolicy.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {
		public value struct TopicBuiltinTopicData {

		private:
			OpenDDSharp::DDS::BuiltinTopicKey key;
			System::String^ name;
			System::String^ type_name;
			OpenDDSharp::DDS::DurabilityQosPolicy^ durability;
			OpenDDSharp::DDS::DurabilityServiceQosPolicy^ durability_service;
			OpenDDSharp::DDS::DeadlineQosPolicy^ deadline;
			OpenDDSharp::DDS::LatencyBudgetQosPolicy^ latency_budget;
			OpenDDSharp::DDS::LivelinessQosPolicy^ liveliness;
			OpenDDSharp::DDS::ReliabilityQosPolicy^ reliability;
			OpenDDSharp::DDS::TransportPriorityQosPolicy^ transport_priority;
			OpenDDSharp::DDS::LifespanQosPolicy^ lifespan;
			OpenDDSharp::DDS::DestinationOrderQosPolicy^ destination_order;
			OpenDDSharp::DDS::HistoryQosPolicy^ history;
			OpenDDSharp::DDS::ResourceLimitsQosPolicy^ resource_limits;
			OpenDDSharp::DDS::OwnershipQosPolicy^ ownership;
			OpenDDSharp::DDS::TopicDataQosPolicy^ topic_data;

		public:
			property OpenDDSharp::DDS::BuiltinTopicKey Key {
				OpenDDSharp::DDS::BuiltinTopicKey get();
			};

			property System::String^ Name {
				System::String^ get();
			};

			property System::String^ TypeName {
				System::String^ get();
			};

			property OpenDDSharp::DDS::DurabilityQosPolicy^ Durability {
				OpenDDSharp::DDS::DurabilityQosPolicy^ get();
			};

			property OpenDDSharp::DDS::DurabilityServiceQosPolicy^ DurabilityService {
				OpenDDSharp::DDS::DurabilityServiceQosPolicy^ get();
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

			property OpenDDSharp::DDS::TransportPriorityQosPolicy^ TransportPriority {
				OpenDDSharp::DDS::TransportPriorityQosPolicy^ get();
			};

			property OpenDDSharp::DDS::LifespanQosPolicy^ Lifespan {
				OpenDDSharp::DDS::LifespanQosPolicy^ get();
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

			property OpenDDSharp::DDS::OwnershipQosPolicy^ Ownership {
				OpenDDSharp::DDS::OwnershipQosPolicy^ get();
			};

			property OpenDDSharp::DDS::TopicDataQosPolicy^ TopicData {
				OpenDDSharp::DDS::TopicDataQosPolicy^ get();
			};

		internal:
			void FromNative(::DDS::TopicBuiltinTopicData native);

		};
	};
};
