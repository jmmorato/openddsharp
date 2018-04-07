#pragma once

#include "DataWriter.h"
#include "DataWriterListenerNative.h"
#include "OfferedDeadlineMissedStatus.h"
#include "OfferedIncompatibleQosStatus.h"
#include "LivelinessLostStatus.h"
#include "PublicationMatchedStatus.h"
#include "PublicationDisconnectedStatus.h"
#include "PublicationReconnectedStatus.h"
#include "PublicationLostStatus.h"

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			public ref class DataWriterListener abstract {

			typedef void(__stdcall *onOfferedDeadlineMissedDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status);
			typedef void(__stdcall *onOfferedIncompatibleQosDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus& status);
			typedef void(__stdcall *onLivelinessLostDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus& status);
			typedef void(__stdcall *onPublicationMatchedDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus& status);
			typedef void(__stdcall *onPublicationDisconnectedDeclaration)(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationDisconnectedStatus& status);
			typedef void(__stdcall *onPublicationReconnectedDeclaration)(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationReconnectedStatus& status);
			typedef void(__stdcall *onPublicationLostDeclaration)(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationLostStatus& status);
			typedef void(__stdcall *onConnectionDeletedDeclaration)(::DDS::DataWriter_ptr writer);

			internal:
				::OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative* impl_entity;

			protected:
				onOfferedDeadlineMissedDeclaration onOfferedDeadlineMissedFunctionCpp;
				onOfferedIncompatibleQosDeclaration onOfferedIncompatibleQosFunctionCpp;
				onLivelinessLostDeclaration onLivelinessLostFunctionCpp;
				onPublicationMatchedDeclaration onPublicationMatchedFunctionCpp;
				onPublicationDisconnectedDeclaration onPublicationDisconnectedFunctionCpp;
				onPublicationReconnectedDeclaration onPublicationReconnectedFunctionCpp;
				onPublicationLostDeclaration onPublicationLostFunctionCpp;
				onConnectionDeletedDeclaration onConnectionDeletedFunctionCpp;

			private:
				delegate void onOfferedDeadlineMissedDelegate(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status);
				void onOfferedDeadlineMissed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}					

					OnOfferedDeadlineMissed(dataWriter, OpenDDSharp::DDS::OfferedDeadlineMissedStatus(status));
				};

				delegate void onOfferedIncompatibleQosDelegate(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status);
				void onOfferedIncompatibleQos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}					

					OnOfferedIncompatibleQos(dataWriter, OpenDDSharp::DDS::OfferedIncompatibleQosStatus(status));
				};

				delegate void onLivelinessLostDelegate(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status);
				void onLivelinessLost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}					

					OnLivelinessLost(dataWriter, OpenDDSharp::DDS::LivelinessLostStatus(status));
				};

				delegate void onPublicationMatchedDelegate(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status);
				void onPublicationMatched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}					

					OnPublicationMatched(dataWriter, OpenDDSharp::DDS::PublicationMatchedStatus(status));
				};

				delegate void onPublicationDisconnectedDelegate(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationDisconnectedStatus& status);
				void onPublicationDisconnected(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationDisconnectedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}					

					OnPublicationDisconnected(dataWriter, OpenDDSharp::OpenDDS::DCPS::PublicationDisconnectedStatus(status));
				};

				delegate void onPublicationReconnectedDelegate(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationReconnectedStatus& status);
				void onPublicationReconnected(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationReconnectedStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}					

					OnPublicationReconnected(dataWriter, OpenDDSharp::OpenDDS::DCPS::PublicationReconnectedStatus(status));
				};

				delegate void onPublicationLostDelegate(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationLostStatus& status);
				void onPublicationLost(::DDS::DataWriter_ptr writer, const ::OpenDDS::DCPS::PublicationLostStatus& status) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}					

					OnPublicationLost(dataWriter, OpenDDSharp::OpenDDS::DCPS::PublicationLostStatus(status));
				};

				delegate void onConnectionDeletedDelegate(::DDS::DataWriter_ptr writer);
				void onConnectionDeleted(::DDS::DataWriter_ptr writer) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}					

					OnConnectionDeleted(gcnew OpenDDSharp::DDS::DataWriter(writer));
				};

			public:
				DataWriterListener();

			public:
				virtual void OnOfferedDeadlineMissed(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::OfferedDeadlineMissedStatus status) = 0;
				virtual void OnOfferedIncompatibleQos(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::OfferedIncompatibleQosStatus status) = 0;
				virtual void OnLivelinessLost(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::LivelinessLostStatus status) = 0;
				virtual void OnPublicationMatched(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::PublicationMatchedStatus status) = 0;
				virtual void OnPublicationDisconnected(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::OpenDDS::DCPS::PublicationDisconnectedStatus status) = 0;
				virtual void OnPublicationReconnected(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::OpenDDS::DCPS::PublicationReconnectedStatus status) = 0;
				virtual void OnPublicationLost(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::OpenDDS::DCPS::PublicationLostStatus status) = 0;
				virtual void OnConnectionDeleted(OpenDDSharp::DDS::DataWriter^ reader) = 0;
			};
		};
	};
};