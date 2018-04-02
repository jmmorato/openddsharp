#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "HistoryQosPolicyKind.h"
#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class HistoryQosPolicy {

		private:
			OpenDDSharp::DDS::HistoryQosPolicyKind kind;
			System::Int32 depth;

		public:
			property ::OpenDDSharp::DDS::HistoryQosPolicyKind Kind {
				::OpenDDSharp::DDS::HistoryQosPolicyKind get();
				void set(::OpenDDSharp::DDS::HistoryQosPolicyKind value);
			};

			property System::Int32 Depth {
				System::Int32 get();
				void set(System::Int32 value);
			};

		internal:
			HistoryQosPolicy();			

		internal:
			::DDS::HistoryQosPolicy ToNative();
			void FromNative(::DDS::HistoryQosPolicy qos);
		};
	};
};

