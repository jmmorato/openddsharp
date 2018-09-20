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
#include <dds/DCPS/Marked_Default_Qos.h>
#pragma managed

#include "Entity.h"
#include "DataReader.h"
#include "DataReaderQos.h"
#include "DataReaderListener.h"
#include "DataReaderListenerNative.h"
#include "StatusMask.h"
#include "StatusKind.h"
#include "Topic.h"
#include "SubscriberQos.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		ref class SubscriberListener;
		ref class DomainParticipant;		

		/// <summary>
		/// A Subscriber is the object responsible for the actual reception of the data resulting from its subscriptions.
		/// </summary>
		/// <remarks>
		/// <para>A Subscriber acts on the behalf of one or several <see cref="DataReader" /> objects that are related to it. When it receives data (from the
		/// other parts of the system), it builds the list of concerned <see cref="DataReader" /> objects, and then indicates to the application that data is
		/// available, through its listener or by enabling related conditions. The application can access the list of concerned <see cref="DataReader" />
		///	objects through the operation GetDataReaders and then access the data available though operations on the <see cref="DataReader" />.</para>
		/// <para>All operations except for the operations <see cref="SetQos" />, <see cref="GetQos" />, SetListener,
		/// <see cref="GetListener" />, <see cref="Entity::Enable" />, <see cref="Entity::StatusCondition" />, CreateDataReader,
		/// return the value <see cref="ReturnCode::NotEnabled" /> if the Subscriber has not been enabled yet.</para>
		/// </remarks>
		public ref class Subscriber : public OpenDDSharp::DDS::Entity {

		internal:
			::DDS::Subscriber_ptr impl_entity;
			OpenDDSharp::DDS::SubscriberListener^ m_listener;

		public:
			/// <summary>
			/// Gets the <see cref="DomainParticipant" /> to which the <see cref="Subscriber" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::DomainParticipant^ Participant {
				virtual OpenDDSharp::DDS::DomainParticipant^ get();
			};

		internal:
			Subscriber(::DDS::Subscriber_ptr subscriber);

		public:
			/// <summary>
			/// Creates a new <see cref="DataReader" /> with the default QoS policies and without listener attached.
			/// </summary>
			/// <remarks>
			/// <para>The returned <see cref="DataReader" /> will be attached and belong to the <see cref="Subscriber" />.</para>
			/// <para>The <see cref="ITopicDescription"/> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to
			/// create this <see cref="Subscriber" />. If the <see cref="ITopicDescription"/> was created from a different <see cref="DomainParticipant" />, the operation will fail and
			///	return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topicDescription">The <see cref="ITopicDescription" /> that the <see cref="DataReader" /> will be associated with.</param>
			/// <returns>The newly created <see cref="DataReader" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription);

			/// <summary>
			/// Creates a new <see cref="DataReader" /> with the desired QoS policies and without listener attached.
			/// </summary>
			/// <remarks>
			/// <para>The returned <see cref="DataReader" /> will be attached and belong to the <see cref="Subscriber" />.</para>
			/// <para>The <see cref="ITopicDescription"/> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to
			/// create this <see cref="Subscriber" />. If the <see cref="ITopicDescription"/> was created from a different <see cref="DomainParticipant" />, the operation will fail and
			///	return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topicDescription">The <see cref="ITopicDescription" /> that the <see cref="DataReader" /> will be associated with.</param>
			/// <param name="qos">The <see cref="DataReaderQos" /> policies to be used for creating the new <see cref="DataReader" />.</param>
			/// <returns>The newly created <see cref="DataReader" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription, ::OpenDDSharp::DDS::DataReaderQos^ qos);

			/// <summary>
			/// Creates a new <see cref="DataReader" /> with the default QoS policies and attaches to it the specified <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" />.
			/// The specified <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> will be attached with the default <see cref="StatusMask" />. 
			/// </summary>
			/// <remarks>
			/// <para>The returned <see cref="DataReader" /> will be attached and belong to the <see cref="Subscriber" />.</para>
			/// <para>The <see cref="ITopicDescription"/> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to
			/// create this <see cref="Subscriber" />. If the <see cref="ITopicDescription"/> was created from a different <see cref="DomainParticipant" />, the operation will fail and
			///	return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topicDescription">The <see cref="ITopicDescription" /> that the <see cref="DataReader" /> will be associated with.</param>
			/// <param name="listener">The <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> to be attached to the newly created <see cref="DataReader" />.</param>
			/// <returns>The newly created <see cref="DataReader" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener);

			/// <summary>
			/// Creates a new <see cref="DataReader" /> with the default QoS policies and attaches to it the specified <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" />.			
			/// </summary>
			/// <remarks>
			/// <para>The returned <see cref="DataReader" /> will be attached and belong to the <see cref="Subscriber" />.</para>
			/// <para>The <see cref="ITopicDescription"/> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to
			/// create this <see cref="Subscriber" />. If the <see cref="ITopicDescription"/> was created from a different <see cref="DomainParticipant" />, the operation will fail and
			///	return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topicDescription">The <see cref="ITopicDescription" /> that the <see cref="DataReader" /> will be associated with.</param>
			/// <param name="listener">The <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> to be attached to the newly created <see cref="DataReader" />.</param>
			/// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns>The newly created <see cref="DataReader" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener, OpenDDSharp::DDS::StatusMask statusMask);

			/// <summary>
			/// Creates a new <see cref="DataReader" /> with the desired QoS policies and attaches to it the specified <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" />.
			/// The specified <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> will be attached with the default <see cref="StatusMask" />. 
			/// </summary>
			/// <param name="topicDescription">The <see cref="ITopicDescription" /> that the <see cref="DataReader" /> will be associated with.</param>
			/// <param name="qos">The <see cref="DataReaderQos" /> policies to be used for creating the new <see cref="DataReader" />.</param>
			/// <param name="listener">The <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> to be attached to the newly created <see cref="DataReader" />.</param>
			/// <returns>The newly created <see cref="DataReader" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription, ::OpenDDSharp::DDS::DataReaderQos^ qos, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener);

			/// <summary>
			/// Creates a new <see cref="DataReader" /> with the desired QoS policies and attaches to it the specified <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" />.			
			/// </summary>
			/// <remarks>
			/// <para>The returned <see cref="DataReader" /> will be attached and belong to the <see cref="Subscriber" />.</para>
			/// <para>The <see cref="ITopicDescription"/> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to
			/// create this <see cref="Subscriber" />. If the <see cref="ITopicDescription"/> was created from a different <see cref="DomainParticipant" />, the operation will fail and
			///	return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topicDescription">The <see cref="ITopicDescription" /> that the <see cref="DataReader" /> will be associated with.</param>
			/// <param name="qos">The <see cref="DataReaderQos" /> policies to be used for creating the new <see cref="DataReader" />.</param>
			/// <param name="listener">The <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> to be attached to the newly created <see cref="DataReader" />.</param>
			/// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns>The newly created <see cref="DataReader" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topicDescription, ::OpenDDSharp::DDS::DataReaderQos^ qos, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener, OpenDDSharp::DDS::StatusMask statusMask);

			/// <summary>
			/// Deletes a <see cref="OpenDDSharp::DDS::DataReader" /> that belongs to the <see cref="OpenDDSharp::DDS::Subscriber" />.
			/// </summary>
			/// <remarks>
			/// <para>If the <see cref="OpenDDSharp::DDS::DataReader" /> does not belong to the <see cref="OpenDDSharp::DDS::Subscriber" />, the operation returns the error <see cref="OpenDDSharp::DDS::ReturnCode::PreconditionNotMet" />.</para>
			/// <para>The deletion of a <see cref="OpenDDSharp::DDS::DataReader" /> is not allowed if there are any existing <see cref="OpenDDSharp::DDS::ReadCondition" /> or <see cref="OpenDDSharp::DDS::QueryCondition" /> objects that are
			/// attached to the <see cref="OpenDDSharp::DDS::DataReader" />. If the DeleteDataReader operation is called on a <see cref="OpenDDSharp::DDS::DataReader" /> with any of these existing objects
			///	attached to it, it will return <see cref="OpenDDSharp::DDS::ReturnCode::PreconditionNotMet" />.</para>
			/// <para>The DeleteDataReader operation must be called on the same <see cref="OpenDDSharp::DDS::Subscriber" /> object used to create the <see cref="OpenDDSharp::DDS::DataReader" />. If
			/// DeleteDataReader is called on a different <see cref="OpenDDSharp::DDS::Subscriber" />, the operation will have no effect and it will return
			///	<see cref="ReturnCode::PreconditionNotMet" />.</para>
			/// </remarks>
			/// <param name="datareader">The <see cref="OpenDDSharp::DDS::DataReader" /> to be deleted.</param>
			/// <returns>The <see cref="OpenDDSharp::DDS::ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode DeleteDataReader(OpenDDSharp::DDS::DataReader^ datareader);

			/// <summary>
			/// This operation deletes all the entities that were created by means of the "create" operations on the <see cref="Subscriber" />. That is, it
			/// deletes all contained <see cref="DataReader" /> objects. This pattern is applied recursively. In this manner the operation
			///	DeleteContainedEntities on the <see cref="Subscriber" /> will end up deleting all the entities recursively contained in the <see cref="Subscriber" />, that
			///	is also the <see cref="QueryCondition" /> and <see cref="ReadCondition" /> objects belonging to the contained <see cref="DataReader" />s.
			/// </summary>
			/// <remarks>
			/// <para>The operation will return <see cref="ReturnCode::PreconditionNotMet" /> if any of the contained entities is in a state where it cannot be deleted.</para>
			/// <para>Once DeleteContainedEntities returns successfully, the application may delete the <see cref="Subscriber" /> knowing that it has no
			/// contained <see cref="DataReader" /> objects.</para>
			/// </remarks>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode DeleteContainedEntities();			

			/// <summary>
			/// Gets a previously-created <see cref="DataReader" /> belonging to the <see cref="Subscriber" /> that is attached to a <see cref="Topic" /> with a
			/// matching topicName. If no such <see cref="DataReader" /> exists, the operation will return <see langword="null"/>.
			/// </summary>
			/// <remarks>
			/// <para>If multiple <see cref="DataReader" />s attached to the <see cref="Subscriber" /> satisfy this condition, then the operation will return one of them.
			/// It is not specified which one.</para>
			/// <para>The use of this operation on the built-in <see cref="Subscriber" /> allows access to the built-in <see cref="DataReader" /> entities for the built-in topics</para>
			/// </remarks>
			/// <param name="topicName">The <see cref="Topic" />'s name related with the <see cref="DataReader" /> to look up.</param>
			/// <returns>The <see cref="DataReader" />, if it exists, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataReader^ LookupDataReader(System::String^ topicName);

			/// <summary>
			/// Allows the application to access the <see cref="DataReader" /> objects that contain samples with any sample states,
			/// any view states, and any instance states.
			/// </summary>
			/// <remarks>
			/// <para>If the <see cref="PresentationQosPolicy" /> of the <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs has the 
			/// <see cref="PresentationQosPolicy::AccessScope" /> set to <see cref="PresentationQosPolicyAccessScopeKind::GroupPresentationQos" />,
			/// this operation should only be invoked inside a <see cref="BeginAccess" />/<see cref="EndAccess" /> block. Otherwise it will return the error
			/// <see cref="ReturnCode::PreconditionNotMet" />.</para>
			/// <para>Depending on the setting of the <see cref="PresentationQosPolicy" />, the returned collection of <see cref="DataReader" /> objects may
			/// be a 'set' containing each <see cref="DataReader" /> at most once in no specified order, or a 'list' containing each <see cref="DataReader" /> one or more
			///	times in a specific order.
			/// <list type="number"> 
			/// <item><description>If <see cref="PresentationQosPolicy::AccessScope" /> is <see cref="PresentationQosPolicyAccessScopeKind::InstancePresentationQos" /> or 
			/// <see cref="PresentationQosPolicyAccessScopeKind::TopicPresentationQos" /> the returned collection behaves as a 'set'.</description></item>
			/// <item><description>If <see cref="PresentationQosPolicy::AccessScope" /> is <see cref="PresentationQosPolicyAccessScopeKind::GroupPresentationQos" /> and 
			/// <see cref="PresentationQosPolicy::OrderedAccess" /> is set to <see langword="true"/>, then the returned collection behaves as a 'list'.</description></item>
			/// </list></para>
			/// <para>This difference is due to the fact that, in the second situation it is required to access samples belonging to different <see cref="DataReader" />
			/// objects in a particular order. In this case, the application should process each <see cref="DataReader" /> in the same order it appears in the
			///	'list' and Read or Take exactly one sample from each <see cref="DataReader" />.</para>
			/// </remarks>
			/// <param name="readers">The <see cref="DataReader" /> collection to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetDataReaders(IList<OpenDDSharp::DDS::DataReader^>^ readers);

			/// <summary>
			/// Allows the application to access the <see cref="DataReader" /> objects that contain samples with the specified sampleStates,
			/// viewStates, and instanceStates.
			/// </summary>
			/// <remarks>
			/// <para>If the <see cref="PresentationQosPolicy" /> of the <see cref="Subscriber" /> to which the <see cref="DataReader" /> belongs has the 
			/// <see cref="PresentationQosPolicy::AccessScope" /> set to <see cref="PresentationQosPolicyAccessScopeKind::GroupPresentationQos" />,
			/// this operation should only be invoked inside a <see cref="BeginAccess" />/<see cref="EndAccess" /> block. Otherwise it will return the error
			/// <see cref="ReturnCode::PreconditionNotMet" />.</para>
			/// <para>Depending on the setting of the <see cref="PresentationQosPolicy" />, the returned collection of <see cref="DataReader" /> objects may
			/// be a 'set' containing each <see cref="DataReader" /> at most once in no specified order, or a 'list' containing each <see cref="DataReader" /> one or more
			///	times in a specific order.
			/// <list type="number"> 
			/// <item><description>If <see cref="PresentationQosPolicy::AccessScope" /> is <see cref="PresentationQosPolicyAccessScopeKind::InstancePresentationQos" /> or 
			/// <see cref="PresentationQosPolicyAccessScopeKind::TopicPresentationQos" /> the returned collection behaves as a 'set'.</description></item>
			/// <item><description>If <see cref="PresentationQosPolicy::AccessScope" /> is <see cref="PresentationQosPolicyAccessScopeKind::GroupPresentationQos" /> and 
			/// <see cref="PresentationQosPolicy::OrderedAccess" /> is set to <see langword="true"/>, then the returned collection behaves as a 'list'.</description></item>
			/// </list></para>
			/// <para>This difference is due to the fact that, in the second situation it is required to access samples belonging to different <see cref="DataReader" />
			/// objects in a particular order. In this case, the application should process each <see cref="DataReader" /> in the same order it appears in the
			///	'list' and Read or Take exactly one sample from each <see cref="DataReader" />.</para>
			/// </remarks>
			/// <param name="readers">The <see cref="DataReader" /> collection to be filled up.</param>
			/// <param name="sampleStates">The returned <see cref="DataReader" /> in the collection must contain samples that have one of the sample states.</param>
			/// <param name="viewStates">The returned <see cref="DataReader" /> in the collection must contain samples that have one of the view states.</param>
			/// <param name="instanceStates">The returned <see cref="DataReader" /> in the collection must contain samples that have one of the instance states.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetDataReaders(IList<OpenDDSharp::DDS::DataReader^>^ readers, OpenDDSharp::DDS::SampleStateMask sampleStates, OpenDDSharp::DDS::ViewStateMask viewStates, OpenDDSharp::DDS::InstanceStateMask instanceStates);

			/// <summary>
			/// Invokes the operation OnDataAvailable on the <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> objects attached to contained <see cref="DataReader" />
			/// entities with a <see cref="StatusKind::DataAvailableStatus" /> that is considered changed.
			/// </summary>
			/// <remarks>
			/// This operation is typically invoked from the OnDataOnReaders operation in the <see cref="SubscriberListener" />. That way the
			/// <see cref="SubscriberListener" /> can delegate to the <see cref="OpenDDSharp::OpenDDS::DCPS::DataReaderListener" /> objects the handling of the data. 
			/// </remarks>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode NotifyDataReaders();			

			/// <summary>
			/// Gets the <see cref="Subscriber" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="SubscriberQos" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::SubscriberQos^ qos);

			/// <summary>
			/// Sets the <see cref="Subscriber" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="SubscriberQos" /> to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::SubscriberQos^ qos);

			/// <summary>
			/// Allows access to the attached <see cref="SubscriberListener" />.
			/// </summary>
			/// <returns>The attached <see cref="SubscriberListener" />.</returns>
			OpenDDSharp::DDS::SubscriberListener^ GetListener();

			/// <summary>
			/// Sets the <see cref="SubscriberListener" /> using the <see cref="StatusMask::DefaultStatusMask" />.
			/// </summary>
			/// <param name="listener">The <see cref="SubscriberListener" /> to be set.</param>			
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::SubscriberListener^ listener);

			/// <summary>
			/// Sets the <see cref="SubscriberListener" />.
			/// </summary>
			/// <param name="listener">The <see cref="SubscriberListener" /> to be set.</param>
			/// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::SubscriberListener^ listener, OpenDDSharp::DDS::StatusMask mask);
			
			/// <summary>
			/// Indicates that the application is about to access the data samples in any of the <see cref="DataReader" /> objects attached to
			/// the <see cref="Subscriber" />.
			/// </summary>
			/// <remarks>
			/// <para>The application is required to use this operation only if <see cref="PresentationQosPolicy" /> of the <see cref="Subscriber" /> to which the
			/// <see cref="DataReader" /> belongs has the <see cref="PresentationQosPolicy::AccessScope" /> set to 
			/// <see cref="PresentationQosPolicyAccessScopeKind::GroupPresentationQos" />.</para>
			/// <para>In the aforementioned case, the operation BeginAccess must be called prior to calling any of the sample-accessing operations,
			/// namely: GetDataReaders on the <see cref="Subscriber" /> and Read, Take on any <see cref="DataReader" />.
			/// Otherwise the sample-accessing operations will return the error <see cref="ReturnCode::PreconditionNotMet" />. Once the application has
			/// finished accessing the data samples it must call <see cref="EndAccess" />.</para>
			/// <para>The calls to BeginAccess/<see cref="EndAccess" /> may be nested. In that case, the application must call <see cref="EndAccess" /> as many times as it
			/// called BeginAccess.</para>
			/// </remarks>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode BeginAccess();

			/// <summary>
			/// Indicates that the application has finished accessing the data samples in <see cref="DataReader" /> objects managed by the <see cref="Subscriber" />.
			/// </summary>
			/// <remarks>
			/// <para>This operation must be used to 'close' a corresponding <see cref="BeginAccess" />.</para>
			/// <para>After calling EndAccess the application should no longer access any of the data or <see cref="SampleInfo" /> elements returned from the
			/// sample-accessing operations. This call must close a previous call to <see cref="BeginAccess" /> otherwise the operation will return the error
			///	<see cref="ReturnCode::PreconditionNotMet" />.</para>
			/// </remarks>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode EndAccess();			

			/// <summary>
			/// Gets the default value of the <see cref="DataReader" /> QoS, that is, the QoS policies which will be used for newly
			/// created <see cref="DataReader" /> entities in the case where the QoS policies are defaulted in the CreateDataReader operation.
			/// </summary>
			/// <remarks>
			/// The values retrieved GetDefaultDataReaderQos will match the set of values specified on the last successful call to
			/// <see cref="SetDefaultDataReaderQos" />, or else, if the call was never made, the default DDS standard values.
			/// </remarks>
			/// <param name="qos">The <see cref="DataReaderQos" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetDefaultDataReaderQos(OpenDDSharp::DDS::DataReaderQos^ qos);

			/// <summary>
			/// Sets a default value of the <see cref="DataReader" /> QoS policies which will be used for newly created <see cref="DataReader" /> entities
			/// in the case where the QoS policies are defaulted in the CreateDataReader operation.
			/// </summary>
			/// <remarks>
			/// This operation will check that the resulting policies are self consistent; if they are not, the operation will have no effect and
			/// return <see cref="ReturnCode::InconsistentPolicy" />.
			/// </remarks>
			/// <param name="qos">The default <see cref="DataReaderQos" /> to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetDefaultDataReaderQos(OpenDDSharp::DDS::DataReaderQos^ qos);

		private:
			OpenDDSharp::DDS::DomainParticipant^ GetParticipant();
		};

	};
};