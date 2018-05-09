#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "BuiltinTopicKey.h"
#include "DurabilityQosPolicy.h"
#include "DeadlineQosPolicy.h"
#include "LatencyBudgetQosPolicy.h"
#include "LivelinessQosPolicy.h"
#include "ReliabilityQosPolicy.h"
#include "OwnershipQosPolicy.h"
#include "DestinationOrderQosPolicy.h"
#include "UserDataQosPolicy.h"
#include "TimeBasedFilterQosPolicy.h"
#include "PresentationQosPolicy.h"
#include "PartitionQosPolicy.h"
#include "TopicDataQosPolicy.h"
#include "GroupDataQosPolicy.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		ref class DataReader;
		ref class Subscriber;
		ref class Topic;
		ref class DomainParticipant;

		/// <summary>
		/// Class that contains information about available <see cref="DataReader" />s within the system.
		/// </summary>
		/// <remarks>
		/// The DCPSSubscription topic communicates the existence of <see cref="DataReader" />s by means of the SubscriptionBuiltinTopicData datatype. 
		/// Each SubscriptionBuiltinTopicData sample in a Domain represents a datareader in that Domain: a new SubscriptionBuiltinTopicData instance is created when a newly-added 
		/// <see cref="DataReader" /> is enabled, and it is disposed when that <see cref="DataReader" /> is deleted. An updated SubscriptionBuiltinTopicData sample is written each time 
		/// the <see cref="DataReader" /> (or the <see cref="Subscriber" /> to which it belongs) modifies a QoS policy that applies to the entities connected to it.
		/// </remarks>
		public value struct SubscriptionBuiltinTopicData {

		private:
			OpenDDSharp::DDS::BuiltinTopicKey key;
			OpenDDSharp::DDS::BuiltinTopicKey participant_key;
			System::String^ topic_name;
			System::String^ type_name;
			OpenDDSharp::DDS::DurabilityQosPolicy^ durability;
			OpenDDSharp::DDS::DeadlineQosPolicy^ deadline;
			OpenDDSharp::DDS::LatencyBudgetQosPolicy^ latency_budget;
			OpenDDSharp::DDS::LivelinessQosPolicy^ liveliness;
			OpenDDSharp::DDS::ReliabilityQosPolicy^ reliability;
			OpenDDSharp::DDS::OwnershipQosPolicy^ ownership;
			OpenDDSharp::DDS::DestinationOrderQosPolicy^ destination_order;
			OpenDDSharp::DDS::UserDataQosPolicy^ user_data;
			OpenDDSharp::DDS::TimeBasedFilterQosPolicy^ time_based_filter;
			OpenDDSharp::DDS::PresentationQosPolicy^ presentation;
			OpenDDSharp::DDS::PartitionQosPolicy^ partition;
			OpenDDSharp::DDS::TopicDataQosPolicy^ topic_data;
			OpenDDSharp::DDS::GroupDataQosPolicy^ group_data;

		public:
			/// <summary>
			/// Gets the global unique identifier of the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::BuiltinTopicKey Key {
				OpenDDSharp::DDS::BuiltinTopicKey get();
			};

			/// <summary>
			/// Gets the global unique identifier of the <see cref="DomainParticipant" /> to which the <see cref="DataReader" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::BuiltinTopicKey ParticipantKey {
				OpenDDSharp::DDS::BuiltinTopicKey get();
			};

			/// <summary>
			/// Gets the name of the <see cref="Topic" /> used by the <see cref="DataReader" />.
			/// </summary>
			property System::String^ TopicName {
				System::String^ get();
			};

			/// <summary>
			/// Gets the type name of the <see cref="Topic" /> used by the <see cref="DataReader" />.
			/// </summary>
			property System::String^ TypeName {
				System::String^ get();
			};

			/// <summary>
			/// Gets the <see cref="DurabilityQosPolicy" /> attached to the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::DurabilityQosPolicy^ Durability {
				OpenDDSharp::DDS::DurabilityQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="DeadlineQosPolicy" /> attached to the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::DeadlineQosPolicy^ Deadline {
				OpenDDSharp::DDS::DeadlineQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="LatencyBudgetQosPolicy" /> attached to the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::LatencyBudgetQosPolicy^ LatencyBudget {
				OpenDDSharp::DDS::LatencyBudgetQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="LivelinessQosPolicy" /> attached to the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::LivelinessQosPolicy^ Liveliness {
				OpenDDSharp::DDS::LivelinessQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="ReliabilityQosPolicy" /> attached to the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::ReliabilityQosPolicy^ Reliability {
				OpenDDSharp::DDS::ReliabilityQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="OwnershipQosPolicy" /> attached to the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::OwnershipQosPolicy^ Ownership {
				OpenDDSharp::DDS::OwnershipQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="DestinationOrderQosPolicy" /> attached to the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::DestinationOrderQosPolicy^ DestinationOrder {
				OpenDDSharp::DDS::DestinationOrderQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="UserDataQosPolicy" /> attached to the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::UserDataQosPolicy^ UserData {
				OpenDDSharp::DDS::UserDataQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="TimeBasedFilterQosPolicy" /> attached to the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::TimeBasedFilterQosPolicy^ TimeBasedFilter {
				OpenDDSharp::DDS::TimeBasedFilterQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="PresentationQosPolicy" /> attached to the <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::PresentationQosPolicy^ Presentation {
				OpenDDSharp::DDS::PresentationQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="PartitionQosPolicy" /> attached to the  <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::PartitionQosPolicy^ Partition {
				OpenDDSharp::DDS::PartitionQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="TopicDataQosPolicy" /> attached to the  <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::TopicDataQosPolicy^ TopicData {
				OpenDDSharp::DDS::TopicDataQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="GroupDataQosPolicy" /> attached to the  <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::GroupDataQosPolicy^ GroupData {
				OpenDDSharp::DDS::GroupDataQosPolicy^ get();
			};

		internal:
			void FromNative(::DDS::SubscriptionBuiltinTopicData native);
		};
	};
};
