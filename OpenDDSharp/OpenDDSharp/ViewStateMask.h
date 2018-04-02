#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public value struct ViewStateMask {

		public:
			static const ViewStateMask AnyViewState = ::DDS::ANY_VIEW_STATE;

		private:
			System::UInt32 m_value;

		internal:
			ViewStateMask(System::UInt32 value);

		public:
			static operator System::UInt32(ViewStateMask self) {
				return self.m_value;
			}

			static operator ViewStateMask(System::UInt32 value) {
				ViewStateMask r(value);
				return r;
			}

		};
	}
};