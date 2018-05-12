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

#include "DataReader.h"
#include "DataWriter.h"
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
#include "StatusKind.h"

namespace OpenDDSharp {
	namespace DDS {
		ref class DataReader;
	}
}
namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {

			/// <summary>
			/// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="OpenDDSharp::DDS::DataReader" />
			/// such that the application can be notified of relevant status changes.		
			/// </summary>
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
				/// <summary>
				/// Creates a new instance of <see cref="DataReaderListener" />
				/// </summary>
				DataReaderListener();

			public:
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
				/// Called when a subscription connection failure has been detected and there are still associations using the connection
				/// after the configurable graceful_disconnected_period.
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::OpenDDS::DCPS::SubscriptionDisconnectedStatus" />.</param>
				virtual void OnSubscriptionDisconnected(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::SubscriptionDisconnectedStatus status) = 0;

				/// <summary>
				/// Called when a disconnected subscription connection has been reconnected.
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::OpenDDS::DCPS::SubscriptionReconnectedStatus" />.</param>
				virtual void OnSubscriptionReconnected(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::SubscriptionReconnectedStatus status) = 0;

				/// <summary>
				/// Called when a subscription connection is lost and hence one or more associations from this publication to some subscribers have been lost.
				/// A connection is "lost" when the retry attempts have been exhausted.
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::OpenDDS::DCPS::SubscriptionLostStatus" />.</param>
				virtual void OnSubscriptionLost(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::SubscriptionLostStatus status) = 0;

				/// <summary>
				/// Allow reporting delays in excess of the	policy duration setting.
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				/// <param name="status">The current <see cref="OpenDDSharp::OpenDDS::DCPS::BudgetExceededStatus" /> status.</param>
				virtual void OnBudgetExceeded(OpenDDSharp::DDS::DataReader^ reader, OpenDDSharp::OpenDDS::DCPS::BudgetExceededStatus status) = 0;

				/// <summary>
				/// Called when the connection object is cleaned up and the reconnect thread exits.
				/// </summary>
				/// <param name="reader">The <see cref="OpenDDSharp::DDS::DataReader" /> that triggered the event.</param>
				virtual void OnConnectionDeleted(OpenDDSharp::DDS::DataReader^ reader) = 0;

			};

		};
	};
};