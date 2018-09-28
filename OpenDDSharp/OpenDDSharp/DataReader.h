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
#include <dds/DdsDcpsSubscriptionC.h>
#pragma managed

#include "Entity.h"
#include "EntityManager.h"
#include "ReturnCode.h"
#include "DataReaderQos.h"
#include "StatusMask.h"
#include "SampleRejectedStatus.h"
#include "LivelinessChangedStatus.h"
#include "RequestedDeadlineMissedStatus.h"
#include "RequestedIncompatibleQosStatus.h"
#include "SubscriptionMatchedStatus.h"
#include "SampleLostStatus.h"
#include "SampleStateMask.h"
#include "ViewStateMask.h"
#include "InstanceStateMask.h"
#include "PublicationBuiltinTopicData.h"
#include "ITopicDescription.h"
#include "SampleInfo.h"

#include <vcclr.h>
#include <msclr/marshal.h>

#pragma make_public(::DDS::DataReader)

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {
			ref class DataReaderListener;
		};
	};
};

namespace OpenDDSharp {
	namespace DDS {

		ref class Subscriber;
		ref class ReadCondition;
		ref class QueryCondition;		
		ref class DataWriter;

		/// <summary>
		/// A DataReader allows the application to declare the data it wishes to receive (i.e., make a subscription) and to access the
		/// data received by the attached <see cref="Subscriber" />.
		/// </summary>
		/// <remarks>
		/// <para>A DataReader refers to exactly one <see cref="ITopicDescription" /> (either a <see cref="Topic" />, a <see cref="ContentFilteredTopic" />, or a <see cref="MultiTopic" />) 
		/// that identifies the data to be read. The subscription has a unique resulting type. The data-reader may give access to several instances of the
		///	resulting type, which can be distinguished from each other by their key.</para>
		/// <para>All operations except for the operations <see cref="SetQos" />, <see cref="GetQos" />, SetListener,
		/// <see cref="GetListener" />, <see cref="Entity::Enable" />, and <see cref="Entity::StatusCondition" />
		/// return the value <see cref="ReturnCode::NotEnabled" /> if the DataReader has not been enabled yet.</para>
		/// </remarks>
		public ref class DataReader : public OpenDDSharp::DDS::Entity {

		public:
			/// <exclude />
			::DDS::DataReader_ptr impl_entity;

		internal:
			OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ _listener;       
            ICollection<ReadCondition^>^ conditions;

        public:
            !DataReader();

		public:
			/// <summary>
			/// Gets the <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::Subscriber^ Subscriber {
				OpenDDSharp::DDS::Subscriber^ get();
			}

			/// <summary>
			/// Gets the <see cref="ITopicDescription" /> associated with the <see cref="DataReader" />. 
			/// This is the same <see cref="ITopicDescription" /> that was used to create the <see cref="DataReader" />.
			/// </summary>
			property OpenDDSharp::DDS::ITopicDescription^ TopicDescription {
				OpenDDSharp::DDS::ITopicDescription^ get();
			}

		protected public:
			DataReader(::DDS::DataReader_ptr dataReader);
		
		public:
			/// <summary>
			/// Creates a <see cref="ReadCondition" /> to read samples with any sample states, any view states and any instance states.
			/// </summary>
			/// <remarks>
			/// The returned <see cref="ReadCondition" /> will be attached and belong to the <see cref="DataReader" />.
			/// </remarks>
			/// <returns>The newly created <see cref="ReadCondition" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::ReadCondition^ CreateReadCondition();

			/// <summary>
			/// Creates a <see cref="ReadCondition" /> to read samples with the desired sample states, view states and instance states.
			/// </summary>
			/// <param name="sampleStates">The desired sample states mask.</param>
			/// <param name="viewStates">The desired view states mask.</param>
			/// <param name="instanceStates">The desired instance states mask.</param>
			/// <returns>The newly created <see cref="ReadCondition" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::ReadCondition^ CreateReadCondition(SampleStateMask sampleStates, ViewStateMask viewStates, InstanceStateMask instanceStates);			

