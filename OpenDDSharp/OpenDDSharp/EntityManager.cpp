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
#include "EntityManager.h"

OpenDDSharp::EntityManager* OpenDDSharp::EntityManager::get_instance() {
	return ACE_Singleton<EntityManager, ACE_SYNCH_MUTEX>::instance();
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