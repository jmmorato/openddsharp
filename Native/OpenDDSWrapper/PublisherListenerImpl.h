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

			class PublisherListenerImpl : public virtual ::OpenDDS::DCPS::LocalObject< ::DDS::PublisherListener> {
			private:
				std::function<void(::DDS::Entity_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> _onOfferedDeadlineMissed;
				std::function<void(::DDS::Entity_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> _onOfferedIncompatibleQos;
				std::function<void(::DDS::Entity_ptr writer, ::DDS::LivelinessLostStatus status)> _onLivelinessLost;
				std::function<void(::DDS::Entity_ptr writer, ::DDS::PublicationMatchedStatus status)> _onPublicationMatched;

			public:
				PublisherListenerImpl(std::function<void(::DDS::Entity_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> onOfferedDeadlineMissed,
									  std::function<void(::DDS::Entity_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> onOfferedIncompatibleQos,
									  std::function<void(::DDS::Entity_ptr writer, ::DDS::LivelinessLostStatus status)> onLivelinessLost,
									  std::function<void(::DDS::Entity_ptr writer, ::DDS::PublicationMatchedStatus status)> onPublicationMatched);

			protected:
				virtual ~PublisherListenerImpl();

			public:
				virtual void on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status);

				virtual void on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus& status);

				virtual void on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus& status);

				virtual void on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus& status);
			};

			typedef OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl* PublisherListenerImpl_ptr;

		};
	};
};
