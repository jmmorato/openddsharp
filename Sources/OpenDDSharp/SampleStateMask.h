/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

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

namespace OpenDDSharp {
	namespace DDS {

		value struct SampleStateKind;

		/// <summary>
		/// Represent a bit-mask of <see cref="SampleStateKind" />
		/// </summary>
		public value struct SampleStateMask {

		public:
			/// <summary>
			/// A mask containing any <see cref="SampleStateKind" />
			/// </summary>
			static const SampleStateMask AnySampleState = ::DDS::ANY_SAMPLE_STATE;

		private:
			System::UInt32 m_value;

		internal:
			SampleStateMask(System::UInt32 value);

		public:
			/// <summary>
			/// Implicit conversion operator from <see cref="SampleStateMask" /> to <see cref="System::UInt32" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="System::UInt32" /> value.</returns>
			static operator System::UInt32(SampleStateMask value) {
				return value.m_value;
			}

			/// <summary>
			/// Implicit conversion operator from <see cref="System::UInt32" /> to <see cref="SampleStateMask" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="SampleStateMask" /> value.</returns>
			static operator SampleStateMask(System::UInt32 value) {
				SampleStateMask r(value);
				return r;
			}

		};
	}
};