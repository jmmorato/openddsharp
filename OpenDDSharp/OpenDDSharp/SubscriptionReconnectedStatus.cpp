#include "SubscriptionReconnectedStatus.h"

OpenDDSharp::OpenDDS::DCPS::SubscriptionReconnectedStatus::SubscriptionReconnectedStatus(::OpenDDS::DCPS::SubscriptionReconnectedStatus status) {
	List<OpenDDSharp::DDS::InstanceHandle>^ list = gcnew List<OpenDDSharp::DDS::InstanceHandle>();
	int length = status.publication_handles.length();
	int i = 0;
	while (i < length) {
		list->Add(status.publication_handles[i]);
		i++;
	}

	publication_handles = list;
};

IEnumerable<OpenDDSharp::DDS::InstanceHandle>^OpenDDSharp::OpenDDS::DCPS::SubscriptionReconnectedStatus::PublicationHandles::get() {
	return publication_handles;
};