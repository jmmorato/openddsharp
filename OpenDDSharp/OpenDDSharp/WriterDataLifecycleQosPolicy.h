#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class WriterDataLifecycleQosPolicy {

		private:
			System::Boolean autodispose_unregistered_instances;			

		public:
			property System::Boolean AutodisposeUnregisteredInstances {
				System::Boolean get();
				void set(System::Boolean value);
			};

		internal:
			WriterDataLifecycleQosPolicy();

		internal:
			::DDS::WriterDataLifecycleQosPolicy ToNative();
			void FromNative(::DDS::WriterDataLifecycleQosPolicy qos);
		};
	};
};
