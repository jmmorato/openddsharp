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
#include "DataWriterQos.h"

OpenDDSharp::DDS::DataWriterQos::DataWriterQos() {
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
	user_data = gcnew OpenDDSharp::DDS::UserDataQosPolicy();
	ownership_strength = gcnew  OpenDDSharp::DDS::OwnershipStrengthQosPolicy();
	writer_data_lifecycle = gcnew OpenDDSharp::DDS::WriterDataLifecycleQosPolicy();
};

OpenDDSharp::DDS::DurabilityQosPolicy^ OpenDDSharp::DDS::DataWriterQos::Durability::get() {
	return durability;
};

OpenDDSharp::DDS::DurabilityServiceQosPolicy^ OpenDDSharp::DDS::DataWriterQos::DurabilityService::get() {
	return durability_service;
};

OpenDDSharp::DDS::DeadlineQosPolicy^ OpenDDSharp::DDS::DataWriterQos::Deadline::get() {
	return deadline;
};

OpenDDSharp::DDS::LatencyBudgetQosPolicy^ OpenDDSharp::DDS::DataWriterQos::LatencyBudget::get() {
	return latency_budget;
};

OpenDDSharp::DDS::LivelinessQosPolicy^ OpenDDSharp::DDS::DataWriterQos::Liveliness::get() {
	return liveliness;
};

OpenDDSharp::DDS::ReliabilityQosPolicy^ OpenDDSharp::DDS::DataWriterQos::Reliability::get() {
	return reliability;
};

OpenDDSharp::DDS::DestinationOrderQosPolicy^ OpenDDSharp::DDS::DataWriterQos::DestinationOrder::get() {
	return destination_order;
};

OpenDDSharp::DDS::HistoryQosPolicy^ OpenDDSharp::DDS::DataWriterQos::History::get() {
	return history;
};

OpenDDSharp::DDS::ResourceLimitsQosPolicy^ OpenDDSharp::DDS::DataWriterQos::ResourceLimits::get() {
	return resource_limits;
};

OpenDDSharp::DDS::TransportPriorityQosPolicy^ OpenDDSharp::DDS::DataWriterQos::TransportPriority::get() {
	return transport_priority;
};

OpenDDSharp::DDS::LifespanQosPolicy^ OpenDDSharp::DDS::DataWriterQos::Lifespan::get() {
	return lifespan;
};

OpenDDSharp::DDS::OwnershipQosPolicy^ OpenDDSharp::DDS::DataWriterQos::Ownership::get() {
	return ownership;
};

OpenDDSharp::DDS::UserDataQosPolicy^ OpenDDSharp::DDS::DataWriterQos::UserData::get() {
	return user_data;
};

OpenDDSharp::DDS::OwnershipStrengthQosPolicy^ OpenDDSharp::DDS::DataWriterQos::OwnershipStrength::get() {
	return ownership_strength;
};

OpenDDSharp::DDS::WriterDataLifecycleQosPolicy^ OpenDDSharp::DDS::DataWriterQos::WriterDataLifecycle::get() {
	return writer_data_lifecycle;
};

::DDS::DataWriterQos OpenDDSharp::DDS::DataWriterQos::ToNative() {
	::DDS::DataWriterQos* qos = new ::DDS::DataWriterQos();
	
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
	qos->user_data = user_data->ToNative();
	qos->ownership_strength = ownership_strength->ToNative();
	qos->writer_data_lifecycle = writer_data_lifecycle->ToNative();

	return *qos;
};

void OpenDDSharp::DDS::DataWriterQos::FromNative(::DDS::DataWriterQos qos) {	
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
	user_data->FromNative(qos.user_data);
	ownership_strength->FromNative(qos.ownership_strength);
	writer_data_lifecycle->FromNative(qos.writer_data_lifecycle);
};
