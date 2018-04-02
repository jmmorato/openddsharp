#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "QosPolicyCount.h"

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {

		public value struct OfferedIncompatibleQosStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			System::Int32 last_policy_id;
			IEnumerable<OpenDDSharp::DDS::QosPolicyCount^>^ policies;

		public:
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

			property System::Int32 LastPolicyId {
				System::Int32 get();
			};

			property IEnumerable<OpenDDSharp::DDS::QosPolicyCount^>^ Policies {
				IEnumerable<OpenDDSharp::DDS::QosPolicyCount^>^ get();
			};

		internal:
			OfferedIncompatibleQosStatus(::DDS::OfferedIncompatibleQosStatus status);
			::DDS::OfferedIncompatibleQosStatus ToNative();
			void FromNative(::DDS::OfferedIncompatibleQosStatus native);
		};
	};
};