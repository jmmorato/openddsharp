#include "SubscriberQos.h"

OpenDDSharp::DDS::SubscriberQos::SubscriberQos() {
	presentation = gcnew OpenDDSharp::DDS::PresentationQosPolicy();
	partition = gcnew OpenDDSharp::DDS::PartitionQosPolicy();
	group_data = gcnew OpenDDSharp::DDS::GroupDataQosPolicy();
	entity_factory = gcnew OpenDDSharp::DDS::EntityFactoryQosPolicy();
};

OpenDDSharp::DDS::PresentationQosPolicy^ OpenDDSharp::DDS::SubscriberQos::Presentation::get() {
	return presentation;
};

OpenDDSharp::DDS::PartitionQosPolicy^ OpenDDSharp::DDS::SubscriberQos::Partition::get() {
	return partition;
};

OpenDDSharp::DDS::GroupDataQosPolicy^ OpenDDSharp::DDS::SubscriberQos::GroupData::get() {
	return group_data;
};

OpenDDSharp::DDS::EntityFactoryQosPolicy^ OpenDDSharp::DDS::SubscriberQos::EntityFactory::get() {
	return entity_factory;
};

::DDS::SubscriberQos OpenDDSharp::DDS::SubscriberQos::ToNative() {
	::DDS::SubscriberQos* qos = new ::DDS::SubscriberQos();

	qos->presentation = presentation->ToNative();
	qos->partition = partition->ToNative();
	qos->group_data = group_data->ToNative();
	qos->entity_factory = entity_factory->ToNative();

	return *qos;
};

void OpenDDSharp::DDS::SubscriberQos::FromNative(::DDS::SubscriberQos qos) {
	presentation->FromNative(qos.presentation);
	partition->FromNative(qos.partition);
	group_data->FromNative(qos.group_data);
	entity_factory->FromNative(qos.entity_factory);
};
