/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

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
#pragma once

#include "Entity.h"
#include <vcclr.h>
#include <map>

#pragma unmanaged 

#include <dds/DdsDcpsInfrastructureC.h>

namespace OpenDDSharp {

	private class EntityManager {

	friend class ACE_Singleton<EntityManager, ACE_SYNCH_MUTEX>;

	private:			
		std::map<::DDS::Entity_ptr, gcroot<OpenDDSharp::DDS::Entity^>> m_entities;

	private:
		EntityManager() = default;
		~EntityManager() = default;

	public:
		static OpenDDSharp::EntityManager* get_instance();
		void add(::DDS::Entity_ptr native, gcroot<::OpenDDSharp::DDS::Entity^> managed);
		void remove(::DDS::Entity_ptr native);
		gcroot<OpenDDSharp::DDS::Entity^> find(::DDS::Entity_ptr native);
	};
};

#pragma managed