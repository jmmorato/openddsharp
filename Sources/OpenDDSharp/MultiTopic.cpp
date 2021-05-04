/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "MultiTopic.h"
#include "DomainParticipant.h"

OpenDDSharp::DDS::MultiTopic::MultiTopic(::DDS::MultiTopic_ptr native) : TopicDescription(native) {
	impl_entity = ::DDS::MultiTopic::_duplicate(native);
}

OpenDDSharp::DDS::MultiTopic::!MultiTopic() {
    impl_entity = NULL;
}

System::String^ OpenDDSharp::DDS::MultiTopic::SubscriptionExpression::get() {
	return GetSubscriptionExpression();
}

System::String^ OpenDDSharp::DDS::MultiTopic::GetSubscriptionExpression() {
	msclr::interop::marshal_context context;

	const char* expression = impl_entity->get_subscription_expression();	
	return context.marshal_as<System::String^>(expression);	
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::MultiTopic::GetExpressionParameters(IList<System::String^>^ params) {
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

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::MultiTopic::SetExpressionParameters(... array<System::String^>^ params) {
	if (params == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	msclr::interop::marshal_context context;

	::DDS::StringSeq seq;
	seq.length(params->Length);

	int i = 0;
	for each (System::String^ s in params)
	{
		seq[i] = context.marshal_as<const char *>(s);
		i++;
	}

	return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_expression_parameters(seq);
}