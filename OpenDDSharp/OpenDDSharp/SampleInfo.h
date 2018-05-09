#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsCoreC.h>
#pragma managed

#include "Timestamp.h"
#include "SampleStateKind.h"
#include "ViewStateKind.h"
#include "InstanceStateKind.h"
#include "InstanceHandle.h"

#pragma make_public(::DDS::SampleInfo)

namespace OpenDDSharp {
	namespace DDS {

		ref class DataReader;
		ref class DataWriter;

		/// <summary>
		/// Information that accompanies each sample that is read or taken.
		/// </summary>
		/// <remarks>
		/// The SampleInfo structure contains information associated with each Sample. The DataReader read and take operations return two vectors. 
		/// One vector contains Sample(s) and the other contains SampleInfo(s). There is a one-to-one correspondence between items in these two vectors. 
		/// Each Sample is described by the corresponding SampleInfo instance.
		/// </remarks>
		public ref class SampleInfo {

		private:			
			System::Boolean valid_data;			
			OpenDDSharp::DDS::SampleStateKind sample_state;
			OpenDDSharp::DDS::ViewStateKind view_state;
			OpenDDSharp::DDS::InstanceStateKind instance_state;
			OpenDDSharp::DDS::Timestamp source_timestamp;
			OpenDDSharp::DDS::InstanceHandle instance_handle;
			OpenDDSharp::DDS::InstanceHandle publication_handle;
			System::Int32 disposed_generation_count;
			System::Int32 no_writers_generation_count;
			System::Int32 sample_rank;
			System::Int32 generation_rank;
			System::Int32 absolute_generation_rank;

		public:
			/// <summary>
			/// Is set to <see langword="true" /> if the associated DataSample contains data. 
			/// The associated DataSample may not contain data if it this sample indicates a change in sample state (for example Alive -> Disposed).
			/// </summary>
			property System::Boolean ValidData { 
				System::Boolean get();				
			}

			/// <summary>
			/// The associated data sample has/has not been read previously.
			/// </summary>
			property OpenDDSharp::DDS::SampleStateKind SampleState {
				OpenDDSharp::DDS::SampleStateKind get();
			}

			/// <summary>
			/// Associated instance has/has not been seen before. ViewState indicates whether the <see cref="DataReader" /> has already seen 
			/// samples for the most current generation of the related instance.
			/// </summary>
			property OpenDDSharp::DDS::ViewStateKind ViewState {
				OpenDDSharp::DDS::ViewStateKind get();
			}

			/// <summary>
			/// Indicates whether the associated instance currently exists. 
			/// </summary>
			property OpenDDSharp::DDS::InstanceStateKind InstanceState {
				OpenDDSharp::DDS::InstanceStateKind get();
			}

			/// <summary>
			/// The time provided by the <see cref="DataWriter" /> when the sample was written.
			/// </summary>
			property OpenDDSharp::DDS::Timestamp SourceTimestamp {
				OpenDDSharp::DDS::Timestamp get();
			}

			/// <summary>
			/// The handle that locally identifies the associated instance.
			/// </summary>
			property OpenDDSharp::DDS::InstanceHandle InstanceHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			}

			/// <summary>
			/// The local handle of the source <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::InstanceHandle PublicationHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			}

			/// <summary>
			/// The number of times the instance has become 'Alive' after being explicitly disposed.
			/// </summary>
			property System::Int32 DisposedGenerationCount {
				System::Int32 get();
			}

			/// <summary>
			/// The number of times the instance has become 'Alive' after being automatically disposed due to no active writers.
			/// </summary>
			property System::Int32 NoWritersGenerationCount {
				System::Int32 get();
			}

			/// <summary>
			/// Number of samples related to this instances that follow in the collection returned by the <see cref="DataReader" /> read or take operations.
			/// </summary>
			property System::Int32 SampleRank {
				System::Int32 get();
			}

			/// <summary>
			/// The generation difference of this sample and the most recent sample in the collection. GenerationRank indicates the generation difference 
			/// (ie, the number of times the instance was disposed and became alive again) between this sample, and the most recent sample in the collection related to this instance.
			/// </summary>
			property System::Int32 GenerationRank {
				System::Int32 get();
			}

			/// <summary>
			/// The generation difference between this sample and the most recent sample. The AbsoluteGenerationRank indicates the generation difference 
			/// (ie, the number of times the instance was disposed and became alive again) between this sample, and the most recent sample 
			/// (possibly not in the returned collection) of this instance.
			/// </summary>
			property System::Int32 AbsoluteGenerationRank {
				System::Int32 get();
			}

		public:
			/// <summary>
			/// Creates a new instance of <see cref="SampleInfo" />
			/// </summary>
			SampleInfo();

		public:
			/// <summary>
			/// Load the current <see cref="SampleInfo" /> structure from the a native OpenDDS structure.
			/// Internal use only.
			/// </summary>
			void FromNative(::DDS::SampleInfo native);

			/// <summary>
			/// Gets the native OpenDDS <see cref="SampleInfo" /> structure.
			/// Internal use only.
			/// </summary>
			::DDS::SampleInfo ToNative();
		};
	};
};
