/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "PresentationQosPolicyAccessScopeKind.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// The Presentation QoS policy controls how changes to instances by publishers are presented to data readers. It affects the relative ordering of these changes and 
		/// the scope of this ordering. Additionally, this policy introduces the concept of coherent change sets.
		/// </summary>
		/// <remarks>
		/// This policy controls the ordering and scope of samples made available to the subscriber, but the subscriber application must use the proper logic in reading samples 
		/// to guarantee the requested behavior.
		/// </remarks>
		public ref class PresentationQosPolicy {

		private:
			::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind access_scope;
			::System::Boolean coherent_access;
			::System::Boolean ordered_access;

		public:
			/// <summary>
			/// Specifies how the samples representing changes to data instances are presented to a subscribing application.
			/// </summary>
			property ::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind AccessScope {
				::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind get();
				void set(::OpenDDSharp::DDS::PresentationQosPolicyAccessScopeKind value);
			}

			/// <summary>
			/// Allow one or more changes to an instance be made available to an associated data reader as a single change. If a data reader does not receive
			/// the entire set of coherent changes made by a publisher, then none of the changes are made available. The semantics of coherent changes are similar in nature 
			/// to those found in transactions provided by many relational databases. By default, CoherentAccess is <see langword="false" />.
			/// </summary>
			property System::Boolean CoherentAccess {
				System::Boolean get();
				void set(System::Boolean value);
			}

			/// <summary>
			/// Controls whether preserve the order of changes. By default, OrderedAccess is <see langword="false" />.
			/// </summary>
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
