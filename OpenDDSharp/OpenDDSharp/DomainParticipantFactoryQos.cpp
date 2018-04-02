#include "DomainParticipantFactoryQos.h"

OpenDDSharp::DDS::DomainParticipantFactoryQos::DomainParticipantFactoryQos() {	
	entity_factory = gcnew OpenDDSharp::DDS::EntityFactoryQosPolicy();
};

OpenDDSharp::DDS::EntityFactoryQosPolicy^ OpenDDSharp::DDS::DomainParticipantFactoryQos::EntityFactory::get() {
	return entity_factory;
};

::DDS::DomainParticipantFactoryQos OpenDDSharp::DDS::DomainParticipantFactoryQos::ToNative() {
	::DDS::DomainParticipantFactoryQos qos;

	qos.entity_factory = entity_factory->ToNative();	

	return qos;
};

void OpenDDSharp::DDS::DomainParticipantFactoryQos::FromNative(::DDS::DomainParticipantFactoryQos qos) {
	entity_factory->FromNative(qos.entity_factory);	
};
