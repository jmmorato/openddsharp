#include "EntityFactoryQosPolicy.h"

OpenDDSharp::DDS::EntityFactoryQosPolicy::EntityFactoryQosPolicy() {
	autoenable_created_entities = true;
};

System::Boolean OpenDDSharp::DDS::EntityFactoryQosPolicy::AutoenableCreatedEntities::get() {
	return autoenable_created_entities;	
}

void OpenDDSharp::DDS::EntityFactoryQosPolicy::AutoenableCreatedEntities::set(System::Boolean value) {	
	autoenable_created_entities = value;
}

::DDS::EntityFactoryQosPolicy OpenDDSharp::DDS::EntityFactoryQosPolicy::ToNative() {
	::DDS::EntityFactoryQosPolicy* qos = new ::DDS::EntityFactoryQosPolicy();

	if (autoenable_created_entities) {
		qos->autoenable_created_entities = TRUE;
	}
	else {
		qos->autoenable_created_entities = FALSE;
	}

	return *qos;
};

void OpenDDSharp::DDS::EntityFactoryQosPolicy::FromNative(::DDS::EntityFactoryQosPolicy qos) {	
	if (qos.autoenable_created_entities == FALSE) {
		autoenable_created_entities = false;
	}
	else {
		autoenable_created_entities = true;
	}
};