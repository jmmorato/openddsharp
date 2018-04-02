#pragma once

#include "PresentationQosPolicy.h"
#include "PartitionQosPolicy.h"
#include "GroupDataQosPolicy.h"
#include "EntityFactoryQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class SubscriberQos {

		private:
			OpenDDSharp::DDS::PresentationQosPolicy^ presentation;
			OpenDDSharp::DDS::PartitionQosPolicy^ partition;
			OpenDDSharp::DDS::GroupDataQosPolicy^ group_data;
			OpenDDSharp::DDS::EntityFactoryQosPolicy^ entity_factory;

		public:			
			property OpenDDSharp::DDS::PresentationQosPolicy^ Presentation {
				OpenDDSharp::DDS::PresentationQosPolicy^ get();
			};

			property OpenDDSharp::DDS::PartitionQosPolicy^ Partition {
				OpenDDSharp::DDS::PartitionQosPolicy^ get();
			};

			property OpenDDSharp::DDS::GroupDataQosPolicy^ GroupData {
				OpenDDSharp::DDS::GroupDataQosPolicy^ get();
			};

			property OpenDDSharp::DDS::EntityFactoryQosPolicy^ EntityFactory {
				OpenDDSharp::DDS::EntityFactoryQosPolicy^ get();
			};

		public:
			SubscriberQos();		

		internal:
			::DDS::SubscriberQos ToNative();
			void FromNative(::DDS::SubscriberQos qos);
		};

	};
};