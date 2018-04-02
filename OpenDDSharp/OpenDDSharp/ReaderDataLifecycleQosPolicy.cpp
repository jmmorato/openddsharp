#include "ReaderDataLifecycleQosPolicy.h"

::OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy::ReaderDataLifecycleQosPolicy() {
	autopurge_nowriter_samples_delay.Seconds = OpenDDSharp::DDS::Duration::DurationInfiniteSec;
	autopurge_nowriter_samples_delay.NanoSeconds = OpenDDSharp::DDS::Duration::DurationInfiniteNsec;

	autopurge_disposed_samples_delay.Seconds = OpenDDSharp::DDS::Duration::DurationInfiniteSec;
	autopurge_disposed_samples_delay.NanoSeconds = OpenDDSharp::DDS::Duration::DurationInfiniteNsec;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy::AutopurgeNowriterSamplesDelay::get() {
	return autopurge_nowriter_samples_delay;
};

void OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy::AutopurgeNowriterSamplesDelay::set(::OpenDDSharp::DDS::Duration value) {
	autopurge_nowriter_samples_delay = value;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy::AutopurgeDisposedSamplesDelay::get() {
	return autopurge_disposed_samples_delay;
};

void OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy::AutopurgeDisposedSamplesDelay::set(::OpenDDSharp::DDS::Duration value) {
	autopurge_disposed_samples_delay = value;
};


::DDS::ReaderDataLifecycleQosPolicy OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy::ToNative() {
	::DDS::ReaderDataLifecycleQosPolicy* qos = new ::DDS::ReaderDataLifecycleQosPolicy();

	qos->autopurge_nowriter_samples_delay = autopurge_nowriter_samples_delay.ToNative();
	qos->autopurge_disposed_samples_delay = autopurge_disposed_samples_delay.ToNative();

	return *qos;
};

void OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy::FromNative(::DDS::ReaderDataLifecycleQosPolicy qos) {
	autopurge_nowriter_samples_delay.FromNative(qos.autopurge_nowriter_samples_delay);
	autopurge_disposed_samples_delay.FromNative(qos.autopurge_disposed_samples_delay);
};