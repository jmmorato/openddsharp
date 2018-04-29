#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "OwnershipQosPolicyKind.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class Entity;

		/// <summary>
		/// This policy controls whether DDS allows multiple <see cref="DataWriter" /> objects to update the same instance (identified by Topic + key) of a data-object.
		/// </summary>
		public ref class OwnershipQosPolicy {

		private:
			OpenDDSharp::DDS::OwnershipQosPolicyKind kind;

		public:
			/// <summary>
			/// Gets or sets the ownership kind applied to the <see cref="Entity" />
			/// </summary>
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

