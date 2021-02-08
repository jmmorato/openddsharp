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
#pragma once

#include "TransportInst.h"
#include <vcclr.h>
#include <map>

#pragma unmanaged 

#include <dds/DCPS/transport/framework/TransportInst_rch.h>

namespace OpenDDSharp {

    private class TransportInstManager {

        friend class ACE_Singleton<TransportInstManager, ACE_SYNCH_MUTEX>;

    private:
        std::map<::OpenDDS::DCPS::TransportInst*, gcroot<OpenDDSharp::OpenDDS::DCPS::TransportInst^>> m_insts;

    private:
        TransportInstManager() = default;
        ~TransportInstManager() = default;

    public:
        static OpenDDSharp::TransportInstManager* get_instance();
        void add(::OpenDDS::DCPS::TransportInst* native, gcroot<OpenDDSharp::OpenDDS::DCPS::TransportInst^> managed);
        void remove(::OpenDDS::DCPS::TransportInst* native);
        gcroot<OpenDDSharp::OpenDDS::DCPS::TransportInst^> find(::OpenDDS::DCPS::TransportInst* native);
    };
};

#pragma managed