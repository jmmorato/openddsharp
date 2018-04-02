#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionC.h>
#include <dds/DCPS/Marked_Default_Qos.h>
#pragma managed

#include "Entity.h"
#include "DataReader.h"
#include "DataReaderQos.h"
#include "DataReaderListener.h"
#include "DataReaderListenerNative.h"
#include "StatusMask.h"
#include "StatusKind.h"
#include "Topic.h"
#include "SubscriberQos.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		ref class SubscriberListener;
		ref class DomainParticipant;

		public ref class Subscriber : public OpenDDSharp::DDS::Entity {

		internal:
			::DDS::Subscriber_ptr impl_entity;
			OpenDDSharp::DDS::SubscriberListener^ m_listener;

		internal:
			Subscriber(::DDS::Subscriber_ptr subscriber);

		public:
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topic);
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topic, ::OpenDDSharp::DDS::DataReaderQos^ qos);
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topic, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener);
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topic, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener, OpenDDSharp::DDS::StatusMask statusMask);
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topic, ::OpenDDSharp::DDS::DataReaderQos^ qos, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener);
			OpenDDSharp::DDS::DataReader^ CreateDataReader(OpenDDSharp::DDS::ITopicDescription^ topic, ::OpenDDSharp::DDS::DataReaderQos^ qos, ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener, OpenDDSharp::DDS::StatusMask statusMask);
			OpenDDSharp::DDS::ReturnCode DeleteDataReader(OpenDDSharp::DDS::DataReader^ datareader);
			OpenDDSharp::DDS::ReturnCode DeleteContainedEntities();			
			OpenDDSharp::DDS::DataReader^ LookupDataReader(System::String^ topicName);
			OpenDDSharp::DDS::ReturnCode GetDatareaders(ICollection<OpenDDSharp::DDS::DataReader^>^ readers, OpenDDSharp::DDS::SampleStateMask sampleStates, OpenDDSharp::DDS::ViewStateMask viewStates, OpenDDSharp::DDS::InstanceStateMask instanceStates);
			OpenDDSharp::DDS::ReturnCode NotifyDataReaders();
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::SubscriberQos^ qos);
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::SubscriberQos^ qos);
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::SubscriberListener^ listener);
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::SubscriberListener^ listener, OpenDDSharp::DDS::StatusMask mask);
			OpenDDSharp::DDS::SubscriberListener^ GetListener();
			OpenDDSharp::DDS::ReturnCode BeginAccess();
			OpenDDSharp::DDS::ReturnCode EndAccess();
			OpenDDSharp::DDS::DomainParticipant^ GetParticipant();
			OpenDDSharp::DDS::ReturnCode SetDefaultDataReaderQos(OpenDDSharp::DDS::DataReaderQos^ qos);
			OpenDDSharp::DDS::ReturnCode GetDefaultDataReaderQos(OpenDDSharp::DDS::DataReaderQos^ qos);
		};

	};
};