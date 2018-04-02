#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public value struct LivelinessChangedStatus {

		private:
			System::Int32 alive_count;
			System::Int32 not_alive_count;
			System::Int32 alive_count_change;
			System::Int32 not_alive_count_change;
			System::Int32 last_publication_handle;

		public:
			property System::Int32 AliveCount {
				System::Int32 get();
			};

			property System::Int32 NotAliveCount {
				System::Int32 get();
			};

			property System::Int32 AliveCountChange {
				System::Int32 get();
			};

			property System::Int32 NotAliveCountChange {
				System::Int32 get();
			};

			property System::Int32 LastPublicationHandle {
				System::Int32 get();
			};

		internal:
			LivelinessChangedStatus(::DDS::LivelinessChangedStatus status);
			::DDS::LivelinessChangedStatus ToNative();
			void FromNative(::DDS::LivelinessChangedStatus native);
		};
	};
};
