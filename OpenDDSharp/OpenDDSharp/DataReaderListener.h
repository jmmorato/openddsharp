#pragma once

#include "DataReader.h"
#include "DataReaderListenerNative.h"
#include "RequestedDeadlineMissedStatus.h"
#include "RequestedIncompatibleQosStatus.h"
#include "SampleRejectedStatus.h"
#include "LivelinessChangedStatus.h"
#include "SubscriptionMatchedStatus.h"
#include "SampleLostStatus.h"
#include "SubscriptionDisconnectedStatus.h"
#include "SubscriptionReconnectedStatus.h"
#include "SubscriptionLostStatus.h"
#include "BudgetExceededStatus.h"

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			public ref class DataReaderListener abstract {

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
			typedef void(__stdcall *onConnectionDeletedDeclaration)(::DDS::DataReader_ptr reader);

			internal:
				::OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative* impl_entity;

			protected:
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
				onConnectionDeletedDeclaration onConnectionDeletedFunctionCpp;

			private:
				delegate void onDataAvailableDelegate(::DDS::DataReader_ptr reader);
				void onDataAvalaible(::DDS::DataReader_ptr reader) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnDataAvailable(dataReader);
				};

				delegate void onRequestedDeadlineMissedDelegate(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status);
				void onRequestedDeadlineMissed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnRequestedDeadlineMissed(dataReader, OpenDDSharp::DDS::RequestedDeadlineMissedStatus(status));
				};

				delegate void onRequestedIncompatibleQosDelegate(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status);
				void onRequestedIncompatibleQos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnRequestedIncompatibleQos(dataReader, OpenDDSharp::DDS::RequestedIncompatibleQosStatus(status));
				};

				delegate void onSampleRejectedDelegate(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status);
				void onSampleRejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnSampleRejected(dataReader, OpenDDSharp::DDS::SampleRejectedStatus(status));
				};

				delegate void onLivelinessChangedDelegate(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status);
				void onLivelinessChanged(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnLivelinessChanged(dataReader, OpenDDSharp::DDS::LivelinessChangedStatus(status));
				};

				delegate void onSubscriptionMatchedDelegate(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status);
				void onSubscriptionMatched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnSubscriptionMatched(dataReader, OpenDDSharp::DDS::SubscriptionMatchedStatus(status));
				};

				delegate void onSampleLostDelegate(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status);
				void onSampleLost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnSampleLost(dataReader, OpenDDSharp::DDS::SampleLostStatus(status));
				};

				delegate void onSubscriptionDisconnectedDelegate(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionDisconnectedStatus& status);
				void onSubscriptionDisconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionDisconnectedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnSubscriptionDisconnected(dataReader, OpenDDSharp::OpenDDS::DCPS::SubscriptionDisconnectedStatus(status));
				};

				delegate void onSubscriptionReconnectedDelegate(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionReconnectedStatus& status);
				void onSubscriptionReconnected(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionReconnectedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnSubscriptionReconnected(dataReader, OpenDDSharp::OpenDDS::DCPS::SubscriptionReconnectedStatus(status));
				};

				delegate void onSubscriptionLostDelegate(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionLostStatus& status);
				void onSubscriptionLost(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::SubscriptionLostStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnSubscriptionLost(dataReader, OpenDDSharp::OpenDDS::DCPS::SubscriptionLostStatus(status));
				};

				delegate void onBudgetExceededDelegate(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::BudgetExceededStatus& status);
				void onBudgetExceeded(::DDS::DataReader_ptr reader, const ::OpenDDS::DCPS::BudgetExceededStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnBudgetExceeded(dataReader, OpenDDSharp::OpenDDS::DCPS::BudgetExceededStatus(status));
				};

				delegate void onConnectionDeletedDelegate(::DDS::DataReader_ptr reader);
				void onConnectionDeleted(::DDS::DataReader_ptr reader) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}

					OnConnectionDeleted(dataReader);
				};

			public:
				DataReaderListener();

			public:
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

			};

		};
	};
};