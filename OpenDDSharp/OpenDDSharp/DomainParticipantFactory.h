/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#pragma unmanaged 
#include <dds/DCPS/Service_Participant.h>
#include <dds/DCPS/Marked_Default_Qos.h>
#pragma managed

#include "DomainParticipant.h"
#include "DomainParticipantQos.h"
#include "DomainParticipantListener.h"
#include "DomainParticipantFactoryQos.h"
#include "ReturnCode.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Allows the creation and destruction of <see cref="DomainParticipant" /> objects.
		/// </summary>
		public ref class DomainParticipantFactory {

		private:
			::DDS::DomainParticipantFactory_ptr impl_entity;

		internal:
			DomainParticipantFactory(::DDS::DomainParticipantFactory_ptr factory);			

		public:
			/// <summary>
			/// Creates a new <see cref="DomainParticipant" /> object with the default QoS policies and without listener attached. 			
			/// </summary>
			/// <param name="domainId">Domain ID that the application intends to join.</param>
			/// <returns> The newly created <see cref="DomainParticipant" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId);

			/// <summary>
			/// Creates a <see cref="DomainParticipant" /> with the desired QoS policies and without listener attached.
			/// </summary>
			/// <remarks>
			/// If the specified QoS policies are not consistent, the operation will fail and no <see cref="DomainParticipant" /> will be created.
			/// </remarks>
			/// <param name="domainId">Domain ID that the application intends to join.</param>
			/// <param name="qos">The <see cref="DomainParticipantQos" /> policies to be used for creating the new <see cref="DomainParticipant" />.</param>
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId, OpenDDSharp::DDS::DomainParticipantQos^ qos);

			/// <summary>
			/// Creates a new <see cref="DomainParticipant" /> with the default QoS policies and attaches to it the specified <see cref="DomainParticipantListener" />.
			/// The specified <see cref="DomainParticipantListener" /> will be attached with the default <see cref="StatusMask" />.
			/// </summary>
			/// <param name="domainId">Domain ID that the application intends to join.</param>
			/// <param name="listener">The <see cref="DomainParticipantListener" /> to be attached to the newly created <see cref="DomainParticipant" />.</param>
			/// <returns> The newly created <see cref="DomainParticipant" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener);

			/// <summary>
			/// Creates a <see cref="DomainParticipant" /> with the default QoS policies and attaches to it the specified <see cref="DomainParticipantListener" />.
			/// </summary>
			/// <param name="domainId">Domain ID that the application intends to join.</param>
			/// <param name="listener">The <see cref="DomainParticipantListener" /> to be attached to the newly created <see cref="DomainParticipant" />.</param>
			/// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns> The newly created <see cref="DomainParticipant" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener, StatusMask statusMask);

			/// <summary>
			/// Creates a new <see cref="DomainParticipant" /> with the desired QoS policies and attaches to it the specified <see cref="DomainParticipantListener" />.
			/// The specified <see cref="DomainParticipantListener" /> will be attached with the default <see cref="StatusMask" />.
			/// </summary>
			/// <remarks>
			/// If the specified QoS policies are not consistent, the operation will fail and no <see cref="DomainParticipant" /> will be created.
			/// </remarks>
			/// <param name="domainId">Domain ID that the application intends to join.</param>
			/// <param name="qos">The <see cref="DomainParticipantQos" /> policies to be used for creating the new <see cref="DomainParticipant" />.</param>
			/// <param name="listener">The <see cref="DomainParticipantListener" /> to be attached to the newly created <see cref="DomainParticipant" />.</param>
			/// <returns> The newly created <see cref="DomainParticipant" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId, OpenDDSharp::DDS::DomainParticipantQos^ qos, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener);

			/// <summary>
			/// Creates a new <see cref="DomainParticipant" /> with the desired QoS policies and attaches to it the specified <see cref="DomainParticipantListener" />.	
			/// </summary>
			/// <remarks>
			/// If the specified QoS policies are not consistent, the operation will fail and no <see cref="DomainParticipant" /> will be created.
			/// </remarks>
			/// <param name="domainId">Domain ID that the application intends to join.</param>
			/// <param name="qos">The <see cref="DomainParticipantQos" /> policies to be used for creating the new <see cref="DomainParticipant" />.</param>
			/// <param name="listener">The <see cref="DomainParticipantListener" /> to be attached to the newly created <see cref="DomainParticipant" />.</param>
			/// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns> The newly created <see cref="DomainParticipant" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DomainParticipant^ CreateParticipant(int domainId, OpenDDSharp::DDS::DomainParticipantQos^ qos, OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener^ listener, StatusMask statusMask);

			/// <summary>
			/// This operation retrieves a previously created <see cref="DomainParticipant" /> belonging to specified <paramref name="domainId"/>.
			/// </summary>
			/// <remarks>
			/// If multiple <see cref="DomainParticipant" /> entities belonging to that <paramref name="domainId"/> exist, then the operation will return one of them. 
			/// It is not specified which one.
			/// <remarks>
			/// <param name="domainId">Domain ID of the participant to lookup.</param>
			/// <returns>The <see cref="DomainParticipant" />, if it exists, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DomainParticipant^ LookupParticipant(int domainId);	

			/// <summary>
			/// This operation returns the value of the <see cref="DomainParticipantFactory" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="DomainParticipantFactoryQos" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::DomainParticipantFactoryQos^ qos);

			/// <summary>
			/// Sets the value of the <see cref="DomainParticipantFactory" /> QoS policies.
			/// </summary>
			/// <remarks>
			/// This operation will check that the resulting policies are self consistent; if they are not, the operation will have no effect and return <see cref="ReturnCode::InconsistentPolicy" />.
			/// </remarks>
			/// <param name="qos">The default <see cref="DomainParticipantFactoryQos" /> to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::DomainParticipantFactoryQos^ qos);

			/// <summary>
			/// Gets the default value of the <see cref="DomainParticipant" /> QoS, that is, the QoS policies which will be used for
			/// newly created <see cref="DomainParticipant" /> entities in the case where the QoS policies are defaulted in the CreateParticipant operation.
			/// </summary>
			/// <remarks>
			/// The values retrieved <see cref="GetDefaultDomainParticipantQos" /> will match the set of values specified on the last successful call to
			/// <see cref="SetDefaultDomainParticipantQos" />, or else, if the call was never made, the default values defined by the DDS standard.
			/// </remarks>
			/// <param name="qos">The <see cref="DomainParticipantQos" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetDefaultDomainParticipantQos(OpenDDSharp::DDS::DomainParticipantQos^ qos);

			/// <summary>
			/// Sets a default value of the <see cref="DomainParticipant" /> QoS policies which will be used for newly created
			/// <see cref="DomainParticipant" /> entities in the case where the QoS policies are defaulted in the CreateParticipant operation.
			/// </summary>
			/// <remarks>
			/// This operation will check that the resulting policies are self consistent; if they are not, the operation will have no effect and return <see cref="ReturnCode::InconsistentPolicy" />.
			/// </remarks>
			/// <param name="qos">The default <see cref="DomainParticipantQos" /> to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetDefaultDomainParticipantQos(OpenDDSharp::DDS::DomainParticipantQos^ qos);

			/// <summary>
			/// Deletes an existing <see cref="DomainParticipant" />.
			/// </summary>
			/// <remarks>
			/// This operation can only be invoked if all domain entities belonging to the participant have already been deleted.
			/// Otherwise the error <see cref="ReturnCode::PreconditionNotMet" /> is returned.
			/// </remarks>
			/// <param name="participant">The <see cref="DomainParticipant" /> to be deleted.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode DeleteParticipant(OpenDDSharp::DDS::DomainParticipant^ participant);
		};

	};
};