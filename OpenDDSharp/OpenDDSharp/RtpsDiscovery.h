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
#include <dds\DCPS\RTPS\RtpsDiscovery.h>
#pragma managed

#include "Discovery.h"
#include "TimeValue.h"

#include <vcclr.h>
#include <msclr/marshal.h>

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace RTPS {
            
            public ref class RtpsDiscovery : public ::OpenDDSharp::OpenDDS::DCPS::Discovery {

            internal:
                ::OpenDDS::RTPS::RtpsDiscovery* impl_entity;

            public:
                property TimeValue ResendPeriod {
                    TimeValue get();
                    void set(TimeValue value);
                }

                property System::UInt16 PB {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                property System::UInt16 DG {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                property System::UInt16 PG {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                property System::UInt16 D0 {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                property System::UInt16 D1 {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                property System::UInt16 DX {
                    System::UInt16 get();
                    void set(System::UInt16 value);
                }

                property System::Byte TTL {
                    System::Byte get();
                    void set(System::Byte value);
                };

                property System::String^ SedpLocalAddress {
                    System::String^ get();
                    void set(System::String^ value);
                };

                property System::String^ SpdpLocalAddress {
                    System::String^ get();
                    void set(System::String^ value);
                };

                property System::Boolean SedpMulticast {
                    System::Boolean get();
                    void set(System::Boolean value);
                };

                property System::String^ MulticastInterface {
                    System::String^ get();
                    void set(System::String^ value);
                };

                property System::String^ DefaultMulticastGroup {
                    System::String^ get();
                    void set(System::String^ value);
                };

                property IEnumerable<System::String^>^ SpdpSendAddrs {
                    IEnumerable<System::String^>^ get();
                };

                property System::String^ GuidInterface {
                    System::String^ get();
                    void set(System::String^ value);
                };
                
            public:
                RtpsDiscovery(System::String^ name);
            };
        }
    }
}