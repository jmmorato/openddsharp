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

#include "Subscriber.h"
#include "SubscriberListenerNative.h"
#include "StatusKind.h"

namespace OpenDDSharp {
	namespace DDS {	

		/// <summary>
		/// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="Subscriber" />
		/// such that the application can be notified of relevant status changes.		
		/// </summary>
		public ref class SubscriberListener abstract {

		typedef void(__stdcall *onDataOnReadersDeclaration)(::DDS::Subscriber_ptr subscriber);
        typedef void(__stdcall *onDataAvailabeDeclaration)(::DDS::DataReader_ptr reader);
        typedef void(__stdcall *onRequestedDeadlineMissedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status);
        typedef void(__stdcall *onRequestedIncompatibleQosDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status);
        typedef void(__stdcall *onSampleRejectedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status);
        typedef void(__stdcall *onLivelinessChangedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status);
        typedef void(__stdcall *onSubscriptionMatchedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status);
        typedef void(__stdcall *onSampleLostDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status);

        private:
            System::Runtime::InteropServices::GCHandle gchDataOnReaders;
            System::Runtime::InteropServices::GCHandle gchDataAvailable;
            System::Runtime::InteropServices::GCHandle gchRequestedDeadlineMissed;
            System::Runtime::InteropServices::GCHandle gchRequestedIncompatibleQos;
            System::Runtime::InteropServices::GCHandle gchSampleRejected;
            System::Runtime::InteropServices::GCHandle gchLivelinessChanged;
            System::Runtime::InteropServices::GCHandle gchSubscriptionMatched;
            System::Runtime::InteropServices::GCHandle gchSampleLost;

		internal:
			::OpenDDSharp::DDS::SubscriberListenerNative* impl_entity;

		protected:
			onDataOnReadersDeclaration onDataOnReadersFunctionCpp;
            onDataAvailabeDeclaration onDataAvalaibleFunctionCpp;
            onRequestedDeadlineMissedDeclaration onRequestedDeadlineMissedFunctionCpp;
            onRequestedIncompatibleQosDeclaration onRequestedIncompatibleQosFunctionCpp;
            onSampleRejectedDeclaration onSampleRejectedFunctionCpp;
            onLivelinessChangedDeclaration onLivelinessChangedFunctionCpp;
            onSubscriptionMatchedDeclaration onSubscriptionMatchedFunctionCpp;
            onSampleLostDeclaration onSampleLostFunctionCpp;

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


		public:
			/// <summary>
			/// Creates a new instance of <see cref="SubscriberListener" />
			/// </summary>
			SubscriberListener();

        protected:
            !SubscriberListener();

		public:
			/// <summary>
			/// Handles the <see cref="OpenDDSharp::DDS::StatusKind::DataOnReadersStatus" /> communication status.
			/// The <see cref="OpenDDSharp::DDS::StatusKind::DataOnReadersStatus" /> indicates that new data is available on some of the data
			/// readers associated with the subscriber. Applications receiving this status can call GetDataReaders on
			/// the subscriber to get the set of data readers with data available.
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

		};
	};
};
