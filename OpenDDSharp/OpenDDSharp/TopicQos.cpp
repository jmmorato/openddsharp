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
#include "TopicQos.h"

OpenDDSharp::DDS::TopicQos::TopicQos() {
	topic_data = gcnew OpenDDSharp::DDS::TopicDataQosPolicy();
	durability = gcnew OpenDDSharp::DDS::DurabilityQosPolicy();
	durability_service = gcnew OpenDDSharp::DDS::DurabilityServiceQosPolicy();
	deadline = gcnew OpenDDSharp::DDS::DeadlineQosPolicy();
	latency_budget = gcnew OpenDDSharp::DDS::LatencyBudgetQosPolicy();
	liveliness = gcnew OpenDDSharp::DDS::LivelinessQosPolicy();
	reliability = gcnew OpenDDSharp::DDS::ReliabilityQosPolicy();
	destination_order = gcnew OpenDDSharp::DDS::DestinationOrderQosPolicy();
	history = gcnew OpenDDSharp::DDS::HistoryQosPolicy();
	resource_limits = gcnew OpenDDSharp::DDS::ResourceLimitsQosPolicy();
	transport_priority = gcnew OpenDDSharp::DDS::TransportPriorityQosPolicy();
	lifespan = gcnew OpenDDSharp::DDS::LifespanQosPolicy();
	ownership = gcnew OpenDDSharp::DDS::OwnershipQosPolicy();
};

OpenDDSharp::DDS::TopicDataQosPolicy^ OpenDDSharp::DDS::TopicQos::TopicData::get() {
	return topic_data;
};

OpenDDSharp::DDS::DurabilityQosPolicy^ OpenDDSharp::DDS::TopicQos::Durability::get() {
	return durability;
};

OpenDDSharp::DDS::DurabilityServiceQosPolicy^ OpenDDSharp::DDS::TopicQos::DurabilityService::get() {
	return durability_service;
};

OpenDDSharp::DDS::DeadlineQosPolicy^ OpenDDSharp::DDS::TopicQos::Deadline::get() {
	return deadline;
};

OpenDDSharp::DDS::LatencyBudgetQosPolicy^ OpenDDSharp::DDS::TopicQos::LatencyBudget::get() {
	return latency_budget;
};

OpenDDSharp::DDS::LivelinessQosPolicy^ OpenDDSharp::DDS::TopicQos::Liveliness::get() {
	return liveliness;
};

OpenDDSharp::DDS::ReliabilityQosPolicy^ OpenDDSharp::DDS::TopicQos::Reliability::get() {
	return reliability;
};

OpenDDSharp::DDS::DestinationOrderQosPolicy^ OpenDDSharp::DDS::TopicQos::DestinationOrder::get() {
	return destination_order;
};

OpenDDSharp::DDS::HistoryQosPolicy^ OpenDDSharp::DDS::TopicQos::History::get() {
	return history;
};

OpenDDSharp::DDS::ResourceLimitsQosPolicy^ OpenDDSharp::DDS::TopicQos::ResourceLimits::get() {
	return resource_limits;
};

OpenDDSharp::DDS::TransportPriorityQosPolicy^ OpenDDSharp::DDS::TopicQos::TransportPriority::get() {
	return transport_priority;
};

OpenDDSharp::DDS::LifespanQosPolicy^ OpenDDSharp::DDS::TopicQos::Lifespan::get() {
	return lifespan;
};

OpenDDSharp::DDS::OwnershipQosPolicy^ OpenDDSharp::DDS::TopicQos::Ownership::get() {
	return ownership;
};

::DDS::TopicQos OpenDDSharp::DDS::TopicQos::ToNative() {
	::DDS::TopicQos* qos = new ::DDS::TopicQos();

	qos->topic_data = topic_data->ToNative();
	qos->durability = durability->ToNative();
	qos->durability_service = durability_service->ToNative();
	qos->deadline = deadline->ToNative();
	qos->latency_budget = latency_budget->ToNative();
	qos->liveliness = liveliness->ToNative();
	qos->reliability = reliability->ToNative();
	qos->destination_order = destination_order->ToNative();
	qos->history = history->ToNative();
	qos->resource_limits = resource_limits->ToNative();
	qos->transport_priority = transport_priority->ToNative();
	qos->lifespan = lifespan->ToNative();
	qos->ownership = ownership->ToNative();

	return *qos;
};

void OpenDDSharp::DDS::TopicQos::FromNative(::DDS::TopicQos qos) {
	topic_data->FromNative(qos.topic_data);
	durability->FromNative(qos.durability);
	durability_service->FromNative(qos.durability_service);
	deadline->FromNative(qos.deadline);
	latency_budget->FromNative(qos.latency_budget);
	liveliness->FromNative(qos.liveliness);
	reliability->FromNative(qos.reliability);
	destination_order->FromNative(qos.destination_order);
	history->FromNative(qos.history);
	resource_limits->FromNative(qos.resource_limits);
	transport_priority->FromNative(qos.transport_priority);
	lifespan->FromNative(qos.lifespan);
	ownership->FromNative(qos.ownership);
};
