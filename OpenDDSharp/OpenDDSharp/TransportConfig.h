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
#include <dds/DCPS/transport/framework/TransportConfig_rch.h>
#include <dds/DCPS/transport/framework/TransportConfig.h>
#pragma managed

#include "TransportInst.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {
            public ref class TransportConfig {

            public:
                static const System::UInt32 DEFAULT_PASSIVE_CONNECT_DURATION = ::OpenDDS::DCPS::TransportConfig::DEFAULT_PASSIVE_CONNECT_DURATION;

            internal:
                ::OpenDDS::DCPS::TransportConfig* impl_entity;

            public:
                /// <summary>
                /// Gets the configuration's name.
                /// </summary>
                property System::String^ Name {
                    System::String^ get();
                };

                /// <summary>
                /// Gets or sets the swap bytes configuration
                /// </summary>
                property System::Boolean SwapBytes {
                    System::Boolean get();
                    void set(System::Boolean value);
                };

                /// <summary>
                /// The time period in milliseconds for the acceptor side
                /// of a connection to wait for the connection.
                /// The default is 60 seconds
                /// </summary>
                property System::UInt32 PassiveConnectDuration {
                    System::UInt32 get();
                    void set(System::UInt32 value);
                };
                
            internal:
                TransportConfig(::OpenDDS::DCPS::TransportConfig* native);

            public:
                /// <summary>
                /// Insert the TransportInst in the instances list.
                /// </summary>
                void Insert(TransportInst^ inst);

                /// <summary>
                /// Insert the TransportInst in sorted order (by name) in the instances_ list.
                /// Use when the names of the TransportInst objects are specifically assigned
                /// to have the sorted order make sense.
                /// </summary>
                void SortedInsert(TransportInst^ inst);

            };
        }
    }
}