			/// <summary>
			/// Creates a <see cref="QueryCondition" /> to read samples with any sample states, any view states and any instance states. 
			/// </summary>
			/// <remarks>
			/// The returned <see cref="QueryCondition" /> will be attached and belong to the <see cref="DataReader" />.
			/// </remarks>						
			/// <param name="queryExpression">The query string, which must be a subset of the SQL query language.</param>
			/// <param name="queryParameters">A sequence of strings which are the parameter values used in the SQL query string. The number of values in queryParameters must be equal or greater than the highest referenced n token in the queryExpression.</param>
			/// <returns>The newly created <see cref="QueryCondition" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::QueryCondition^ CreateQueryCondition(System::String^ queryExpression, ... array<System::String^>^ queryParameters);

			/// <summary>
			/// Creates a <see cref="QueryCondition" /> with the desired sample states, view states and instance states. 
			/// </summary>
			/// <remarks>
			/// The returned <see cref="QueryCondition" /> will be attached and belong to the <see cref="DataReader" />.
			/// </remarks>			
			/// <param name="sampleStates">The desired sample states mask.</param>
			/// <param name="viewStates">The desired view states mask.</param>
			/// <param name="instanceStates">The desired instance states mask.</param>
			/// <param name="queryExpression">The query string, which must be a subset of the SQL query language.</param>
			/// <param name="queryParameters">A sequence of strings which are the parameter values used in the SQL query string. The number of values in queryParameters must be equal or greater than the highest referenced n token in the queryExpression.</param>
			/// <returns>The newly created <see cref="QueryCondition" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::QueryCondition^ CreateQueryCondition(SampleStateMask sampleStates, ViewStateMask viewStates, InstanceStateMask instanceStates, System::String^ queryExpression, ... array<System::String^>^ queryParameters);

			/// <summary>
			/// Deletes a <see cref="ReadCondition" /> attached to the <see cref="DataReader" />. Since <see cref="QueryCondition" /> specializes <see cref="ReadCondition" /> it can
			/// also be used to delete a <see cref="QueryCondition" />.
			/// </summary>
			/// <remarks>
			/// If the <see cref="ReadCondition" /> is not attached to the <see cref="DataReader" />, the operation will return the error <see cref="ReturnCode::PreconditionNotMet" />.
			/// </remarks>
			/// <param name="condition">The <see cref="ReadCondition" /> to be deleted.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode DeleteReadCondition(OpenDDSharp::DDS::ReadCondition^ condition);

			/// <summary>
			/// Deletes all the entities that were created by means of the "create" operations on the <see cref="DataReader" />. That is, it
			/// deletes all contained <see cref="ReadCondition" /> and <see cref="QueryCondition" /> objects.
			/// </summary>
			/// <remarks>
			/// <para>The operation will return <see cref="ReturnCode::PreconditionNotMet" /> if the any of the contained entities is in a state where it cannot be deleted.</para>
			/// <para>Once DeleteContainedEntities returns successfully, the application may delete the <see cref="DataReader" /> knowing that it has no
			/// contained <see cref="ReadCondition" /> and <see cref="QueryCondition" /> objects.</para>
			/// </remarks>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode DeleteContainedEntities();

			/// <summary>
			/// Gets the <see cref="DataReader" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="DataReaderQos" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::DataReaderQos^ qos);

			/// <summary>
			/// Sets the <see cref="DataReader" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="DataReaderQos" /> to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::DataReaderQos^ qos);

			/// <summary>
			/// Allows access to the attached <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" />.
			/// </summary>
			/// <returns>The attached <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" />.</returns>
			OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ GetListener();

			/// <summary>
			/// Sets the <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> using the <see cref="StatusMask::DefaultStatusMask" />.
			/// </summary>
			/// <param name="listener">The <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> to be set.</param>			
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener);

			/// <summary>
			/// Sets the <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" />.
			/// </summary>
			/// <param name="listener">The <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> to be set.</param>
			/// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener, OpenDDSharp::DDS::StatusMask mask);						
			
			/// <summary>
			/// Allows access to the <see cref="SampleRejectedStatus" /> communication status.
			/// </summary>
			/// <param name="status">The <see cref="SampleRejectedStatus" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetSampleRejectedStatus(OpenDDSharp::DDS::SampleRejectedStatus% status);

			/// <summary>
			/// Allows access to the <see cref="LivelinessChangedStatus" /> communication status.
			/// </summary>
			/// <param name="status">The <see cref="LivelinessChangedStatus" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetLivelinessChangedStatus(OpenDDSharp::DDS::LivelinessChangedStatus% status);

