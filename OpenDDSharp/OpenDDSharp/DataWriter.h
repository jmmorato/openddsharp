#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsPublicationC.h>
#pragma managed

#include "Entity.h"
#include "EntityManager.h"
#include "ReturnCode.h"
#include "DataWriterQos.h"
#include "LivelinessLostStatus.h"
#include "OfferedDeadlineMissedStatus.h"
#include "OfferedIncompatibleQosStatus.h"
#include "PublicationMatchedStatus.h"
#include "Topic.h"
#include "SubscriptionBuiltinTopicData.h"

#pragma make_public(::DDS::DataWriter)

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {
			ref class DataWriterListener;
		};
	};
};

namespace OpenDDSharp {
	namespace DDS {

		ref class Publisher;

		public ref class DataWriter : public OpenDDSharp::DDS::Entity {

		public:
			::DDS::DataWriter_ptr impl_entity;

		internal:
			OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ _listener;

		protected public:
			DataWriter(::DDS::DataWriter_ptr dataWriter);

		public:
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::DataWriterQos^ qos);
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::DataWriterQos^ qos);
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener);
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener, OpenDDSharp::DDS::StatusMask mask);
			OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ GetListener();
			OpenDDSharp::DDS::Topic^ GetTopic();
			OpenDDSharp::DDS::Publisher^ GetPublisher();
			OpenDDSharp::DDS::ReturnCode WaitForAcknowledgments(OpenDDSharp::DDS::Duration maxWait);
			OpenDDSharp::DDS::ReturnCode GetLivelinessLostStatus(OpenDDSharp::DDS::LivelinessLostStatus status);
			OpenDDSharp::DDS::ReturnCode GetOfferedDeadlineMissedStatus(OpenDDSharp::DDS::OfferedDeadlineMissedStatus status);
			OpenDDSharp::DDS::ReturnCode GetOfferedIncompatibleQosStatus(OpenDDSharp::DDS::OfferedIncompatibleQosStatus status);
			OpenDDSharp::DDS::ReturnCode GetPublicationMatchedStatus(OpenDDSharp::DDS::PublicationMatchedStatus status);
			OpenDDSharp::DDS::ReturnCode AssertLiveliness();
			OpenDDSharp::DDS::ReturnCode GetMatchedSubscriptions(ICollection<OpenDDSharp::DDS::InstanceHandle>^ subscriptionHandles);
			OpenDDSharp::DDS::ReturnCode GetMatchedSubscriptionData(OpenDDSharp::DDS::SubscriptionBuiltinTopicData subscriptionData, OpenDDSharp::DDS::InstanceHandle subscriptionHandle);
		};

	};
};