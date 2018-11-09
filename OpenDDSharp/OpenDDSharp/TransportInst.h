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
#include <dds/DCPS/transport/framework/TransportInst.h>
#include <dds/DCPS/transport/framework/TransportInst_rch.h>
#pragma managed

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {            

            /// <summary>
            /// Base class to hold configuration settings for TransportImpls.
            /// </summary>
            /// <remarks>
            /// Each transport implementation will need to define a concrete
            /// subclass of the TransportInst class.The base
            /// class (TransportInst) contains configuration settings that
            /// are common to all (or most) concrete transport implementations.
            /// The concrete transport implementation defines any configuration
            /// settings that it requires within its concrete subclass of this
            /// TransportInst base class.
            /// </remarks>
            public ref class TransportInst {

            internal:
                ::OpenDDS::DCPS::TransportInst* impl_entity;

            public:
                /// <summary>
                /// Gets the configuration's name.
                /// </summary>
                property System::String^ Name {
                    System::String^ get();
                };

                /// <summary>
                /// Number of pre-created link (list) objects per pool for the
                /// "send queue" of each DataLink.
                /// </summary>
                property size_t QueueMessagesPerPool {
                    size_t get();
                    void set(size_t value);
                };

                /// <summary>
                /// Initial number of pre-allocated pools of link (list) objects
                /// for the "send queue" of each DataLink.
                /// </summary>
                property size_t QueueInitialPools {
                    size_t get();
                    void set(size_t value);
                };

                /// <summary>
                /// Max size (in bytes) of a packet (packet header + sample(s)).
                /// </summary>
                property System::UInt32 MaxPacketSize {
                    System::UInt32 get();
                    void set(System::UInt32 value);
                };

                /// <summary>
                /// Optimum size (in bytes) of a packet (packet header + sample(s)).
                /// </summary>
                property System::UInt32 OptimumPacketSize {
                    System::UInt32 get();
                    void set(System::UInt32 value);
                };

                /// <summary>
                /// Flag for whether a new thread is needed for connection to
                /// send without backpressure.
                /// </summary>
                property System::Boolean ThreadPerConnection {
                    System::Boolean get();
                    void set(System::Boolean value);
                };

                /// <summary>
                /// Delay in milliseconds that the datalink should be released after all
                /// associations are removed. The default value is 10 seconds.
                /// </summary>
                property long DatalinkReleaseDelay {
                    long get();
                    void set(long value);
                };

                /// <summary>
                /// The number of chunks used to size allocators for transport control
                /// samples. The default value is 32.
                /// </summary>
                property size_t DatalinkControlChunks {
                    size_t get();
                    void set(size_t value);
                };

            internal:
                TransportInst(::OpenDDS::DCPS::TransportInst* native);            

            };
        }
    }
}
