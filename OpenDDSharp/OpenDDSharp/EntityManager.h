#pragma once

#include "Entity.h"
#include <vcclr.h>
#include <map>

#pragma unmanaged 

#include <dds/DdsDcpsInfrastructureC.h>

namespace OpenDDSharp {

	private class EntityManager {

	private:				
		std::map<::DDS::Entity_ptr, gcroot<OpenDDSharp::DDS::Entity^>> m_entities;

	private:
		EntityManager() = default;
		~EntityManager() = default;
		EntityManager(const EntityManager&) = delete;
		EntityManager& operator=(const EntityManager&) = delete;

	public:
		static OpenDDSharp::EntityManager& get_instance();
		void add(::DDS::Entity_ptr native, gcroot<::OpenDDSharp::DDS::Entity^> managed);
		void remove(::DDS::Entity_ptr native);
		gcroot<OpenDDSharp::DDS::Entity^> find(::DDS::Entity_ptr native);
	};
};

#pragma managed