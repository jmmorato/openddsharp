#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsPublicationC.h>
#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#pragma managed

#include "LNK4248.h"
//#include "DataWriter.h"

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
				std::function<void(::DDS::DataWriter_ptr)> _onConnectionDeleted;

			public:
				DataWriterListenerNative(std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedDeadlineMissedStatus status)> onOfferedDeadlineMissed,
									     std::function<void(::DDS::DataWriter_ptr writer, ::DDS::OfferedIncompatibleQosStatus status)> onOfferedIncompatibleQos,
									     std::function<void(::DDS::DataWriter_ptr writer, ::DDS::LivelinessLostStatus status)> onLivelinessLost,
									     std::function<void(::DDS::DataWriter_ptr writer, ::DDS::PublicationMatchedStatus status)> onPublicationMatched,
									     std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationDisconnectedStatus)> onPublicationDisconnected,
									     std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationReconnectedStatus)> onPublicationReconnected,
									     std::function<void(::DDS::DataWriter_ptr, ::OpenDDS::DCPS::PublicationLostStatus)> onPublicationLost,
									     std::function<void(::DDS::DataWriter_ptr)> onConnectionDeleted);

				virtual ~DataWriterListenerNative(void);

				virtual void on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus & status);

				virtual void on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus & status);

				virtual void on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus & status);

				virtual void on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus & status);

				virtual void on_publication_disconnected(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationDisconnectedStatus & status);

				virtual void on_publication_reconnected(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationReconnectedStatus & status);

				virtual void on_publication_lost(::DDS::DataWriter_ptr reader, const ::OpenDDS::DCPS::PublicationLostStatus & status);

				virtual void on_connection_deleted(::DDS::DataWriter_ptr);
			};
		};		
	};
};
