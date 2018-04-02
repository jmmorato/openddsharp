#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsTopicC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		ref class DomainParticipant;
		public interface class ITopicDescription {
			System::String^ GetTypeName();
			System::String^ GetName();
			OpenDDSharp::DDS::DomainParticipant^ GetParticipant();
			::DDS::TopicDescription_ptr ToNative();
		};
	};
};