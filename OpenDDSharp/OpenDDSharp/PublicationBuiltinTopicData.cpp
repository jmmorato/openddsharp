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
#include "PublicationBuiltinTopicData.h"

OpenDDSharp::DDS::BuiltinTopicKey OpenDDSharp::DDS::PublicationBuiltinTopicData::Key::get() {
	return key;
};

OpenDDSharp::DDS::BuiltinTopicKey OpenDDSharp::DDS::PublicationBuiltinTopicData::ParticipantKey::get() {
	return participant_key;
};

System::String^ OpenDDSharp::DDS::PublicationBuiltinTopicData::TopicName::get() {
	return topic_name;
};

System::String^ OpenDDSharp::DDS::PublicationBuiltinTopicData::TypeName::get() {
	return type_name;
};

OpenDDSharp::DDS::DurabilityQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::Durability::get() {
	return durability;
};

OpenDDSharp::DDS::DurabilityServiceQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::DurabilityService::get() {
	return durability_service;
};

OpenDDSharp::DDS::DeadlineQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::Deadline::get() {
	return deadline;
};

OpenDDSharp::DDS::LatencyBudgetQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::LatencyBudget::get() {
	return latency_budget;
};

OpenDDSharp::DDS::LivelinessQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::Liveliness::get() {
	return liveliness;
};

OpenDDSharp::DDS::ReliabilityQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::Reliability::get() {
	return reliability;
};

OpenDDSharp::DDS::LifespanQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::Lifespan::get() {
	return lifespan;
};

OpenDDSharp::DDS::UserDataQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::UserData::get() {
	return user_data;
};

OpenDDSharp::DDS::OwnershipQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::Ownership::get() {
	return ownership;
};

OpenDDSharp::DDS::OwnershipStrengthQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::OwnershipStrength::get() {
	return ownership_strength;
};

OpenDDSharp::DDS::DestinationOrderQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::DestinationOrder::get() {
	return destination_order;
};

OpenDDSharp::DDS::PresentationQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::Presentation::get() {
	return presentation;
};

OpenDDSharp::DDS::PartitionQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::Partition::get() {
	return partition;
};

OpenDDSharp::DDS::TopicDataQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::TopicData::get() {
	return topic_data;
};

OpenDDSharp::DDS::GroupDataQosPolicy^ OpenDDSharp::DDS::PublicationBuiltinTopicData::GroupData::get() {
	return group_data;
};

void OpenDDSharp::DDS::PublicationBuiltinTopicData::FromNative(::DDS::PublicationBuiltinTopicData native) {
	msclr::interop::marshal_context context;

	key.FromNative(native.key);
	participant_key.FromNative(native.participant_key);

	const char * name_ptr = native.topic_name;
	topic_name = context.marshal_as<System::String^>(name_ptr);

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

	lifespan = gcnew OpenDDSharp::DDS::LifespanQosPolicy();
	lifespan->FromNative(native.lifespan);

	user_data = gcnew OpenDDSharp::DDS::UserDataQosPolicy();
	user_data->FromNative(native.user_data);

	ownership = gcnew OpenDDSharp::DDS::OwnershipQosPolicy();
	ownership->FromNative(native.ownership);

	ownership_strength = gcnew OpenDDSharp::DDS::OwnershipStrengthQosPolicy();
	ownership_strength->FromNative(native.ownership_strength);

	destination_order = gcnew OpenDDSharp::DDS::DestinationOrderQosPolicy();
	destination_order->FromNative(native.destination_order);

	presentation = gcnew OpenDDSharp::DDS::PresentationQosPolicy();
	presentation->FromNative(native.presentation);

	partition = gcnew OpenDDSharp::DDS::PartitionQosPolicy();
	partition->FromNative(native.partition);

	topic_data = gcnew OpenDDSharp::DDS::TopicDataQosPolicy();
	topic_data->FromNative(native.topic_data);

	group_data = gcnew OpenDDSharp::DDS::GroupDataQosPolicy();
	group_data->FromNative(native.group_data);
}