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

#include "TopicDataQosPolicy.h"
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

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Holds the <see cref="Topic" /> Quality of Service policies.
		/// </summary>
		public ref class TopicQos {

		private:
			OpenDDSharp::DDS::TopicDataQosPolicy^ topic_data;
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

		public:
			/// <summary>
			/// Gets the topic data QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::TopicDataQosPolicy^ TopicData {
				OpenDDSharp::DDS::TopicDataQosPolicy^ get();
			};

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
			/// Gets the lifespan QoS policy.
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

		public:
			/// <summary>
			/// Creates a new instance of <see cref="TopicQos" />.
			/// </summary>
			TopicQos();			

		internal:
			::DDS::TopicQos ToNative();
			void FromNative(::DDS::TopicQos qos);
		};

	};
};
