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
#include "TopicBuiltinTopicData.h"

OpenDDSharp::DDS::BuiltinTopicKey OpenDDSharp::DDS::TopicBuiltinTopicData::Key::get() {
	return key;
};

System::String^ OpenDDSharp::DDS::TopicBuiltinTopicData::Name::get() {
	return name;
};

System::String^ OpenDDSharp::DDS::TopicBuiltinTopicData::TypeName::get() {
	return type_name;
};

OpenDDSharp::DDS::DurabilityQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::Durability::get() {
	return durability;
};

OpenDDSharp::DDS::DurabilityServiceQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::DurabilityService::get() {
	return durability_service;
};

OpenDDSharp::DDS::DeadlineQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::Deadline::get() {
	return deadline;
};

OpenDDSharp::DDS::LatencyBudgetQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::LatencyBudget::get() {
	return latency_budget;
};

OpenDDSharp::DDS::LivelinessQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::Liveliness::get() {
	return liveliness;
};

OpenDDSharp::DDS::ReliabilityQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::Reliability::get() {
	return reliability;
};

OpenDDSharp::DDS::TransportPriorityQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::TransportPriority::get() {
	return transport_priority;
};

OpenDDSharp::DDS::LifespanQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::Lifespan::get() {
	return lifespan;
};

OpenDDSharp::DDS::DestinationOrderQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::DestinationOrder::get() {
	return destination_order;
};

OpenDDSharp::DDS::HistoryQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::History::get() {
	return history;
};

OpenDDSharp::DDS::ResourceLimitsQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::ResourceLimits::get() {
	return resource_limits;
};

OpenDDSharp::DDS::OwnershipQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::Ownership::get() {
	return ownership;
};

OpenDDSharp::DDS::TopicDataQosPolicy^ OpenDDSharp::DDS::TopicBuiltinTopicData::TopicData::get() {
	return topic_data;
};

void OpenDDSharp::DDS::TopicBuiltinTopicData::FromNative(::DDS::TopicBuiltinTopicData native) {
	msclr::interop::marshal_context context;

	key.FromNative(native.key);	

	const char * name_ptr = native.name;
	name = context.marshal_as<System::String^>(name_ptr);

	const char * type_name_ptr = native.type_name;
	type_name = context.marshal_as<System::String^>(type_name_ptr);

	durability = gcnew OpenDDSharp::DDS::DurabilityQosPolicy();
	durability->FromNative(native.durability);

	durability_service = gcnew OpenDDSharp::DDS::DurabilityServiceQosPolicy();
	durability_service->FromNative(native.durability_service);

	deadline = gcnew OpenDDSharp::DDS::DeadlineQosPolicy();
	deadline->FromNative(native.deadline);

	latency_budget = gcnew OpenDDSharp::DDS::LatencyBudgetQosPolicy();
	latency_budget->FromNative(native.latency_budget);

	liveliness = gcnew OpenDDSharp::DDS::LivelinessQosPolicy();
	liveliness->FromNative(native.liveliness);

	reliability = gcnew OpenDDSharp::DDS::ReliabilityQosPolicy();
	reliability->FromNative(native.reliability);

	transport_priority = gcnew OpenDDSharp::DDS::TransportPriorityQosPolicy();
	transport_priority->FromNative(native.transport_priority);

	lifespan = gcnew OpenDDSharp::DDS::LifespanQosPolicy();
	lifespan->FromNative(native.lifespan);

	destination_order = gcnew OpenDDSharp::DDS::DestinationOrderQosPolicy();
	destination_order->FromNative(native.destination_order);

	history = gcnew OpenDDSharp::DDS::HistoryQosPolicy();
	history->FromNative(native.history);

	resource_limits = gcnew OpenDDSharp::DDS::ResourceLimitsQosPolicy();
	resource_limits->FromNative(native.resource_limits);

	ownership = gcnew OpenDDSharp::DDS::OwnershipQosPolicy();
	ownership->FromNative(native.ownership);

	topic_data = gcnew OpenDDSharp::DDS::TopicDataQosPolicy();
	topic_data->FromNative(native.topic_data);
}


::DDS::TopicBuiltinTopicData OpenDDSharp::DDS::TopicBuiltinTopicData::ToNative() {
	::DDS::TopicBuiltinTopicData native;
	msclr::interop::marshal_context context;

	native.key = key.ToNative();		
	native.name = context.marshal_as<const char*>(name);
	native.type_name = context.marshal_as<const char*>(type_name);	
	native.durability = durability->ToNative();
	native.durability_service = durability_service->ToNative();	
	native.deadline = deadline->ToNative();	
	native.latency_budget = latency_budget->ToNative();	
	native.liveliness = liveliness->ToNative();	
	native.reliability = reliability->ToNative();	
	native.transport_priority = transport_priority->ToNative();	
	native.lifespan = lifespan->ToNative();	
	native.destination_order = destination_order->ToNative();	
	native.history = history->ToNative();	
	native.resource_limits = resource_limits->ToNative();	
	native.ownership = ownership->ToNative();	
	native.topic_data = topic_data->ToNative();

	return native;
}