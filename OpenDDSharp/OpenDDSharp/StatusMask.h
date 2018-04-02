#pragma once

#pragma unmanaged
#include "dds/DdsDcpsInfrastructureC.h"
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		/*public enum class StatusMask : System::UInt32 {
			DefaultStatusMask = ::OpenDDS::DCPS::DEFAULT_STATUS_MASK,
			AllStatusMask = ::OpenDDS::DCPS::ALL_STATUS_MASK,
			NoStatusMask = ::OpenDDS::DCPS::NO_STATUS_MASK
		};*/

		public value struct StatusMask {

		public:
			static const StatusMask DefaultStatusMask = ::OpenDDS::DCPS::DEFAULT_STATUS_MASK;
			static const StatusMask AllStatusMask = ::OpenDDS::DCPS::ALL_STATUS_MASK;
			static const StatusMask NoStatusMask = ::OpenDDS::DCPS::NO_STATUS_MASK;

		private:
			System::UInt32 m_value;

		public:
			StatusMask(System::UInt32 value);

			static operator System::UInt32(StatusMask self) {
				return self.m_value;
			}

			static operator StatusMask(System::UInt32 value) {
				StatusMask r(value);
				return r;
			}

		};
	};
};
