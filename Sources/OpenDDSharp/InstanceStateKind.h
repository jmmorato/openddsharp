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

#include "InstanceStateMask.h"

namespace OpenDDSharp {
	namespace DDS {

		value struct InstanceStateKind;
		ref class DataWriter;
		ref class DataReader;

		/// <summary>
		/// Indicates if the samples are from a live <see cref="DataWriter" /> or not.
		/// </summary>
		public value struct InstanceStateKind {

		public:
			/// <summary>
			/// Indicates that:
			/// <list type="bullet">
			///		<item><description>Samples have been received for the instance</description></item>
			///		<item><description>There are live <see cref="DataWriter" /> entities writing the instance</description></item>
			///		<item><description>The instance has not been explicitly disposed (or else more samples have been received after it was disposed)</description></item>
			///	</list>
			/// </summary>
			static const InstanceStateKind AliveInstanceState = ::DDS::ALIVE_INSTANCE_STATE;

			/// <summary>
			/// Indicates the instance was explicitly disposed by a <see cref="DataWriter" /> by means of the Delete operation.
			/// </summary>
			static const InstanceStateKind NotAliveDisposedInstanceState = ::DDS::NOT_ALIVE_DISPOSED_INSTANCE_STATE;

			/// <summary>
			/// Indicates the instance has been declared as not-alive by the <see cref="DataReader" /> because it detected that 
			/// there are no live <see cref="DataWriter" /> entities writing that instance.
			/// </summary>
			static const InstanceStateKind NotAliveNoWritersInstanceState = ::DDS::NOT_ALIVE_NO_WRITERS_INSTANCE_STATE;

		private:
			System::UInt32 m_value;

		internal:
			InstanceStateKind(System::UInt32 value);

		public:
			/// <summary>
			/// Implicit conversion operator from <see cref="InstanceStateKind" /> to <see cref="System::UInt32" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="System::UInt32" /> value.</returns>
			static operator System::UInt32(InstanceStateKind value) {
				return value.m_value;
			}

			/// <summary>
			/// Implicit conversion operator from <see cref="System::UInt32" /> to <see cref="InstanceStateKind" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="InstanceStateKind" /> value.</returns>
			static operator InstanceStateKind(System::UInt32 value) {
				InstanceStateKind r(value);
				return r;
			}

			/// <summary>
			/// Implicit conversion operator from <see cref="InstanceStateKind" /> to <see cref="InstanceStateMask" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="InstanceStateMask" /> value.</returns>
			static operator InstanceStateMask(InstanceStateKind value) {
				InstanceStateMask r(value);
				return r;
			}

			/// <summary>
			/// Bit-wise operator.
			/// </summary>
			/// <param name="left">The left value of the operator.</param>
			/// <param name="right">The right value of the operator.</param>
			/// <returns>The resulting <see cref="InstanceStateMask" />.</returns>
			static InstanceStateMask operator | (InstanceStateKind left, InstanceStateKind right) {
				return static_cast<InstanceStateMask>(static_cast<unsigned int>(left) | static_cast<unsigned int>(right));
			}
			
		};
	}
};
