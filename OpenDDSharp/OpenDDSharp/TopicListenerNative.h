#pragma once

#pragma unmanaged 
#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		class TopicListenerNative : public virtual ::OpenDDS::DCPS::LocalObject<::DDS::TopicListener> {

		private:
			std::function<void(::DDS::Topic_ptr topic, ::DDS::InconsistentTopicStatus status)> _onInconsistentTopic;

		public:
			TopicListenerNative(std::function<void(::DDS::Topic_ptr topic, ::DDS::InconsistentTopicStatus status)> onInconsistentTopic);

			virtual ~TopicListenerNative(void);

			virtual void on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus & status);
		};
	};
};
