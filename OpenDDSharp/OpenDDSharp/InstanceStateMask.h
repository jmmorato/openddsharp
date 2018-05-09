#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		value struct InstanceStateKind;
		 
		/// <summary>
		/// Represent a bit-mask of <see cref="InstanceStateKind" />
		/// </summary>
		public value struct InstanceStateMask {

		public:
			/// <summary>
			/// A mask containing any <see cref="InstanceStateKind" />
			/// </summary>
			static const InstanceStateMask AnyInstanceState = ::DDS::ANY_INSTANCE_STATE;

			/// <summary>
			/// A mask containing not alive <see cref="InstanceStateKind" /> (i.e. NotAliveDisposedInstanceState and NotAliveNoWritersInstanceState)
			/// </summary>
			static const InstanceStateMask NotAliveInstanceState = ::DDS::NOT_ALIVE_INSTANCE_STATE;

		private:
			System::UInt32 m_value;

		internal:
			InstanceStateMask(System::UInt32 value);

		public:
			static operator System::UInt32(InstanceStateMask self) {
				return self.m_value;
			}

			static operator InstanceStateMask(System::UInt32 value) {
				InstanceStateMask r(value);
				return r;
			}

		};
	}
};
