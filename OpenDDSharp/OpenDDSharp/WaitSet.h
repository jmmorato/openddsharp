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
#include <dds/DCPS/WaitSet.h>
#pragma managed

#include "ReturnCode.h"
#include "Duration.h"
#include "Condition.h"

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {
		
		ref class DomainParticipant;

		/// <summary>
		/// A WaitSet object allows an application to wait until one or more of the attached <see cref="Condition" /> objects has a <see cref="Condition::TriggerValue" /> of
		/// <see langword="true"/> or else until the timeout expires.
		/// </summary>
		/// <remarks>
		/// WaitSet has no factory. This is because it is not necessarily associated with a single DomainParticipant and could be used to wait on
		/// <see cref="Condition" /> objects associated with different <see cref="DomainParticipant" /> objects.
		/// </remarks>
		public ref class WaitSet {

		internal:
			::DDS::WaitSet_ptr impl_entity; 

		private:
			ICollection<Condition^>^ conditions;

		public:
			/// <summary>
			/// Creates a new instance of <see cref="WaitSet" />
			/// </summary>
			WaitSet();

		internal:
			WaitSet(::DDS::WaitSet_ptr waitSet);
				
		public:
			/// <summary>
			/// <para>This operation allows an application thread to wait for the occurrence of certain conditions. If none of the conditions attached
			/// to the <see cref="WaitSet" /> have a <see cref="OpenDDSharp::DDS::Condition::TriggerValue" /> of <see langword="true"/>, the wait operation will block suspending the calling thread.</para>
			/// <para>The wait operation will wait infinite time for the conditions.</para>
			/// </summary>
			/// <remarks>
			/// It is not allowed for more than one application thread to be waiting on the same <see cref="WaitSet" />. If the wait operation is invoked on a
			/// <see cref="WaitSet" /> that already has a thread blocking on it, the operation will return immediately with the value <see cref="ReturnCode::PreconditionNotMet" />.
			/// </remarks>
			/// <param name="activeConditions">
			/// The collection of <see cref="OpenDDSharp::DDS::Condition" />s with the <see cref="OpenDDSharp::DDS::Condition::TriggerValue" /> equals <see langword="true"/> when the thread is unblocked.
			/// </param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode Wait(ICollection<Condition^>^ activeConditions);

			/// <summary>
			/// This operation allows an application thread to wait for the occurrence of certain conditions. If none of the conditions attached
			/// to the <see cref="WaitSet" /> have a <see cref="Condition::TriggerValue" /> of <see langword="true"/>, the wait operation will block suspending the calling thread.
			/// </summary>
			/// <remarks>
			/// <para>It is not allowed for more than one application thread to be waiting on the same <see cref="WaitSet" />. If the wait operation is invoked on a
			/// <see cref="WaitSet" /> that already has a thread blocking on it, the operation will return immediately with the value <see cref="ReturnCode::PreconditionNotMet" />.</para>
			/// <para>The wait operation takes a timeout argument that specifies the maximum duration for the wait. It this duration is exceeded and
			/// none of the attached <see cref="OpenDDSharp::DDS::Condition" /> objects is <see langword="true"/>, wait will return with the return code <see cref="ReturnCode::Timeout" />.</para>
			/// </remarks>
			/// <param name="activeConditions">
			/// The collection of <see cref="OpenDDSharp::DDS::Condition" />s with the <see cref="OpenDDSharp::DDS::Condition::TriggerValue" /> equals <see langword="true"/> when the thread is unblocked.
			/// </param>
			/// <param name="timeout">Maximum duration for the wait.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode Wait(ICollection<Condition^>^ activeConditions, OpenDDSharp::DDS::Duration timeout);

			/// <summary>
			/// Attaches a <see cref="OpenDDSharp::DDS::Condition" /> to the <see cref="WaitSet" />.
			/// </summary>
			/// <remarks>
			/// <para>It is possible to attach a <see cref="OpenDDSharp::DDS::Condition" /> on a <see cref="WaitSet" /> that is currently being waited upon (via the wait operation). In this case, if the
			/// <see cref="Condition" /> has a <see cref="OpenDDSharp::DDS::Condition::TriggerValue" /> of <see langword="true"/>, then attaching the condition will unblock the <see cref="WaitSet" />.</para>
			/// <para>Adding a <see cref="OpenDDSharp::DDS::Condition" /> that is already attached to the <see cref="WaitSet" /> has no effect.</para>
			/// </remarks>
			/// <param name="cond">The <see cref="Condition" /> to be attached.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode AttachCondition(Condition^ cond);

			/// <summary>
			/// Detaches a <see cref="OpenDDSharp::DDS::Condition" /> from the <see cref="WaitSet" />.
			/// </summary>
			/// <remarks>
			/// If the <see cref="Condition" /> was not attached to the <see cref="WaitSet" />, the operation will return <see cref="ReturnCode::PreconditionNotMet" />.
			/// </remarks>
			/// <param name="cond">The <see cref="OpenDDSharp::DDS::Condition" /> to be detached.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode DetachCondition(Condition^ cond);

			/// <summary>
			/// Retrieves the list of attached conditions.
			/// </summary>
			/// <param name="attachedConditions">The collection of <see cref="OpenDDSharp::DDS::Condition" />s to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetConditions(ICollection<OpenDDSharp::DDS::Condition^>^ attachedConditions);

			/// <summary>
			/// Convenience method for detaching multiple conditions, for example when shutting down.
			/// </summary>
			/// <param name="conditions">The collection of <see cref="OpenDDSharp::DDS::Condition" />s to be detached.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode DetachConditions(ICollection<OpenDDSharp::DDS::Condition^>^ conditions);

		};
	};
};
