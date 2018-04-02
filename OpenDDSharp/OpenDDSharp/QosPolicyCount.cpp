#include "QosPolicyCount.h"

OpenDDSharp::DDS::QosPolicyCount::QosPolicyCount(::DDS::QosPolicyCount native) {
	policy_id = native.policy_id;
	count = native.count;
};

System::Int32 OpenDDSharp::DDS::QosPolicyCount::PolicyId::get() {
	return policy_id;
};

System::Int32 OpenDDSharp::DDS::QosPolicyCount::Count::get() {
	return count;
};

::DDS::QosPolicyCount OpenDDSharp::DDS::QosPolicyCount::ToNative() {
	::DDS::QosPolicyCount ret;

	ret.count = count;
	ret.policy_id = policy_id;	

	return ret;
};

void OpenDDSharp::DDS::QosPolicyCount::FromNative(::DDS::QosPolicyCount native) {
	count = native.count;
	policy_id = native.policy_id;
};