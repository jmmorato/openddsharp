#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "InstanceHandle.h"

namespace OpenDDSharp {
	namespace DDS {

		public value struct PublicationMatchedStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			System::Int32 current_count;
			System::Int32 current_count_change;
			OpenDDSharp::DDS::InstanceHandle last_subscription_handle;

		public:
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

			property System::Int32 CurrentCount {
				System::Int32 get();
			};

			property System::Int32 CurrentCountChange {
				System::Int32 get();
			};

			property OpenDDSharp::DDS::InstanceHandle LastSubscriptionHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			};

		internal:
			PublicationMatchedStatus(::DDS::PublicationMatchedStatus status);
			::DDS::PublicationMatchedStatus ToNative();
			void FromNative(::DDS::PublicationMatchedStatus native);
		};
	};
};