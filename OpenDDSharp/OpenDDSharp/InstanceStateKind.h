#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

#include "InstanceStateMask.h"

namespace OpenDDSharp {
	namespace DDS {
		public value struct InstanceStateKind {

		public:
			static const InstanceStateKind AliveInstanceState = ::DDS::ALIVE_INSTANCE_STATE;
			static const InstanceStateKind NotAliveDisposedInstanceState = ::DDS::NOT_ALIVE_DISPOSED_INSTANCE_STATE;
			static const InstanceStateKind NotAliveNoWritersInstanceState = ::DDS::NOT_ALIVE_NO_WRITERS_INSTANCE_STATE;

		private:
			System::UInt32 m_value;

		internal:
			InstanceStateKind(System::UInt32 value);

		public:
			static operator System::UInt32(InstanceStateKind self) {
				return self.m_value;
			}

			static operator InstanceStateKind(System::UInt32 value) {
				InstanceStateKind r(value);
				return r;
			}

			static operator InstanceStateMask(InstanceStateKind value) {
				InstanceStateMask r(value);
				return r;
			}

			static InstanceStateMask operator  | (InstanceStateKind a, InstanceStateKind b) {
				return static_cast<InstanceStateMask>(static_cast<unsigned int>(a) | static_cast<unsigned int>(b));
			}
			
		};
	}
};
