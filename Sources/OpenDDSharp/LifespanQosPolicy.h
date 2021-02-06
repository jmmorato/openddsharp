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
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataWriter;
		ref class DataReader;

		/// <summary>
		/// <para>The purpose of this QoS is to avoid delivering "stale" data to the application.</para>
		/// <para>Each data sample written by the <see cref="DataWriter" /> has an associated 'expiration time' beyond which the data should not be delivered
		/// to any application. Once the sample expires, the data will be removed from the <see cref="DataReader" /> caches as well as from the
		/// transient and persistent information caches.</para>
		/// </summary>
		/// <remarks>
		/// <para>The value of this policy may be changed at any time. Changes to this policy affect only data written after the change.</para>
		/// <para>The 'expiration time' of each sample is computed by adding the duration specified by the Lifespan QoS to the source timestamp.</para>
		/// <para>This QoS relies on the sender and receiving applications having their clocks sufficiently synchronized. If this is not the case
		/// and DDS can detect it, the <see cref="DataReader" /> is allowed to use the reception timestamp instead of the source timestamp in its
		/// computation of the 'expiration time'.</para>
		/// </remarks>
		public ref class LifespanQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration duration;

		public:
			/// <summary>
			/// Gets or sets the expiration time duration. The default value is infinite, which means samples never expire.
			/// </summary>
			property ::OpenDDSharp::DDS::Duration Duration {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			LifespanQosPolicy();			

		internal:
			::DDS::LifespanQosPolicy ToNative();
			void FromNative(::DDS::LifespanQosPolicy qos);
		};
	};
};
