#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsCoreC.h>
#pragma managed

#include "Timestamp.h"

#pragma make_public(::DDS::SampleInfo)

namespace OpenDDSharp {
	namespace DDS {
		public ref class SampleInfo {

		private:			
			System::Boolean valid_data;			
			System::UInt32 sample_state;
			System::UInt32 view_state;
			System::UInt32 instance_state;
			OpenDDSharp::DDS::Timestamp source_timestamp;
			System::Int32 instance_handle;
			System::Int32 publication_handle;
			System::Int32 disposed_generation_count;
			System::Int32 no_writers_generation_count;
			System::Int32 sample_rank;
			System::Int32 generation_rank;
			System::Int32 absolute_generation_rank;

		public:
			property System::Boolean ValidData { 
				System::Boolean get();				
			}

			property System::UInt32 SampleState {
				System::UInt32 get();
			}

			property System::UInt32 ViewState {
				System::UInt32 get();
			}

			property System::UInt32 InstanceState {
				System::UInt32 get();
			}

			property OpenDDSharp::DDS::Timestamp SourceTimestamp {
				OpenDDSharp::DDS::Timestamp get();
			}

			property System::Int32 InstanceHandle {
				System::Int32 get();
			}

			property System::Int32 PublicationHandle {
				System::Int32 get();
			}

			property System::Int32 DisposedGenerationCount {
				System::Int32 get();
			}

			property System::Int32 NoWritersGenerationCount {
				System::Int32 get();
			}

			property System::Int32 SampleRank {
				System::Int32 get();
			}

			property System::Int32 GenerationRank {
				System::Int32 get();
			}

			property System::Int32 AbsoluteGenerationRank {
				System::Int32 get();
			}

		public:
			SampleInfo();

		public:
			void FromNative(::DDS::SampleInfo native);
			::DDS::SampleInfo ToNative();
		};
	};
};
