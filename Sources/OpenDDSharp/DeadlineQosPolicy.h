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
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class Topic;
		ref class DataWriter;
		ref class DataReader;

		/// <summary>
		/// This policy is useful for cases where a <see cref="Topic" /> is expected to have each instance updated periodically. On the publishing side this
		/// setting establishes a contract that the application must meet. On the subscribing side the setting establishes a minimum
		/// requirement for the remote publishers that are expected to supply the data values.
		/// </summary>
		/// <remarks>
		/// <para>When the DDS 'matches' a <see cref="DataWriter" /> and a <see cref="DataReader" /> it checks whether the settings are compatible (i.e., offered
		/// deadline period &lt;= requested deadline period) if they are not, the two entities are informed (via the listener or condition
		/// mechanism) of the incompatibility of the QoS settings and communication will not occur.</para>
		/// <para>Assuming that the reader and writer ends have compatible settings, the fulfillment of this contract is monitored by DDS
		/// and the application is informed of any violations by means of the proper listener or condition.</para>
		/// <para>The value offered is considered compatible with the value requested if and only if the inequality "offered deadline period &lt;=
		/// requested deadline period" evaluates to 'true'.</para>
		/// <para>The setting of the Deadline policy must be set consistently with that of the TimeBasedFilter. For these two policies
		/// to be consistent the settings must be such that "deadline period &gt;= minimum separation".</para>
		/// </remarks>
		public ref class DeadlineQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration period;

		public:
			/// <summary>
			/// Duration of the deadline period. The default value of the period member is infinite, which requires no behavior.
			/// </summary>
			/// <remarks>
			/// When this policy is set to a finite value, then the <see cref="DataWriter" /> monitors the changes to data made by the
			/// application and indicates failure to honor the policy by setting the corresponding status
			/// condition and triggering the OnOfferedDeadlineMissed() listener callback. A <see cref="DataReader" />
			/// that detects that the data has not changed before the period has expired sets the
			/// corresponding status condition and triggers the OnRequestedDeadlineMissed() listener callback.
			/// </remarks>
			property ::OpenDDSharp::DDS::Duration Period {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			DeadlineQosPolicy();			

		internal:
			::DDS::DeadlineQosPolicy ToNative();
			void FromNative(::DDS::DeadlineQosPolicy qos);
		};
	};
};
