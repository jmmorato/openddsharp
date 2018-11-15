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
#include <dds/DCPS/Service_Participant.h>
#include <dds/DCPS/RcHandle_T.h>
#pragma managed

#include "Discovery.h"
#include "RtpsDiscovery.h"

#pragma make_public(DDS::SampleInfo)

#include "DomainParticipantFactory.h"

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			/// <summary>
			/// Singleton object to obtain the <see cref="OpenDDSharp::DDS::DomainParticipantFactory" />.
			/// </summary>
			public ref class ParticipantService {

			private:
				static ParticipantService _instance;
				::OpenDDS::DCPS::Service_Participant* impl_entity;
            
            internal:
                unsigned long counter = 0;

			public:
				/// <summary>
				/// Gets the singleton instance.
				/// </summary>
                static property ParticipantService^ Instance
                { 
                    ParticipantService^ get();
                }

                /// <summary>
                /// Indicates if the participant has been shutdown.
                /// </summary>
                property System::Boolean IsShutdown {
                    System::Boolean get();                    
                }

                /// <summary>
                /// Gets or sets the default discovery.
                /// </summary>
                property System::String^ DefaultDiscovery {
                    System::String^ get();
                    void set(System::String^ value);
                }

			private:
				ParticipantService();
		
			public:
				/// <summary>
				/// Initialize the DDS client environment and get the <see cref="OpenDDSharp::DDS::DomainParticipantFactory" />.
				/// </summary>
				/// <returns> The <see cref="OpenDDSharp::DDS::DomainParticipantFactory" />.</returns>
				OpenDDSharp::DDS::DomainParticipantFactory^ GetDomainParticipantFactory();

				/// <summary>
				/// Initialize the DDS client environment and get the <see cref="OpenDDSharp::DDS::DomainParticipantFactory" />.
				/// This method consumes -DCPS* and -ORB* options and their arguments.
				/// </summary>
				/// <param name="argv">The array of parameters to be consumed (i.e. -DCPS* and -ORB* options).</param>
				/// <returns> The <see cref="OpenDDSharp::DDS::DomainParticipantFactory" />.</returns>
				OpenDDSharp::DDS::DomainParticipantFactory^ GetDomainParticipantFactory(...array<System::String^>^ argv);

                /// <summary>
                /// Add a new <see cref="::OpenDDSharp::OpenDDS::DCPS::Discovery" />
                /// </summary>
                void AddDiscovery(::OpenDDSharp::OpenDDS::DCPS::Discovery^ discovery);

                /// <summary>
                /// Set the discovery repository for a specifi domain id.
                /// </summary>
                /// <param name="domain">The domain id.</param>
                /// <param name="repo">The repository key.</param>
                void SetRepoDomain(System::Int32 domain, System::String^ repo);

                /// <summary>
                /// Set the discovery repository for a specifi domain id.
                /// </summary>
                /// <param name="domain">The domain id.</param>
                /// <param name="repo">The repository key.</param>
                /// <param name="attachParticipant">Indicates if the current participant should be attached to the new repository.</param>
                void SetRepoDomain(System::Int32 domain,  System::String^ repo, bool attachParticipant);

				/// <summary>
				/// Stop being a participant in the service.
				/// </summary>
				/// <remarks>
				/// Required Precondition: All DomainParticipants have been deleted.
				/// </remarks>
				void Shutdown();
			};
		};
	};
};