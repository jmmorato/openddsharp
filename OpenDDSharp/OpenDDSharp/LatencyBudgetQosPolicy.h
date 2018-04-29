#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// This policy provides a means for the application to indicate to the middleware the “urgency” of the data-communication. By
		/// having a non-zero duration DDS can optimize its internal operation. This policy is considered a hint. There is no specified mechanism as 
		/// to how the service should take advantage of this hint.
		/// </summary>
		public ref class LatencyBudgetQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration duration;

		public:
			/// <summary>
			/// Gets or sets the maximum duration for the latency budget. 
			/// </summary>
			/// <remarks>
			/// The value offered is considered compatible with the value requested if and only if the inequality 
			/// "offered duration &lt;= requested duration" evaluates to 'true'.
			/// </remarks>
			property ::OpenDDSharp::DDS::Duration Duration {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			LatencyBudgetQosPolicy();			

		internal:
			::DDS::LatencyBudgetQosPolicy ToNative();
			void FromNative(::DDS::LatencyBudgetQosPolicy qos);
		};
	};
};
