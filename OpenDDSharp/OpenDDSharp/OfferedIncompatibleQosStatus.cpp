#include "OfferedIncompatibleQosStatus.h"

OpenDDSharp::DDS::OfferedIncompatibleQosStatus::OfferedIncompatibleQosStatus(::DDS::OfferedIncompatibleQosStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
	last_policy_id = status.last_policy_id;

	List<QosPolicyCount^>^ list = gcnew List<QosPolicyCount^>();
	int length = status.policies.length();
	int i = 0;
	while (i < length) {
		list->Add(gcnew QosPolicyCount(status.policies[i]));
		i++;
	}

	policies = list;
};

System::Int32 OpenDDSharp::DDS::OfferedIncompatibleQosStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::DDS::OfferedIncompatibleQosStatus::TotalCountChange::get() {
	return total_count_change;
};

System::Int32 OpenDDSharp::DDS::OfferedIncompatibleQosStatus::LastPolicyId::get() {
	return last_policy_id;
};

IEnumerable<OpenDDSharp::DDS::QosPolicyCount^>^ OpenDDSharp::DDS::OfferedIncompatibleQosStatus::Policies::get() {
	return policies;
};

::DDS::OfferedIncompatibleQosStatus OpenDDSharp::DDS::OfferedIncompatibleQosStatus::ToNative() {
	::DDS::OfferedIncompatibleQosStatus ret;

	ret.total_count = total_count;
	ret.total_count_change = total_count_change;
	ret.last_policy_id = last_policy_id;

	::DDS::QosPolicyCountSeq seq;
	if (policies != nullptr) {
		int count = System::Linq::Enumerable::Count(policies);
		seq.length(count);

		int i = 0;
		while (i < count) {
			QosPolicyCount^ policy = System::Linq::Enumerable::ElementAt(policies, i);
			seq[i] = policy->ToNative();
			i++;
		}
	}
	else {
		seq.length(0);
	}

	ret.policies = seq;

	return ret;
}

void OpenDDSharp::DDS::OfferedIncompatibleQosStatus::FromNative(::DDS::OfferedIncompatibleQosStatus native) {
	total_count = native.total_count;
	total_count_change = native.total_count_change;
	last_policy_id = native.last_policy_id;

	List<QosPolicyCount^>^ list = gcnew List<QosPolicyCount^>();
	int length = native.policies.length();
	int i = 0;
	while (i < length) {
		list->Add(gcnew QosPolicyCount(native.policies[i]));
		i++;
	}

	policies = list;
}
