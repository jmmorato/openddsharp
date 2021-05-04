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

#include "DomainParticipantListenerNative.h"

namespace OpenDDSharp {
	namespace DDS {
		value struct StatusKind;
		ref class DeadlineQosPolicy;
		ref class DomainParticipant;
		ref class Entity;
	}
};

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			/// <summary>
			/// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="OpenDDSharp::DDS::DomainParticipant" />
			/// such that the application can be notified of relevant status changes.		
			/// </summary>
			/// <remarks>
			/// The purpose of the DomainParticipantListener is to be the listener of last resort that is notified of all status changes not
			/// captured by more specific listeners attached to the <see cref="OpenDDSharp::DDS::Entity" /> objects.When a relevant status change occurs, DDS will first attempt 
			/// to notify the listener attached to the concerned <see cref="OpenDDSharp::DDS::Entity" /> if one is installed. Otherwise, DDS will notify the Listener 
			/// attached to the <see cref="OpenDDSharp::DDS::DomainParticipant" />.
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
			typedef void(__stdcall *onOfferedDeadlineMissedDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status);
			typedef void(__stdcall *onOfferedIncompatibleQosDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus& status);
			typedef void(__stdcall *onLivelinessLostDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus& status);
			typedef void(__stdcall *onPublicationMatchedDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus& status);
			typedef void(__stdcall *onInconsistentTopicDeclaration)(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status);

            private:
                System::Runtime::InteropServices::GCHandle gchInconsistentTopic;
                System::Runtime::InteropServices::GCHandle gchDataOnReaders;
                System::Runtime::InteropServices::GCHandle gchDataAvailable;
                System::Runtime::InteropServices::GCHandle gchRequestedDeadlineMissed;
                System::Runtime::InteropServices::GCHandle gchRequestedIncompatibleQos;
                System::Runtime::InteropServices::GCHandle gchSampleRejected;
                System::Runtime::InteropServices::GCHandle gchLivelinessChanged;
                System::Runtime::InteropServices::GCHandle gchSubscriptionMatched;
                System::Runtime::InteropServices::GCHandle gchSampleLost;
                System::Runtime::InteropServices::GCHandle gchOfferedDeadlineMissed;
                System::Runtime::InteropServices::GCHandle gchOfferedIncompatibleQos;
                System::Runtime::InteropServices::GCHandle gchLivelinessLost;
                System::Runtime::InteropServices::GCHandle gchPublicationMatched;

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
				onOfferedDeadlineMissedDeclaration onOfferedDeadlineMissedFunctionCpp;
				onOfferedIncompatibleQosDeclaration onOfferedIncompatibleQosFunctionCpp;
				onLivelinessLostDeclaration onLivelinessLostFunctionCpp;
				onPublicationMatchedDeclaration onPublicationMatchedFunctionCpp;
				onInconsistentTopicDeclaration onInconsistentTopicFunctionCpp;

			private:
				delegate void onDataOnReadersDelegate(::DDS::Subscriber_ptr subscriber);
				void onDataOnReaders(::DDS::Subscriber_ptr subscriber) {
					
					OpenDDSharp::DDS::Subscriber^ managedSubscriber = nullptr;

					if (subscriber != NULL) {
						OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance()->find(subscriber);
						if (entity != nullptr) {
							managedSubscriber = static_cast<OpenDDSharp::DDS::Subscriber^>(entity);
						}
						else {
							managedSubscriber = gcnew OpenDDSharp::DDS::Subscriber(subscriber);
							EntityManager::get_instance()->add(subscriber, managedSubscriber);
						}
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

            protected:
                !DomainParticipantListener();

			public:
				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::DataOnReadersStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::DataOnReadersStatus" /> indicates that new data is available on some of the data
				/// readers associated with the subscriber. Applications receiving this status can call GetDataReaders on
				/// the subscriber to get the set of data readers with data available.</para>
				/// </summary>
				/// <param name="subscriber">The <see cref="OpenDDSharp::DDS::Subscriber" /> that triggered the event.</param>
				virtual void OnDataOnReaders(OpenDDSharp::DDS::Subscriber^ subscriber) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::DataAvailableStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::DataAvailableStatus" /> indicates that samples are available on the <see cref="OpenDDSharp::DDS::DataReader" />.
				/// Applications receiving this status can use the various take and read operations on the <see cref="OpenDDSharp::DDS::DataReader" /> to retrieve the data.</para>
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				virtual void OnDataAvailable(OpenDDSharp::DDS::DataReader^ reader) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::RequestedDeadlineMissedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::RequestedDeadlineMissedStatus" /> indicates that the deadline requested via the
				/// <see cref="OpenDDSharp::DDS::DeadlineQosPolicy" /> was not respected for a specific instance.</para>
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::RequestedDeadlineMissedStatus" />.</param>
				virtual void OnRequestedDeadlineMissed(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::RequestedDeadlineMissedStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::RequestedIncompatibleQosStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::RequestedIncompatibleQosStatus" /> indicates that one or more QoS policy values that
				/// were requested were incompatible with what was offered.</para>
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::RequestedIncompatibleQosStatus" />.</param>
				virtual void OnRequestedIncompatibleQos(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::RequestedIncompatibleQosStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::SampleRejectedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::SampleRejectedStatus" /> indicates that a sample received by the 
				/// <see cref="OpenDDSharp::DDS::DataReader" /> has been rejected.</para>
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::SampleRejectedStatus" />.</param>
				virtual void OnSampleRejected(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::SampleRejectedStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::LivelinessChangedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::LivelinessChangedStatus" /> indicates that there have been liveliness changes for one or
				/// more <see cref="OpenDDSharp::DDS::DataWriter" />s that are publishing instances for this <see cref="OpenDDSharp::DDS::DataReader" />.</para>
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::LivelinessChangedStatus" />.</param>
				virtual void OnLivelinessChanged(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::LivelinessChangedStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::SubscriptionMatchedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::SubscriptionMatchedStatus" /> indicates that either a compatible <see cref="OpenDDSharp::DDS::DataWriter" /> has been
				/// matched or a previously matched <see cref="OpenDDSharp::DDS::DataWriter" /> has ceased to be matched.</para>
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::SubscriptionMatchedStatus" />.</param>
				virtual void OnSubscriptionMatched(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::SubscriptionMatchedStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::SampleLostStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::SampleLostStatus" /> indicates that a sample has been lost and 
				/// never received by the <see cref="OpenDDSharp::DDS::DataReader" />.</para>
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::SampleLostStatus" />.</param>
				virtual void OnSampleLost(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::DDS::SampleLostStatus status) = 0;

				/// <summary>
				/// Called when the connection object is cleaned up and the reconnect thread exits.
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				virtual void OnConnectionDeleted(OpenDDSharp::DDS::DataReader^ reader) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::OfferedDeadlineMissedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::OfferedDeadlineMissedStatus" /> indicates that the deadline offered by the
				/// <see cref="OpenDDSharp::DDS::DataWriter" /> has been missed for one or more instances.</para>
				/// </summary>
				/// <param name="writer">The <see cref="OpenDDSharp::DDS::DataWriter" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::OfferedDeadlineMissedStatus" />.</param>
				virtual void OnOfferedDeadlineMissed(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::OfferedDeadlineMissedStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::OfferedIncompatibleQosStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::OfferedIncompatibleQosStatus" /> indicates that an offered QoS was incompatible with
				/// the requested QoS of a <see cref="OpenDDSharp::DDS::DataReader" />.</para>
				/// </summary>
				/// <param name="writer">The <see cref="OpenDDSharp::DDS::DataWriter" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::OfferedIncompatibleQosStatus" />.</param>
				virtual void OnOfferedIncompatibleQos(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::OfferedIncompatibleQosStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::LivelinessLostStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::LivelinessLostStatus" /> indicates that the liveliness that the <see cref="OpenDDSharp::DDS::DataWriter" /> committed
				/// through its Liveliness QoS has not been respected. This means that any connected <see cref="OpenDDSharp::DDS::DataReader" />s will consider this 
				/// <see cref="OpenDDSharp::DDS::DataWriter" /> no longer active</para>
				/// </summary>
				/// <param name="writer">The <see cref="OpenDDSharp::DDS::DataWriter" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::LivelinessLostStatus" />.</param>
				virtual void OnLivelinessLost(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::LivelinessLostStatus status) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::PublicationMatchedStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::PublicationMatchedStatus" /> indicates that the liveliness that the <see cref="OpenDDSharp::DDS::DataWriter" /> committed
				/// through its Liveliness QoS has not been respected. This means that any connected <see cref="OpenDDSharp::DDS::DataReader" />s 
				/// will consider this <see cref="OpenDDSharp::DDS::DataWriter" /> no longer active.</para>
				/// </summary>
				/// <param name="writer">The <see cref="OpenDDSharp::DDS::DataWriter" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::PublicationMatchedStatus" />.</param>
				virtual void OnPublicationMatched(OpenDDSharp::DDS::DataWriter^ writer, OpenDDSharp::DDS::PublicationMatchedStatus status) = 0;				

				/// <summary>
				/// Called when the publication connection object is cleaned up and the reconnect thread exits.
				/// </summary>
				/// <param name="writer">The <see cref="OpenDDSharp::DDS::DataWriter" /> that triggered the event.</param>
				virtual void OnConnectionDeleted(OpenDDSharp::DDS::DataWriter^ writer) = 0;

				/// <summary>
				/// <para>Handles the <see cref="OpenDDSharp::DDS::StatusKind::InconsistentTopicStatus" /> communication status.</para>
				/// <para>The <see cref="OpenDDSharp::DDS::StatusKind::InconsistentTopicStatus" /> indicates that a <see cref="OpenDDSharp::DDS::Topic" /> was attempted to be registered that
				/// already exists with different characteristics. Typically, the existing <see cref="OpenDDSharp::DDS::Topic" /> may have a different type associated with it.</para>
				/// </summary>
				/// <param name="topic">The <see cref="OpenDDSharp::DDS::Topic" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::DDS::InconsistentTopicStatus" />.</param>
				virtual void OnInconsistentTopic(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::InconsistentTopicStatus status) = 0;

			};
		};
	};
};