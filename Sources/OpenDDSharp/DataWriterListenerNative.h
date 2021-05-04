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

#pragma unmanaged 
#include <dds/DdsDcpsPublicationC.h>
#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#pragma managed

#include "LNK4248.h"

// incomplete types generate LNK4248 warnings when compiled for .NET
SUPPRESS_LNK4248_CORBA

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {
			class DataWriterListenerNative : public virtual ::OpenDDS::DCPS::LocalObject<::OpenDDS::DCPS::DataWriterListener> {

			private:
				std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> _onOfferedDeadlineMissed;
				std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> _onOfferedIncompatibleQos;
				std::function<void(::DDS::DataWriter_ptr writer, ::DDS::LivelinessLostStatus status)> _onLivelinessLost;
				std::function<void(::DDS::DataWriter_ptr writer, ::DDS::PublicationMatchedStatus status)> _onPublicationMatched;
				std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationDisconnectedStatus)> _onPublicationDisconnected;
				std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationReconnectedStatus)> _onPublicationReconnected;
				std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationLostStatus)> _onPublicationLost;				

			public:
				DataWriterListenerNative(std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> onOfferedDeadlineMissed,
									     std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> onOfferedIncompatibleQos,
									     std::function<void(::DDS::DataWriter_ptr writer, ::DDS::LivelinessLostStatus status)> onLivelinessLost,
									     std::function<void(::DDS::DataWriter_ptr writer, ::DDS::PublicationMatchedStatus status)> onPublicationMatched,
									     std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationDisconnectedStatus)> onPublicationDisconnected,
									     std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationReconnectedStatus)> onPublicationReconnected,
									     std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationLostStatus)> onPublicationLost);
            protected:
				virtual ~DataWriterListenerNative();

            public:
				virtual void on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status);

				virtual void on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status);

				virtual void on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status);

				virtual void on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status);

				virtual void on_publication_disconnected(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationDisconnectedStatus & status);

				virtual void on_publication_reconnected(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationReconnectedStatus & status);

				virtual void on_publication_lost(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationLostStatus & status);
				
			};
		};		
	};
};
