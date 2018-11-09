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
#include <dds/DCPS/transport/tcp/Tcp.h>
#include <dds/DCPS/transport/tcp/TcpInst.h>
#include <dds/DCPS/transport/tcp/TcpInst_rch.h>
#pragma managed

#include "TransportInst.h"

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {

            public ref class TcpInst : public TransportInst {

            internal:
                ::OpenDDS::DCPS::TcpInst* impl_entity;

            public:
                property System::Boolean IsReliable {
                    System::Boolean get();
                };

                property System::Boolean EnableNagleAlgorithm {
                    System::Boolean get();
                    void set(System::Boolean value);
                };

                /// <summary>
                /// The initial retry delay in milliseconds.
                /// The first connection retry will be when the loss of connection
                /// is detected.  The second try will be after this delay.
                /// The default is 500 miliseconds.
                /// </summary>
                property System::Int32 ConnRetryInitialDelay {
                    System::Int32 get();
                    void set(System::Int32 value);
                };

                /// <summary>
                /// The backoff multiplier for reconnection strategy.
                /// The third and so on reconnect will be this value * the previous delay.
                /// Hence with conn_retry_initial_delay=500 and conn_retry_backoff_multiplier=1.5
                /// the second reconnect attempt will be at 0.5 seconds after first retry connect
                /// fails; the third attempt will be 0.75 seconds after the second retry connect
                /// fails; the fourth attempt will be 1.125 seconds after the third retry connect
                /// fails. The default value is 2.0.
                /// </summary>
                property System::Double ConnRetryBackoffMultiplier {
                    System::Double get();
                    void set(System::Double value);
                };

                /// <summary>
                /// Number of attemps to reconnect before giving up and calling
                /// OnPublicationLost() and OnSubscriptionLost() callbacks.
                /// The default is 3.
                /// </summary>
                property System::Int32 ConnRetryAttempts {
                    System::Int32 get();
                    void set(System::Int32 value);
                };

                /// <summary>
                /// Maximum period (in milliseconds) of not being able to send queued
                /// messages. If there are samples queued and no output for longer
                /// than this period then the connection will be closed and on_*_lost()
                /// callbacks will be called. If the value is zero, the default, then
                /// this check will not be made.
                /// </summary>
                property System::Int32 MaxOutputPausePeriod {
                    System::Int32 get();
                    void set(System::Int32 value);
                };

                /// <summary>
                /// The time period in milliseconds for the acceptor side
                /// of a connection to wait for the connection to be reconnected.
                /// If not reconnected within this period then
                /// OnPublicationLost() and OnSubscriptionLost() callbacks
                /// will be called. The default is 2 seconds (2000 millseconds).
                /// </summary>
                property System::Int32 PassiveReconnectDuration {
                    System::Int32 get();
                    void set(System::Int32 value);
                };

                /// <summary>
                /// The public address is our publicly advertised address.
                /// Usually this is the same as the local address, but if
                /// a public address is explicitly specified, use that.
                /// </summary>
                property System::String^ PublicAddress {
                    System::String^ get();
                    void set(System::String^ value);
                }

                property System::String^ LocalAddress {
                    System::String^ get();
                    void set(System::String^ value);
                }

            public:
                TcpInst(TransportInst^ inst);

            /*public:
                /// <summary>
                /// Implicit conversion operator from <see cref="TransportInst" /> to <see cref="TcpInst" />.
                /// </summary>
                /// <param name="value">The value to transform.</param>
                /// <returns>The <see cref="TcpInst" /> value.</returns>
                static operator TcpInst^(TransportInst^ value) {
                    TcpInst^ tcpi = gcnew TcpInst(value);
                    return tcpi;
                }*/
            };
        }
    }
}