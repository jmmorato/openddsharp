#include "TopicListener.h"

::OpenDDSharp::DDS::TopicListener::TopicListener() {
	onInconsistentTopicDelegate^ fpInconsistentTopic = gcnew onInconsistentTopicDelegate(this, &::OpenDDSharp::DDS::TopicListener::onInconsistentTopic);
	System::Runtime::InteropServices::GCHandle gchInconsistentTopic = System::Runtime::InteropServices::GCHandle::Alloc(fpInconsistentTopic);
	System::IntPtr ipInconsistentTopic = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpInconsistentTopic);
	onInconsistentTopicFunctionCpp = static_cast<onInconsistentTopicDeclaration>(ipInconsistentTopic.ToPointer());

	impl_entity = new OpenDDSharp::DDS::TopicListenerNative(onInconsistentTopicFunctionCpp);
}