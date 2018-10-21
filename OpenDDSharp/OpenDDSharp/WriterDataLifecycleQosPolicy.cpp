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
	::DDS::WriterDataLifecycleQosPolicy qos;

	qos.autodispose_unregistered_instances = autodispose_unregistered_instances;

	return qos;
};

void OpenDDSharp::DDS::WriterDataLifecycleQosPolicy::FromNative(::DDS::WriterDataLifecycleQosPolicy qos) {
	autodispose_unregistered_instances = qos.autodispose_unregistered_instances;
};