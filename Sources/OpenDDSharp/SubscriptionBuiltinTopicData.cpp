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
#include "SubscriptionBuiltinTopicData.h"

OpenDDSharp::DDS::BuiltinTopicKey OpenDDSharp::DDS::SubscriptionBuiltinTopicData::Key::get() {
	return key;
};

OpenDDSharp::DDS::BuiltinTopicKey OpenDDSharp::DDS::SubscriptionBuiltinTopicData::ParticipantKey::get() {
	return participant_key;
};

System::String^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::TopicName::get() {
	return topic_name;
};

System::String^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::TypeName::get() {
	return type_name;
};

OpenDDSharp::DDS::DurabilityQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::Durability::get() {
	return durability;
};

OpenDDSharp::DDS::DeadlineQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::Deadline::get() {
	return deadline;
};

OpenDDSharp::DDS::LatencyBudgetQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::LatencyBudget::get() {
	return latency_budget;
};

OpenDDSharp::DDS::LivelinessQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::Liveliness::get() {
	return liveliness;
};

OpenDDSharp::DDS::ReliabilityQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::Reliability::get() {
	return reliability;
};

OpenDDSharp::DDS::OwnershipQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::Ownership::get() {
	return ownership;
};

OpenDDSharp::DDS::DestinationOrderQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::DestinationOrder::get() {
	return destination_order;
};

OpenDDSharp::DDS::UserDataQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::UserData::get() {
	return user_data;
};

OpenDDSharp::DDS::TimeBasedFilterQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::TimeBasedFilter::get() {
	return time_based_filter;
};

OpenDDSharp::DDS::PresentationQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::Presentation::get() {
	return presentation;
};

OpenDDSharp::DDS::PartitionQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::Partition::get() {
	return partition;
};

OpenDDSharp::DDS::TopicDataQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::TopicData::get() {
	return topic_data;
};

OpenDDSharp::DDS::GroupDataQosPolicy^ OpenDDSharp::DDS::SubscriptionBuiltinTopicData::GroupData::get() {
	return group_data;
};

void OpenDDSharp::DDS::SubscriptionBuiltinTopicData::FromNative(::DDS::SubscriptionBuiltinTopicData native) {
	msclr::interop::marshal_context context;

	key.FromNative(native.key);
	participant_key.FromNative(native.participant_key);

	const char * name_ptr = native.topic_name;
	topic_name = context.marshal_as<System::String^>(name_ptr);

	const char * type_name_ptr = native.type_name;
	type_name = context.marshal_as<System::String^>(type_name_ptr);

	durability = gcnew OpenDDSharp::DDS::DurabilityQosPolicy();
	durability->FromNative(native.durability);

	deadline = gcnew OpenDDSharp::DDS::DeadlineQosPolicy();
	deadline->FromNative(native.deadline);

	latency_budget = gcnew OpenDDSharp::DDS::LatencyBudgetQosPolicy();
	latency_budget->FromNative(native.latency_budget);

	liveliness = gcnew OpenDDSharp::DDS::LivelinessQosPolicy();
	liveliness->FromNative(native.liveliness);

	reliability = gcnew OpenDDSharp::DDS::ReliabilityQosPolicy();
	reliability->FromNative(native.reliability);

	ownership = gcnew OpenDDSharp::DDS::OwnershipQosPolicy();
	ownership->FromNative(native.ownership);

	destination_order = gcnew OpenDDSharp::DDS::DestinationOrderQosPolicy();
	destination_order->FromNative(native.destination_order);

	user_data = gcnew OpenDDSharp::DDS::UserDataQosPolicy();
	user_data->FromNative(native.user_data);

	time_based_filter = gcnew OpenDDSharp::DDS::TimeBasedFilterQosPolicy();
	time_based_filter->FromNative(native.time_based_filter);

	presentation = gcnew OpenDDSharp::DDS::PresentationQosPolicy();
	presentation->FromNative(native.presentation);

	partition = gcnew OpenDDSharp::DDS::PartitionQosPolicy();
	partition->FromNative(native.partition);

	topic_data = gcnew OpenDDSharp::DDS::TopicDataQosPolicy();
	topic_data->FromNative(native.topic_data);

	group_data = gcnew OpenDDSharp::DDS::GroupDataQosPolicy();
	group_data->FromNative(native.group_data);
}

::DDS::SubscriptionBuiltinTopicData OpenDDSharp::DDS::SubscriptionBuiltinTopicData::ToNative() {
	::DDS::SubscriptionBuiltinTopicData native;
	msclr::interop::marshal_context context;

	native.key = key.ToNative();
	native.participant_key = participant_key.ToNative();
	native.topic_name = context.marshal_as<const char*>(topic_name);
	native.type_name = context.marshal_as<const char*>(type_name);
	native.durability = durability->ToNative();	
	native.deadline = deadline->ToNative();
	native.latency_budget = latency_budget->ToNative();
	native.liveliness = liveliness->ToNative();
	native.reliability = reliability->ToNative();
	native.ownership = ownership->ToNative();
	native.destination_order = destination_order->ToNative();
	native.user_data = user_data->ToNative();
	native.topic_data = topic_data->ToNative();
	native.time_based_filter = time_based_filter->ToNative();
	native.presentation = presentation->ToNative();
	native.partition = partition->ToNative();
	native.topic_data = topic_data->ToNative();
	native.group_data = group_data->ToNative();

	return native;
}