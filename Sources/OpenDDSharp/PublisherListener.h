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

#include "DataWriterListener.h"
#include "PublisherListenerNative.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="Publisher" />
		/// such that the application can be notified of relevant status changes.		
		/// </summary>
		public ref class PublisherListener abstract {

            typedef void(__stdcall *onOfferedDeadlineMissedDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status);
            typedef void(__stdcall *onOfferedIncompatibleQosDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus& status);
            typedef void(__stdcall *onLivelinessLostDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus& status);
            typedef void(__stdcall *onPublicationMatchedDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus& status);

        private:
            System::Runtime::InteropServices::GCHandle gchOfferedDeadlineMissed;
            System::Runtime::InteropServices::GCHandle gchOfferedIncompatibleQos;
            System::Runtime::InteropServices::GCHandle gchLivelinessLost;
            System::Runtime::InteropServices::GCHandle gchPublicationMatched;

		internal:
			::OpenDDSharp::DDS::PublisherListenerNative* impl_entity;

        protected:
            onOfferedDeadlineMissedDeclaration onOfferedDeadlineMissedFunctionCpp;
            onOfferedIncompatibleQosDeclaration onOfferedIncompatibleQosFunctionCpp;
            onLivelinessLostDeclaration onLivelinessLostFunctionCpp;
            onPublicationMatchedDeclaration onPublicationMatchedFunctionCpp;

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

		public:
			/// <summary>
			/// Creates a new instance of <see cref="PublisherListener" />
			/// </summary>
			PublisherListener();

        protected:
            !PublisherListener();

        public:
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

		};
	};
};

