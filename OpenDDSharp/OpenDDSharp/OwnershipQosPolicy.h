#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "OwnershipQosPolicyKind.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class OwnershipQosPolicy {

		private:
			OpenDDSharp::DDS::OwnershipQosPolicyKind kind;

		public:
			property ::OpenDDSharp::DDS::OwnershipQosPolicyKind Kind {
				::OpenDDSharp::DDS::OwnershipQosPolicyKind get();
				void set(::OpenDDSharp::DDS::OwnershipQosPolicyKind value);
			};

		internal:
			OwnershipQosPolicy();			

		internal:
			::DDS::OwnershipQosPolicy ToNative();
			void FromNative(::DDS::OwnershipQosPolicy qos);
		};
	};
};

