#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public value struct SampleStateMask {

		public:
			static const SampleStateMask AnySampleState = ::DDS::ANY_SAMPLE_STATE;

		private:
			System::UInt32 m_value;

		internal:
			SampleStateMask(System::UInt32 value);

		public:
			static operator System::UInt32(SampleStateMask self) {
				return self.m_value;
			}

			static operator SampleStateMask(System::UInt32 value) {
				SampleStateMask r(value);
				return r;
			}

		};
	}
};