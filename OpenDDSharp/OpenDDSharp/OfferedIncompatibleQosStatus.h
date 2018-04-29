#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "QosPolicyCount.h"

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {

		ref class DataReader;

		/// <summary>
		/// The OfferedIncompatibleQos status indicates that an offered QoS was incompatible with the requested QoS of a <see cref="DataReader" />.
		/// </summary>
		public value struct OfferedIncompatibleQosStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;
			System::Int32 last_policy_id;
			IEnumerable<OpenDDSharp::DDS::QosPolicyCount^>^ policies;

		public:
			/// <summary>
			/// Gets the cumulative count of times that data readers with incompatible QoS have been found.
			/// </summary>
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the incremental change in the total count since the last time this status was accessed.
			/// </summary>
			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

			/// <summary>
			/// Gets one of the QoS policies that was incompatible in the last incompatibility detected.
			/// </summary>
			property System::Int32 LastPolicyId {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the sequence of values that indicates the total number of incompatibilities that have been detected for each QoS policy.
			/// </summary>
			property IEnumerable<OpenDDSharp::DDS::QosPolicyCount^>^ Policies {
				IEnumerable<OpenDDSharp::DDS::QosPolicyCount^>^ get();
			};

		internal:
			OfferedIncompatibleQosStatus(::DDS::OfferedIncompatibleQosStatus status);
			::DDS::OfferedIncompatibleQosStatus ToNative();
			void FromNative(::DDS::OfferedIncompatibleQosStatus native);
		};
	};
};