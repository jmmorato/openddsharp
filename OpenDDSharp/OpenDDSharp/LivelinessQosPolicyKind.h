#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public enum class LivelinessQosPolicyKind : System::Int32 {
			AutomaticLivelinessQos = ::DDS::AUTOMATIC_LIVELINESS_QOS,
			ManualByParticipantLivelinessQos = ::DDS::MANUAL_BY_PARTICIPANT_LIVELINESS_QOS,
			ManualByTopicLivelinessQos = ::DDS::MANUAL_BY_TOPIC_LIVELINESS_QOS
		};
	};
};