#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsTopicC.h>
#pragma managed

#include "Entity.h"
#include "ReturnCode.h"
#include "TopicQos.h"
#include "StatusMask.h"
#include "InconsistentTopicStatus.h"
#include "ITopicDescription.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		ref class TopicListener;
		ref class DomainParticipant;

		public ref class Topic : public OpenDDSharp::DDS::Entity, public ITopicDescription {

		internal:
			::DDS::Topic_ptr impl_entity;
			OpenDDSharp::DDS::TopicListener^ _listener;

		internal:
			Topic(::DDS::Topic_ptr topic);

		public:
			virtual System::String^ GetTypeName();
			virtual System::String^ GetName();
			virtual OpenDDSharp::DDS::DomainParticipant^ GetParticipant();
			virtual ::DDS::TopicDescription_ptr ToNative();
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::TopicQos^ qos);
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::TopicQos^ qos);
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::TopicListener^ listener);
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::DDS::TopicListener^ listener, StatusMask mask);
			OpenDDSharp::DDS::TopicListener^ GetListener();
			OpenDDSharp::DDS::ReturnCode GetInconsistentTopicStatus(OpenDDSharp::DDS::InconsistentTopicStatus% status);
			
		};

	};
};