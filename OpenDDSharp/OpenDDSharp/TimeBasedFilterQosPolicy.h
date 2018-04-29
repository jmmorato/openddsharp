#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataReader;
		ref class Topic;

		/// <summary>
		/// This policy allows a <see cref="DataReader" /> to indicate that it does not necessarily want to see all values of each instance published under
		/// the <see cref="Topic" />. Rather, it wants to see at most one change every <see cref="MinimumSeparation" /> period.
		/// </summary>
		/// <remarks>
		/// This QoS policy does not conserve bandwidth as instance value changes are still sent to the subscriber process. 
		/// It only affects which samples are made available via the <see cref="DataReader" />.
		/// </remarks>
		public ref class TimeBasedFilterQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration minimum_separation;

		public:
			/// <summary>
			/// This interval defines a minimum delay between instance value changes; this permits the <see cref="DataReader" /> to throttle
			/// changes without affecting the state of the associated data writer. By default, MinimumSeparation is zero, which indicates that no data is filtered.
			/// </summary>
			property ::OpenDDSharp::DDS::Duration MinimumSeparation {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			TimeBasedFilterQosPolicy();

		internal:
			::DDS::TimeBasedFilterQosPolicy ToNative();
			void FromNative(::DDS::TimeBasedFilterQosPolicy qos);
		};
	};
};
