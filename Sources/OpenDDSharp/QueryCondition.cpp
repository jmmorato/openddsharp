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
#include "QueryCondition.h"

OpenDDSharp::DDS::QueryCondition::QueryCondition(::DDS::QueryCondition_ptr query_condition, OpenDDSharp::DDS::DataReader^ reader) : ReadCondition(static_cast<::DDS::ReadCondition_ptr>(query_condition), reader) {
	impl_entity = ::DDS::QueryCondition::_duplicate(query_condition);
}

OpenDDSharp::DDS::QueryCondition::!QueryCondition() {
    impl_entity = NULL;
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

    // get_query_parameters always returns OK, don't need to check it
	System::UInt32 i = 0;
	while (i < seq.length()) {
		const char * s = seq[i];
		queryParameters->Add(context.marshal_as<System::String^>(s));
		i++;
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