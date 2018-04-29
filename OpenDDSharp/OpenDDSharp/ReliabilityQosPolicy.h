#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "ReliabilityQosPolicyKind.h"
#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataReader;
		ref class DataWriter;
		ref class Entity;

		/// <summary>
		/// This policy indicates the level of reliability requested by a <see cref="DataReader" /> or offered by a <see cref="DataWriter" />.
		/// </summary>
		/// <remarks>
		/// This policy is considered during the creation of associations between data writers and data readers. The value of both sides of the association must be compatible in order for an
		/// association to be created. The value offered is considered compatible with the value requested if and only if the inequality "offered kind >= requested kind" evaluates to 'true'. 
		/// For the purposes of this inequality, the values of Reliability kind are considered ordered such that BestEffort &lt; Reliable.
		/// </remarks>
		public ref class ReliabilityQosPolicy {

		private:
			OpenDDSharp::DDS::ReliabilityQosPolicyKind kind;
			OpenDDSharp::DDS::Duration max_blocking_time;

		public:
			/// <summary>
			/// Gets or sets the reliability kind applied to the <see cref="Entity" />
			/// </summary>
			property ::OpenDDSharp::DDS::ReliabilityQosPolicyKind Kind {
				::OpenDDSharp::DDS::ReliabilityQosPolicyKind get();
				void set(::OpenDDSharp::DDS::ReliabilityQosPolicyKind value);
			};

			/// <summary>
			/// Gets or sets the maximum blocking time when the history QoS policy is set to
			/// "keep all" and the writer is unable to proceed because of resource limits.
			/// </summary>
			property ::OpenDDSharp::DDS::Duration MaxBlockingTime {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			ReliabilityQosPolicy();			

		internal:
			::DDS::ReliabilityQosPolicy ToNative();
			void FromNative(::DDS::ReliabilityQosPolicy qos);
		};
	};
};

