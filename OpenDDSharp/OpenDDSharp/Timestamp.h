#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#pragma make_public(::DDS::SampleInfo)

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Structure for timestamp representation
		/// </summary>
		public value struct Timestamp {

		private:
			System::Int32 sec;
			System::UInt32 nanosec;

		public:
			/// <summary>
			/// The seconds 
			/// </summary>
			property System::Int32 Seconds {
				System::Int32 get();
				void set(System::Int32 value);
			};

			/// <summary>
			/// The nanosseconds 
			/// </summary>
			property System::UInt32 NanoSeconds {
				System::UInt32 get();
				void set(System::UInt32 value);
			};

		internal:
			::DDS::Time_t ToNative();
			void FromNative(::DDS::Time_t qos);
		};

	};
};