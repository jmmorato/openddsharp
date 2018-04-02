#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

#include "ReturnCode.h"
#include "StatusMask.h"
#include "InstanceHandle.h"
#include "StatusCondition.h"

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {

		public ref class Entity {

		internal:
			::DDS::Entity_ptr impl_entity;
			ICollection<Entity^>^ contained_entities;

		public:
			Entity(::DDS::Entity_ptr entity);

			OpenDDSharp::DDS::ReturnCode Enable();

			OpenDDSharp::DDS::StatusCondition^ GetStatusCondition();

			OpenDDSharp::DDS::StatusMask GetStatusChanges();

			OpenDDSharp::DDS::InstanceHandle GetInstanceHandle();

		internal:
			ICollection<Entity^>^ GetContainedEntities();
		};

	};
};