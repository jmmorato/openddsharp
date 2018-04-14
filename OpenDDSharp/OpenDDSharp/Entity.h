#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "ReturnCode.h"
#include "StatusMask.h"
#include "InstanceHandle.h"
#include "StatusCondition.h"

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {
		
		ref class WaitSet;

		/// <summary>
		/// This class is the abstract base class for all the DCPS objects that support QoS policies, a listener and a status condition. That is,
		/// <see cref="DomainPartipant" />, <see cref="Topic" />, <see cref="Publisher" />, <see cref="Subscriber" />, <see cref="DataWriter" /> and <see cref="DataReader" />. 
		/// <summary>
		public ref class Entity abstract {

		internal:
			::DDS::Entity_ptr impl_entity;
			ICollection<Entity^>^ contained_entities;

		public:
			/// <summary>
			/// Allows access to the <see cref="StatusCondition" /> associated with the <see cref="Entity" />. The returned
			/// condition can then be added to a <see cref="WaitSet" /> so that the application can wait for specific status changes
			///	that affect the <see cref="Entity" />.
			/// </summary>
			property OpenDDSharp::DDS::StatusCondition^ StatusCondition {
				OpenDDSharp::DDS::StatusCondition^ get();
			}

			/// <summary>
			/// Gets the list of communication statuses in the <see cref="Entity" /> that are 'triggered'. That is, the list of statuses whose
			/// value has changed since the last time the application read the status.
			/// </summary>
			/// <remarks>
			/// <p>When the <see cref="Entity" /> is first created or if the entity is not enabled, all communication statuses are in the “untriggered”.</p>
			/// <p>The statuses returned by the StatusChanges property refers to the status that are triggered on the <see cref="Entity" /> itself
			/// and does not include statuses that apply to contained entities.</p>
			/// </remarks>
			property OpenDDSharp::DDS::StatusMask StatusChanges {
				OpenDDSharp::DDS::StatusMask get();
			}

			/// <summary>
			/// Gets the <see cref="InstanceHandle" /> that represents the <see cref="Entity" />.
			/// </summary>
			property OpenDDSharp::DDS::InstanceHandle InstanceHandle {
				OpenDDSharp::DDS::InstanceHandle get();
			}

		public:
			Entity(::DDS::Entity_ptr entity);

		public:
			/// <summary>
			/// Enables the <see cref="Entity" />.
			/// The enable operation is idempotent. Calling enable on an already enabled <see cref="Entity" /> returns <see cref="ReturnCode::Ok" /> and has no effect.
			/// </summary>
			/// <remarks>
			/// <p>Entity objects can be created either enabled or disabled. This is controlled by the value of
			/// the <see cref="EntityFactoryQosPolicy" /> on the corresponding QoS for the <see cref="Entity" />. 
			/// The default setting of <see cref="EntityFactoryQosPolicy" /> is such that, by default, it is not necessary to explicitly call enable on newly
			/// created entities</p>
			/// <p>If an <see cref="Entity" /> has not yet been enabled, the following kinds of operations may be invoked on it:
			/// <list type="bullet">
			///		<item><description>Operations to set or get an Entity’s QoS policies (including default QoS policies) and listener</description></item>
			///		<item><description>Access to the <see cref="Entity::StatusCondition" /> property</description></item>
			///		<item><description>'factory' operations</description></item>
			///		<item><description>Access to the  <see cref="Entity::StatusChanges" /> property and other status operations(although the status of a disabled entity never changes)</description></item>
			///		<item><description>'lookup' operations</description></item>
			/// </list>
			/// Other operations may explicitly state that they may be called on disabled entities; those that do not will return the error
			/// <see cref="ReturnCode::NotEnabled" />.
			/// <p>It is legal to delete an <see cref="Entity" /> that has not been enabled by calling the proper operation on its factory.<p/>
			/// <p>Entities created from a factory that is disabled, are created disabled regardless of the setting of the <see cref="EntityFactoryQosPolicy" />.</p>
			/// <p>Calling enable on an <see cref="Entity" /> whose factory is not enabled will fail and return <see cref="ReturnCode::PreconditionNotMet" />.</p>
			/// <p>If the <see cref="EntityFactoryQosPolicy" /> has <see cref="EntityFactoryQosPolicy::AutoenableCreatedEntities" /> set to <see langword="true"/>, 
			/// the enable operation on the factory will automatically enable all entities created from the factory.</p>
			/// <p>The listeners associated with an <see cref="Entity" /> are not called until the entity is enabled.</p>
			/// </remarks>
			OpenDDSharp::DDS::ReturnCode Enable();
		
		private:
			OpenDDSharp::DDS::StatusCondition^ GetStatusCondition();

			OpenDDSharp::DDS::StatusMask GetStatusChanges();

			OpenDDSharp::DDS::InstanceHandle GetInstanceHandle();

		internal:
			ICollection<Entity^>^ GetContainedEntities();
		};

	};
};