#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		public ref class ResourceLimitsQosPolicy {

		public:
			static const System::Int32 LengthUnlimited = ::DDS::LENGTH_UNLIMITED;

		private:			
			System::Int32 max_samples;
			System::Int32 max_instances;
			System::Int32 max_samples_per_instance;

		public:
			property System::Int32 MaxSamples {
				System::Int32 get();
				void set(System::Int32 value);
			};

			property System::Int32 MaxInstances {
				System::Int32 get();
				void set(System::Int32 value);
			};

			property System::Int32 MaxSamplesPerInstance {
				System::Int32 get();
				void set(System::Int32 value);
			};

		internal:
			ResourceLimitsQosPolicy();			

		internal:
			::DDS::ResourceLimitsQosPolicy ToNative();
			void FromNative(::DDS::ResourceLimitsQosPolicy qos);
		};
	};
};

