#include "DomainParticipantQos.h"

OpenDDSharp::DDS::DomainParticipantQos::DomainParticipantQos() {
	user_data = gcnew OpenDDSharp::DDS::UserDataQosPolicy();
	entity_factory = gcnew OpenDDSharp::DDS::EntityFactoryQosPolicy();	
};

OpenDDSharp::DDS::UserDataQosPolicy^ OpenDDSharp::DDS::DomainParticipantQos::UserData::get() {
	return user_data;
};

OpenDDSharp::DDS::EntityFactoryQosPolicy^ OpenDDSharp::DDS::DomainParticipantQos::EntityFactory::get() {
	return entity_factory;
};

::DDS::DomainParticipantQos OpenDDSharp::DDS::DomainParticipantQos::ToNative() {
	::DDS::DomainParticipantQos* qos = new ::DDS::DomainParticipantQos();

	qos->entity_factory = entity_factory->ToNative();
	qos->user_data = user_data->ToNative();

	return *qos;
};

void OpenDDSharp::DDS::DomainParticipantQos::FromNative(::DDS::DomainParticipantQos qos) {
	entity_factory->FromNative(qos.entity_factory);
	user_data->FromNative(qos.user_data);
};
