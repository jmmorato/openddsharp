#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "InstanceHandle.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataWriter;

		/// <summary>
		/// The LivelinessChanged status indicates that there have been liveliness changes for one or more data writers that are publishing instances for this data reader.
		/// </summary>
		public value struct LivelinessChangedStatus {

		private:
			System::Int32 alive_count;
			System::Int32 not_alive_count;
			System::Int32 alive_count_change;
			System::Int32 not_alive_count_change;
			System::Int32 last_publication_handle;

		public:
			/// <summary>
			/// Gets the total number of data writers currently active on the topic this data reader is reading.
			/// </summary>
			property System::Int32 AliveCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the total number of data writers writing to the data reader's topic that are no longer asserting their liveliness.
			/// </summary>
			property System::Int32 NotAliveCount {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the change in the alive count since the last time the status was accessed.
			/// </summary>
			property System::Int32 AliveCountChange {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the change in the not alive count since the last time the status was accessed.
			/// </summary>
			property System::Int32 NotAliveCountChange {
				System::Int32 get();
			};

			/// <summary>
			/// Gets the handle of the last <see cref="DataWriter" /> whose liveliness has changed.
			/// </summary>
			property OpenDDSharp::DDS::InstanceHandle LastPublicationHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			};

		internal:
			LivelinessChangedStatus(::DDS::LivelinessChangedStatus status);
			::DDS::LivelinessChangedStatus ToNative();
			void FromNative(::DDS::LivelinessChangedStatus native);
		};
	};
};
