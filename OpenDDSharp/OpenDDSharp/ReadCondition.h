#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionC.h>
#pragma managed

#include "Condition.h"
#include "SampleStateMask.h"
#include "ViewStateMask.h"
#include "InstanceStateMask.h"
#include "DataReader.h"

namespace OpenDDSharp {
	namespace DDS {
		public ref class ReadCondition : public Condition {

		internal:
			::DDS::ReadCondition_ptr impl_entity;
			OpenDDSharp::DDS::DataReader^ data_reader;

		internal:
			ReadCondition(::DDS::ReadCondition_ptr read_condition, OpenDDSharp::DDS::DataReader^ reader);

		public:
			OpenDDSharp::DDS::SampleStateMask GetSampleStateMask();
			OpenDDSharp::DDS::ViewStateMask GetViewStateMask();
			OpenDDSharp::DDS::InstanceStateMask GetInstanceStateMask();
			OpenDDSharp::DDS::DataReader^ GetDatareader();
		};
	};
};