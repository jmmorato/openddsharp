#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "DurabilityQosPolicyKind.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class Entity;
		ref class Topic;
		ref class DataReader;
		ref class DataWriter;

		/// <summary>
		/// The durability policy controls whether data writers should maintain samples after they
		/// have been sent to known subscribers. This policy applies to the <see cref="Topic" />, <see cref="DataReader" />, and 
		/// <see cref="DataWriter" /> entities via the durability member of their respective QoS structures.
		/// </summary>
		/// <remarks>
		/// <para>The value offered is considered compatible with the value requested if and only if the inequality "offered kind >= requested
		/// kind" evaluates to 'true'. For the purposes of this inequality, the values of Durability kind are considered ordered such
		/// that Volatile &lt; TransientLocal &lt; Transient &lt; Persistent.</para>
		/// </remarks>
		public ref class DurabilityQosPolicy {

		private:
			::OpenDDSharp::DDS::DurabilityQosPolicyKind kind;

		public:
			/// <summary>
			/// Gets or sets the <see cref="DurabilityQosPolicyKind" /> assigned to the related <see cref="Entity" />
			/// </summary>
			property ::OpenDDSharp::DDS::DurabilityQosPolicyKind Kind {
				::OpenDDSharp::DDS::DurabilityQosPolicyKind get();
				void set(::OpenDDSharp::DDS::DurabilityQosPolicyKind value);
			}			

		internal:
			DurabilityQosPolicy();			

		internal:
			::DDS::DurabilityQosPolicy ToNative();
			void FromNative(::DDS::DurabilityQosPolicy qos);
		};
	};
};

