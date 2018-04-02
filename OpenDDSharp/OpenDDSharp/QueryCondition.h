#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionC.h>
#pragma managed

#include "DataReader.h"
#include "ReadCondition.h"
#include "SampleStateMask.h"
#include "ViewStateMask.h"
#include "InstanceStateMask.h"
#include "ReturnCode.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {
		public ref class QueryCondition : public ReadCondition {

		internal:
			::DDS::QueryCondition_ptr impl_entity;

		internal:
			QueryCondition(::DDS::QueryCondition_ptr query_condition, OpenDDSharp::DDS::DataReader^ reader);

		public:
			System::String^ GetQueryExpression();
			OpenDDSharp::DDS::ReturnCode GetQueryParameters(List<System::String^>^ queryParameters);
			OpenDDSharp::DDS::ReturnCode SetQueryParameters(List<System::String^>^ queryParameters);

		};
	};
};
