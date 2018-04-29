#pragma once

#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		ref class DataReader;

		/// <summary>
		/// The SampleLost status indicates that a sample has been lost and never received by the <see cref="DataReader" />.
		/// </summary>
		public value struct SampleLostStatus {

		private:
			System::Int32 total_count;
			System::Int32 total_count_change;

		public:
			/// <summary>
			/// Gets the cumulative count of samples reported as lost.
			/// </summary>
			property System::Int32 TotalCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the incremental count of lost samples since the last time this status was accessed.
			/// </summary>
			property System::Int32 TotalCountChange {
				System::Int32 get();
			};

		internal:
			SampleLostStatus(::DDS::SampleLostStatus status);
			::DDS::SampleLostStatus ToNative();
			void FromNative(::DDS::SampleLostStatus native);
		};
	};
};
