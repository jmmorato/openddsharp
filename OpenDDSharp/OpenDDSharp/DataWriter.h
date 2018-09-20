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
#include <dds/DdsDcpsPublicationC.h>
#pragma managed

#include "Entity.h"
#include "EntityManager.h"
#include "ReturnCode.h"
#include "DataWriterQos.h"
#include "LivelinessLostStatus.h"
#include "OfferedDeadlineMissedStatus.h"
#include "OfferedIncompatibleQosStatus.h"
#include "PublicationMatchedStatus.h"
#include "Topic.h"
#include "SubscriptionBuiltinTopicData.h"
#include "SampleInfo.h"

#pragma make_public(::DDS::DataWriter)

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {
			ref class DataWriterListener;
		};
	};
};

namespace OpenDDSharp {
	namespace DDS {

		ref class Publisher;
		ref class DataReader;		

		/// <summary>
		/// DataWriter allows the application to set the value of the data to be published under a given <see cref="Topic" />.
		/// </summary>
		/// <remarks>
		/// <para>A DataWriter is attached to exactly one <see cref="Publisher" /> that acts as a factory for it.</para>
		/// <para>A DataWriter is bound to exactly one <see cref="Topic" /> and therefore to exactly one data type. The <see cref="Topic" /> 
		/// must exist prior to the DataWriter’s creation.</para>
		/// <para>The DataWriter must be specialized for each particular application data-type.</para>
		/// <para>All operations except for the operations <see cref="DataWriter::SetQos" />, <see cref="DataWriter::GetQos" />, SetListener,
		/// <see cref="DataWriter::GetListener" />, <see cref="Entity::Enable" />, and <see cref="Entity::StatusCondition" />
		/// return the value <see cref="ReturnCode::NotEnabled" /> if the DataWriter has not been enabled yet.</para>
		/// </remarks>
		public ref class DataWriter : public OpenDDSharp::DDS::Entity {

		public:
			/// <exclude />
			::DDS::DataWriter_ptr impl_entity;

		internal:
			OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ _listener;

		public:
			/// <summary>
			/// Gets the <see cref="Topic" /> associated with the <see cref="DataWriter" />. 
			/// This is the same <see cref="Topic" /> that was used to create the <see cref="DataWriter" />.
			/// </summary>
			property OpenDDSharp::DDS::Topic^ Topic{
				OpenDDSharp::DDS::Topic^ get();
			}

			/// <summary>
			/// Gets the <see cref="Publisher" /> to which the <see cref="DataWriter" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::Publisher^ Publisher{
				OpenDDSharp::DDS::Publisher^ get();
			}

		protected public:
			DataWriter(::DDS::DataWriter_ptr dataWriter);

		public:
			/// <summary>
			/// Gets the <see cref="DataWriter" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="DataWriterQos" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::DataWriterQos^ qos);

			/// <summary>
			/// Sets the <see cref="DataWriter" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="DataWriterQos" /> to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::DataWriterQos^ qos);

			/// <summary>
			/// Allows access to the attached <see cref="OpenDDSharp::OpenDDS::DCPS::DataWriterListener" />.
			/// </summary>
			/// <returns>The attached <see cref="OpenDDSharp::OpenDDS::DCPS::DataWriterListener" />.</returns>
			OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ GetListener();

			/// <summary>
			/// Sets the <see cref="OpenDDSharp::OpenDDS::DCPS::DataWriterListener" /> using the <see cref="StatusMask::DefaultStatusMask" />.
			/// </summary>
			/// <param name="listener">The <see cref="OpenDDSharp::OpenDDS::DCPS::DataWriterListener" /> to be set.</param>			
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener);

			/// <summary>
			/// Sets the <see cref="OpenDDSharp::OpenDDS::DCPS::DataWriterListener" />.
			/// </summary>
			/// <param name="listener">The <see cref="OpenDDSharp::OpenDDS::DCPS::DataWriterListener" /> to be set.</param> 
			/// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener, OpenDDSharp::DDS::StatusMask mask);			

			/// <summary>
			/// Blocks the calling thread until either all data written by the <see cref="DataWriter" /> is
			/// acknowledged by all matched <see cref="DataReader" /> entities that have <see cref="ReliabilityQosPolicyKind::ReliableReliabilityQos" />, or else the duration
			///	specified by the maxWait parameter elapses, whichever happens first.
			/// </summary>
			/// <remarks>
			/// <para>This operation is intended to be used only if the <see cref="DataWriter" /> has configured <see cref="ReliabilityQosPolicyKind::ReliableReliabilityQos" />. 
			/// Otherwise the operation will return immediately with <see cref="ReturnCode::Ok" />.</para>
			/// <para>A return value of <see cref="ReturnCode::Ok" /> indicates that all the samples
			/// written have been acknowledged by all reliable matched data readers; a return value of <see cref="ReturnCode::Timeout" /> indicates that maxWait
			///	elapsed before all the data was acknowledged.</para>
			/// </remarks>
			/// <param name="maxWait">The maximum <see cref="Duration" /> time to wait for the acknowledgments.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode WaitForAcknowledgments(OpenDDSharp::DDS::Duration maxWait);

