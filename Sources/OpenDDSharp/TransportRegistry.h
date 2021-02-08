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
#include <dds/DCPS/transport/framework/TransportRegistry.h>
#include "TransportConfigManager.h"
#include "TransportInstManager.h"
#pragma managed

#include "TransportConfig.h"
#include "Entity.h"

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
            /// The TransportRegistry is a singleton object which provides a mechanism to
            /// the application code to configure OpenDDS's use of the transport layer.
            /// </summary>
            public ref class TransportRegistry {

            private:
                static TransportRegistry _instance;      
            
            public:
                /// <summary>
                /// Gets the singleton instance
                /// </summary>
                static property TransportRegistry^ Instance { TransportRegistry^ get(); }

                /// <summary>
                /// Indicates whether the <see cref="TransportRegistry" /> has been released or not.
                /// </summary>
                property System::Boolean Released {
                    System::Boolean get();
                }

                /// <summary>
                /// Gets or sets the global <see cref="TransportConfig" />.
                /// </summary>
                property TransportConfig^ GlobalConfig {
                    TransportConfig^ get();
                    void set(TransportConfig^ value);
                }

            public:
                /// <summary>
                /// Close the singleton instance of this class.
                /// </summary>
                static void Close();

                /// <summary>
                /// This will shutdown all TransportImpl objects.
                /// Client Application calls this method to tear down the transport framework.
                /// </summary>
                void Release();

                /// <summary>
                /// Creates a new <see cref="TransportInst" />.
                /// </summary>
                /// <param name="name">A unique name for the transport instance.</param>
                /// <param name="transportType">The transport type for the instance. It should be one of the included transports (i.e. tcp, udp, multicast, shmem, and rtps_udp).</param>
                /// <returns>The newly created <see cref="TransportInst" /> or null if failed.</returns>
                TransportInst^ CreateInst(System::String^ name, System::String^ transportType);

                /// <summary>
                /// Gets an already created <see cref="TransportInst" />.
                /// </summary>
                /// <param name="name">The name given to the <see cref="TransportInst" /> during the creation.</param>
                /// <returns>The <see cref="TransportInst" /> or null if not found.</returns>
                TransportInst^ GetInst(System::String^ name);

                /// <summary>
                /// Removes a <see cref="TransportInst" />.
                /// </summary>
                /// <param name="inst">The <see cref="TransportInst" /> to be removed.</param>
                void RemoveInst(TransportInst^ inst);

                /// <summary>
                /// Creates a new <see cref="TransportConfig" />.
                /// </summary>
                /// <param name="name">A unique name for the config.</param>
                /// <returns>The newly created <see cref="TransportConfig" /> or null if failed.</returns>
                TransportConfig^ CreateConfig(System::String^ name);

                /// <summary>
                /// Gets an already created <see cref="TransportConfig" />.
                /// </summary>
                /// <param name="name">The name given to the <see cref="TransportConfig" /> during the creation.</param>
                /// <returns>The <see cref="TransportConfig" /> or null if not found.</returns>
                TransportConfig^ GetConfig(System::String^ name);

                /// <summary>
                /// Removes a <see cref="TransportConfig" />. 
                /// </summary>
                /// <param name="cfg">The <see cref="TransportConfig" /> to be removed.</param>
                void RemoveConfig(TransportConfig^ cfg);

                /// <summary>
                /// Gets the specific domain default <see cref="TransportConfig" />. 
                /// </summary>
                /// <param name="domain">The requested default <see cref="TransportConfig" /> domain id.</param>
                /// <returns>The default <see cref="TransportConfig" /> domain id if found, otherwise null.</returns>
                TransportConfig^ GetDomainDefaultConfig(System::Int32 domain);

                /// <summary>
                /// Sets the specific domain default <see cref="TransportConfig" />.  
                /// </summary>
                /// <param name="domain">The domain id where the default <see cref="TransportConfig" /> will be applied.</param>
                /// <param name="cfg">The <see cref="TransportConfig" /> to be set.</param>
                void SetDomainDefaultConfig(System::Int32 domain, TransportConfig^ cfg);

                /// <summary>
                /// Binds a <see cref="TransportConfig" /> to a <see cref="::OpenDDSharp::DDS::Entity" />.
                /// </summary>
                /// <param name="name">The name given to the <see cref="TransportConfig" /> during the creation.</param>
                /// <param name="entity">The <see cref="::OpenDDSharp::DDS::Entity" /> to be bound.</param>
                void BindConfig(System::String^ name, ::OpenDDSharp::DDS::Entity^ entity);

                /// <summary>
                /// Binds a <see cref="TransportConfig" /> to a <see cref="::OpenDDSharp::DDS::Entity" />.
                /// </summary>
                /// <param name="cfg">The <see cref="TransportConfig" /> to be applied.</param>
                /// <param name="entity">The <see cref="::OpenDDSharp::DDS::Entity" /> to be bound.</param>
                void BindConfig(TransportConfig^ cfg, ::OpenDDSharp::DDS::Entity^ entity);

            private:
                TransportRegistry();

            };
        }
    }
}
