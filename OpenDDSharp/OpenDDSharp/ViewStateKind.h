/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
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
			/// <summary>
			/// Implicit conversion operator from <see cref="ViewStateKind" /> to <see cref="System::UInt32" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="System::UInt32" /> value.</returns>
			static operator System::UInt32(ViewStateKind value) {
				return value.m_value;
			}

			/// <summary>
			/// Implicit conversion operator from <see cref="System::UInt32" /> to <see cref="ViewStateKind" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="ViewStateKind" /> value.</returns>
			static operator ViewStateKind(System::UInt32 value) {
				ViewStateKind r(value);
				return r;
			}
	
			/// <summary>
			/// Implicit conversion operator from <see cref="ViewStateKind" /> to <see cref="ViewStateMask" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="ViewStateMask" /> value.</returns>
			static operator ViewStateMask(ViewStateKind value) {
				ViewStateMask r(value);
				return r;
			}

			/// <summary>
			/// Bit-wise operator.
			/// </summary>
			/// <param name="left">The left value of the operator.</param>
			/// <param name="right">The right value of the operator.</param>
			/// <returns>The resulting <see cref="ViewStateMask" />.</returns>
			static ViewStateMask operator | (ViewStateKind left, ViewStateKind right) {
				return static_cast<ViewStateMask>(static_cast<unsigned int>(left) | static_cast<unsigned int>(right));
			}

		};
	}
};

