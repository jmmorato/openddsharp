#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "DestinationOrderQosPolicyKind.h"
#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class Entity;

		/// <summary>
		/// This policy controls how each <see cref="Subscriber" /> resolves the final value of a data instance that is written by multiple <see cref="DataWriter" />
		/// objects (which may be associated with different <see cref="Publisher" /> objects) running on different nodes.
		/// </summary>
		/// <remarks>
		/// The value offered is considered compatible with the value requested if and only if the inequality "offered kind >= requested
		/// kind" evaluates to 'true'. For the purposes of this inequality, the values of DestinationOrder kind are considered
		/// ordered such that ByReceptionTimestamp < BySourceTimestamp.
		/// </remarks>
		public ref class DestinationOrderQosPolicy {

		private:
			OpenDDSharp::DDS::DestinationOrderQosPolicyKind kind;			

		public:
			/// <summary>
			/// Gets or sets the destination order kind applied to the <see cref="Entity" />
			/// </summary>
			property ::OpenDDSharp::DDS::DestinationOrderQosPolicyKind Kind {
				::OpenDDSharp::DDS::DestinationOrderQosPolicyKind get();
				void set(::OpenDDSharp::DDS::DestinationOrderQosPolicyKind value);
			};

		internal:
			DestinationOrderQosPolicy();			

		internal:
			::DDS::DestinationOrderQosPolicy ToNative();
			void FromNative(::DDS::DestinationOrderQosPolicy qos);
		};
	};
};

