/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
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
	::DDS::ResourceLimitsQosPolicy qos;

	qos.max_instances = max_instances;
	qos.max_samples = max_samples;
	qos.max_samples_per_instance = max_samples_per_instance;

	return qos;
};

void OpenDDSharp::DDS::ResourceLimitsQosPolicy::FromNative(::DDS::ResourceLimitsQosPolicy qos) {
	max_instances = qos.max_instances;
	max_samples = qos.max_samples;
	max_samples_per_instance = qos.max_samples_per_instance;
};