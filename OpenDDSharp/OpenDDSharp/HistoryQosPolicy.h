#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "HistoryQosPolicyKind.h"
#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// This policy controls the behavior of DDS when the value of an instance changes before it is finally communicated to some of its existing <see cref="DataReader" /> entities.
		/// </summary>
		/// <remarks>
		/// <para>This policy defaults to a "keep last" with a depth of one.</para>
		/// <para>The setting of History Depth must be consistent with the ResourceLimits MaxSamplesPerInstance. 
		/// For these two QoS to be consistent, they must verify that Depth <= MaxSamplesPerInstance.</para>
		/// <remarks>
		public ref class HistoryQosPolicy {

		private:
			OpenDDSharp::DDS::HistoryQosPolicyKind kind;
			System::Int32 depth;

		public:
			/// <summary>
			/// Gets or sets the history kind applied to the <see cref="Entity" />
			/// </summary>
			property ::OpenDDSharp::DDS::HistoryQosPolicyKind Kind {
				::OpenDDSharp::DDS::HistoryQosPolicyKind get();
				void set(::OpenDDSharp::DDS::HistoryQosPolicyKind value);
			};

			/// <summary>
			/// Gets or sets the history maximum depth.
			/// </summary>
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

