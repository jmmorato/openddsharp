#include "WriterDataLifecycleQosPolicy.h"

::OpenDDSharp::DDS::WriterDataLifecycleQosPolicy::WriterDataLifecycleQosPolicy() {
	autodispose_unregistered_instances = true;
};

System::Boolean OpenDDSharp::DDS::WriterDataLifecycleQosPolicy::AutodisposeUnregisteredInstances::get() {
	return autodispose_unregistered_instances;
};

void OpenDDSharp::DDS::WriterDataLifecycleQosPolicy::AutodisposeUnregisteredInstances::set(System::Boolean value) {
	autodispose_unregistered_instances = value;
};

::DDS::WriterDataLifecycleQosPolicy OpenDDSharp::DDS::WriterDataLifecycleQosPolicy::ToNative() {
	::DDS::WriterDataLifecycleQosPolicy* qos = new ::DDS::WriterDataLifecycleQosPolicy();

	qos->autodispose_unregistered_instances = autodispose_unregistered_instances;

	return *qos;
};

void OpenDDSharp::DDS::WriterDataLifecycleQosPolicy::FromNative(::DDS::WriterDataLifecycleQosPolicy qos) {
	autodispose_unregistered_instances = qos.autodispose_unregistered_instances;
};