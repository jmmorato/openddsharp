/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "ReaderDataLifecycleQosPolicy.h"

::OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy::ReaderDataLifecycleQosPolicy() {
	autopurge_nowriter_samples_delay.Seconds = OpenDDSharp::DDS::Duration::InfiniteSeconds;
	autopurge_nowriter_samples_delay.NanoSeconds = OpenDDSharp::DDS::Duration::InfiniteNanoseconds;

	autopurge_disposed_samples_delay.Seconds = OpenDDSharp::DDS::Duration::InfiniteSeconds;
	autopurge_disposed_samples_delay.NanoSeconds = OpenDDSharp::DDS::Duration::InfiniteNanoseconds;
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