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

#include "SampleStateMask.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Indicates whether or not a sample has ever been read.
		/// </summary>
		public value struct SampleStateKind {

		public:
			/// <summary>
			/// Sample has been read. 
			/// </summary>
			static const SampleStateKind ReadSampleState = ::DDS::READ_SAMPLE_STATE;

			/// <summary>
			/// Sample has not been read.
			/// </summary>
			static const SampleStateKind NotReadSampleState = ::DDS::NOT_READ_SAMPLE_STATE;			

		private:
			System::UInt32 m_value;

		internal:
			SampleStateKind(System::UInt32 value);

		public:
			static operator System::UInt32(SampleStateKind self) {
				return self.m_value;
			}

			static operator SampleStateKind(System::UInt32 value) {
				SampleStateKind r(value);
				return r;
			}

			static operator SampleStateMask(SampleStateKind value) {
				SampleStateMask r(value);
				return r;
			}

			static SampleStateMask operator  | (SampleStateKind a, SampleStateKind b) {
				return static_cast<SampleStateMask>(static_cast<unsigned int>(a) | static_cast<unsigned int>(b));
			}

		};
	}
};


