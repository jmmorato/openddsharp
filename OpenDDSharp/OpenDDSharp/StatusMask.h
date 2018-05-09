#pragma once

#pragma unmanaged
#include "dds/DdsDcpsInfrastructureC.h"
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		value struct StatusKind;

		/// <summary>
		/// Represent a bit-mask of <see cref="StatusKind" />
		/// </summary>
		public value struct StatusMask {

		public:
			/// <summary>
			/// A mask containing the default <see cref="StatusKind" />
			/// </summary>
			static const StatusMask DefaultStatusMask = ::OpenDDS::DCPS::DEFAULT_STATUS_MASK;

			/// <summary>
			/// A mask containing all <see cref="StatusKind" />
			/// </summary>
			static const StatusMask AllStatusMask = ::OpenDDS::DCPS::ALL_STATUS_MASK;

			/// <summary>
			/// A mask containing none <see cref="StatusKind" />
			/// </summary>
			static const StatusMask NoStatusMask = ::OpenDDS::DCPS::NO_STATUS_MASK;

		private:
			System::UInt32 m_value;

		internal:
			StatusMask(System::UInt32 value);

		public:
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
