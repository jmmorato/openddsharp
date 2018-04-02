#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "Condition.h"
#include "StatusMask.h"
#include "ReturnCode.h"

namespace OpenDDSharp {
	namespace DDS {
		ref class Entity;

		public ref class StatusCondition : public Condition {

		internal:
			::DDS::StatusCondition_ptr impl_entity;
			OpenDDSharp::DDS::Entity^ m_entity;

		internal:
			StatusCondition(::DDS::StatusCondition_ptr status_condition, OpenDDSharp::DDS::Entity^ entity);

		public:
			OpenDDSharp::DDS::StatusMask GetEnabledStatuses();
			OpenDDSharp::DDS::ReturnCode SetEnabledStatuses(OpenDDSharp::DDS::StatusMask mask);
			OpenDDSharp::DDS::Entity^ GetEntity();
		};
	};
};
