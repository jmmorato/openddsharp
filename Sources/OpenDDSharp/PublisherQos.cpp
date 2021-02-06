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
#include "PublisherQos.h"

OpenDDSharp::DDS::PublisherQos::PublisherQos() {	
	presentation = gcnew OpenDDSharp::DDS::PresentationQosPolicy();
	partition = gcnew OpenDDSharp::DDS::PartitionQosPolicy();
	group_data = gcnew OpenDDSharp::DDS::GroupDataQosPolicy();
	entity_factory = gcnew OpenDDSharp::DDS::EntityFactoryQosPolicy();
};

OpenDDSharp::DDS::PresentationQosPolicy^ OpenDDSharp::DDS::PublisherQos::Presentation::get() {
	return presentation;
};

OpenDDSharp::DDS::PartitionQosPolicy^ OpenDDSharp::DDS::PublisherQos::Partition::get() {
	return partition;
};

OpenDDSharp::DDS::GroupDataQosPolicy^ OpenDDSharp::DDS::PublisherQos::GroupData::get() {
	return group_data;
};

OpenDDSharp::DDS::EntityFactoryQosPolicy^ OpenDDSharp::DDS::PublisherQos::EntityFactory::get() {
	return entity_factory;
};

::DDS::PublisherQos OpenDDSharp::DDS::PublisherQos::ToNative() {
	::DDS::PublisherQos qos;

	qos.presentation = presentation->ToNative();
	qos.partition = partition->ToNative();
	qos.group_data = group_data->ToNative();
	qos.entity_factory = entity_factory->ToNative();

	return qos;
};

void OpenDDSharp::DDS::PublisherQos::FromNative(::DDS::PublisherQos qos) {
	presentation->FromNative(qos.presentation);
	partition->FromNative(qos.partition);
	group_data->FromNative(qos.group_data);
	entity_factory->FromNative(qos.entity_factory);
};
