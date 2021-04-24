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
#include "TransportConfigManager.h"

OpenDDSharp::TransportConfigManager* OpenDDSharp::TransportConfigManager::get_instance() {
    return ACE_Singleton<TransportConfigManager, ACE_SYNCH_MUTEX>::instance();
}

void OpenDDSharp::TransportConfigManager::add(::OpenDDS::DCPS::TransportConfig* native, gcroot<OpenDDSharp::OpenDDS::DCPS::TransportConfig^> managed) {
    m_configs.emplace(native, managed);
}

void OpenDDSharp::TransportConfigManager::remove(::OpenDDS::DCPS::TransportConfig* native) {
    m_configs.erase(native);
}

gcroot<OpenDDSharp::OpenDDS::DCPS::TransportConfig^> OpenDDSharp::TransportConfigManager::find(::OpenDDS::DCPS::TransportConfig* native) {
    std::map<::OpenDDS::DCPS::TransportConfig*, gcroot<OpenDDSharp::OpenDDS::DCPS::TransportConfig^>>::iterator it;
    it = m_configs.find(native);
    if (it != m_configs.end()) {
        return it->second;
    }
    else {
        return nullptr;
    }
}