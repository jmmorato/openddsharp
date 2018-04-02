#pragma once

#include "Subscriber.h"
#include "SubscriberListenerNative.h"

namespace OpenDDSharp {
	namespace DDS {		
		public ref class SubscriberListener abstract : public OpenDDSharp::OpenDDS::DCPS::DataReaderListener {

		typedef void(__stdcall *onDataOnReadersDeclaration)(::DDS::Subscriber_ptr subscriber);

		internal:
			::OpenDDSharp::DDS::SubscriberListenerNative* impl_entity;

		protected:
			onDataOnReadersDeclaration onDataOnReadersFunctionCpp;

		private:
			delegate void onDataOnReadersDelegate(::DDS::Subscriber_ptr subscriber);
			void onDataOnReaders(::DDS::Subscriber_ptr subscriber) {
				OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(subscriber);
				OpenDDSharp::DDS::Subscriber^ managedSubscriber = nullptr;
				if (entity != nullptr) {
					managedSubscriber = static_cast<OpenDDSharp::DDS::Subscriber^>(entity);
				}
				
				OnDataOnReaders(managedSubscriber);
			};

		public:
			SubscriberListener();

		public:
			virtual void OnDataOnReaders(OpenDDSharp::DDS::Subscriber^ subscriber) = 0;

		};
	};
};
