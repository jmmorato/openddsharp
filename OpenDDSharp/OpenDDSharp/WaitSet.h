#pragma once

#pragma unmanaged 
#include <dds/DCPS/WaitSet.h>
#pragma managed

#include "ReturnCode.h"
#include "Duration.h"
#include "Condition.h"

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {
		public ref class WaitSet {

		internal:
			::DDS::WaitSet_ptr impl_entity;

		private:
			ICollection<Condition^>^ conditions;

		public:
			WaitSet();

		internal:
			WaitSet(::DDS::WaitSet_ptr waitSet);
				
		public:
			OpenDDSharp::DDS::ReturnCode Wait(ICollection<Condition^>^ activeConditions);
			OpenDDSharp::DDS::ReturnCode Wait(ICollection<Condition^>^ activeConditions, OpenDDSharp::DDS::Duration timeout);
			OpenDDSharp::DDS::ReturnCode AttachCondition(Condition^ cond);
			OpenDDSharp::DDS::ReturnCode DetachCondition(Condition^ cond);
			OpenDDSharp::DDS::ReturnCode GetConditions(ICollection<Condition^>^ attachedConditions);
			OpenDDSharp::DDS::ReturnCode DetachConditions(ICollection<Condition^>^ conditions);

		};
	};
};
