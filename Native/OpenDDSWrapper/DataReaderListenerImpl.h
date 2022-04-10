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

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DDS {

			class DataReaderListenerImpl : public virtual ::OpenDDS::DCPS::LocalObject< ::DDS::DataReaderListener> {
			private:				
				std::function<void(::DDS::Entity_ptr)> _onDataAvalaible;
				std::function<void(::DDS::Entity_ptr, ::DDS::RequestedDeadlineMissedStatus)> _onRequestedDeadlineMissed;
				std::function<void(::DDS::Entity_ptr, ::DDS::RequestedIncompatibleQosStatus)> _onRequestedIncompatibleQos;
				std::function<void(::DDS::Entity_ptr, ::DDS::SampleRejectedStatus)> _onSampleRejected;
				std::function<void(::DDS::Entity_ptr, ::DDS::LivelinessChangedStatus)> _onLivelinessChanged;
				std::function<void(::DDS::Entity_ptr, ::DDS::SubscriptionMatchedStatus)> _onSubscriptionMatched;
				std::function<void(::DDS::Entity_ptr, ::DDS::SampleLostStatus)> _onSampleLost;

			public:
				DataReaderListenerImpl(std::function<void(::DDS::Entity_ptr)> onDataAvalaible,
									   std::function<void(::DDS::Entity_ptr, ::DDS::RequestedDeadlineMissedStatus)> onRequestedDeadlineMissed,
									   std::function<void(::DDS::Entity_ptr, ::DDS::RequestedIncompatibleQosStatus)> onRequestedIncompatibleQos,
									   std::function<void(::DDS::Entity_ptr, ::DDS::SampleRejectedStatus)> onSampleRejected,
									   std::function<void(::DDS::Entity_ptr, ::DDS::LivelinessChangedStatus)> onLivelinessChanged,
									   std::function<void(::DDS::Entity_ptr, ::DDS::SubscriptionMatchedStatus)> onSubscriptionMatched,
									   std::function<void(::DDS::Entity_ptr, ::DDS::SampleLostStatus)> onSampleLost);

			protected:
				virtual ~DataReaderListenerImpl();

			public:
				virtual void on_data_available(::DDS::DataReader_ptr reader);

				virtual void on_requested_deadline_missed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status);

				virtual void on_requested_incompatible_qos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status);

				virtual void on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status);

				virtual void on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status);

				virtual void on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status);

				virtual void on_sample_lost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status);
			};

			typedef OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl* DataReaderListenerImpl_ptr;

		};
	};
};