			/// <summary>
			/// Allows access to the <see cref="RequestedDeadlineMissedStatus" /> communication status.
			/// </summary>
			/// <param name="status">The <see cref="RequestedDeadlineMissedStatus" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetRequestedDeadlineMissedStatus(OpenDDSharp::DDS::RequestedDeadlineMissedStatus% status);

			/// <summary>
			/// Allows access to the <see cref="RequestedIncompatibleQosStatus" /> communication status.
			/// </summary>
			/// <param name="status">The <see cref="RequestedIncompatibleQosStatus" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetRequestedIncompatibleQosStatus(OpenDDSharp::DDS::RequestedIncompatibleQosStatus% status);

			/// <summary>
			/// Allows access to the <see cref="SubscriptionMatchedStatus" /> communication status.
			/// </summary>
			/// <param name="status">The <see cref="SubscriptionMatchedStatus" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetSubscriptionMatchedStatus(OpenDDSharp::DDS::SubscriptionMatchedStatus% status);

			/// <summary>
			/// Allows access to the <see cref="SampleLostStatus" /> communication status.
			/// </summary>
			/// <param name="status">The <see cref="SampleLostStatus" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetSampleLostStatus(OpenDDSharp::DDS::SampleLostStatus% status);

			/// <summary>
			/// Waits until all "historical" data is received.
			/// This operation is intended only for <see cref="DataReader" /> entities that have a non-Volatile <see cref="DurabilityQosPolicyKind" />.
			/// </summary>
			/// <remarks>
			/// The operation WaitForHistoricalData blocks the calling thread until either all "historical" data is received, or else the
			/// duration specified by the maxWait parameter elapses, whichever happens first. A return value of <see cref="ReturnCode::Ok" /> indicates that all the
			///	"historical" data was received; a return value of <see cref="ReturnCode::Timeout" /> indicates that maxWait elapsed before all the data was received.
			/// </remarks>
			/// <param name="maxWait">The maximum <see cref="Duration" /> time to wait for the historical data.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode WaitForHistoricalData(OpenDDSharp::DDS::Duration maxWait);

			/// <summary>
			/// Gets the list of publications currently "associated" with the <see cref="DataReader" />; that is, publications that have a
			/// matching <see cref="Topic" /> and compatible QoS that the application has not indicated should be "ignored" by means of the
			///	<see cref="DomainParticipant" /> IgnorePublication operation.
			/// </summary>
			/// <remarks>
			/// The handles returned in the 'publicationHandles' collection are the ones that are used by the DDS implementation to locally identify
			/// the corresponding matched <see cref="DataWriter" /> entities. These handles match the ones that appear in the <see cref="SampleInfo::InstanceHandle" /> property of the
			///	<see cref="SampleInfo" /> when reading the "DCPSPublications" builtin topic.
			/// </remarks>
			/// <param name="publicationHandles">The collection of publication <see cref="InstanceHandle" />s to be filled up.</param> 
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetMatchedPublications(ICollection<OpenDDSharp::DDS::InstanceHandle>^ publicationHandles);

			/// <summary>
			/// This operation retrieves information on a publication that is currently "associated" with the <see cref="DataReader" />; that is, a publication
			/// with a matching <see cref="Topic" /> and compatible QoS that the application has not indicated should be "ignored" by means of the
			///	<see cref="DomainParticipant" /> IgnorePublication operation.
			/// </summary>
			/// <remarks>
			/// The publicationHandle must correspond to a publication currently associated with the <see cref="DataReader" /> otherwise the operation
			/// will fail and return <see cref="ReturnCode::BadParameter" />. The operation GetMatchedPublications can be used to find the publications that
			///	are currently matched with the <see cref="DataReader" />.
			/// </remarks>
			/// <param name="publicationHandle">The <see cref="InstanceHandle" /> of the publication data requested.</param> 
			/// <param name="publicationData">The <see cref="PublicationBuiltinTopicData" /> structure to be filled up.</param>			
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetMatchedPublicationData(OpenDDSharp::DDS::InstanceHandle publicationHandle, OpenDDSharp::DDS::PublicationBuiltinTopicData% publicationData);

		private:
			OpenDDSharp::DDS::Subscriber^ GetSubscriber();
			OpenDDSharp::DDS::ITopicDescription^ GetTopicDescription();
		};
	};
};