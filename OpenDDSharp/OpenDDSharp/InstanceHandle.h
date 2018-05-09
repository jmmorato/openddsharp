#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		
		/// <summary>
		/// Type definition for an instance handle.
		/// </summary>
		public value struct InstanceHandle {

		public:
			/// <summary>
			/// Represent a nil instance handle
			/// </summary>
			static const InstanceHandle HandleNil = ::DDS::HANDLE_NIL;

		private:
			System::Int32 m_value;

		internal:
			InstanceHandle(System::Int32 value);

		public:
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

