#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsPublicationC.h>
#include <dds/DCPS/Marked_Default_Qos.h>
#pragma managed

#include "Entity.h"
#include "Topic.h"
#include "DataWriter.h"
#include "StatusMask.h"
#include "DataWriterQos.h"
#include "DataWriterListener.h"
#include "PublisherQos.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		ref class PublisherListener;
		ref class DomainParticipant;
		ref class DataWriterListener;

		/// <summary>
		/// A Publisher is the object responsible for the actual dissemination of publications.
		/// The Publisher acts on the behalf of one or several <see cref="DataWriter" /> objects that belong to it. When it is informed of a change to the
		/// data associated with one of its <see cref="DataWriter" /> objects, it decides when it is appropriate to actually send the data-update message.
		///	In making this decision, it considers any extra information that goes with the data(timestamp, writer, etc.) as well as the QoS
		///	of the Publisher and the <see cref="DataWriter" />.
		/// </summary>
		/// <remarks>
		/// All operations except for the operations <see cref="Publisher::SetQos" />, <see cref="Publisher::GetQos" />, <see cref="Publisher::SetListener" />,
		/// <see cref="Publisher::GetListener" />, <see cref="Entity::Enable" />, <see cref="Entity::GetStatusCondition" />, CreateDataWriter,
		/// and <see cref="Entity::DeleteDataWriter" /> return the value <see cref="ReturnCode::NotEnabled" />.
		/// <remarks>
		public ref class Publisher : public OpenDDSharp::DDS::Entity {

		internal:
			::DDS::Publisher_ptr impl_entity;
			OpenDDSharp::DDS::PublisherListener^ m_listener;

		public:
			/// <summary>
			/// Gets the <see cref="DomainParticipant" /> to which the <see cref="Publisher" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::DomainParticipant^ Participant {
				virtual OpenDDSharp::DDS::DomainParticipant^ get();
			};

		internal:
			Publisher(::DDS::Publisher_ptr publisher);			

		public:
			/// <summary>
			/// Creates a new <see cref="DataWriter" /> with the default QoS policies and without listener attached.
			/// </summary>
			/// <remarks>
			/// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
			/// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
			/// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />, 
			/// the operation will fail and return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
			/// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic);

			/// <summary>
			/// Creates a new <see cref="DataWriter" /> with the desired QoS policies and without listener attached.
			/// </summary>
			/// <remarks>
			/// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
			/// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
			/// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />, 
			/// the operation will fail and return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
			/// <param name="qos">The <see cref="DataWriterQos" /> policies to be used for creating the new <see cref="DataWriter" />.</param>
			/// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::DataWriterQos^ qos);

			/// <summary>
			/// Creates a new <see cref="DataWriter" /> with the default QoS policies and attaches to it the specified <see cref="DataWriterListener" />.
			/// The specified <see cref="DataWriterListener" /> will be attached with the default <see cref="StatusMask" />. 
			/// </summary>
			/// <remarks>
			/// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
			/// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
			/// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />, 
			/// the operation will fail and return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
			/// <param name="listener">The <see cref="DataWriterListener" /> to be attached to the newly created <see cref="DataWriter" />.</param>
			/// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener);

			/// <summary>
			/// Creates a new <see cref="DataWriter" /> with the default QoS policies and attaches to it the specified <see cref="DataWriterListener" />.			
			/// </summary>
			/// <remarks>
			/// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
			/// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
			/// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />, 
			/// the operation will fail and return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
			/// <param name="listener">The <see cref="DataWriterListener" /> to be attached to the newly created <see cref="DataWriter" />.</param>
			/// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener, StatusMask statusMask);

			/// <summary>
			/// Creates a new <see cref="DataWriter" /> with the desired QoS policies and attaches to it the specified <see cref="DataWriterListener" />.
			/// The specified <see cref="DataWriterListener" /> will be attached with the default <see cref="StatusMask" />. 
			/// </summary>
			/// <remarks>
			/// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
			/// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
			/// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />, 
			/// the operation will fail and return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
			/// <param name="qos">The <see cref="DataWriterQos" /> policies to be used for creating the new <see cref="DataWriter" />.</param>
			/// <param name="listener">The <see cref="DataWriterListener" /> to be attached to the newly created <see cref="DataWriter" />.</param>			
			/// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::DataWriterQos^ qos, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener);

			/// <summary>
			/// Creates a new <see cref="DataWriter" /> with the desired QoS policies and attaches to it the specified <see cref="DataWriterListener" />.			
			/// </summary>
			/// <remarks>
			/// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
			/// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
			/// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />, 
			/// the operation will fail and return a <see langword="null"/> result.</para>
			/// </remarks>
			/// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
			/// <param name="qos">The <see cref="DataWriterQos" /> policies to be used for creating the new <see cref="DataWriter" />.</param>
			/// <param name="listener">The <see cref="DataWriterListener" /> to be attached to the newly created <see cref="DataWriter" />.</param>	
			/// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::DataWriterQos^ qos, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener, StatusMask statusMask);

			/// <summary>
			/// Deletes a <see cref="DataWriter" /> that belongs to the <see cref="Publisher" />.
			/// </summary>
			/// <remarks>
			/// <para>The DeleteDataWriter operation must be called on the same <see cref="Publisher" /> object used to create the <see cref="DataWriter" />. If
			/// DeleteDataWriter operation is called on a different <see cref="Publisher" />, the operation will have no effect and it will return
			///	<see cref="ReturnCode::PreconditionNotMet".</para>
			/// <para>The deletion of the <see cref="DataWriter" /> will automatically unregister all instances. Depending on the settings of the
			/// <see cref="WriterDataLifecycleQosPolicy" />, the deletion of the <see cref="DataWriter" /> may also dispose all instances.</para>
			/// </remarks>
			/// <param name="datawriter">The <see cref="DataWriter" /> to be deleted.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode DeleteDataWriter(OpenDDSharp::DDS::DataWriter^ datawriter);

			/// <summary>
			/// Gets a previously created <see cref="DataWriter" /> belonging to the <see cref="Publisher" /> that is attached to a <see cref="Topic" /> with a matching
			/// topic name. If no such <see cref="DataWriter" /> exists, the operation will return <see langword="null"/>.
			/// </summary>
			/// <remarks>
			/// If multiple <see cref="DataWriter" /> attached to the <see cref="Publisher" /> satisfy the topic name condition, then the operation will return one of them. It is not
			/// specified which one.
			/// </remarks>
			/// <param name="topicName">The <see cref="Topic" />'s name related with the <see cref="DataWriter" /> to look up.</param>
			/// <returns>The <see cref="DataWriter" />, if it exists, otherwise <see langword="null"/>.</returns>
			OpenDDSharp::DDS::DataWriter^ LookupDataWriter(System::String^ topicName);

			/// <summary>
			/// This operation deletes all the entities that were created by means of the “create” operations on the <see cref="Publisher" />. That is, it deletes
			/// all contained <see cref="DataWriter" /> objects.
			/// </summary>
			/// <remarks>
			/// <para>The operation will return <see cref="ReturnCode::PreconditionNotMet" /> if the any of the contained entities is in a state where it cannot be deleted.</para>
			/// <para>Once DeleteContainedEntities returns successfully, the application may delete the <see cref="Publisher" /> knowing that it has no
			/// contained <see cref="DataWriter" /> objects.</para>
			/// </remarks>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode DeleteContainedEntities();

			/// <summary>
			/// Gets the <see cref="Publisher" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="PublisherQos" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::PublisherQos^ qos);

			/// <summary>
			/// Sets the <see cref="Publisher" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="PublisherQos" /> to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::PublisherQos^ qos);

			/// <summary>
			/// Allows access to the attached <see cref="PublisherListener" />.
			/// </summary>
			/// <returns>The attached <see cref="PublisherListener" />.</returns>
			OpenDDSharp::DDS::PublisherListener^ GetListener();

			/// <summary>
			/// Sets the <see cref="PublisherListener" /> using the <see cref="StatusMask::DefaultStatusMask" />.
			/// </summary>
			/// <param name="listener">The <see cref="PublisherListener" /> to be set.</param>			
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::PublisherListener^ listener);

			/// <summary>
			/// Sets the <see cref="PublisherListener" />.
			/// </summary>
			/// <param name="listener">The <see cref="PublisherListener" /> to be set.</param>
			/// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::PublisherListener^ listener, OpenDDSharp::DDS::StatusMask mask);

			/// <summary>
			/// This operation indicates to DDS that the application is about to make multiple modifications using <see cref="DataWriter" /> objects
			/// belonging to the <see cref="Publisher" />. It is a hint to DDS so it can optimize its performance by e.g., holding the dissemination of the modifications and then
			///	batching them.
			/// </summary>
			/// <remarks>
			/// The use of this operation must be matched by a corresponding call to <see cref="Publisher::ResumePublications" /> indicating that the set of
			/// modifications has completed. If the <see cref="Publisher" /> is deleted before <see cref="Publisher::ResumePublications" /> is called, any suspended updates yet to
			///	be published will be discarded.
			/// </remarks>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SuspendPublications();

			/// <summary>
			/// This operation indicates to DDS that the application has completed the multiple changes initiated by the previous <see cref="Publisher::SuspendPublications" />.
			/// </summary>
			/// <remarks>
			/// The call to ResumePublications must match a previous call to <see cref="Publisher::SuspendPublications" />. 
			/// Otherwise the operation will return the error <see cref="ReturnCode::PreconditionNotMet" />.
			/// </remarks>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode ResumePublications();

			/// <summary>
			/// Requests that the application will begin a 'coherent set' of modifications using <see cref="DataWriter" /> objects attached to
			/// the <see cref="Publisher" />. The 'coherent set' will be completed by a matching call to <see cref="Publisher::EndCoherentChanges" />.
			/// </summary>
			/// <remarks>
			/// <para>A 'coherent set' is a set of modifications that must be propagated in such a way that they are interpreted at the receivers' side
			/// as a consistent set of modifications; that is, the receiver will only be able to access the data after all the modifications in the set
			///	are available at the receiver end.</para>
			/// <para>A connectivity change may occur in the middle of a set of coherent changes; for example, the set of partitions used by the
			/// <see cref="Publisher" /> or one of its <see cref="Subscriber" />s may change, a late-joining <see cref="DataReader" /> may appear on the network, or a communication
			///	failure may occur. In the event that such a change prevents an entity from receiving the entire set of coherent changes, that
			///	entity must behave as if it had received none of the set.</para>
			/// <para>These calls can be nested. In that case, the coherent set terminates only with the last call to <see cref="Publisher::EndCoherentChanges" />.</para>
			/// <para>The support for 'coherent changes' enables a publishing application to change the value of several data-instances that could
			/// belong to the same or different topics and have those changes be seen 'atomically' by the readers. This is useful in cases where
			///	the values are inter-related (for example, if there are two data-instances representing the 'altitude' and 'velocity vector' of the
			/// same aircraft and both are changed, it may be useful to communicate those values in a way the reader can see both together;
			/// otherwise, it may e.g., erroneously interpret that the aircraft is on a collision course).</para>
			/// </remarks>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode BeginCoherentChanges();

			/// <summary>
			/// Terminates the 'coherent set' initiated by the matching call to <see cref="Publisher::BeginCoherentChanges" />. If there is no matching
			/// call to <see cref="Publisher::BeginCoherentChanges" />, the operation will return the error <see cref="ReturnCode::PreconditionNotMet" />.
			/// </summary>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode EndCoherentChanges();

			/// <summary>
			/// Blocks the calling thread until either all data written by the reliable <see cref="DataWriter" /> entities is acknowledged by all
			/// matched reliable <see cref="DataReader" /> entities, or else the duration specified by the maxWait parameter elapses, whichever happens
			///	first. A return value of <see cref="ReturnCode::Ok" /> indicates that all the samples written have been acknowledged by all reliable matched data readers;
			/// a return value of <see cref="ReturnCode::Timeout" /> indicates that maxWait elapsed before all the data was acknowledged.
			/// </summary>
			/// <param name="maxWait">The maximum <see cref="Duration" /> time to wait for the acknowledgments.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode WaitForAcknowledgments(OpenDDSharp::DDS::Duration maxWait);

			/// <summary>
			/// Gets the default value of the <see cref="DataWriter" /> QoS, that is, the QoS policies which will be used for newly created
			/// <see cref="DataWriter" /> entities in the case where the QoS policies are defaulted in the CreateDataWriter operation.
			/// </summary>
			/// <remarks>
			/// The values retrieved by <see cref="GetDefaultDataWriterQos" /> will match the set of values specified on the last successful call to
			/// <see cref="SetDefaultDataWriterQos" />, or else, if the call was never made, the default DDS standard values.
			/// </remarks>
			/// <param name="qos">The <see cref="DataWriterQos" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetDefaultDataWriterQos(OpenDDSharp::DDS::DataWriterQos^ qos);

			/// <summary>
			/// Sets a default value of the <see cref="DataWriter" /> QoS policies which will be used for newly created <see cref="DataWriter" /> entities in
			/// the case where the QoS policies are defaulted in the CreateDataWriter operation.
			/// <summary>
			/// <remarks>
			/// This operation will check that the resulting policies are self consistent; if they are not, the operation will have no effect and
			/// return <see cref="ReturnCode::InconsistentPolicy" />.
			/// </remarks>
			/// <param name="qos">The default <see cref="DataWriterQos" /> to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetDefaultDataWriterQos(OpenDDSharp::DDS::DataWriterQos^ qos);
				
		private:
			OpenDDSharp::DDS::DomainParticipant^ GetParticipant();
			
		};

	};
};