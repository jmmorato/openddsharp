#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		
		ref class DataReader;
		ref class DataWriter;
		ref class Publisher;
		ref class Subscriber;
		ref class PresentationQosPolicy;

		/// <summary>
		/// This enumeration defines the valid kinds of the <see cref="PresentationQosPolicy" /> AccessScope.
		/// </summary>
		public enum class PresentationQosPolicyAccessScopeKind : System::Int32 {

			/// <summary>
			/// Indicates that changes occur to instances independently. Instance access essentially acts as a no-op with respect to
			/// CoherentAccess and OrderedAccess. Setting either of these values to true has no observable affect within the subscribing application.
			/// </summary>
			InstancePresentationQos = ::DDS::INSTANCE_PRESENTATION_QOS,

			/// <summary>
			/// Indicates that accepted changes are limited to all instances within the same <see cref="DataReader" /> or <see cref="DataWriter" />.
			/// </summary>
			TopicPresentationQos = ::DDS::TOPIC_PRESENTATION_QOS,

			/// <summary>
			/// Indicates that accepted changes are limited to all instances within the same <see cref="Publisher" /> or <see cref="Subscriber" />.
			/// </summary>
			GroupPresentationQos = ::DDS::GROUP_PRESENTATION_QOS
		};
	};
};