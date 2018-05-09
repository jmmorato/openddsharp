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
#include "DurabilityServiceQosPolicy.h"

::OpenDDSharp::DDS::DurabilityServiceQosPolicy::DurabilityServiceQosPolicy() {	
	service_cleanup_delay.Seconds = OpenDDSharp::DDS::Duration::ZeroSeconds;
	service_cleanup_delay.NanoSeconds = OpenDDSharp::DDS::Duration::ZeroSeconds;
	history_kind = OpenDDSharp::DDS::HistoryQosPolicyKind::KeepLastHistoryQos;
	history_depth = 1;
	max_samples = OpenDDSharp::DDS::ResourceLimitsQosPolicy::LengthUnlimited;
	max_instances = OpenDDSharp::DDS::ResourceLimitsQosPolicy::LengthUnlimited;
	max_samples_per_instance = OpenDDSharp::DDS::ResourceLimitsQosPolicy::LengthUnlimited;
};

::OpenDDSharp::DDS::Duration OpenDDSharp::DDS::DurabilityServiceQosPolicy::ServiceCleanupDelay::get() {
	return service_cleanup_delay;
};

void OpenDDSharp::DDS::DurabilityServiceQosPolicy::ServiceCleanupDelay::set(::OpenDDSharp::DDS::Duration value) {
	service_cleanup_delay = value;
};

::OpenDDSharp::DDS::HistoryQosPolicyKind OpenDDSharp::DDS::DurabilityServiceQosPolicy::HistoryKind::get() {
	return history_kind;
};

void OpenDDSharp::DDS::DurabilityServiceQosPolicy::HistoryKind::set(::OpenDDSharp::DDS::HistoryQosPolicyKind value) {
	history_kind = value;
};

System::Int32 OpenDDSharp::DDS::DurabilityServiceQosPolicy::HistoryDepth::get() {
	return history_depth;
};

void OpenDDSharp::DDS::DurabilityServiceQosPolicy::HistoryDepth::set(System::Int32 value) {
	history_depth = value;
};

System::Int32 OpenDDSharp::DDS::DurabilityServiceQosPolicy::MaxSamples::get() {
	return max_samples;
};

void OpenDDSharp::DDS::DurabilityServiceQosPolicy::MaxSamples::set(System::Int32 value) {
	max_samples = value;
};

System::Int32 OpenDDSharp::DDS::DurabilityServiceQosPolicy::MaxInstances::get() {
	return max_instances;
};

void OpenDDSharp::DDS::DurabilityServiceQosPolicy::MaxInstances::set(System::Int32 value) {
	max_instances = value;
};

System::Int32 OpenDDSharp::DDS::DurabilityServiceQosPolicy::MaxSamplesPerInstance::get() {
	return max_samples_per_instance;
};

void OpenDDSharp::DDS::DurabilityServiceQosPolicy::MaxSamplesPerInstance::set(System::Int32 value) {
	max_samples_per_instance = value;
};

::DDS::DurabilityServiceQosPolicy OpenDDSharp::DDS::DurabilityServiceQosPolicy::ToNative() {
	::DDS::DurabilityServiceQosPolicy* qos = new ::DDS::DurabilityServiceQosPolicy();

	qos->service_cleanup_delay = service_cleanup_delay.ToNative();
	qos->history_kind = (::DDS::HistoryQosPolicyKind)history_kind;
	qos->history_depth = history_depth;
	qos->max_samples = max_samples;
	qos->max_instances = max_instances;
	qos->max_samples_per_instance = max_samples_per_instance;
	
	return *qos;
};

void OpenDDSharp::DDS::DurabilityServiceQosPolicy::FromNative(::DDS::DurabilityServiceQosPolicy qos) {
	service_cleanup_delay.FromNative(qos.service_cleanup_delay);
	history_kind = (::OpenDDSharp::DDS::HistoryQosPolicyKind)qos.history_kind;
	history_depth = qos.history_depth;
	max_samples = qos.max_samples;
	max_instances = qos.max_instances;
	max_samples_per_instance = qos.max_samples_per_instance;
};