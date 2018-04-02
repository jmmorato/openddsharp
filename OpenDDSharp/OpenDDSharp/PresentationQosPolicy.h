#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "PresentationQosPolicyAccessScopeKind.h"

namespace OpenDDSharp {
	namespace DDS {

		public ref class PresentationQosPolicy {

		private:
			::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind access_scope;
			::System::Boolean coherent_access;
			::System::Boolean ordered_access;

		public:
			property ::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind AccessScope {
				::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind get();
				void set(::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind value);
			}

			property System::Boolean CoherentAccess {
				System::Boolean get();
				void set(System::Boolean value);
			}

			property System::Boolean OrderedAccess {
				System::Boolean get();
				void set(System::Boolean value);
			}

		internal:
			PresentationQosPolicy();						

		internal:
			::DDS::PresentationQosPolicy ToNative();
			void FromNative(::DDS::PresentationQosPolicy qos);
		};
	};
};
