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

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataReader;

		/// <summary>
		/// This policy controls the behavior of the <see cref="DataReader" /> with regards to the lifecycle of the data-instances it manages, that is, the
		/// data-instances that have been received and for which the DataReader maintains some internal resources.
		/// </summary>
		public ref class ReaderDataLifecycleQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration autopurge_nowriter_samples_delay;
			::OpenDDSharp::DDS::Duration autopurge_disposed_samples_delay;

		public:
			/// <summary>
			/// Gets or sets the maximum duration for which the <see cref="DataReader" /> will maintain information
			/// regarding an instance once its InstanceState becomes NotAliveNoWriters. After this time elapses, the <see cref="DataReader" />
			/// will purge all internal information regarding the instance, any untaken samples will also be lost.
			/// </summary>
			property ::OpenDDSharp::DDS::Duration AutopurgeNowriterSamplesDelay {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

			/// <summary>
			/// Gets or sets the maximum duration for which the <see cref="DataReader" /> will maintain samples for
			/// an instance once its InstanceState becomes NotAliveDisposed. After this time elapses, the <see cref="DataReader" /> will purge all
			/// samples for the instance.
			/// </summary>
			property ::OpenDDSharp::DDS::Duration AutopurgeDisposedSamplesDelay {
				::OpenDDSharp::DDS::Duration get();
				void set(::OpenDDSharp::DDS::Duration value);
			};

		internal:
			ReaderDataLifecycleQosPolicy();

		internal:
			::DDS::ReaderDataLifecycleQosPolicy ToNative();
			void FromNative(::DDS::ReaderDataLifecycleQosPolicy qos);
		};
	};
};
