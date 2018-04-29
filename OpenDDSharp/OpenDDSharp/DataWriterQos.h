#pragma once

#include "DurabilityQosPolicy.h"
#include "DurabilityServiceQosPolicy.h"
#include "DeadlineQosPolicy.h"
#include "LatencyBudgetQosPolicy.h"
#include "LivelinessQosPolicy.h"
#include "ReliabilityQosPolicy.h"
#include "DestinationOrderQosPolicy.h"
#include "HistoryQosPolicy.h"
#include "ResourceLimitsQosPolicy.h"
#include "TransportPriorityQosPolicy.h"
#include "LifespanQosPolicy.h"
#include "OwnershipQosPolicy.h"
#include "UserDataQosPolicy.h"
#include "OwnershipStrengthQosPolicy.h"
#include "WriterDataLifecycleQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Holds the <see cref="DataWriter" /> Quality of Service policies.
		/// </summary>
		public ref class DataWriterQos {

		private:
			OpenDDSharp::DDS::DurabilityQosPolicy^ durability;
			OpenDDSharp::DDS::DurabilityServiceQosPolicy^ durability_service;
			OpenDDSharp::DDS::DeadlineQosPolicy^ deadline;
			OpenDDSharp::DDS::LatencyBudgetQosPolicy^ latency_budget;
			OpenDDSharp::DDS::LivelinessQosPolicy^ liveliness;
			OpenDDSharp::DDS::ReliabilityQosPolicy^ reliability;
			OpenDDSharp::DDS::DestinationOrderQosPolicy^ destination_order;
			OpenDDSharp::DDS::HistoryQosPolicy^ history;
			OpenDDSharp::DDS::ResourceLimitsQosPolicy^ resource_limits;
			OpenDDSharp::DDS::TransportPriorityQosPolicy^ transport_priority;
			OpenDDSharp::DDS::LifespanQosPolicy^ lifespan;
			OpenDDSharp::DDS::OwnershipQosPolicy^ ownership;
			OpenDDSharp::DDS::UserDataQosPolicy^ user_data;			
			OpenDDSharp::DDS::OwnershipStrengthQosPolicy^ ownership_strength;
			OpenDDSharp::DDS::WriterDataLifecycleQosPolicy^ writer_data_lifecycle;

		public:
			/// <summary>
			/// Gets the durability QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::DurabilityQosPolicy^ Durability {
				OpenDDSharp::DDS::DurabilityQosPolicy^ get();
			};

			/// <summary>
			/// Gets the durability service QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::DurabilityServiceQosPolicy^ DurabilityService {
				OpenDDSharp::DDS::DurabilityServiceQosPolicy^ get();
			};

			/// <summary>
			/// Gets the deadline QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::DeadlineQosPolicy^ Deadline {
				OpenDDSharp::DDS::DeadlineQosPolicy^ get();
			};

			/// <summary>
			/// Gets the latency budget QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::LatencyBudgetQosPolicy^ LatencyBudget {
				OpenDDSharp::DDS::LatencyBudgetQosPolicy^ get();
			};

			/// <summary>
			/// Gets the liveliness QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::LivelinessQosPolicy^ Liveliness {
				OpenDDSharp::DDS::LivelinessQosPolicy^ get();
			};

			/// <summary>
			/// Gets the reliability QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::ReliabilityQosPolicy^ Reliability {
				OpenDDSharp::DDS::ReliabilityQosPolicy^ get();
			};

			/// <summary>
			/// Gets the destination order QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::DestinationOrderQosPolicy^ DestinationOrder {
				OpenDDSharp::DDS::DestinationOrderQosPolicy^ get();
			};

			/// <summary>
			/// Gets the history QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::HistoryQosPolicy^ History {
				OpenDDSharp::DDS::HistoryQosPolicy^ get();
			};

			/// <summary>
			/// Gets the resource limits QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::ResourceLimitsQosPolicy^ ResourceLimits {
				OpenDDSharp::DDS::ResourceLimitsQosPolicy^ get();
			};

			/// <summary>
			/// Gets the transport priority QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::TransportPriorityQosPolicy^ TransportPriority {
				OpenDDSharp::DDS::TransportPriorityQosPolicy^ get();
			};

			/// <summary>
			/// Gets the lifspan QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::LifespanQosPolicy^ Lifespan {
				OpenDDSharp::DDS::LifespanQosPolicy^ get();
			};

			/// <summary>
			/// Gets the ownership QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::OwnershipQosPolicy^ Ownership {
				OpenDDSharp::DDS::OwnershipQosPolicy^ get();
			};

			/// <summary>
			/// Gets the user data QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::UserDataQosPolicy^ UserData {
				OpenDDSharp::DDS::UserDataQosPolicy^ get();
			};

			/// <summary>
			/// Gets the ownership strength QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::OwnershipStrengthQosPolicy^ OwnershipStrength {
				OpenDDSharp::DDS::OwnershipStrengthQosPolicy^ get();
			};

			/// <summary>
			/// Gets the writer data lifecycle QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::WriterDataLifecycleQosPolicy^ WriterDataLifecycle {
				OpenDDSharp::DDS::WriterDataLifecycleQosPolicy^ get();
			};

		public:
			/// <summary>
			/// Creates a new instance of <see cref="DataWriterQos" />.
			/// </summary>
			DataWriterQos();

		internal:
			::DDS::DataWriterQos ToNative();
			void FromNative(::DDS::DataWriterQos qos);
		};

	};
};
