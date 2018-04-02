#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public value struct InstanceHandle {

		public:
			static const InstanceHandle HandleNil = ::DDS::HANDLE_NIL;

		private:
			System::Int32 m_value;

		public:
			InstanceHandle(System::Int32 value);

			static operator System::Int32(InstanceHandle self) {
				return self.m_value;
			}

			static operator InstanceHandle(System::Int32 value) {
				InstanceHandle r(value);
				return r;
			}

		};
	}
};

