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

#include "Subscriber.h"
#include "SubscriberListenerNative.h"
#include "StatusKind.h"

namespace OpenDDSharp {
	namespace DDS {	

		/// <summary>
		/// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="Subscriber" />
		/// such that the application can be notified of relevant status changes.		
		/// <summary>
		public ref class SubscriberListener abstract : public OpenDDSharp::OpenDDS::DCPS::DataReaderListener {

		typedef void(__stdcall *onDataOnReadersDeclaration)(::DDS::Subscriber_ptr subscriber);

		internal:
			::OpenDDSharp::DDS::SubscriberListenerNative* impl_entity;

		protected:
			onDataOnReadersDeclaration onDataOnReadersFunctionCpp;

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

		public:
			/// <summary>
			/// Creates a new instance of <see cref="SubscriberListener" />
			/// </summary>
			SubscriberListener();

		public:
			/// <summary>
			/// <para>Handles the <see cref="StatusKind::DataOnReadersStatus" /> communication status.</para>
			/// <para>The <see cref="StatusKind::DataOnReadersStatus" /> indicates that new data is available on some of the data
			/// readers associated with the subscriber. Applications receiving this status can call <see cref="Subscriber::GetDataReaders /> on
			/// the subscriber to get the set of data readers with data available.</para>
			/// </summary>
			virtual void OnDataOnReaders(OpenDDSharp::DDS::Subscriber^ subscriber) = 0;

		};
	};
};
