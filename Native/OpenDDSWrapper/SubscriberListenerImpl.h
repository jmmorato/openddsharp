#pragma once
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

#include <dds/DdsDcpsDomainC.h>
#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#include "ListenerDelegates.h"

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DDS {

			class SubscriberListenerImpl : public virtual ::OpenDDS::DCPS::LocalObject< ::DDS::SubscriberListener> {
			private:
          ACE_Thread_Mutex _lock;
          bool _disposed = false;

          onDataOnReadersDeclaration* _onDataOnReaders;
          onDataAvailableDeclaration*  _onDataAvailable;
          onRequestedDeadlineMissedDeclaration* _onRequestedDeadlineMissed;
          onRequestedIncompatibleQosDeclaration* _onRequestedIncompatibleQos;
          onSampleRejectedDeclaration* _onSampleRejected;
          onLivelinessChangedDeclaration* _onLivelinessChanged;
          onSubscriptionMatchedDeclaration* _onSubscriptionMatched;
          onSampleLostDeclaration* _onSampleLost;

			public:
				SubscriberListenerImpl(onDataOnReadersDeclaration* onDataOnReaders,
                               onDataAvailableDeclaration* onDataAvailable,
                               onRequestedDeadlineMissedDeclaration* onRequestedDeadlineMissed,
                               onRequestedIncompatibleQosDeclaration* onRequestedIncompatibleQos,
                               onSampleRejectedDeclaration* onSampleRejected,
                               onLivelinessChangedDeclaration* onLivelinessChanged,
                               onSubscriptionMatchedDeclaration* onSubscriptionMatched,
                               onSampleLostDeclaration* onSampleLost);

			protected:
				virtual ~SubscriberListenerImpl();

			public:
				virtual void on_data_on_readers(::DDS::Subscriber_ptr subscriber);

				virtual void on_data_available(::DDS::DataReader_ptr reader);

				virtual void on_requested_deadline_missed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status);

				virtual void on_requested_incompatible_qos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status);

				virtual void on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status);

				virtual void on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status);

				virtual void on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status);

				virtual void on_sample_lost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status);

        void dispose();
			};

			typedef OpenDDSharp::OpenDDS::DDS::SubscriberListenerImpl* SubscriberListenerImpl_ptr;

		};
	};
};
