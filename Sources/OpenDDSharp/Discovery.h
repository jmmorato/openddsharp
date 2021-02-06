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
#include <dds\DCPS\Discovery.h>
#pragma managed

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
    namespace OpenDDS {
        namespace DCPS {

            /// <summary>
            /// Discovery base class.
            /// </summary>
            public ref class Discovery {

            internal:
                ::OpenDDS::DCPS::Discovery* impl_entity;

            public:
                /// <summary>
                /// The InfoRepo discovery default key.
                /// </summary>
                static const System::String^ DEFAULT_REPO = "DEFAULT_REPO";
                /// <summary>
                /// The RTPS discovery default key.
                /// </summary>
                static const System::String^ DEFAULT_RTPS = "DEFAULT_RTPS";
                /// <summary>
                /// The static discovery default key.
                /// </summary>
                static const System::String^ DEFAULT_STATIC = "DEFAULT_STATIC";

            public:
                /// <summary>
                /// The discovery unique key
                /// </summary>
                property System::String^ Key {
                    System::String^ get() {
                        msclr::interop::marshal_context context;
                        return context.marshal_as<System::String^>(impl_entity->key().c_str());
                    }
                };

            internal:
                Discovery() { };

            };
        }
    }
}