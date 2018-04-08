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
		/// <summary>
		public ref class TopicListener abstract {

		typedef void(__stdcall *onInconsistentTopicDeclaration)(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status);

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
			/// Creates a new instance of the <see cref="TopicListener" />
			/// </summary>
			TopicListener();

		public:
			/// <summary>
			/// <para>Handles the <see cref="StatusKind::InconsistentTopicStatus" /> communication status.</para>
			/// <para>The <see cref="StatusKind::InconsistentTopicStatus" /> indicates that a <see cref="Topic" /> was attempted to be registered that
			/// already exists with different characteristics. Typically, the existing <see cref="Topic" /> may have a different type associated with it.</para>
			/// </summary>
			virtual void OnInconsistentTopic(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::InconsistentTopicStatus status) = 0;
		};
	};
};
