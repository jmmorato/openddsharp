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
			/// <summary>
			/// Implicit conversion operator from <see cref="InstanceHandle" /> to <see cref="System::Int32" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="System::Int32" /> value.</returns>
			static operator System::Int32(InstanceHandle value) {
				return value.m_value;
			}

			/// <summary>
			/// Implicit conversion operator from <see cref="System::Int32" /> to <see cref="InstanceHandle" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="InstanceHandle" /> value.</returns>
			static operator InstanceHandle(System::Int32 value) {
				InstanceHandle r(value);
				return r;
			}

            /// <summary>
            /// Equals comparison operator.
            /// </summary>
            /// <param name="x">The left value for the comparison.</param>
            /// <param name="y">The right value for the comparison.</param>
            /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
            static bool operator ==(InstanceHandle x, InstanceHandle y) {
                return x.m_value == y.m_value;
            }

            /// <summary>
            /// Not equals comparison operator.
            /// </summary>
            /// <param name="x">The left value for the comparison.</param>
            /// <param name="y">The right value for the comparison.</param>
            /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
            static bool operator !=(InstanceHandle x, InstanceHandle y) {
                return x.m_value != y.m_value;
            }

            /// <summary>
            /// Determines whether the specified object is equal to the current object.
            /// </summary>
            /// <param name="other">The object to compare with the current object.</param>
            /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>            
            virtual bool Equals(Object^ other) override;
            
            /// <summary>
            /// Serves as the default hash function.
            /// </summary>            
            /// <returns>A hash code for the current object.</returns>
            virtual int GetHashCode() override;
		};
	}
};

