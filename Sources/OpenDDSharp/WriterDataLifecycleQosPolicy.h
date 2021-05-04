/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

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
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "Duration.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataWriter;
		ref class Publisher;
		ref class DomainParticipant;

		/// <summary>
		/// This policy controls the behavior of the <see cref="DataWriter" /> with regards to the lifecycle of the data-instances it manages, that is, the
		/// data-instances that have been either explicitly registered with the <see cref="DataWriter" /> using the register operations or implicitly by directly writing the data.
		/// </summary>
		public ref class WriterDataLifecycleQosPolicy {

		private:
			System::Boolean autodispose_unregistered_instances;			

		public:
			/// <summary>
			/// Controls the behavior when the <see cref="DataWriter" /> unregisters an instance by means of the unregister operations.
			/// If the value of this property is equals <see langword="true" />, causes the DataWriter to dispose the instance each time it
			/// is unregistered. The behavior is identical to explicitly calling one of the dispose operations on the instance prior to calling the unregister operation.
			/// Otherwise, if the value of this property is equals <see langword="false" />, will not cause this automatic disposition upon
			/// unregistering. The application can still call one of the dispose operations prior to unregistering the instance and accomplish the same effect.
			/// </summary>
			/// <remarks>
			/// Note that the deletion of a <see cref="DataWriter" /> automatically unregisters all data-instances it manages. Therefore the
			/// setting of the AutodisposeUnregisteredInstances flag will determine whether instances are ultimately disposed when the
			/// <see cref="DataWriter" /> is deleted either directly by means of the <see cref="Publisher" /> DeleteDataWriter operation or indirectly as a consequence of
			/// calling DeleteContainedEntities on the <see cref="Publisher" /> or the <see cref="DomainParticipant" /> that contains the <see cref="DataWriter" />.
			/// </remarks>
			property System::Boolean AutodisposeUnregisteredInstances {
				System::Boolean get();
				void set(System::Boolean value);
			};

		internal:
			WriterDataLifecycleQosPolicy();

		internal:
			::DDS::WriterDataLifecycleQosPolicy ToNative();
			void FromNative(::DDS::WriterDataLifecycleQosPolicy qos);
		};
	};
};
