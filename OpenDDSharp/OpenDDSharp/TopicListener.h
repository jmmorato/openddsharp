#pragma once

#include "Topic.h"
#include "TopicListenerNative.h"
#include "InconsistentTopicStatus.h"
#include "EntityManager.h"

namespace OpenDDSharp {
	namespace DDS {

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
			TopicListener();

		public:			
			virtual void OnInconsistentTopic(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::InconsistentTopicStatus status) = 0;
		};
	};
};
