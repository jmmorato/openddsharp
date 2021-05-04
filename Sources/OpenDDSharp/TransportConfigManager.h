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

#include "TransportConfig.h"
#include <vcclr.h>
#include <map>

#pragma unmanaged 

#include <dds/DCPS/transport/framework/TransportConfig_rch.h>

namespace OpenDDSharp {

    private class TransportConfigManager {

    friend class ACE_Singleton<TransportConfigManager, ACE_SYNCH_MUTEX>;

    private:
        std::map<::OpenDDS::DCPS::TransportConfig*, gcroot<OpenDDSharp::OpenDDS::DCPS::TransportConfig^>> m_configs;

    private:
        TransportConfigManager() = default;
        ~TransportConfigManager() = default;

    public:
        static OpenDDSharp::TransportConfigManager* get_instance();
        void add(::OpenDDS::DCPS::TransportConfig* native, gcroot<OpenDDSharp::OpenDDS::DCPS::TransportConfig^> managed);
        void remove(::OpenDDS::DCPS::TransportConfig* native);
        gcroot<OpenDDSharp::OpenDDS::DCPS::TransportConfig^> find(::OpenDDS::DCPS::TransportConfig* native);
    };
};

#pragma managed