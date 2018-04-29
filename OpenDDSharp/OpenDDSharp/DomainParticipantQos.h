#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "UserDataQosPolicy.h"
#include "EntityFactoryQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Holds the <see cref="DomainParticipant" /> Quality of Service policies.
		/// </summary>
		public ref class DomainParticipantQos {		

		private:
			OpenDDSharp::DDS::UserDataQosPolicy^ user_data;
			OpenDDSharp::DDS::EntityFactoryQosPolicy^ entity_factory;
	
		public:
			/// <summary>
			/// Gets the user data QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::UserDataQosPolicy^ UserData {
				OpenDDSharp::DDS::UserDataQosPolicy^ get();				
			};

			/// <summary>
			/// Gets the entity factory QoS policy.
			/// </summary>
			property OpenDDSharp::DDS::EntityFactoryQosPolicy^ EntityFactory {
				OpenDDSharp::DDS::EntityFactoryQosPolicy^ get();			
			};

		public:
			/// <summary>
			/// Creates a new instance of <see cref="DomainParticipantQos" />.
			/// </summary>
			DomainParticipantQos();						

		internal:
			::DDS::DomainParticipantQos ToNative();
			void FromNative(::DDS::DomainParticipantQos qos);

		};
	};
};
