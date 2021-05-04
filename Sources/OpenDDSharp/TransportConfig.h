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

#pragma unmanaged 
#include <dds/DCPS/transport/framework/TransportConfig_rch.h>
#include <dds/DCPS/transport/framework/TransportConfig.h>
#pragma managed

#include "TransportInst.h"
#include "TransportInstManager.h"
#include <vcclr.h>
#include <msclr/marshal.h>

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {

            /// <summary>
            /// Represents a transport configuration.
            /// </summary>
            public ref class TransportConfig {

            public:
                /// <summary>
                /// The default passive connection duration
                /// </summary>
                static const System::UInt32 DEFAULT_PASSIVE_CONNECT_DURATION = ::OpenDDS::DCPS::TransportConfig::DEFAULT_PASSIVE_CONNECT_DURATION;

            internal:
                ::OpenDDS::DCPS::TransportConfig* impl_entity;

            public:
                /// <summary>
                /// Gets the configuration unique name.
                /// </summary>
                property System::String^ Name {
                    System::String^ get();
                };

                /// <summary>
                /// The ordered list of transport instances that
                /// this configuration will utilize.
                /// </summary>
                property IReadOnlyCollection<TransportInst^>^ Transports {
                    IReadOnlyCollection<TransportInst^>^ get();
                };

                /// <summary>
                /// A value of false causes DDS to serialize data in the
                /// source machine's native endianness; a value of true
                /// causes DDS to serialize data in the opposite
                /// endianness. The receiving side will adjust the data
                /// for its endianness so there is no need to match
                /// this option between machines. The purpose of this
                /// option is to allow the developer to decide which
                /// side will make the endian adjustment, if necessary.
                /// The default value is false.
                /// </summary>
                property System::Boolean SwapBytes {
                    System::Boolean get();
                    void set(System::Boolean value);
                };

                /// <summary>
                /// Timeout (milliseconds) for initial passive
                /// connection establishment. By default, this option
                /// waits for 60 seconds. A value of zero would wait
                /// indefnitely (not recommended).
                /// </summary>
                property System::UInt32 PassiveConnectDuration {
                    System::UInt32 get();
                    void set(System::UInt32 value);
                };
                
            internal:
                TransportConfig(::OpenDDS::DCPS::TransportConfig* native);

            public:
                /// <summary>
                /// Insert the <see cref="TransportInst" /> in the instances list.
                /// </summary>
                /// <param name="inst">The <see cref="TransportInst" /> to be inserted.</param>
                void Insert(TransportInst^ inst);

                /// <summary>
                /// Insert the <see cref="TransportInst" /> in sorted order (by name) in the instances_ list.
                /// Use when the names of the TransportInst objects are specifically assigned
                /// to have the sorted order make sense.
                /// </summary>
                /// <param name="inst">The <see cref="TransportInst" /> to be inserted.</param>
                void SortedInsert(TransportInst^ inst);

            };
        }
    }
}
