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
                /// Type of the transport; tcp, udp, multicast,
                /// shmem, and rtps_udp are included with OpenDDSharp.
                /// </summary>
                property System::String^ TransportType {
                    System::String^ get();
                };

                /// <summary>
                /// Gets the configuration's name.
                /// </summary>
                property System::String^ Name {
                    System::String^ get();
                };

                /// <summary>
                /// Number of pre-created link (list) objects per pool for the
                /// "send queue" of each DataLink. The default value is 10.
                /// </summary>
                /// <remarks>
                /// When backpressure is detected, messages to be
                /// sent are queued.When the message queue must
                /// grow, it grows by this number.                 
                /// </remarks>
                property size_t QueueMessagesPerPool {
                    size_t get();
                    void set(size_t value);
                };

                /// <summary>
                /// The initial number of pools for the backpressure
                /// queue. The default value is 5.
                /// </summary>
                /// <remarks>
                /// The default settings of the two backpressure queue values
                /// preallocate space for 50 messages(5 pools of 10 messages).                 
                /// </remarks>
                property size_t QueueInitialPools {
                    size_t get();
                    void set(size_t value);
                };

                /// <summary>
                /// The maximum size of a transport packet, including
                /// its transport header, sample header, and sample data.
                /// The default value is 2147481599.
                /// </summary>
                property System::UInt32 MaxPacketSize {
                    System::UInt32 get();
                    void set(System::UInt32 value);
                };

                /// <summary>
                /// Maximum number of samples in a transport packet.
                /// The default value is 10.
                /// </summary>
                property size_t MaxSamplesPerPacket {
                    size_t get();
                    void set(size_t value);
                };

                /// <summary>
                /// Optimum size (in bytes) of a packet (packet header + sample(s)).
                /// The default value is 4096.
                /// </summary>
                /// <remarks>
                /// Transport packets greater than this size will be sent over the wire even if there are still queued
                /// samples to be sent. This value may impact performance depending on your network
                /// confguration and application nature.
                /// </remarks>
                property System::UInt32 OptimumPacketSize {
                    System::UInt32 get();
                    void set(System::UInt32 value);
                };

                /// <summary>
                /// Enable or disable the thread per connection send
                /// strategy. By default, this option is disabled (false).
                /// </summary>
                /// <remarks>
                /// Enabling the ThreadPerConnection option will increase performance when writing to
                /// multiple data readers on diferent process as long as the overhead of thread context
                /// switching does not outweigh the benefts of parallel writes.This balance of network
                /// performance to context switching overhead is best determined by experimenting. If a
                /// machine has multiple network cards, it may improve performance by creating a transport
                /// for each network card.
                /// </remarks>
                property System::Boolean ThreadPerConnection {
                    System::Boolean get();
                    void set(System::Boolean value);
                };

                /// <summary>
                /// Delay in milliseconds that the datalink should be released after all
                /// associations are removed. The default value is 10 seconds.
                /// </summary>
                /// <remarks>
                /// The DatalinkReleaseDelay is the delay for datalink release after no associations.
                /// Increasing this value may reduce the overhead of re-establishment when reader/writer
                /// associations are added and removed frequently.
                /// </remarks>
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
