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
