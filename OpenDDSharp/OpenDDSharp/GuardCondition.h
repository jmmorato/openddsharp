#pragma once

#pragma unmanaged 
#include <dds/DCPS/GuardCondition.h>
#pragma managed

#include "Condition.h"
#include "ReturnCode.h"
#include "WaitSet.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// A GuardCondition object is a specific <see cref="Condition" /> whose <see cref="Condition::TriggerValue" /> is completely under the control of the application.
		/// </summary>
		/// <remarks>
		/// <para>GuardCondition has no factory. When first created the <see cref="Condition::TriggerValue" /> is set to <see langword="false"/>.</para>
		/// <para>The purpose of the GuardCondition is to provide the means for the application to manually wakeup a <see cref="WaitSet" />. This is
		/// accomplished by attaching the GuardCondition to the <see cref="WaitSet" /> and then setting the <see cref="Condition::TriggerValue" /> by means of the
		/// <see cref="TriggerValue" /> set operation.</para>
		/// </remarks>
		public ref class GuardCondition : public OpenDDSharp::DDS::Condition {

		internal:
			::DDS::GuardCondition_ptr impl_entity;

		public:
			/// <summary>
			/// Gets/Sets the trigger value of the <see cref="GuardCondition" />
			/// </summary>
			property System::Boolean TriggerValue  {
				System::Boolean get();
				void set(System::Boolean value);
			}

		internal:
			GuardCondition(::DDS::GuardCondition_ptr native);

		public:
			/// <summary>
			/// Creates a new instance of the <see cref="GuardCondition" />
			/// </summary>
			GuardCondition();

		public:
			/// <summary>
			/// Attach the <see cref="GuardCondition" /> to a <see cref="WaitSet" />
			/// </summary>
			/// <param name="ws">The <see cref="WaitSet" /> to be attached.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode AttachToWaitSet(OpenDDSharp::DDS::WaitSet^ ws);

			/// <summary>
			/// Detach the <see cref="GuardCondition" /> from a <see cref="WaitSet" />
			/// </summary>
			/// <param name="ws">The <see cref="WaitSet" /> to be detached from.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode DetachFromWaitSet(OpenDDSharp::DDS::WaitSet^ ws);

			/// <summary>
			/// Send a signal to all the attached <see cref="WaitSet" />s. 
			/// </summary>
			void SignalAll();

		};
	};
};