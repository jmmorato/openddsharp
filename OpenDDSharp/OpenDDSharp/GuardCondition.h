#pragma once

#pragma unmanaged 
#include <dds/DCPS/GuardCondition.h>
#pragma managed

#include "Condition.h"
#include "ReturnCode.h"
#include "WaitSet.h"

namespace OpenDDSharp {
	namespace DDS {
		public ref class GuardCondition : public OpenDDSharp::DDS::Condition {

		internal:
			::DDS::GuardCondition_ptr impl_entity;

		internal:
			GuardCondition(::DDS::GuardCondition_ptr native);
		public:
			GuardCondition();

		public:
			System::Boolean GetTriggerValue();
			OpenDDSharp::DDS::ReturnCode SetTriggerValue(System::Boolean value);
			OpenDDSharp::DDS::ReturnCode AttachToWaitSet(OpenDDSharp::DDS::WaitSet^ ws);
			OpenDDSharp::DDS::ReturnCode DetachFromWaitSet(OpenDDSharp::DDS::WaitSet^ ws);
			void SignalAll();

		};
	};
};