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
#include "dds/DdsDcpsInfrastructureC.h"
#pragma managed

#include "StatusMask.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataWriter;
		ref class DataReader;
		ref class Topic;
		ref class DeadlineQosPolicy;
		ref class LivelinessQosPolicy; 

		/// <summary>
		/// Kinds of communication status.
		/// </summary>
		public value struct StatusKind {

		public:
			/// <summary>
			/// Another topic exists with the same name but different characteristics.
			/// </summary>
			static const StatusKind InconsistentTopicStatus = ::DDS::INCONSISTENT_TOPIC_STATUS;

			/// <summary>
			/// The deadline that the <see cref="DataWriter" /> has committed through its <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.
			/// </summary>
			static const StatusKind OfferedDeadlineMissedStatus = ::DDS::OFFERED_DEADLINE_MISSED_STATUS;

			/// <summary>
			/// The deadline that the <see cref="DataReader" /> was expecting through its <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.
			/// </summary>
			static const StatusKind RequestedDeadlineMissedStatus = ::DDS::REQUESTED_DEADLINE_MISSED_STATUS;

			/// <summary>
			/// A QoS policy value was incompatible with what was requested.
			/// </summary>
			static const StatusKind OfferedIncompatibleQosStatus = ::DDS::OFFERED_INCOMPATIBLE_QOS_STATUS;

			/// <summary>
			/// A QoS policy value was incompatible with what is offered.
			/// </summary>
			static const StatusKind RequestedIncompatibleQosStatus = ::DDS::REQUESTED_INCOMPATIBLE_QOS_STATUS;

			/// <summary>
			/// A sample has been lost (i.e. was never received).
			/// </summary>
			static const StatusKind SampleLostStatus = ::DDS::SAMPLE_LOST_STATUS;

			/// <summary>
			/// A (received) sample has been rejected.
			/// </summary>
			static const StatusKind SampleRejectedStatus = ::DDS::SAMPLE_REJECTED_STATUS;

			/// <summary>
			/// New data is available.
			/// </summary>
			static const StatusKind DataOnReadersStatus = ::DDS::DATA_ON_READERS_STATUS;

			/// <summary>
			/// One or more new data samples have been received.
			/// </summary>
			static const StatusKind DataAvailableStatus = ::DDS::DATA_AVAILABLE_STATUS;

			/// <summary>
			/// The liveliness that the <see cref="DataWriter" /> has committed to through its  <see cref="LivelinessQosPolicy" /> was not respected, 
			/// thus <see cref="DataReader" /> entities will consider the <see cref="DataWriter" /> as no longer alive.
			/// </summary>
			static const StatusKind LivelinessLostStatus = ::DDS::LIVELINESS_LOST_STATUS;

			/// <summary>
			/// The liveliness of one or more <see cref="DataWriter" /> that were writing instances read through the <see cref="DataReader" /> has changed. 
			/// Some <see cref="DataWriter" /> have become alive or not alive.
			/// </summary>
			static const StatusKind LivelinessChangedStatus = ::DDS::LIVELINESS_CHANGED_STATUS;

			/// <summary>
			/// The <see cref="DataWriter" /> has found <see cref="DataReader" /> that matches the <see cref="Topic" /> and has compatible QoS.
			/// </summary>
			static const StatusKind PublicationMatchedStatus = ::DDS::PUBLICATION_MATCHED_STATUS;

			/// <summary>
			/// The <see cref="DataReader" /> has found <see cref="DataWriter" /> that matches the <see cref="Topic" /> and has compatible QoS.
			/// </summary>
			static const StatusKind SubscriptionMatchedStatus = ::DDS::SUBSCRIPTION_MATCHED_STATUS;			

		private:
			System::UInt32 m_value;

		internal:
			StatusKind(System::UInt32 value);

		public:
			/// <summary>
			/// Implicit conversion operator from <see cref="StatusKind" /> to <see cref="System::UInt32" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="System::UInt32" /> value.</returns>
			static operator System::UInt32(StatusKind value) {
				return value.m_value;
			}

			/// <summary>
			/// Implicit conversion operator from <see cref="System::UInt32" /> to <see cref="StatusKind" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="StatusKind" /> value.</returns>
			static operator StatusKind(System::UInt32 value) {
				StatusKind r(value);
				return r;
			}

			/// <summary>
			/// Implicit conversion operator from <see cref="StatusKind" /> to <see cref="StatusMask" />.
			/// </summary>
			/// <param name="value">The value to transform.</param>
			/// <returns>The <see cref="StatusMask" /> value.</returns>
			static operator StatusMask(StatusKind value) {
				StatusMask r(value);
				return r;
			}

			/// <summary>
			/// Bit-wise operator.
			/// </summary>
			/// <param name="left">The left value of the operator.</param>
			/// <param name="right">The right value of the operator.</param>
			/// <returns>The resulting <see cref="StatusMask" />.</returns>
			static StatusMask operator | (StatusKind left, StatusKind right) {
				return static_cast<StatusMask>(static_cast<unsigned int>(left) | static_cast<unsigned int>(right));
			}

		};
	};
};