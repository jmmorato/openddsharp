#include "QueryCondition.h"

OpenDDSharp::DDS::QueryCondition::QueryCondition(::DDS::QueryCondition_ptr query_condition, OpenDDSharp::DDS::DataReader^ reader) : ReadCondition(query_condition, reader) {
	impl_entity = query_condition;	
}


System::String^ OpenDDSharp::DDS::QueryCondition::QueryExpression::get() {
	return GetQueryExpression();
}

System::String^ OpenDDSharp::DDS::QueryCondition::GetQueryExpression() {
	msclr::interop::marshal_context context;

	const char * s = impl_entity->get_query_expression();
	return context.marshal_as<System::String^>(s);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::QueryCondition::GetQueryParameters(IList<System::String^>^ queryParameters) {
	if (queryParameters == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	queryParameters->Clear();

	msclr::interop::marshal_context context;

	::DDS::StringSeq seq;
	::DDS::ReturnCode_t ret = impl_entity->get_query_parameters(seq);

	if (ret == ::DDS::RETCODE_OK) {
		System::UInt32 i = 0;
		while (i < seq.length()) {
			const char * s = seq[i];
			queryParameters->Add(context.marshal_as<System::String^>(s));
			i++;
		}
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::QueryCondition::SetQueryParameters(... array<System::String^>^ queryParameters) {
	if (queryParameters == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	msclr::interop::marshal_context context;

	::DDS::StringSeq seq;
	seq.length(queryParameters->Length);

	System::Int32 i = 0;
	while(i < queryParameters->Length)
	{
		seq[i] = context.marshal_as<const char*>(queryParameters[i]);
		i++;
	}

	return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_query_parameters(seq);
}