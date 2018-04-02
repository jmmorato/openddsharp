#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public value struct Duration {

		public:
			static const System::Int32 DurationInfiniteSec = ::DDS::DURATION_INFINITE_SEC;
			static const System::UInt32 DurationInfiniteNsec = ::DDS::DURATION_INFINITE_NSEC;
			static const System::Int32 DurationZeroSec = ::DDS::DURATION_ZERO_SEC;
			static const System::UInt32 DurationZeraoNsec = ::DDS::DURATION_ZERO_NSEC;

		private:
			System::Int32 sec;
			System::UInt32 nanosec;

		public:
			property System::Int32 Seconds {
				System::Int32 get();
				void set(System::Int32 value);
			};

			property System::UInt32 NanoSeconds {
				System::UInt32 get();
				void set(System::UInt32 value);
			};
		
		internal:
			::DDS::Duration_t ToNative();
			void FromNative(::DDS::Duration_t qos);
		};

	};
};