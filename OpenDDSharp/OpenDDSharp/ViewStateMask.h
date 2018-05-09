/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
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