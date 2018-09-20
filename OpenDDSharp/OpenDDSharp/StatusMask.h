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
			/// <summary>
			/// Implicit conversion operator from <see cref="StatusMask" /> to <see cref="System::UInt32" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="System::UInt32" /> value.</returns>
			static operator System::UInt32(StatusMask value) {
				return value.m_value;
			}

			/// <summary>
			/// Implicit conversion operator from <see cref="System::UInt32" /> to <see cref="StatusMask" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="StatusMask" /> value.</returns>
			static operator StatusMask(System::UInt32 value) {
				StatusMask r(value);
				return r;
			}

		};
	};
};
