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
#include <dds/DCPS/transport/udp/Udp.h>
#include <dds/DCPS/transport/udp/UdpInst.h>
#include <dds/DCPS/transport/udp/UdpInst_rch.h>
#pragma managed

#include "TransportInst.h"

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {

            public ref class UdpInst : public TransportInst {

            internal:
                ::OpenDDS::DCPS::UdpInst* impl_entity;

            public:
                property System::Boolean IsReliable {
                    System::Boolean get();
                }

                property System::Int32 SendBufferSize {
                    System::Int32 get();
                    void set(System::Int32 value);
                }

                property System::Int32 RcvBufferSize {
                    System::Int32 get();
                    void set(System::Int32 value);
                }

                property System::String^ LocalAddress {
                    System::String^ get();
                    void set(System::String^ value);
                }

            public:
                UdpInst(TransportInst^ inst);

                /*public:
                    /// <summary>
                    /// Implicit conversion operator from <see cref="TransportInst" /> to <see cref="UdpInst" />.
                    /// </summary>
                    /// <param name="value">The value to transform.</param>
                    /// <returns>The <see cref="UdpInst" /> value.</returns>
                    static operator UdpInst^(TransportInst^ value) {
                        UdpInst^ udpi = gcnew UdpInst(value);
                        return udpi;
                    }*/
            };

        }
    }
}