#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsInfrastructureC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		
		ref class Entity;

		/// <summary>
		/// Represents the various return code values that DDS operations return.
		/// </summary>
		public enum class ReturnCode : System::Int32 {
			/// <summary>
			/// Successful return.
			/// </summary>
			Ok = ::DDS::RETCODE_OK,

			/// <summary>
			/// Generic, unspecified error
			/// </summary>
			Error = ::DDS::RETCODE_ERROR,

			/// <summary>
			/// Unsupported operation or QoS policy setting. Can only be returned by operations that are optional or operations that uses an optional &lt;Entity&gt;QoS as a parameter.
			/// </summary>
			Unsupported = ::DDS::RETCODE_UNSUPPORTED,

			/// <summary>
			/// Illegal parameter value.
			/// </summary>
			BadParameter = ::DDS::RETCODE_BAD_PARAMETER,

			/// <summary>
			/// A pre-condition for the operation was not met.
			/// </summary>
			PreconditionNotMet = ::DDS::RETCODE_PRECONDITION_NOT_MET,

			/// <summary>
			/// Service ran out of the resources needed to complete the operation.
			/// </summary>
			OutOfResources = ::DDS::RETCODE_OUT_OF_RESOURCES,

			/// <summary>
			/// Operation invoked on an <see cref="Entity" /> that is not yet enabled.
			/// </summary>
			NotEnabled = ::DDS::RETCODE_NOT_ENABLED,

			/// <summary>
			/// Application attempted to modify an immutable QoS policy.
			/// </summary>
			ImmutablePolicy = ::DDS::RETCODE_IMMUTABLE_POLICY,

			/// <summary>
			/// Application specified a set of policies that are not consistent with each other.
			/// </summary>
			InconsistentPolicy = ::DDS::RETCODE_INCONSISTENT_POLICY,

			/// <summary>
			/// The object target of this operation has already been deleted.
			/// </summary>
			AlreadyDeleted = ::DDS::RETCODE_ALREADY_DELETED,

			/// <summary>
			/// The operation timed out.
			/// </summary>
			Timeout = ::DDS::RETCODE_TIMEOUT,

			/// <summary>
			/// Indicates a situation where the operation did not return any data.
			/// </summary>
			NoData = ::DDS::RETCODE_NO_DATA,

			/// <summary>
			/// An operation was invoked on an inappropriate object or at an inappropriate time (as determined by QoS policies that control the behaviour of the object in question). 
			/// There is no precondition that could be changed to make the operation succeed. 
			/// </summary>
			IllegalOperation = ::DDS::RETCODE_ILLEGAL_OPERATION
		};
	};
};