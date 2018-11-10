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

            /// <summary>
            /// Represent a DCPSInfoRepository discovery.
            /// </summary>
            /// <remarks>
            /// An OpenDDS DCPSInfoRepo is a service on a local or remote node used for participant
            /// discovery. Confguring how participants should fnd DCPSInfoRepo is the purpose of this class.
            /// </remarks>
            public ref class InfoRepoDiscovery : Discovery {

            internal:
                ::OpenDDS::DCPS::InfoRepoDiscovery* impl_entity;

            public:
                /// <summary>
                /// Port used by the tcp transport for Built-In Topics. If the
                /// default of '0' is used, the operating system will choose a port
                /// to use. The default value is 0.
                /// </summary>
                property System::Int32 BitTransportPort {
                    System::Int32 get();
                    void set(System::Int32 value);
                }

                /// <summary>
                /// IP address identifying the local interface to be used by tcp
                /// transport for the Built-In Topics.
                /// </summary>
                property System::String^ BitTransportIp {
                    System::String^ get();
                    void set(System::String^ value);
                }

            public:
                /// <summary>
                /// Creates a new instance of <see cref="InfoRepoDiscovery" />.
                /// </summary>
                /// <param name="key">Unique key value for the repository.</param>
                /// <param name="ior">Repository IOR or host:port.</param>
                InfoRepoDiscovery(System::String^ key, System::String^ ior);
            };
        }
    }
}
