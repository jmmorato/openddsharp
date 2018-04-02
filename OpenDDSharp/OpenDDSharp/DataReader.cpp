#include "DataReader.h"
#include "DataReaderListener.h"
#include "Subscriber.h"
#include "ReadCondition.h"
#include "QueryCondition.h"
#include "TopicDescription.h"

OpenDDSharp::DDS::DataReader::DataReader(::DDS::DataReader_ptr dataReader) : OpenDDSharp::DDS::Entity(dataReader) {
	impl_entity = dataReader;
}

OpenDDSharp::DDS::ReadCondition^ OpenDDSharp::DDS::DataReader::CreateReadCondition(OpenDDSharp::DDS::SampleStateMask sampleStates, OpenDDSharp::DDS::ViewStateMask viewStates, OpenDDSharp::DDS::InstanceStateMask instanceStates) {
	::DDS::ReadCondition_ptr native =  impl_entity->create_readcondition(sampleStates, viewStates, instanceStates);
	if (native != NULL) {
		return gcnew OpenDDSharp::DDS::ReadCondition(native, this);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::QueryCondition^ OpenDDSharp::DDS::DataReader::CreateQueryCondition(SampleStateMask sampleStates, ViewStateMask viewStates, InstanceStateMask instanceStates, System::String^ queryExpression, List<System::String^>^ queryParameters) {
	msclr::interop::marshal_context context;

	int count = 0;
	if (queryParameters != nullptr) {
		count = queryParameters->Count;
	}

	::DDS::StringSeq seq;	
	seq.length(count);

	System::Int32 i = 0;
	while (i < count)
	{
		seq[i] = context.marshal_as<const char*>(queryParameters[i]);
		i++;
	}

	::DDS::QueryCondition_ptr native = impl_entity->create_querycondition(sampleStates, viewStates, instanceStates, context.marshal_as<const char*>(queryExpression), seq);
	if (native != NULL) {
		return gcnew OpenDDSharp::DDS::QueryCondition(native, this);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::DeleteReadcondition(OpenDDSharp::DDS::ReadCondition^ condition) {
	if (condition == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::Ok;
	}

	return (OpenDDSharp::DDS::ReturnCode)impl_entity->delete_readcondition(condition->impl_entity);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::DeleteContainedEntities() {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->delete_contained_entities();
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::SetQos(OpenDDSharp::DDS::DataReaderQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	return (::OpenDDSharp::DDS::ReturnCode)impl_entity->set_qos(qos->ToNative());
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::GetQos(OpenDDSharp::DDS::DataReaderQos^ qos) {
	if (qos == nullptr) {
		return ::OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	::DDS::DataReaderQos nativeQos;
	::DDS::ReturnCode_t ret = impl_entity->get_qos(nativeQos);

	if (ret == ::DDS::RETCODE_OK) {
		qos->FromNative(nativeQos);
	}

	return (::OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::SetListener(OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener) {
	return OpenDDSharp::DDS::DataReader::SetListener(listener, StatusMask::DefaultStatusMask);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::SetListener(OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ listener, OpenDDSharp::DDS::StatusMask mask) {
	_listener = listener;

	if (_listener != nullptr) {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(listener->impl_entity, (System::UInt32)mask);
	}
	else {
		return (OpenDDSharp::DDS::ReturnCode)impl_entity->set_listener(NULL, (System::UInt32)mask);
	}
}

OpenDDSharp::OpenDDS::DCPS::DataReaderListener^ OpenDDSharp::DDS::DataReader::GetListener() {
	return _listener;
}

OpenDDSharp::DDS::ITopicDescription^ OpenDDSharp::DDS::DataReader::GetTopicDescription(System::String^ name) {		
	::DDS::TopicDescription_ptr desc = impl_entity->get_topicdescription();
	if (desc != NULL) {
		return gcnew OpenDDSharp::DDS::TopicDescription(desc);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::Subscriber^ OpenDDSharp::DDS::DataReader::GetSubscriber() {
	::DDS::Subscriber_ptr subscriber = impl_entity->get_subscriber();

	OpenDDSharp::DDS::Entity^ entity = EntityManager::get_instance().find(subscriber);
	if (entity != nullptr) {
		return static_cast<OpenDDSharp::DDS::Subscriber^>(entity);
	}
	else {
		return nullptr;
	}
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::GetSampleRejectedStatus(OpenDDSharp::DDS::SampleRejectedStatus status) {
	::DDS::SampleRejectedStatus s;
	::DDS::ReturnCode_t ret = impl_entity->get_sample_rejected_status(s);
	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(s);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::GetLivelinessChangedStatus(OpenDDSharp::DDS::LivelinessChangedStatus status) {
	::DDS::LivelinessChangedStatus s;
	::DDS::ReturnCode_t ret = impl_entity->get_liveliness_changed_status(s);
	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(s);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::GetRequestedDeadlineMissedStatus(OpenDDSharp::DDS::RequestedDeadlineMissedStatus status) {
	::DDS::RequestedDeadlineMissedStatus s;
	::DDS::ReturnCode_t ret = impl_entity->get_requested_deadline_missed_status(s);
	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(s);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::GetRequestedIncompatibleQosStatus(OpenDDSharp::DDS::RequestedIncompatibleQosStatus status) {
	::DDS::RequestedIncompatibleQosStatus s;
	::DDS::ReturnCode_t ret = impl_entity->get_requested_incompatible_qos_status(s);
	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(s);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::GetSubscriptionMatchedStatus(OpenDDSharp::DDS::SubscriptionMatchedStatus status) {
	::DDS::SubscriptionMatchedStatus s;
	::DDS::ReturnCode_t ret = impl_entity->get_subscription_matched_status(s);
	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(s);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::GetSampleLostStatus(OpenDDSharp::DDS::SampleLostStatus status) {
	::DDS::SampleLostStatus s;
	::DDS::ReturnCode_t ret = impl_entity->get_sample_lost_status(s);
	if (ret == ::DDS::RETCODE_OK) {
		status.FromNative(s);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::WaitForHistoricalData(OpenDDSharp::DDS::Duration maxWait) {
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->wait_for_historical_data(maxWait.ToNative());
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::GetMatchedPublications(ICollection<OpenDDSharp::DDS::InstanceHandle>^ publicationHandles) {
	if (publicationHandles == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	publicationHandles->Clear();

	::DDS::InstanceHandleSeq seq;
	::DDS::ReturnCode_t ret = impl_entity->get_matched_publications(seq);	

	if (ret == ::DDS::RETCODE_OK) {
		System::UInt32 i = 0;
		while (i < seq.length()) {
			publicationHandles->Add(seq[i]);
		}
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::DataReader::GetMatchedPublicationData(OpenDDSharp::DDS::PublicationBuiltinTopicData publicationData, OpenDDSharp::DDS::InstanceHandle publicationHandle) {
	::DDS::PublicationBuiltinTopicData data;
	::DDS::ReturnCode_t ret = impl_entity->get_matched_publication_data(data, publicationHandle);

	if (ret == ::DDS::RETCODE_OK) {
		publicationData.FromNative(data);
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}