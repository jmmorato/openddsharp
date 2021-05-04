/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

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
#include "PartitionQosPolicy.h"

OpenDDSharp::DDS::PartitionQosPolicy::PartitionQosPolicy() {
	name = gcnew List<System::String^>();
};

IEnumerable<System::String^>^ OpenDDSharp::DDS::PartitionQosPolicy::Name::get() {
	return name;
};

void OpenDDSharp::DDS::PartitionQosPolicy::Name::set(IEnumerable<System::String^>^ value) {
	name = value;
};

::DDS::PartitionQosPolicy OpenDDSharp::DDS::PartitionQosPolicy::ToNative() {
	msclr::interop::marshal_context context;

	if (name == nullptr) {
		name = gcnew List<System::String^>();
	}

	::DDS::PartitionQosPolicy qos;

	int count = System::Linq::Enumerable::Count(name);
	qos.name.length(count);

	int i = 0;
	while (i < count) {
		System::String^ str = System::Linq::Enumerable::ElementAt(name, i);
		qos.name[i] = context.marshal_as<const char*>(str);
		i++;
	}

	return qos;
};

void OpenDDSharp::DDS::PartitionQosPolicy::FromNative(::DDS::PartitionQosPolicy qos) {
	msclr::interop::marshal_context context;

	List<System::String^>^ list = gcnew List<System::String^>();
	int length = qos.name.length();
	int i = 0;
	while (i < length) {
		const char * str = qos.name[i];
		list->Add(context.marshal_as<System::String^>(str));
		i++;
	}

	name = list;
};