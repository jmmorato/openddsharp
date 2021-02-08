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

#include "Topic.h"
#include "TopicListenerNative.h"
#include "InconsistentTopicStatus.h"
#include "EntityManager.h"
#include "StatusKind.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="Topic" />
		/// such that the application can be notified of relevant status changes.		
		/// </summary>
		public ref class TopicListener abstract {

		typedef void(__stdcall *onInconsistentTopicDeclaration)(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status);

        private:
            System::Runtime::InteropServices::GCHandle gchInconsistentTopic;

		internal:
			::OpenDDSharp::DDS::TopicListenerNative* impl_entity;

		protected:
			onInconsistentTopicDeclaration onInconsistentTopicFunctionCpp;

		private:
			delegate void onInconsistentTopicDelegate(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status);
			void onInconsistentTopic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status) {
				OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(topic);
				OpenDDSharp::DDS::Topic^ managedTopic = nullptr;
				if (entity != nullptr) {
					managedTopic = static_cast<OpenDDSharp::DDS::Topic^>(entity);
				}				

				OnInconsistentTopic(managedTopic, OpenDDSharp::DDS::InconsistentTopicStatus(status));
			};

		public:
			/// <summary>
			/// Creates a new instance of <see cref="TopicListener" />
			/// </summary>
			TopicListener();

        protected:
            !TopicListener();

		public:
			/// <summary>
			/// <para>Handles the <see cref="StatusKind::InconsistentTopicStatus" /> communication status.</para>
			/// <para>The <see cref="StatusKind::InconsistentTopicStatus" /> indicates that a <see cref="Topic" /> was attempted to be registered that
			/// already exists with different characteristics. Typically, the existing <see cref="Topic" /> may have a different type associated with it.</para>
			/// </summary>
			/// <param name="topic">The <see cref="OpenDDSharp::DDS::Topic" /> that triggered the event.</param>
			/// <param name="status">The current <see cref="OpenDDSharp::DDS::InconsistentTopicStatus" />.</param>
			virtual void OnInconsistentTopic(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::InconsistentTopicStatus status) = 0;
		};
	};
};
