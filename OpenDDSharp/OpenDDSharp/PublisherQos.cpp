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
	::DDS::PublisherQos* qos = new ::DDS::PublisherQos();

	qos->presentation = presentation->ToNative();
	qos->partition = partition->ToNative();
	qos->group_data = group_data->ToNative();
	qos->entity_factory = entity_factory->ToNative();

	return *qos;
};

void OpenDDSharp::DDS::PublisherQos::FromNative(::DDS::PublisherQos qos) {
	presentation->FromNative(qos.presentation);
	partition->FromNative(qos.partition);
	group_data->FromNative(qos.group_data);
	entity_factory->FromNative(qos.entity_factory);
};
