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
                ::OpenDDS::DCPS::TransportRegistry* impl_entity;        
            
            public:
                /// <summary>
                /// Gets the singleton instance
                /// </summary>
                static property TransportRegistry^ Instance { TransportRegistry^ get(); }

                property System::Boolean Released {
                    System::Boolean get();
                }

                property TransportConfig^ GlobalConfig {
                    TransportConfig^ get();
                    void set(TransportConfig^ value);
                }

            public:
                static void Close();
                void Release();

                TransportInst^ CreateInst(System::String^ name, System::String^ transportType);
                TransportInst^ GetInst(System::String^ name);
                void RemoveInst(TransportInst^ inst);

                TransportConfig^ CreateConfig(System::String^ name);
                TransportConfig^ GetConfig(System::String^ name);
                void RemoveConfig(TransportConfig^ cfg);

                TransportConfig^ GetDomainDefaultConfig(System::Int32 domain);
                void SetDomainDefaultConfig(System::Int32 domain, TransportConfig^ cfg);

                void BindConfig(System::String^ name, ::OpenDDSharp::DDS::Entity^ entity);
                void BindConfig(TransportConfig^ cfg, ::OpenDDSharp::DDS::Entity^ entity);

            private:
                TransportRegistry();

            };
        }
    }
}
