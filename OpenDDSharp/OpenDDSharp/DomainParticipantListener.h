/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#include "DomainParticipantListenerNative.h"

namespace OpenDDSharp {
	namespace DDS {
		value struct StatusKind;
		ref class DeadlineQosPolicy;
	}
};

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {
			
			/// <summary>
			/// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="DomainParticipant" />
			/// such that the application can be notified of relevant status changes.		
			/// <summary>
			/// <remarks>
			/// The purpose of the <see cref="DomainParticipantListener" /> is to be the listener of last resort that is notified of all status changes not
			/// captured by more specific listeners attached to the <see cref="Entity" /> objects.When a relevant status change occurs, DDS will first attempt 
			/// to notify the listener attached to the concerned <see cref="Entity" /> if one is installed. Otherwise, DDS will notify the Listener 
			/// attached to the <see cref="DomainParticipant" />.
			/// </remarks>
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
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(subscriber);
					OpenDDSharp::DDS::Subscriber^ managedSubscriber = nullptr;
					if (entity != nullptr) {
						managedSubscriber = static_cast<OpenDDSharp::DDS::Subscriber^>(entity);
					}
					
					OnDataOnReaders(managedSubscriber);
				};

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

				delegate void onReaderConnectionDeletedDelegate(::DDS::DataReader_ptr reader);
				void onReaderConnectionDeleted(::DDS::DataReader_ptr reader) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(reader);
					OpenDDSharp::DDS::DataReader^ dataReader = nullptr;
					if (entity != nullptr) {
						dataReader = static_cast<OpenDDSharp::DDS::DataReader^>(entity);
					}
					
					OnConnectionDeleted(dataReader);
				};

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

				delegate void onWriterConnectionDeletedDelegate(::DDS::DataWriter_ptr writer);
				void onWriterConnectionDeleted(::DDS::DataWriter_ptr writer) {
					OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(writer);
					OpenDDSharp::DDS::DataWriter^ dataWriter = nullptr;
					if (entity != nullptr) {
						dataWriter = static_cast<OpenDDSharp::DDS::DataWriter^>(entity);
					}
					
					OnConnectionDeleted(dataWriter);
				};

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
				/// Creates a new instance of <see cref="DomainParticipantListener" />
				/// </summary>
				DomainParticipantListener();

			public:
				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::DataOnReadersStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::DataOnReadersStatus" /> indicates that new data is available on some of the data
				/// readers associated with the subscriber. Applications receiving this status can call <see cref="Subscriber::GetDataReaders /> on
				/// the subscriber to get the set of data readers with data available.</para>
				/// </summary>
				virtual void OnDataOnReaders(OpenDDSharp::DDS::Subscriber^ subscriber) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::DataAvailableStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::DataAvailableStatus" /> indicates that samples are available on the <see cref="OpenDDSharp::DDS::DataReader" />.
				/// Applications receiving this status can use the various take and read operations on the <see cref="OpenDDSharp::DDS::DataReader" /> to retrieve the data.</para>
				/// </summary>
				virtual void OnDataAvailable(OpenDDSharp::DDS::DataReader^ reader) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::RequestedDeadlineMissedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::RequestedDeadlineMissedStatus" /> indicates that the deadline requested via the
				/// <see cref="OpenDDSharp::DDS::DeadlineQosPolicy" /> was not respected for a specific instance.</para>
				/// </summary>
				virtual void OnRequestedDeadlineMissed(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::RequestedDeadlineMissedStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::RequestedIncompatibleQosStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::RequestedIncompatibleQosStatus" /> indicates that one or more QoS policy values that
				/// were requested were incompatible with what was offered.</para>
				/// </summary>
				virtual void OnRequestedIncompatibleQos(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::RequestedIncompatibleQosStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::SampleRejectedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::SampleRejectedStatus" /> indicates that a sample received by the 
				/// <see cref="OpenDDSharp::DDS::DataReader" /> has been rejected.</para>
				/// </summary>
				virtual void OnSampleRejected(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::SampleRejectedStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::LivelinessChangedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::LivelinessChangedStatus" /> indicates that there have been liveliness changes for one or
				/// more <see cref="OpenDDSharp::DDS::DataWriter" />s that are publishing instances for this <see cref="OpenDDSharp::DDS::DataReader" />.</para>
				/// </summary>
				virtual void OnLivelinessChanged(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::LivelinessChangedStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::SubscriptionMatchedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::SubscriptionMatchedStatus" /> indicates that either a compatible <see cref="OpenDDSharp::DDS::DataWriter" /> has been
				/// matched or a previously matched <see cref="OpenDDSharp::DDS::DataWriter" /> has ceased to be matched.</para>
				/// </summary>
				virtual void OnSubscriptionMatched(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::SubscriptionMatchedStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::SampleLostStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::SampleLostStatus" /> indicates that a sample has been lost and 
				/// never received by the <see cref="OpenDDSharp::DDS::DataReader" />.</para>
				/// </summary>
				virtual void OnSampleLost(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::SampleLostStatus status) = 0;

				/// <summary>
				/// Called when a subscription connection failure has been detected and there are still associations using the connection
				/// after the configurable graceful_disconnected_period.
				/// </summary>
				virtual void OnSubscriptionDisconnected(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::SubscriptionDisconnectedStatus status) = 0;

				/// <summary>
				/// Called when a disconnected subscription connection has been reconnected.
				/// </summary>
				virtual void OnSubscriptionReconnected(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::SubscriptionReconnectedStatus status) = 0;

				/// <summary>
				/// Called when a subscription connection is lost and hence one or more associations from this publication to some subscribers have been lost.
				/// A connection is "lost" when the retry attempts have been exhausted.
				/// </summary>
				virtual void OnSubscriptionLost(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::SubscriptionLostStatus status) = 0;

				/// <summary>
				/// Allow reporting delays in excess of the	policy duration setting.
				/// </summary>
				virtual void OnBudgetExceeded(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::BudgetExceededStatus status) = 0;

				/// <summary>
				/// Called when the connection object is cleaned up and the reconnect thread exits.
				/// </summary>
				virtual void OnConnectionDeleted(OpenDDSharp::DDS::DataReader^ reader) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::OfferedDeadlineMissedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::OfferedDeadlineMissedStatus" /> indicates that the deadline offered by the
				/// <see cref="OpenDDSharp::DDS::DataWriter" /> has been missed for one or more instances.</para>
				/// </summary>
				virtual void OnOfferedDeadlineMissed(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::OfferedDeadlineMissedStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::OfferedIncompatibleQosStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::OfferedIncompatibleQosStatus" /> indicates that an offered QoS was incompatible with
				/// the requested QoS of a <see cref="OpenDDSharp::DDS::DataReader" />.</para>
				/// </summary>
				virtual void OnOfferedIncompatibleQos(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::OfferedIncompatibleQosStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::LivelinessLostStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::LivelinessLostStatus" /> indicates that the liveliness that the <see cref="OpenDDSharp::DDS::DataWriter" /> committed
				/// through its Liveliness QoS has not been respected. This means that any connected <see cref="OpenDDSharp::DDS::DataReader" />s will consider this 
				/// <see cref="OpenDDSharp::DDS::DataWriter" /> no longer active</para>
				/// </summary>
				virtual void OnLivelinessLost(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::LivelinessLostStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::PublicationMatchedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::PublicationMatchedStatus" /> indicates that the liveliness that the <see cref="OpenDDSharp::DDS::DataWriter" /> committed
				/// through its Liveliness QoS has not been respected. This means that any connected <see cref="OpenDDSharp::DDS::DataReader" />s 
				/// will consider this <see cref="OpenDDSharp::DDS::DataWriter" /> no longer active.</para>
				/// </summary>
				virtual void OnPublicationMatched(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::PublicationMatchedStatus status) = 0;

				/// <summary>
			    /// Called when a publication connection failure has been detected and there are still associations using the connection
				/// after the configurable graceful_disconnected_period.
				/// </summary>
				virtual void OnPublicationDisconnected(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::OpenDDS::DCPS::PublicationDisconnectedStatus status) = 0;

				/// <summary>
				/// Called when a disconnected publication connection has been reconnected.
				/// </summary>
				virtual void OnPublicationReconnected(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::OpenDDS::DCPS::PublicationReconnectedStatus status) = 0;

				/// <summary>
				/// Called when a publication connection is lost and hence one or more associations from this publication to some subscribers have been lost.
				/// A connection is "lost" when the retry attempts have been exhausted.
				/// </summary>
				virtual void OnPublicationLost(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::OpenDDS::DCPS::PublicationLostStatus status) = 0;

				/// <summary>
				/// Called when the publication connection object is cleaned up and the reconnect thread exits.
				/// </summary>
				virtual void OnConnectionDeleted(OpenDDSharp::DDS::DataWriter^ reader) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::InconsistentTopicStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::InconsistentTopicStatus" /> indicates that a <see cref="OpenDDSharp::DDS::Topic" /> was attempted to be registered that
				/// already exists with different characteristics. Typically, the existing <see cref="OpenDDSharp::DDS::Topic" /> may have a different type associated with it.</para>
				/// </summary>
				virtual void OnInconsistentTopic(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::InconsistentTopicStatus status) = 0;

			};
		};
	};
};