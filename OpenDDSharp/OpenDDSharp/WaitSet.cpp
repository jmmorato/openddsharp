/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "WaitSet.h"

OpenDDSharp::DDS::WaitSet::WaitSet() : WaitSet(new ::DDS::WaitSet()) { };

OpenDDSharp::DDS::WaitSet::WaitSet(::DDS::WaitSet_ptr waitSet) {
	impl_entity = waitSet;
	conditions = gcnew List<Condition^>();
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::WaitSet::Wait(ICollection<Condition^>^ activeConditions) {
	Duration duration;
	duration.Seconds = duration.InfiniteSeconds;
	duration.NanoSeconds = duration.InfiniteNanoseconds;

	return OpenDDSharp::DDS::WaitSet::Wait(activeConditions, duration);
}

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::WaitSet::Wait(ICollection<Condition^>^ activeConditions, OpenDDSharp::DDS::Duration timeout) {
	if (activeConditions == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	activeConditions->Clear();

	::DDS::ConditionSeq seq;
	::DDS::ReturnCode_t ret = impl_entity->wait(seq, timeout.ToNative());

	if (ret == ::DDS::RETCODE_OK) {
		System::UInt32 i = 0;
		while (i < seq.length()) {			
			Condition^ cond = nullptr;
			for each (Condition^ c in conditions) {
				if (c->impl_entity == seq[i]) {
					cond = c;
					break;
				}
			}
			
			if (cond != nullptr) {
				activeConditions->Add(cond);
			}
			i++;
		}
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::WaitSet::AttachCondition(Condition^ cond) {
	if (cond == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	conditions->Add(cond);
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->attach_condition(cond->impl_entity);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::WaitSet::DetachCondition(Condition^ cond) {
	if (cond == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	conditions->Remove(cond);
	return (OpenDDSharp::DDS::ReturnCode)impl_entity->detach_condition(cond->impl_entity);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::WaitSet::GetConditions(ICollection<Condition^>^ attachedConditions) {
	if (attachedConditions == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}
	attachedConditions->Clear();

	::DDS::ConditionSeq seq;
	::DDS::ReturnCode_t ret = impl_entity->get_conditions(seq);

	if (ret == ::DDS::RETCODE_OK) {
		System::UInt32 i = 0;
		while (i < seq.length()) {			
			Condition^ cond = nullptr;
			for each (Condition^ c in conditions) {
				if (c->impl_entity == seq[i]) {
					cond = c;
					break;
				}
			}

			if (cond != nullptr) {
				attachedConditions->Add(cond);
			}
			i++;
		}
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
	
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::WaitSet::DetachConditions(ICollection<Condition^>^ conditions) {
	if (conditions == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::Ok;
	}

	::DDS::ConditionSeq seq;
	seq.length(conditions->Count);

	System::UInt32 i = 0;
	for each (Condition^ cond in conditions) {		
		seq[i] = cond->impl_entity;
		i++;
	}

	::DDS::ReturnCode_t ret = impl_entity->detach_conditions(seq);
	if (ret == ::DDS::RETCODE_OK) {
		for each (Condition^ cond in conditions) {
			conditions->Remove(cond);
		}
	}

	return (OpenDDSharp::DDS::ReturnCode)ret;
}