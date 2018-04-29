#pragma once

#include "PresentationQosPolicy.h"
#include "PartitionQosPolicy.h"
#include "GroupDataQosPolicy.h"
#include "EntityFactoryQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Holds the <see cref="Subscriber" /> Quality of Service policies.
		/// </summary>
		public ref class SubscriberQos {

		private:
			OpenDDSharp::DDS::PresentationQosPolicy^ presentation;
			OpenDDSharp::DDS::PartitionQosPolicy^ partition;
			OpenDDSharp::DDS::GroupDataQosPolicy^ group_data;
			OpenDDSharp::DDS::EntityFactoryQosPolicy^ entity_factory;

		public:		
			/// <summary>
			/// Gets the presentation QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::PresentationQosPolicy^ Presentation {
				OpenDDSharp::DDS::PresentationQosPolicy^ get();
			};

			/// <summary>
			/// Gets the partition QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::PartitionQosPolicy^ Partition {
				OpenDDSharp::DDS::PartitionQosPolicy^ get();
			};

			/// <summary>
			/// Gets the group data QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::GroupDataQosPolicy^ GroupData {
				OpenDDSharp::DDS::GroupDataQosPolicy^ get();
			};

			/// <summary>
			/// Gets the entity factory QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::EntityFactoryQosPolicy^ EntityFactory {
				OpenDDSharp::DDS::EntityFactoryQosPolicy^ get();
			};

		public:
			/// <summary>
			/// Creates a new instance of <see cref="SubscriberQos" />.
			/// </summary>
			SubscriberQos();		

		internal:
			::DDS::SubscriberQos ToNative();
			void FromNative(::DDS::SubscriberQos qos);
		};

	};
};