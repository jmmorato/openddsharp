#pragma once

#pragma once

#include "DomainParticipantListenerNative.h"

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {
			public ref class DomainParticipantListener abstract {

			typedef void(__stdcall *onDataOnReadersDeclaration)(::DDS::Subscriber_ptr subscriber);
			typedef void(__stdcall *onDataAvailabeDeclaration)(::DDS::DataReader_ptr reader);
			typedef void(__stdcall *onRequestedDeadlineMissedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status);
			typedef void(__stdcall *onRequestedIncompatibleQosDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status);
			typedef void(__stdcall *onSampleRejectedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status);
			typedef void(__stdcall *onLivelinessChangedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status);
			typedef void(__stdcall *onSubscriptionMatchedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status);
			typedef void(__stdcall *onSampleLostDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status);
			typedef void(__stdcall *onSubscriptionDisconnectedDeclaration)(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionDisconnectedStatus& status);
			typedef void(__stdcall *onSubscriptionReconnectedDeclaration)(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionReconnectedStatus& status);
			typedef void(__stdcall *onSubscriptionLostDeclaration)(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionLostStatus& status);
			typedef void(__stdcall *onBudgetExceededDeclaration)(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::BudgetExceededStatus& status);
			typedef void(__stdcall *onReaderConnectionDeletedDeclaration)(::DDS::DataReader_ptr reader);
			typedef void(__stdcall *onOfferedDeadlineMissedDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status);
			typedef void(__stdcall *onOfferedIncompatibleQosDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus& status);
			typedef void(__stdcall *onLivelinessLostDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus& status);
			typedef void(__stdcall *onPublicationMatchedDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus& status);
			typedef void(__stdcall *onPublicationDisconnectedDeclaration)(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationDisconnectedStatus& status);
			typedef void(__stdcall *onPublicationReconnectedDeclaration)(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationReconnectedStatus& status);
			typedef void(__stdcall *onPublicationLostDeclaration)(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationLostStatus& status);
			typedef void(__stdcall *onWriterConnectionDeletedDeclaration)(::DDS::DataWriter_ptr writer);
			typedef void(__stdcall *onInconsistentTopicDeclaration)(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status);

			internal:
				::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative* impl_entity;

			protected:
				onDataOnReadersDeclaration onDataOnReadersFunctionCpp;
				onDataAvailabeDeclaration onDataAvalaibleFunctionCpp;
				onRequestedDeadlineMissedDeclaration onRequestedDeadlineMissedFunctionCpp;
				onRequestedIncompatibleQosDeclaration onRequestedIncompatibleQosFunctionCpp;
				onSampleRejectedDeclaration onSampleRejectedFunctionCpp;
				onLivelinessChangedDeclaration onLivelinessChangedFunctionCpp;
				onSubscriptionMatchedDeclaration onSubscriptionMatchedFunctionCpp;
				onSampleLostDeclaration onSampleLostFunctionCpp;
				onSubscriptionDisconnectedDeclaration onSubscriptionDisconnectedFunctionCpp;
				onSubscriptionReconnectedDeclaration onSubscriptionReconnectedFunctionCpp;
				onSubscriptionLostDeclaration onSubscriptionLostFunctionCpp;
				onBudgetExceededDeclaration onBudgetExceededFunctionCpp;
				onReaderConnectionDeletedDeclaration onReaderConnectionDeletedFunctionCpp;
				onOfferedDeadlineMissedDeclaration onOfferedDeadlineMissedFunctionCpp;
				onOfferedIncompatibleQosDeclaration onOfferedIncompatibleQosFunctionCpp;
				onLivelinessLostDeclaration onLivelinessLostFunctionCpp;
				onPublicationMatchedDeclaration onPublicationMatchedFunctionCpp;
				onPublicationDisconnectedDeclaration onPublicationDisconnectedFunctionCpp;
				onPublicationReconnectedDeclaration onPublicationReconnectedFunctionCpp;
				onPublicationLostDeclaration onPublicationLostFunctionCpp;
				onWriterConnectionDeletedDeclaration onWriterConnectionDeletedFunctionCpp;
				onInconsistentTopicDeclaration onInconsistentTopicFunctionCpp;

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

				delegate void onDataAvailableDelegate(::DDS::DataReader_ptr reader);
				void onDataAvalaible(::DDS::DataReader_ptr reader) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnDataAvailable(dataReader);
				};

				delegate void onRequestedDeadlineMissedDelegate(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status);
				void onRequestedDeadlineMissed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnRequestedDeadlineMissed(dataReader, OpenDDSharp::DDS::RequestedDeadlineMissedStatus(status));
				};

				delegate void onRequestedIncompatibleQosDelegate(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status);
				void onRequestedIncompatibleQos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnRequestedIncompatibleQos(dataReader, OpenDDSharp::DDS::RequestedIncompatibleQosStatus(status));
				};

				delegate void onSampleRejectedDelegate(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status);
				void onSampleRejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnSampleRejected(dataReader, OpenDDSharp::DDS::SampleRejectedStatus(status));
				};

				delegate void onLivelinessChangedDelegate(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status);
				void onLivelinessChanged(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnLivelinessChanged(dataReader, OpenDDSharp::DDS::LivelinessChangedStatus(status));
				};

				delegate void onSubscriptionMatchedDelegate(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status);
				void onSubscriptionMatched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnSubscriptionMatched(dataReader, OpenDDSharp::DDS::SubscriptionMatchedStatus(status));
				};

				delegate void onSampleLostDelegate(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status);
				void onSampleLost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnSampleLost(dataReader, OpenDDSharp::DDS::SampleLostStatus(status));
				};

				delegate void onSubscriptionDisconnectedDelegate(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionDisconnectedStatus& status);
				void onSubscriptionDisconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionDisconnectedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnSubscriptionDisconnected(dataReader, OpenDDSharp::OpenDDS::DCPS::SubscriptionDisconnectedStatus(status));
				};

				delegate void onSubscriptionReconnectedDelegate(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionReconnectedStatus& status);
				void onSubscriptionReconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionReconnectedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnSubscriptionReconnected(dataReader, OpenDDSharp::OpenDDS::DCPS::SubscriptionReconnectedStatus(status));
				};

				delegate void onSubscriptionLostDelegate(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionLostStatus& status);
				void onSubscriptionLost(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionLostStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnSubscriptionLost(dataReader, OpenDDSharp::OpenDDS::DCPS::SubscriptionLostStatus(status));
				};

				delegate void onBudgetExceededDelegate(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::BudgetExceededStatus& status);
				void onBudgetExceeded(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::BudgetExceededStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnBudgetExceeded(dataReader, OpenDDSharp::OpenDDS::DCPS::BudgetExceededStatus(status));
				};

				delegate void onReaderConnectionDeletedDelegate(::DDS::DataReader_ptr reader);
				void onReaderConnectionDeleted(::DDS::DataReader_ptr reader) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnConnectionDeleted(dataReader);
				};

				delegate void onOfferedDeadlineMissedDelegate(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status);
				void onOfferedDeadlineMissed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}
					
					OnOfferedDeadlineMissed(dataWriter, OpenDDSharp::DDS::OfferedDeadlineMissedStatus(status));
				};

				delegate void onOfferedIncompatibleQosDelegate(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status);
				void onOfferedIncompatibleQos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}
					
					OnOfferedIncompatibleQos(dataWriter, OpenDDSharp::DDS::OfferedIncompatibleQosStatus(status));
				};

				delegate void onLivelinessLostDelegate(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status);
				void onLivelinessLost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}
					
					OnLivelinessLost(dataWriter, OpenDDSharp::DDS::LivelinessLostStatus(status));
				};

				delegate void onPublicationMatchedDelegate(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status);
				void onPublicationMatched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}					

					OnPublicationMatched(dataWriter, OpenDDSharp::DDS::PublicationMatchedStatus(status));
				};

				delegate void onPublicationDisconnectedDelegate(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationDisconnectedStatus& status);
				void onPublicationDisconnected(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationDisconnectedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}
					
					OnPublicationDisconnected(dataWriter, OpenDDSharp::OpenDDS::DCPS::PublicationDisconnectedStatus(status));
				};

				delegate void onPublicationReconnectedDelegate(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationReconnectedStatus& status);
				void onPublicationReconnected(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationReconnectedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}
					
					OnPublicationReconnected(dataWriter, OpenDDSharp::OpenDDS::DCPS::PublicationReconnectedStatus(status));
				};

				delegate void onPublicationLostDelegate(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationLostStatus& status);
				void onPublicationLost(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationLostStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}
					
					OnPublicationLost(dataWriter, OpenDDSharp::OpenDDS::DCPS::PublicationLostStatus(status));
				};

				delegate void onWriterConnectionDeletedDelegate(::DDS::DataWriter_ptr writer);
				void onWriterConnectionDeleted(::DDS::DataWriter_ptr writer) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}
					
					OnConnectionDeleted(dataWriter);
				};

				delegate void onInconsistentTopicDelegate(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status);
				void onInconsistentTopic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(topic);
					OpenDDSharp::DDS::Topic^ managedTopic = nullptr;
					if (entity != nullptr) {
						managedTopic = static_cast<OpenDDSharp::DDS::Topic^>(entity);
					}
					
					OnInconsistentTopic(managedTopic, OpenDDSharp::DDS::InconsistentTopicStatus(status));
				};

			public:
				DomainParticipantListener();

			public:
				virtual void OnDataOnReaders(OpenDDSharp::DDS::Subscriber^ subscriber) = 0;
				virtual void OnDataAvailable(OpenDDSharp::DDS::DataReader^ reader) = 0;
				virtual void OnRequestedDeadlineMissed(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::RequestedDeadlineMissedStatus status) = 0;
				virtual void OnRequestedIncompatibleQos(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::RequestedIncompatibleQosStatus status) = 0;
				virtual void OnSampleRejected(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::SampleRejectedStatus status) = 0;
				virtual void OnLivelinessChanged(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::LivelinessChangedStatus status) = 0;
				virtual void OnSubscriptionMatched(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::SubscriptionMatchedStatus status) = 0;
				virtual void OnSampleLost(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::SampleLostStatus status) = 0;
				virtual void OnSubscriptionDisconnected(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::SubscriptionDisconnectedStatus status) = 0;
				virtual void OnSubscriptionReconnected(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::SubscriptionReconnectedStatus status) = 0;
				virtual void OnSubscriptionLost(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::SubscriptionLostStatus status) = 0;
				virtual void OnBudgetExceeded(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::BudgetExceededStatus status) = 0;
				virtual void OnConnectionDeleted(OpenDDSharp::DDS::DataReader^ reader) = 0;
				virtual void OnOfferedDeadlineMissed(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::OfferedDeadlineMissedStatus status) = 0;
				virtual void OnOfferedIncompatibleQos(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::OfferedIncompatibleQosStatus status) = 0;
				virtual void OnLivelinessLost(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::LivelinessLostStatus status) = 0;
				virtual void OnPublicationMatched(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::PublicationMatchedStatus status) = 0;
				virtual void OnPublicationDisconnected(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::OpenDDS::DCPS::PublicationDisconnectedStatus status) = 0;
				virtual void OnPublicationReconnected(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::OpenDDS::DCPS::PublicationReconnectedStatus status) = 0;
				virtual void OnPublicationLost(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::OpenDDS::DCPS::PublicationLostStatus status) = 0;
				virtual void OnConnectionDeleted(OpenDDSharp::DDS::DataWriter^ reader) = 0;
				virtual void OnInconsistentTopic(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::InconsistentTopicStatus status) = 0;

			};
		};
	};
};