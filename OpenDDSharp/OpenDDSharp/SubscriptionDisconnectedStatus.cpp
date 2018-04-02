#include "SubscriptionDisconnectedStatus.h"

OpenDDSharp::OpenDDS::DCPS::SubscriptionDisconnectedStatus::SubscriptionDisconnectedStatus(::OpenDDS::DCPS::SubscriptionDisconnectedStatus status) {
	List<System::Int32>^ list = gcnew List<System::Int32>();
	int length = status.publication_handles.length();
	int i = 0;
	while (i < length) {
		list->Add(status.publication_handles[i]);
		i++;
	}

	publication_handles = list;
};

IEnumerable<System::Int32>^OpenDDSharp::OpenDDS::DCPS::SubscriptionDisconnectedStatus::PublicationHandles::get() {
	return publication_handles;
};