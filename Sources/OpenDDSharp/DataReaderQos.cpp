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
#include "DataReaderQos.h"

OpenDDSharp::DDS::DataReaderQos::DataReaderQos() {	
	durability = gcnew OpenDDSharp::DDS::DurabilityQosPolicy();	
	deadline = gcnew OpenDDSharp::DDS::DeadlineQosPolicy();
	latency_budget = gcnew OpenDDSharp::DDS::LatencyBudgetQosPolicy();
	liveliness = gcnew OpenDDSharp::DDS::LivelinessQosPolicy();
	reliability = gcnew OpenDDSharp::DDS::ReliabilityQosPolicy();
	destination_order = gcnew OpenDDSharp::DDS::DestinationOrderQosPolicy();
	history = gcnew OpenDDSharp::DDS::HistoryQosPolicy();
	resource_limits = gcnew OpenDDSharp::DDS::ResourceLimitsQosPolicy();
	user_data = gcnew OpenDDSharp::DDS::UserDataQosPolicy();
	ownership = gcnew OpenDDSharp::DDS::OwnershipQosPolicy();
	time_based_filter = gcnew OpenDDSharp::DDS::TimeBasedFilterQosPolicy();
	reader_data_lifecycle = gcnew OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy();
	
};

OpenDDSharp::DDS::DurabilityQosPolicy^ OpenDDSharp::DDS::DataReaderQos::Durability::get() {
	return durability;
};

OpenDDSharp::DDS::DeadlineQosPolicy^ OpenDDSharp::DDS::DataReaderQos::Deadline::get() {
	return deadline;
};

OpenDDSharp::DDS::LatencyBudgetQosPolicy^ OpenDDSharp::DDS::DataReaderQos::LatencyBudget::get() {
	return latency_budget;
};

OpenDDSharp::DDS::LivelinessQosPolicy^ OpenDDSharp::DDS::DataReaderQos::Liveliness::get() {
	return liveliness;
};

OpenDDSharp::DDS::ReliabilityQosPolicy^ OpenDDSharp::DDS::DataReaderQos::Reliability::get() {
	return reliability;
};

OpenDDSharp::DDS::DestinationOrderQosPolicy^ OpenDDSharp::DDS::DataReaderQos::DestinationOrder::get() {
	return destination_order;
};

OpenDDSharp::DDS::HistoryQosPolicy^ OpenDDSharp::DDS::DataReaderQos::History::get() {
	return history;
};

OpenDDSharp::DDS::ResourceLimitsQosPolicy^ OpenDDSharp::DDS::DataReaderQos::ResourceLimits::get() {
	return resource_limits;
};

OpenDDSharp::DDS::UserDataQosPolicy^ OpenDDSharp::DDS::DataReaderQos::UserData::get() {
	return user_data;
};

OpenDDSharp::DDS::OwnershipQosPolicy^ OpenDDSharp::DDS::DataReaderQos::Ownership::get() {
	return ownership;
};

OpenDDSharp::DDS::TimeBasedFilterQosPolicy^ OpenDDSharp::DDS::DataReaderQos::TimeBasedFilter::get() {
	return time_based_filter;
};

OpenDDSharp::DDS::ReaderDataLifecycleQosPolicy^ OpenDDSharp::DDS::DataReaderQos::ReaderDataLifecycle::get() {
	return reader_data_lifecycle;
};

::DDS::DataReaderQos OpenDDSharp::DDS::DataReaderQos::ToNative() {
	::DDS::DataReaderQos qos;
	
	qos.durability = durability->ToNative();	
	qos.deadline = deadline->ToNative();
	qos.latency_budget = latency_budget->ToNative();
	qos.liveliness = liveliness->ToNative();
	qos.reliability = reliability->ToNative();
	qos.destination_order = destination_order->ToNative();
	qos.history = history->ToNative();
	qos.resource_limits = resource_limits->ToNative();
	qos.user_data = user_data->ToNative();
	qos.ownership = ownership->ToNative();
	qos.time_based_filter = time_based_filter->ToNative();
	qos.reader_data_lifecycle = reader_data_lifecycle->ToNative();

	return qos;
};

void OpenDDSharp::DDS::DataReaderQos::FromNative(::DDS::DataReaderQos qos) {	
	durability->FromNative(qos.durability);	
	deadline->FromNative(qos.deadline);
	latency_budget->FromNative(qos.latency_budget);
	liveliness->FromNative(qos.liveliness);
	reliability->FromNative(qos.reliability);
	destination_order->FromNative(qos.destination_order);
	history->FromNative(qos.history);
	resource_limits->FromNative(qos.resource_limits);
	user_data->FromNative(qos.user_data);
	ownership->FromNative(qos.ownership);
	time_based_filter->FromNative(qos.time_based_filter);
	reader_data_lifecycle->FromNative(qos.reader_data_lifecycle);	
};
