#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionExtC.h>
#pragma managed

#include "InstanceHandle.h"

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			ref class DataReader;
			ref class LatencyBudgetQosPolicy;

			/// <summary>
			/// The BudgetExceeded status indicates delays in excess of the <see cref="LatencyBudgetQosPolicy" /> duration.
			/// </summary>
			public value struct BudgetExceededStatus {

			private:
				System::Int32 total_count;
				System::Int32 total_count_change;
				System::Int32 last_instance_handle;

			public:
				/// <summary>
				/// Gets the cumulative count of reported latency budget exceeded.
				/// </summary>
				property System::Int32 TotalCount {
					System::Int32 get();
				};

				/// <summary>
				/// Gets the incremental count of latency budget exceeded reported since the last time this status was accessed.
				/// </summary>
				property System::Int32 TotalCountChange {
					System::Int32 get();
				};

				/// <summary>
				/// Gets the instance handle of the last latency budged exceeded.
				/// </summary>
				property OpenDDSharp::DDS::InstanceHandle LastInstanceHandle {
					OpenDDSharp::DDS::InstanceHandle get();
				};

			internal:
				BudgetExceededStatus(::OpenDDS::DCPS::BudgetExceededStatus status);
			};

		};
	};
};