#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsInfrastructureC.h>
#pragma managed

#include "InstanceStateMask.h"

namespace OpenDDSharp {
	namespace DDS {

		value struct InstanceStateKind;
		ref class DataWriter;
		ref class DataReader;

		/// <summary>
		/// Indicates if the samples are from a live <see cref="DataWriter" /> or not.
		/// </summary>
		public value struct InstanceStateKind {

		public:
			/// <summary>
			/// Indicates that:
			/// <list type="bullet">
			///		<item><description>Samples have been received for the instance</description></item>
			///		<item><description>There are live <see cref="DataWriter" /> entities writing the instance</description></item>
			///		<item><description>The instance has not been explicitly disposed (or else more samples have been received after it was disposed)</description></item>
			///	</list>
			/// </summary>
			static const InstanceStateKind AliveInstanceState = ::DDS::ALIVE_INSTANCE_STATE;

			/// <summary>
			/// Indicates the instance was explicitly disposed by a <see cref="DataWriter" /> by means of the Delete operation.
			/// </summary>
			static const InstanceStateKind NotAliveDisposedInstanceState = ::DDS::NOT_ALIVE_DISPOSED_INSTANCE_STATE;

			/// <summary>
			/// Indicates the instance has been declared as not-alive by the <see cref="DataReader" /> because it detected that 
			/// there are no live <see cref="DataWriter" /> entities writing that instance.
			/// </summary>
			static const InstanceStateKind NotAliveNoWritersInstanceState = ::DDS::NOT_ALIVE_NO_WRITERS_INSTANCE_STATE;

		private:
			System::UInt32 m_value;

		internal:
			InstanceStateKind(System::UInt32 value);

		public:
			static operator System::UInt32(InstanceStateKind self) {
				return self.m_value;
			}

			static operator InstanceStateKind(System::UInt32 value) {
				InstanceStateKind r(value);
				return r;
			}

			static operator InstanceStateMask(InstanceStateKind value) {
				InstanceStateMask r(value);
				return r;
			}

			static InstanceStateMask operator  | (InstanceStateKind a, InstanceStateKind b) {
				return static_cast<InstanceStateMask>(static_cast<unsigned int>(a) | static_cast<unsigned int>(b));
			}
			
		};
	}
};
