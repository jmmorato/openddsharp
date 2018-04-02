#include "ResourceLimitsQosPolicy.h"

OpenDDSharp::DDS::ResourceLimitsQosPolicy::ResourceLimitsQosPolicy() {
	max_instances = OpenDDSharp::DDS::ResourceLimitsQosPolicy::LengthUnlimited;
	max_samples = OpenDDSharp::DDS::ResourceLimitsQosPolicy::LengthUnlimited;
	max_samples_per_instance = OpenDDSharp::DDS::ResourceLimitsQosPolicy::LengthUnlimited;
};

System::Int32 OpenDDSharp::DDS::ResourceLimitsQosPolicy::MaxInstances::get() {
	return max_instances;
};

void OpenDDSharp::DDS::ResourceLimitsQosPolicy::MaxInstances::set(System::Int32 value) {
	max_instances = value;
};

System::Int32 OpenDDSharp::DDS::ResourceLimitsQosPolicy::MaxSamples::get() {
	return max_samples;
};

void OpenDDSharp::DDS::ResourceLimitsQosPolicy::MaxSamples::set(System::Int32 value) {
	max_samples = value;
};

System::Int32 OpenDDSharp::DDS::ResourceLimitsQosPolicy::MaxSamplesPerInstance::get() {
	return max_samples_per_instance;
};

void OpenDDSharp::DDS::ResourceLimitsQosPolicy::MaxSamplesPerInstance::set(System::Int32 value) {
	max_samples_per_instance = value;
};

::DDS::ResourceLimitsQosPolicy OpenDDSharp::DDS::ResourceLimitsQosPolicy::ToNative() {
	::DDS::ResourceLimitsQosPolicy* qos = new ::DDS::ResourceLimitsQosPolicy();

	qos->max_instances = max_instances;
	qos->max_samples = max_samples;
	qos->max_samples_per_instance = max_samples_per_instance;

	return *qos;
};

void OpenDDSharp::DDS::ResourceLimitsQosPolicy::FromNative(::DDS::ResourceLimitsQosPolicy qos) {
	max_instances = qos.max_instances;
	max_samples = qos.max_samples;
	max_samples_per_instance = qos.max_samples_per_instance;
};