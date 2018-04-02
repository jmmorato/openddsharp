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
		public ref class ContentFilteredTopic : public OpenDDSharp::DDS::TopicDescription {

		internal:
			::DDS::ContentFilteredTopic_ptr impl_entity;

		internal:
			ContentFilteredTopic(::DDS::ContentFilteredTopic_ptr native);

		public:
			System::String^ GetFilterExpression();
			OpenDDSharp::DDS::ReturnCode GetExpressionParameters(ICollection<System::String^>^ params);
			OpenDDSharp::DDS::ReturnCode SetExpressionParameters(ICollection<System::String^>^ params);
			OpenDDSharp::DDS::Topic^ GetRelatedTopic();
		};
	};
};
