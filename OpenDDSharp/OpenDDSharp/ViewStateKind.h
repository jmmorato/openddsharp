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

#include "ViewStateMask.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Indicates whether or not an instance is new.
		/// </summary>
		public value struct ViewStateKind {

		public:
			/// <summary>
			/// New instance.
			/// </summary>
			static const ViewStateKind NewViewState = ::DDS::NEW_VIEW_STATE;

			/// <summary>
			/// Not a new instance.
			/// </summary>
			static const ViewStateKind NotNewViewState = ::DDS::NOT_NEW_VIEW_STATE;

		private:
			System::UInt32 m_value;	

		internal:
			ViewStateKind(System::UInt32 value);

		public:
			static operator System::UInt32(ViewStateKind self) {
				return self.m_value;
			}

			static operator ViewStateKind(System::UInt32 value) {
				ViewStateKind r(value);
				return r;
			}
	
			static operator ViewStateMask(ViewStateKind value) {
				ViewStateMask r(value);
				return r;
			}

			static ViewStateMask operator  | (ViewStateKind a, ViewStateKind b) {
				return static_cast<ViewStateMask>(static_cast<unsigned int>(a) | static_cast<unsigned int>(b));
			}

		};
	}
};

