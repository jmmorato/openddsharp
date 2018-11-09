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
#include <dds/DCPS/transport/shmem/Shmem.h>
#include <dds/DCPS/transport/shmem/ShmemInst.h>
#include <dds/DCPS/transport/shmem/ShmemInst_rch.h>
#pragma managed

#include "TransportInst.h"

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {
            
            public ref class ShmemInst : public TransportInst {

            internal:
                ::OpenDDS::DCPS::ShmemInst* impl_entity;

            public:
                property System::Boolean IsReliable {
                    System::Boolean get();
                };

                /// <summary>
                /// Size (in bytes) of the single shared-memory pool allocated by this
                /// transport instance.  Defaults to 16 megabytes.
                /// </summary>
                property size_t PoolSize {
                    size_t get();
                    void set(size_t value);
                }

                /// <summary>
                /// Size (in bytes) of the control area allocated for each data link.
                /// This allocation comes out of the shared-memory pool defined by <see cref="PoolSize" />.
                /// Defaults to 4 kilobytes.
                /// </summary>
                property size_t DatalinkControlSize {
                    size_t get();
                    void set(size_t value);
                }

                property System::String^ HostName {
                    System::String^ get();
                }

                property System::String^ PoolName {
                    System::String^ get();
                }

            public:
                ShmemInst(TransportInst^ inst);

            /*public:
                /// <summary>
                /// Implicit conversion operator from <see cref="TransportInst" /> to <see cref="ShmemInst" />.
                /// </summary>
                /// <param name="value">The value to transform.</param>
                /// <returns>The <see cref="ShmemInst" /> value.</returns>
                static operator ShmemInst^(TransportInst^ value) {
                    ShmemInst^ shmemi = gcnew ShmemInst(value);
                    return shmemi;
                }*/
            };
        }
    }
}
