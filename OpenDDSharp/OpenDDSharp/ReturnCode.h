#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		public enum class ReturnCode : System::Int32 {
			Ok = ::DDS::RETCODE_OK,
			Error = ::DDS::RETCODE_ERROR,
			Unsupported = ::DDS::RETCODE_UNSUPPORTED,
			BadParameter = ::DDS::RETCODE_BAD_PARAMETER,
			PreconditionNotMet = ::DDS::RETCODE_PRECONDITION_NOT_MET,
			OutOfResources = ::DDS::RETCODE_OUT_OF_RESOURCES,
			NotEnabled = ::DDS::RETCODE_NOT_ENABLED,
			ImmutablePolicy = ::DDS::RETCODE_IMMUTABLE_POLICY,
			InconsistentPolicy = ::DDS::RETCODE_INCONSISTENT_POLICY,
			AlreadyDeleted = ::DDS::RETCODE_ALREADY_DELETED,
			Timeout = ::DDS::RETCODE_TIMEOUT,
			NoData = ::DDS::RETCODE_NO_DATA,
			IllegalOperation = ::DDS::RETCODE_ILLEGAL_OPERATION
		};
	};
};