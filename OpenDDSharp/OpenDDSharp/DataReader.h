#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsSubscriptionC.h>
#pragma managed

#include "Entity.h"
#include "EntityManager.h"
#include "ReturnCode.h"
#include "DataReaderQos.h"
#include "StatusMask.h"
#include "SampleRejectedStatus.h"
#include "LivelinessChangedStatus.h"
#include "RequestedDeadlineMissedStatus.h"
#include "RequestedIncompatibleQosStatus.h"
#include "SubscriptionMatchedStatus.h"
#include "SampleLostStatus.h"
#include "SampleStateMask.h"
#include "ViewStateMask.h"
#include "InstanceStateMask.h"
#include "PublicationBuiltinTopicData.h"
#include "ITopicDescription.h"

#include <vcclr.h>
#include <msclr/marshal.h>

#pragma make_public(::DDS::DataReader)

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DCPS {
			ref class DataReaderListener;
		};
	};
};

namespace OpenDDSharp {
	namespace DDS {

		ref class Subscriber;
		ref class ReadCondition;
		ref class QueryCondition;

		public ref class DataReader : public OpenDDSharp::DDS::Entity {

		public:
			::DDS::DataReader_ptr impl_entity;
			OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ _listener;

		protected public:
			DataReader(::DDS::DataReader_ptr dataReader);
		
		public:
			OpenDDSharp::DDS::ReadCondition^ CreateReadCondition(SampleStateMask sampleStates, ViewStateMask viewStates, InstanceStateMask instanceStates);
			OpenDDSharp::DDS::ReturnCode DeleteReadcondition(OpenDDSharp::DDS::ReadCondition^ condition);
			OpenDDSharp::DDS::QueryCondition^ CreateQueryCondition(SampleStateMask sampleStates, ViewStateMask viewStates, InstanceStateMask instanceStates, System::String^ queryExpression, List<System::String^>^ queryParameters);
			OpenDDSharp::DDS::ReturnCode DeleteContainedEntities();
			OpenDDSharp::DDS::ReturnCode SetQos(OpenDDSharp::DDS::DataReaderQos^ qos);
			OpenDDSharp::DDS::ReturnCode GetQos(OpenDDSharp::DDS::DataReaderQos^ qos);
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener);
			OpenDDSharp::DDS::ReturnCode SetListener(OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener, OpenDDSharp::DDS::StatusMask mask);
			OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ GetListener();
			OpenDDSharp::DDS::ITopicDescription^ GetTopicDescription(System::String^ name);
			OpenDDSharp::DDS::Subscriber^ GetSubscriber();
			OpenDDSharp::DDS::ReturnCode GetSampleRejectedStatus(OpenDDSharp::DDS::SampleRejectedStatus% status);
			OpenDDSharp::DDS::ReturnCode GetLivelinessChangedStatus(OpenDDSharp::DDS::LivelinessChangedStatus% status);
			OpenDDSharp::DDS::ReturnCode GetRequestedDeadlineMissedStatus(OpenDDSharp::DDS::RequestedDeadlineMissedStatus% status);
			OpenDDSharp::DDS::ReturnCode GetRequestedIncompatibleQosStatus(OpenDDSharp::DDS::RequestedIncompatibleQosStatus% status);
			OpenDDSharp::DDS::ReturnCode GetSubscriptionMatchedStatus(OpenDDSharp::DDS::SubscriptionMatchedStatus% status);
			OpenDDSharp::DDS::ReturnCode GetSampleLostStatus(OpenDDSharp::DDS::SampleLostStatus% status);
			OpenDDSharp::DDS::ReturnCode WaitForHistoricalData(OpenDDSharp::DDS::Duration maxWait);
			OpenDDSharp::DDS::ReturnCode GetMatchedPublications(ICollection<OpenDDSharp::DDS::InstanceHandle>^ publicationHandles);
			OpenDDSharp::DDS::ReturnCode GetMatchedPublicationData(OpenDDSharp::DDS::PublicationBuiltinTopicData% publicationData, OpenDDSharp::DDS::InstanceHandle publicationHandle);
		};
	};
};