			/// <summary>
			/// Allows access to the <see cref="LivelinessLostStatus" /> communication status.
			/// </summary>
			/// <param name="status">The <see cref="LivelinessLostStatus" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetLivelinessLostStatus(OpenDDSharp::DDS::LivelinessLostStatus% status);

			/// <summary>
			/// Allows access to the <see cref="OfferedDeadlineMissedStatus" /> communication status.
			/// </summary>
			/// <param name="status">The <see cref="OfferedDeadlineMissedStatus" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetOfferedDeadlineMissedStatus(OpenDDSharp::DDS::OfferedDeadlineMissedStatus% status);

			/// <summary>
			/// Allows access to the <see cref="OfferedIncompatibleQosStatus" /> communication status.
			/// </summary>
			/// <param name="status">The <see cref="OfferedIncompatibleQosStatus" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetOfferedIncompatibleQosStatus(OpenDDSharp::DDS::OfferedIncompatibleQosStatus% status);

			/// <summary>
			/// Allows access to the <see cref="PublicationMatchedStatus" /> communication status.
			/// </summary>
			/// <param name="status">The <see cref="PublicationMatchedStatus" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetPublicationMatchedStatus(OpenDDSharp::DDS::PublicationMatchedStatus% status);

			/// <summary>
			/// Manually asserts the liveliness of the <see cref="DataWriter" />. This is used in combination with the liveliness QoS
			/// policy to indicate to DDS that the entity remains active.
			/// </summary>
			/// <remarks>
			/// <para>This operation need only be used if the <see cref="LivelinessQosPolicy" /> setting is either <see cref="LivelinessQosPolicyKind::ManualByParticipantLivelinessQos" />			
			/// or <see cref="LivelinessQosPolicyKind::ManualByTopicLivelinessQos" />. Otherwise, it has no effect.</para>
			/// <para>NOTE: Writing data via the write operation on a <see cref="DataWriter" /> asserts liveliness on the <see cref="DataWriter" /> itself and its
			/// <see cref="DomainParticipant" />. Consequently the use of AssertLiveliness is only needed if the application is not writing data regularly.</para>
			/// </remarks>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode AssertLiveliness();
			
			/// <summary>
			/// Gets the collection of subscriptions currently "associated" with the <see cref="DataWriter" />; that is, subscriptions that have a
			/// matching <see cref="Topic" /> and compatible QoS that the application has not indicated should be "ignored" by means of the
			///	<see cref="DomainParticipant" /> IgnoreSubscription operation.
			/// </summary>
			/// <remarks>
			/// The handles returned in the 'subscriptionHandles' collection are the ones that are used by the DDS implementation to locally
			/// identify the corresponding matched <see cref="DataReader" /> entities. These handles match the ones that appear in the <see cref="SampleInfo::InstanceState" />
			///	property of the <see cref="SampleInfo" /> when reading the "DCPSSubscriptions" builtin topic.
			/// </remarks>
			/// <param name="subscriptionHandles">The collection of subscription <see cref="InstanceHandle" />s to be filled up.</param> 
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetMatchedSubscriptions(ICollection<OpenDDSharp::DDS::InstanceHandle>^ subscriptionHandles);

			
			/// <summary>
			/// Retrieves information on a subscription that is currently "associated" with the <see cref="DataWriter" />; that is, a subscription
			/// with a matching <see cref="Topic" /> and compatible QoS that the application has not indicated should be "ignored" by means of the
			///	<see cref="DomainParticipant" /> IgnoreSubscription operation.
			/// </summary>
			/// <remarks>
			/// <para>The subscriptionHandle must correspond to a subscription currently associated with the <see cref="DataWriter" />, otherwise the operation
			/// will fail and return <see cref="ReturnCode::BadParameter" />. The operation GetMatchedSubscriptions can be used to find the subscriptions that
			///	are currently matched with the <see cref="DataWriter" />.</para>
			/// </remarks>
			/// <param name="subscriptionHandle">The <see cref="InstanceHandle" /> of the subscription data requested.</param> 
			/// <param name="subscriptionData">The <see cref="SubscriptionBuiltinTopicData" /> structure to be filled up.</param> 	
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetMatchedSubscriptionData(OpenDDSharp::DDS::InstanceHandle subscriptionHandle, OpenDDSharp::DDS::SubscriptionBuiltinTopicData% subscriptionData);

		private:
			OpenDDSharp::DDS::Topic^ GetTopic();
			OpenDDSharp::DDS::Publisher^ GetPublisher();
		};

	};
};