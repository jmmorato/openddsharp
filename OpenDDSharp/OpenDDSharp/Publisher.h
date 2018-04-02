#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsPublicationC.h>
#include <dds/DCPS/Marked_Default_Qos.h>
#pragma managed

#include "Entity.h"
#include "Topic.h"
#include "DataWriter.h"
#include "StatusMask.h"
#include "DataWriterQos.h"
#include "DataWriterListener.h"
#include "PublisherQos.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		ref class PublisherListener;
		ref class DomainParticipant;

		public ref class Publisher : public OpenDDSharp::DDS::Entity {

		internal:
			::DDS::Publisher_ptr impl_entity;
			OpenDDSharp::DDS::PublisherListener^ m_listener;

		internal:
			Publisher(::DDS::Publisher_ptr publisher);			

		public:
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic);
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::DataWriterQos^ qos);
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener);
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener, StatusMask statusMask);
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::DataWriterQos^ qos, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener);
			OpenDDSharp::DDS::DataWriter^ CreateDataWriter(OpenDDSharp::DDS::Topic^ topic, OpenDDSharp::DDS::DataWriterQos^ qos, OpenDDSharp::OpenDDS::DCPS::DataWriterListener^ listener, StatusMask statusMask);
			OpenDDSharp::DDS::ReturnCode DeleteDataWriter(OpenDDSharp::DDS::DataWriter^ datawriter);
			OpenDDSharp::DDS::DataWriter^ LookupDataWriter(System::String^ topicName);
			OpenDDSharp::DDS::ReturnCode DeleteContainedEntities();
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::PublisherQos^ qos);
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::PublisherQos^ qos);
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::PublisherListener^ listener);
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::PublisherListener^ listener, OpenDDSharp::DDS::StatusMask mask);
			OpenDDSharp::DDS::PublisherListener^ GetListener();
			OpenDDSharp::DDS::ReturnCode SuspendPublications();
			OpenDDSharp::DDS::ReturnCode ResumePublications();
			OpenDDSharp::DDS::ReturnCode BeginCoherentChanges();
			OpenDDSharp::DDS::ReturnCode EndCoherentChanges();
			OpenDDSharp::DDS::ReturnCode WaitForAcknowledgments(OpenDDSharp::DDS::Duration maxWait);
			OpenDDSharp::DDS::DomainParticipant^ GetParticipant();
			OpenDDSharp::DDS::ReturnCode SetDefaultDataWriterQos(OpenDDSharp::DDS::DataWriterQos^ qos);
			OpenDDSharp::DDS::ReturnCode GetDefaultDataWriterQos(OpenDDSharp::DDS::DataWriterQos^ qos);		
			
		};

	};
};