#include "EntityManager.h"

OpenDDSharp::EntityManager& OpenDDSharp::EntityManager::get_instance() {
	static EntityManager instance;
	return instance;
}

void OpenDDSharp::EntityManager::add(::DDS::Entity_ptr native, gcroot<OpenDDSharp::DDS::Entity^> managed) {
	m_entities.emplace(native, managed);
}

void OpenDDSharp::EntityManager::remove(::DDS::Entity_ptr native) {
	m_entities.erase(native);
}

gcroot<OpenDDSharp::DDS::Entity^> OpenDDSharp::EntityManager::find(::DDS::Entity_ptr native) {
	std::map<::DDS::Entity_ptr, gcroot<OpenDDSharp::DDS::Entity^>>::iterator it;
	it = m_entities.find(native);
	if (it != m_entities.end()) {
		return it->second;
	}
	else {
		return nullptr;
	}
}