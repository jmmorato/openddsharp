#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public enum class PresentationQosPolicyAccessScopeKind : System::Int32 {
			InstancePresentationQos = ::DDS::INSTANCE_PRESENTATION_QOS,
			TopicPresentationQos = ::DDS::TOPIC_PRESENTATION_QOS,
			GroupPresentationQos = ::DDS::GROUP_PRESENTATION_QOS
		};
	};
};