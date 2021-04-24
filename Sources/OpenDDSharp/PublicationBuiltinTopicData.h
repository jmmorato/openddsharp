/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

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
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "BuiltinTopicKey.h"
#include "DurabilityQosPolicy.h"
#include "DurabilityServiceQosPolicy.h"
#include "DeadlineQosPolicy.h"
#include "LatencyBudgetQosPolicy.h"
#include "LivelinessQosPolicy.h"
#include "ReliabilityQosPolicy.h"
#include "LifespanQosPolicy.h"
#include "UserDataQosPolicy.h"
#include "OwnershipQosPolicy.h"
#include "OwnershipStrengthQosPolicy.h"
#include "DestinationOrderQosPolicy.h"
#include "PresentationQosPolicy.h"
#include "PartitionQosPolicy.h"
#include "TopicDataQosPolicy.h"
#include "GroupDataQosPolicy.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {
		
		ref class DataWriter;
		ref class Publisher;
		ref class Topic;
		ref class DomainParticipant;

		/// <summary>
		/// Class that contains information about available <see cref="DataWriter" />s within the system.
		/// </summary>
		/// <remarks>
		/// The DCPSPublication topic communicates the existence of datawriters by means of the PublicationBuiltinTopicData datatype. 
		/// Each PublicationBuiltinTopicData sample in a Domain represents a datawriter in that Domain: a new PublicationBuiltinTopicData instance is created when a 
		/// newly-added <see cref="DataWriter" /> is enabled, and it is disposed when that <see cref="DataWriter" /> is deleted. An updated PublicationBuiltinTopicData 
		/// sample is written each time the <see cref="DataWriter" /> (or the <see cref="Publisher" /> to which it belongs) modifies a QoS policy that applies to the entities connected to it. 
		/// Also will it be updated when the writer looses or regains its liveliness.
		/// </remarks>
		public value struct PublicationBuiltinTopicData {

		private:
			OpenDDSharp::DDS::BuiltinTopicKey key;
			OpenDDSharp::DDS::BuiltinTopicKey participant_key;
			System::String^ topic_name;
			System::String^ type_name;
			OpenDDSharp::DDS::DurabilityQosPolicy^ durability;
			OpenDDSharp::DDS::DurabilityServiceQosPolicy^ durability_service;
			OpenDDSharp::DDS::DeadlineQosPolicy^ deadline;
			OpenDDSharp::DDS::LatencyBudgetQosPolicy^ latency_budget;
			OpenDDSharp::DDS::LivelinessQosPolicy^ liveliness;
			OpenDDSharp::DDS::ReliabilityQosPolicy^ reliability;
			OpenDDSharp::DDS::LifespanQosPolicy^ lifespan;
			OpenDDSharp::DDS::UserDataQosPolicy^ user_data;
			OpenDDSharp::DDS::OwnershipQosPolicy^ ownership;
			OpenDDSharp::DDS::OwnershipStrengthQosPolicy^ ownership_strength;
			OpenDDSharp::DDS::DestinationOrderQosPolicy^ destination_order;
			OpenDDSharp::DDS::PresentationQosPolicy^ presentation;
			OpenDDSharp::DDS::PartitionQosPolicy^ partition;
			OpenDDSharp::DDS::TopicDataQosPolicy^ topic_data;
			OpenDDSharp::DDS::GroupDataQosPolicy^ group_data;

		public:
			/// <summary>
			/// Gets the global unique identifier of the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::BuiltinTopicKey Key {
				OpenDDSharp::DDS::BuiltinTopicKey get();
			};

			/// <summary>
			/// Gets the global unique identifier of the <see cref="DomainParticipant" /> to which the <see cref="DataWriter" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::BuiltinTopicKey ParticipantKey {
				OpenDDSharp::DDS::BuiltinTopicKey get();
			};

			/// <summary>
			/// Gets the name of the <see cref="Topic" /> used by the <see cref="DataWriter" />.
			/// </summary>
			property System::String^ TopicName {
				System::String^ get();
			};

			/// <summary>
			/// Gets the type name of the Topic used by the <see cref="DataWriter" />.
			/// </summary>
			property System::String^ TypeName {
				System::String^ get();
			};

			/// <summary>
			/// Gets the <see cref="DurabilityQosPolicy" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::DurabilityQosPolicy^ Durability {
				OpenDDSharp::DDS::DurabilityQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="DurabilityQosPolicy" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::DurabilityServiceQosPolicy^ DurabilityService {
				OpenDDSharp::DDS::DurabilityServiceQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="DeadlineQosPolicy" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::DeadlineQosPolicy^ Deadline {
				OpenDDSharp::DDS::DeadlineQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="LatencyBudgetQosPolicy" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::LatencyBudgetQosPolicy^ LatencyBudget {
				OpenDDSharp::DDS::LatencyBudgetQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="LivelinessQosPolicy" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::LivelinessQosPolicy^ Liveliness {
				OpenDDSharp::DDS::LivelinessQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="Reliability" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::ReliabilityQosPolicy^ Reliability {
				OpenDDSharp::DDS::ReliabilityQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="LifespanQosPolicy" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::LifespanQosPolicy^ Lifespan {
				OpenDDSharp::DDS::LifespanQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="UserDataQosPolicy" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::UserDataQosPolicy^ UserData {
				OpenDDSharp::DDS::UserDataQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="OwnershipQosPolicy" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::OwnershipQosPolicy^ Ownership {
				OpenDDSharp::DDS::OwnershipQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="OwnershipStrengthQosPolicy" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::OwnershipStrengthQosPolicy^ OwnershipStrength {
				OpenDDSharp::DDS::OwnershipStrengthQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="DestinationOrderQosPolicy" /> attached to the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::DestinationOrderQosPolicy^ DestinationOrder {
				OpenDDSharp::DDS::DestinationOrderQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="PresentationQosPolicy" /> attached to the <see cref="Publisher" /> to which the <see cref="DataWriter" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::PresentationQosPolicy^ Presentation {
				OpenDDSharp::DDS::PresentationQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="PartitionQosPolicy" /> attached to the <see cref="Publisher" /> to which the <see cref="DataWriter" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::PartitionQosPolicy^ Partition {
				OpenDDSharp::DDS::PartitionQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="TopicDataQosPolicy" /> attached to the <see cref="Publisher" /> to which the <see cref="DataWriter" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::TopicDataQosPolicy^ TopicData {
				OpenDDSharp::DDS::TopicDataQosPolicy^ get();
			};

			/// <summary>
			/// Gets the <see cref="GroupDataQosPolicy" /> attached to the <see cref="Publisher" /> to which the <see cref="DataWriter" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::GroupDataQosPolicy^ GroupData {
				OpenDDSharp::DDS::GroupDataQosPolicy^ get();
			};

		internal:
			void FromNative(::DDS::PublicationBuiltinTopicData native);
			::DDS::PublicationBuiltinTopicData ToNative();
		};
	};
};