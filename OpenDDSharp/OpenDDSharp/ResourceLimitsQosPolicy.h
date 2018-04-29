#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {

		ref class DataWriter;
		ref class DataReader;

		/// <summary>
		/// This policy controls the resources that DDS can use in order to meet the requirements imposed by the application and other QoS settings.
		/// </summary>
		/// <remarks>
		/// <para>If the <see cref="DataWriter" /> objects are communicating samples faster than they are ultimately taken by the <see cref="DataReader" /> objects, the
		/// middleware will eventually hit against some of the QoS-imposed resource limits. Note that this may occur when just a single
		/// <see cref="DataReader" /> cannot keep up with its corresponding <see cref="DataWriter" />. The behavior in this case depends on the setting for the
		/// Reliability QoS. If reliability is BestEffort then DDS is allowed to drop samples. If the reliability is Reliable, DDS will block the <see cref="DataWriter" /> 
		/// or discard the sample at the <see cref="DataReader" /> in order not to lose existing samples.</para>
		/// <para>The setting of ResourceLimits MaxSamples must be consistent with the MaxSamplesPerInstance. For these two
		/// values to be consistent they must verify that “MaxSamples &gt;= MaxSamplesPerInstance.”</para>
		/// <para>The setting of ResourceLimits MaxSamplesPerInstance must be consistent with the History Depth. For these two
		/// QoS to be consistent, they must verify that "depth &lt;= MaxSamplesPerInstance".</para>
		/// <para>An attempt to set this policy to inconsistent values when an entity is created of via a SetQos operation will cause the operation to fail.</para>
		/// </remarks>
		public ref class ResourceLimitsQosPolicy {

		public:
			/// <summary>
			/// Used to indicate the absence of a particular limit.
			/// </summary>
			static const System::Int32 LengthUnlimited = ::DDS::LENGTH_UNLIMITED;

		private:			
			System::Int32 max_samples;
			System::Int32 max_instances;
			System::Int32 max_samples_per_instance;

		public:
			/// <summary>
			/// Gets or sets the maximum number of samples a single <see cref="DataWriter" /> or <see cref="DataReader" /> can manage across all of its instances.
			/// The default value is <see cref="LengthUnlimited" />
			/// </summary>
			property System::Int32 MaxSamples {
				System::Int32 get();
				void set(System::Int32 value);
			};

			/// <summary>
			/// Gets or sets the maximum number of instances that a <see cref="DataWriter" /> or <see cref="DataReader" /> can manage.
			/// The default value is <see cref="LengthUnlimited" />
			/// </summary>
			property System::Int32 MaxInstances {
				System::Int32 get();
				void set(System::Int32 value);
			};

			/// <summary>
			/// Gets or sets the maximum number of samples that can be managed for an individual instance in a single <see cref="DataWriter" /> or <see cref="DataReader" />.
			/// The default value is <see cref="LengthUnlimited" />
			/// </summary>
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

