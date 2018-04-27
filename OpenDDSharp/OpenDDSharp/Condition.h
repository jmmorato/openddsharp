#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		ref class GuardCondition;
		ref class StatusCondition;
		ref class ReadCondition;

		/// <summary>
		/// A Condition is a root class for all the conditions that may be attached to a WaitSet. This basic class is specialized in three
		///	classes that are known by the middleware: <see cref="GuardCondition" />, <see cref="StatusCondition" />, and <see cref="ReadCondition" />
		/// </summary>
		public ref class Condition {

		internal:
			::DDS::Condition_ptr impl_entity;

		public:
			/// <summary>
			/// Gets the trigger value of the <see cref="Condition" />
			/// </summary>
			property System::Boolean TriggerValue {
				System::Boolean get();
			}

		internal:
			Condition(::DDS::Condition_ptr condition);

		private:
			System::Boolean GetTriggerValue();

		};
	};
};