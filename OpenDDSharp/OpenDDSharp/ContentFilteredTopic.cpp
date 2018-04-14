#include "ContentFilteredTopic.h"

OpenDDSharp::DDS::ContentFilteredTopic::ContentFilteredTopic(::DDS::ContentFilteredTopic_ptr native) : TopicDescription(native) {
	impl_entity = native;
}

System::String^ OpenDDSharp::DDS::ContentFilteredTopic::FilterExpression::get() {
	return GetFilterExpression();
}

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::ContentFilteredTopic::RelatedTopic::get() {
	return GetRelatedTopic();
}

System::String^ OpenDDSharp::DDS::ContentFilteredTopic::GetFilterExpression() {
	msclr::interop::marshal_context context;

	const char* filter = impl_entity->get_filter_expression();
	if (filter != NULL) {
		return context.marshal_as<System::String^>(filter);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::ContentFilteredTopic::GetExpressionParameters(ICollection<System::String^>^ params) {
	if (params == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	params->Clear();

	msclr::interop::marshal_context context;

	::DDS::StringSeq seq;
	::DDS::ReturnCode_t ret = impl_entity->get_expression_parameters(seq);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < seq.length(); i++) {
			const char * s = seq[i];
			params->Add(context.marshal_as<System::String^>(s));
		}
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::ContentFilteredTopic::SetExpressionParameters(ICollection<System::String^>^ params) {
	if (params == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	msclr::interop::marshal_context context;

	::DDS::StringSeq seq;
	seq.length(params->Count);

	int i = 0;
	for each (System::String^ s in params)
	{
		seq[i] = context.marshal_as<const char *>(s);
		i++;
	}

	return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_expression_parameters(seq);
}

OpenDDSharp::DDS::Topic^ OpenDDSharp::DDS::ContentFilteredTopic::GetRelatedTopic() {
	::DDS::Topic_ptr topic = impl_entity->get_related_topic();

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(topic);
	if (entity != nullptr) {
		return static_cast<OpenDDSharp::DDS::Topic^>(entity);
	}
	else {
		return nullptr;
	}
}