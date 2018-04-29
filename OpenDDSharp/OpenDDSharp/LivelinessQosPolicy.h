#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "LivelinessQosPolicyKind.h"
#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class Entity;

		/// <summary>
		/// This policy controls the mechanism and parameters used by DDS to ensure that particular entities on the network are still "alive".
		/// </summary>
		/// <remarks>
		/// The value offered is considered compatible with the value requested if and only if the following conditions are met:
		/// <list type="bullet">
		///		<item><description>The inequality "offered Kind >= requested Kind" evaluates to 'true'. For the purposes of this inequality, the values of Liveliness kind are considered ordered such that: Automatic &lt; ManualByParticipant &lt; ManualByTopic.</description></item>
		///		<item><description>The inequality "offered LeaseDuration &lt;= requested LeaseDuration" evaluates to 'true'.</description></item>
		///	</list>
		/// </remarks>
		public ref class LivelinessQosPolicy {

		private:
			OpenDDSharp::DDS::LivelinessQosPolicyKind kind;
			OpenDDSharp::DDS::Duration lease_duration;

		public:
			/// <summary>
			/// Gets or sets the liveliness kind applied to the <see cref="Entity" />
			/// </summary>
			property ::OpenDDSharp::DDS::LivelinessQosPolicyKind Kind {
				::OpenDDSharp::DDS::LivelinessQosPolicyKind get(); 
				void set(::OpenDDSharp::DDS::LivelinessQosPolicyKind value);
			};

			/// <summary>
			/// Gets or sets the liveliness lease duration
			/// </summary>
			property ::OpenDDSharp::DDS::Duration LeaseDuration {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			LivelinessQosPolicy();			

		internal:
			::DDS::LivelinessQosPolicy ToNative();
			void FromNative(::DDS::LivelinessQosPolicy qos);
		};
	};
};

