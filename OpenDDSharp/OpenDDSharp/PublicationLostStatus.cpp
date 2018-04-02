#include "PublicationLostStatus.h"

OpenDDSharp::OpenDDS::DCPS::PublicationLostStatus::PublicationLostStatus(::OpenDDS::DCPS::PublicationLostStatus status) {
	List<OpenDDSharp::DDS::InstanceHandle>^ list = gcnew List<OpenDDSharp::DDS::InstanceHandle>();
	int length = status.subscription_handles.length();
	int i = 0;
	while (i < length) {
		list->Add(status.subscription_handles[i]);
		i++;
	}

	subscription_handles = list;
};

IEnumerable<OpenDDSharp::DDS::InstanceHandle>^OpenDDSharp::OpenDDS::DCPS::PublicationLostStatus::SubscriptionHandles::get() {
	return subscription_handles;
};