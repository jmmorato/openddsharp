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
#include "TransportInstManager.h"

OpenDDSharp::TransportInstManager* OpenDDSharp::TransportInstManager::get_instance() {
    return ACE_Singleton<TransportInstManager, ACE_SYNCH_MUTEX>::instance();
}

void OpenDDSharp::TransportInstManager::add(::OpenDDS::DCPS::TransportInst* native, gcroot<OpenDDSharp::OpenDDS::DCPS::TransportInst^> managed) {
    m_insts.emplace(native, managed);
}

void OpenDDSharp::TransportInstManager::remove(::OpenDDS::DCPS::TransportInst* native) {
    m_insts.erase(native);
}

gcroot<OpenDDSharp::OpenDDS::DCPS::TransportInst^> OpenDDSharp::TransportInstManager::find(::OpenDDS::DCPS::TransportInst* native) {
    std::map<::OpenDDS::DCPS::TransportInst*, gcroot<OpenDDSharp::OpenDDS::DCPS::TransportInst^>>::iterator it;
    it = m_insts.find(native);
    if (it != m_insts.end()) {
        return it->second;
    }
    else {
        return nullptr;
    }
}