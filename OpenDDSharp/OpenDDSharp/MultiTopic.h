#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsTopicC.h>
#pragma managed

#include "TopicDescription.h"
#include "DomainParticipant.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {
		public ref class MultiTopic : public OpenDDSharp::DDS::TopicDescription {

		internal:
			::DDS::MultiTopic_ptr impl_entity;

		internal:
			MultiTopic(::DDS::MultiTopic_ptr native);

		public:
			System::String^ GetSubscriptionExpression();
			OpenDDSharp::DDS::ReturnCode GetExpressionParameters(ICollection<System::String^>^ params);
			OpenDDSharp::DDS::ReturnCode SetExpressionParameters(ICollection<System::String^>^ params);
		};
	};
};
