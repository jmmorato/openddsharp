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
#include <dds/DdsDcpsTopicC.h>
#pragma managed

#include "Entity.h"
#include "ReturnCode.h"
#include "TopicQos.h"
#include "StatusMask.h"
#include "InconsistentTopicStatus.h"
#include "ITopicDescription.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		ref class TopicListener;
		ref class DomainParticipant;
		ref class DataWriter;

		/// <summary>
		/// Topic is the most basic description of the data to be published and subscribed.
		/// A Topic is identified by its name, which must be unique in the whole Domain. In addition (by virtue of implemeting
		/// <see cref="ITopicDescription" />) it fully specifies the type of the data that can be communicated when publishing or subscribing to the Topic.
		/// Topic is the only <see cref="ITopicDescription" /> that can be used for publications and therefore associated to a <see cref="OpenDDSharp::DDS::DataWriter" />.
		/// </summary>
		public ref class Topic : public OpenDDSharp::DDS::Entity, public ITopicDescription {

		internal:
			::DDS::Topic_ptr impl_entity;
			OpenDDSharp::DDS::TopicListener^ _listener;

		internal:
			Topic(::DDS::Topic_ptr topic);

        public:
            !Topic();

		public:
			/// <summary>
			/// Gets type name used to create the <see cref="ITopicDescription" />.
			/// </summary>
			property System::String^ TypeName {
				virtual System::String^ get();
			};

			/// <summary>
			/// Gets the name used to create the <see cref="ITopicDescription" />.
			/// </summary>
			property System::String^ Name {
				virtual System::String^ get();
			};

			/// <summary>
			/// Gets the <see cref="DomainParticipant" /> to which the <see cref="ITopicDescription" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::DomainParticipant^ Participant {
				virtual OpenDDSharp::DDS::DomainParticipant^ get();
			};

		public:
			/// <exclude />
			virtual ::DDS::TopicDescription_ptr ToNative();

			/// <summary>
			/// Sets the <see cref="Topic" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="TopicQos" /> to be set.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::TopicQos^ qos);

			/// <summary>
			/// Gets the <see cref="Topic" /> QoS policies.
			/// </summary>
			/// <param name="qos">The <see cref="TopicQos" /> to be filled up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::TopicQos^ qos);

			/// <summary>
			/// Allows access to the attached <see cref="TopicListener" />.
			/// </summary>
			/// <returns>The attached <see cref="TopicListener" />.</returns>
			OpenDDSharp::DDS::TopicListener^ GetListener();

			/// <summary>
			/// Sets the <see cref="TopicListener" /> using the <see cref="StatusMask::DefaultStatusMask" />.
			/// </summary>			
			/// <param name="listener">The <see cref="TopicListener" /> to be set.</param>			
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::TopicListener^ listener);
			
			/// <summary>
			/// Sets the <see cref="TopicListener" />.
			/// </summary>
			/// <param name="listener">The <see cref="TopicListener" /> to be set.</param>
			/// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::TopicListener^ listener, StatusMask mask);


			/// <summary>
			/// This method allows the application to retrieve the <see cref="InconsistentTopicStatus" /> of the <see cref="Topic" />.
			/// </summary>
			/// <param name="status">The <see cref="InconsistentTopicStatus" /> structure to be fill up.</param>
			/// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
			OpenDDSharp::DDS::ReturnCode GetInconsistentTopicStatus(OpenDDSharp::DDS::InconsistentTopicStatus% status);

		private:
			System::String^ GetTypeName();
			System::String^ GetName();
			OpenDDSharp::DDS::DomainParticipant^ GetParticipant();
			
		};

	};
};