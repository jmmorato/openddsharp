#include "TopicListenerNative.h"

::OpenDDSharp::DDS::TopicListenerNative::TopicListenerNative(std::function<void(::DDS::Topic_ptr topic, ::DDS::InconsistentTopicStatus status)> onInconsistentTopic) {
	_onInconsistentTopic = onInconsistentTopic;
}

::OpenDDSharp::DDS::TopicListenerNative::~TopicListenerNative() {
	_onInconsistentTopic = nullptr;
};

void ::OpenDDSharp::DDS::TopicListenerNative::on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus & status) {
	if (_onInconsistentTopic != nullptr)
		_onInconsistentTopic(topic, status);
};