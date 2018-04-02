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