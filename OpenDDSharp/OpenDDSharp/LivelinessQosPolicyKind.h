#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		ref class Entity;
		ref class DomainParticipant;
		ref class DataWriter;
		ref class LivelinessQosPolicy;

		/// <summary>
		/// This enumeration defines the valid kinds of the <see cref="LivelinessQosPolicy" /> Kind.
		/// </summary>
		public enum class LivelinessQosPolicyKind : System::Int32 {
			/// <summary>
			/// Means that the service will send a liveliness indication if the participant has not sent any network traffic for the LeaseDuration
			/// </summary>
			AutomaticLivelinessQos = ::DDS::AUTOMATIC_LIVELINESS_QOS,
			/// <summary>
			/// requires only that one Entity within the publisher is asserted to be alive to deduce all other <see cref="Entity" /> objects within the 
			/// same <see cref="DomainParticipant" /> are also alive.
			/// </summary>
			ManualByParticipantLivelinessQos = ::DDS::MANUAL_BY_PARTICIPANT_LIVELINESS_QOS,
			/// <summary>
			/// Requires that at least one instance within the <see cref="DataWriter" /> is asserted.
			/// </summary>
			ManualByTopicLivelinessQos = ::DDS::MANUAL_BY_TOPIC_LIVELINESS_QOS
		};
	};
};