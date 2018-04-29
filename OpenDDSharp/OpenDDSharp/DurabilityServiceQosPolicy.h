#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"
#include "HistoryQosPolicyKind.h"
#include "ResourceLimitsQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// This policy is used to configure the history QoS and the resource limits QoS used by the fictitious <see cref="DataReader" /> and
		/// <see cref="DataWriter" /> used by the "persistence service".
		/// </summrary>
		public ref class DurabilityServiceQosPolicy {

		private:
			::OpenDDSharp::DDS::Duration service_cleanup_delay;
			::OpenDDSharp::DDS::HistoryQosPolicyKind history_kind;
			System::Int32 history_depth;
			System::Int32 max_samples;
			System::Int32 max_instances;
			System::Int32 max_samples_per_instance;

		public:
			/// <summary>
			/// Specifies how long the durability service must wait before it is allowed to remove the information on 
			/// the transient or persistent topic data-instances as a result of incoming dispose messages.
			/// </summary>
			property OpenDDSharp::DDS::Duration ServiceCleanupDelay {
				OpenDDSharp::DDS::Duration get();
				void set(OpenDDSharp::DDS::Duration value);
			};

			/// <summary>
			/// Specifies the type of history the durability service must apply for the transient or 
			/// persistent topic data-instances.			 
			/// </summary>
			property ::OpenDDSharp::DDS::HistoryQosPolicyKind HistoryKind {
				::OpenDDSharp::DDS::HistoryQosPolicyKind get();
				void set(::OpenDDSharp::DDS::HistoryQosPolicyKind value);
			};

			/// <summary>
			/// Specifies the number of samples of each instance of data (identified by its key) that is managed by the durability service 
			/// for the transient or persistent topic data-instances.
			/// </summary>
			property System::Int32 HistoryDepth {
				System::Int32 get();
				void set(System::Int32 value);
			};

			/// <summary>
			/// Specifies the maximum number of data-samples the <see cref="DataWriter" /> (or <see cref="DataReader" />) can manage across all the instances associated with it. 
			/// Represents the maximum samples the middleware can store for any one <see cref="DataWriter" /> (or <see cref="DataReader" />).
			/// </summary>
			property System::Int32 MaxSamples {
				System::Int32 get();
				void set(System::Int32 value);
			};

			/// <summary>
			/// Represents the maximum number of instances the <see cref="DataWriter" /> (or <see cref="DataReader" />) can manage.
			/// </summary>
			property System::Int32 MaxInstances {
				System::Int32 get();
				void set(System::Int32 value);
			};

			/// <summary>
			/// Represents the maximum number of samples of any one instance a <see cref="DataWriter" /> (or <see cref="DataReader" />) can manage. 
			/// </summary>
			property System::Int32 MaxSamplesPerInstance {
				System::Int32 get();
				void set(System::Int32 value);
			};

		internal:
			DurabilityServiceQosPolicy();			

		internal:
			::DDS::DurabilityServiceQosPolicy ToNative();
			void FromNative(::DDS::DurabilityServiceQosPolicy qos);
		};
	};
};

