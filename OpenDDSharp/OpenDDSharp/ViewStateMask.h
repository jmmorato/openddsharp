#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		
		value struct  ViewStateKind;

		/// <summary>
		/// Represent a bit-mask of <see cref="ViewStateKind" />
		/// </summary>
		public value struct ViewStateMask {

		public:
			/// <summary>
			/// A mask containing any <see cref="ViewStateKind" />
			/// </summary>
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