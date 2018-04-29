#include "BudgetExceededStatus.h"

OpenDDSharp::OpenDDS::DCPS::BudgetExceededStatus::BudgetExceededStatus(::OpenDDS::DCPS::BudgetExceededStatus status) {
	total_count = status.total_count;
	total_count_change = status.total_count_change;
	last_instance_handle = status.last_instance_handle;
};

System::Int32 OpenDDSharp::OpenDDS::DCPS::BudgetExceededStatus::TotalCount::get() {
	return total_count;
};

System::Int32 OpenDDSharp::OpenDDS::DCPS::BudgetExceededStatus::TotalCountChange::get() {
	return total_count_change;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::OpenDDS::DCPS::BudgetExceededStatus::LastInstanceHandle::get() {
	return last_instance_handle;
};
