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

#pragma unmanaged 
#include <dds\DCPS\InfoRepoDiscovery\InfoRepoDiscovery.h>
#pragma managed

#include "Discovery.h"

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {

            public ref class InfoRepoDiscovery : Discovery {

            internal:
                ::OpenDDS::DCPS::InfoRepoDiscovery* impl_entity;

            public:
                property System::Int32 BitTransportPort {
                    System::Int32 get();
                    void set(System::Int32 value);
                }

                property System::String^ BitTransportIp {
                    System::String^ get();
                    void set(System::String^ value);
                }

            public:
                InfoRepoDiscovery(System::String^ key, System::String^ ior);
            };
        }
    }
}
