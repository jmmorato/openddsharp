#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		ref class DataWriter;

		/// <summary>
		/// The LivelinessLost status indicates that the liveliness that the data writer committed through its Liveliness QoS has not been respected.
		/// This means that any connected data readers will consider this <see cref="DataWriter" /> no longer active.
		/// </summary>
		public value struct LivelinessLostStatus {

		private:
			System::Int32 total_count;		
			System::Int32 total_count_change;

		public:
			/// <summary>
			/// Gets the cumulative count of times that an alive data writer has become not alive.
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

		internal:
			LivelinessLostStatus(::DDS::LivelinessLostStatus status);
			::DDS::LivelinessLostStatus ToNative();
			void FromNative(::DDS::LivelinessLostStatus native);
		};
	};
};
