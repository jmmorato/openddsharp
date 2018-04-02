#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "InstanceHandle.h"

namespace OpenDDSharp {
	namespace DDS {

		public value struct SubscriptionMatchedStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			System::Int32 current_count;
			System::Int32 current_count_change;
			OpenDDSharp::DDS::InstanceHandle last_publication_handle;

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

			property OpenDDSharp::DDS::InstanceHandle LastPublicationHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			};

		internal:
			SubscriptionMatchedStatus(::DDS::SubscriptionMatchedStatus status);
			::DDS::SubscriptionMatchedStatus ToNative();
			void FromNative(::DDS::SubscriptionMatchedStatus native);
		};
	};
};