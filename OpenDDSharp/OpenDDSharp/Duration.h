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
#include <dds\DdsDcpsCoreC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Structure for duration representation
		/// </summary>
		public value struct Duration {

		public:
			/// <summary>
			/// Infinite seconds duration
			/// </summary>
			static const System::Int32 InfiniteSeconds = ::DDS::DURATION_INFINITE_SEC;

			/// <summary>
			/// Infinite nanoseconds duration
			/// </summary>
			static const System::UInt32 InfiniteNanoseconds = ::DDS::DURATION_INFINITE_NSEC;

			/// <summary>
			/// Zero seconds duration
			/// </summary>
			static const System::Int32 ZeroSeconds = ::DDS::DURATION_ZERO_SEC;

			/// <summary>
			/// Zero nanoseconds duration
			/// </summary>
			static const System::UInt32 ZeroNanoseconds = ::DDS::DURATION_ZERO_NSEC;

		private:
			System::Int32 sec;
			System::UInt32 nanosec;

		public:
			/// <summary>
			/// The seconds 
			/// </summary>
			property System::Int32 Seconds {
				System::Int32 get();
				void set(System::Int32 value);
			};

			/// <summary>
			/// The nanosseconds 
			/// </summary>
			property System::UInt32 NanoSeconds {
				System::UInt32 get();
				void set(System::UInt32 value);
			};
		
		internal:
			::DDS::Duration_t ToNative();
			void FromNative(::DDS::Duration_t qos);
		};

	};
};