#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsTopicC.h>
#pragma managed

#include "DomainParticipant.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		public ref class TopicDescription : ITopicDescription {

		internal:
			::DDS::TopicDescription_ptr impl_entity;

		internal:
			TopicDescription(::DDS::TopicDescription_ptr topicDescription);

		public:
			virtual System::String^ GetTypeName();
			virtual System::String^ GetName();
			virtual OpenDDSharp::DDS::DomainParticipant^ GetParticipant();
			virtual ::DDS::TopicDescription_ptr ToNative();
		};
	};
};