#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionC.h>
#pragma managed

#include "Condition.h"
#include "SampleStateMask.h"
#include "ViewStateMask.h"
#include "InstanceStateMask.h"
#include "DataReader.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// ReadCondition objects are conditions specifically dedicated to read operations and attached to one <see cref="DataReader" />
		/// </summary>
		/// <remarks>
		/// ReadCondition objects allow an application to specify the data samples it is interested in (by specifying the desired sample-states,
		///	view-states, and instance-states). This allows the middleware to enable the condition only when suitable information is available. 
		/// They are to be used in conjunction with a WaitSet as normal conditions. More than one ReadCondition may be attached to the same <see cref="DataReader" />.
		/// </remarks>
		public ref class ReadCondition : public Condition {

		internal:
			::DDS::ReadCondition_ptr impl_entity;
			OpenDDSharp::DDS::DataReader^ data_reader;

		public:
			/// <summary>
			/// Gets the set of sample-states that are taken into account to determine the trigger value of the <see cref="ReadCondition" />. 
			/// These are the sample-states specified when the <see cref="ReadCondition" /> was created.
			/// </summary>
			property OpenDDSharp::DDS::SampleStateMask SampleStateMask {
				OpenDDSharp::DDS::SampleStateMask get();
			}

			/// <summary>
			/// Gets the set of view-states that are taken into account to determine the trigger value of the <see cref="ReadCondition" />.
			///	These are the view-states specified when the <see cref="ReadCondition" /> was created.
			/// </summary>
			property OpenDDSharp::DDS::ViewStateMask ViewStateMask {
				OpenDDSharp::DDS::ViewStateMask get();
			}

			/// <summary>
			/// Gets the set of instance-states that are taken into account to determine the trigger value of the <see cref="ReadCondition" />.
			///	These are the instance-states specified when the ReadCondition was created.
			/// </summary>
			property OpenDDSharp::DDS::InstanceStateMask InstanceStateMask {
				OpenDDSharp::DDS::InstanceStateMask get();
			}

			/// <summary>
			/// Gets the <see cref="DataReader" /> associated with the <see cref="ReadCondition" />. 
			/// </summary>
			/// <remarks>
			/// Note that there is exactly one <see cref="DataReader" /> associated with each <see cref="ReadCondition" />.
			/// </remarks>
			property OpenDDSharp::DDS::DataReader^ DataReader {
				OpenDDSharp::DDS::DataReader^ get();
			}

		internal:
			ReadCondition(::DDS::ReadCondition_ptr read_condition, OpenDDSharp::DDS::DataReader^ reader);
		};
	};
};