#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include <vcclr.h>
#include <msclr/marshal.h>

#using <System.Core.dll>
#using <System.Linq.dll>

using namespace System::Linq;
using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {

		ref class DataReader;
		ref class DataWriter;
		ref class Topic;

		/// <summary>
		/// This policy allows the introduction of a logical partition concept inside the 'physical' partition induced by a domain.
		/// </summary>
		/// <remarks>
		/// <para>For a <see cref="DataReader" /> to see the changes made to an instance by a <see cref="DataWriter" />, not only the <see cref="Topic" /> must match, 
		/// but also they must share a common partition. Each string in the list that defines this QoS policy defines a partition name. A partition name may
		/// contain wildcards. Sharing a common partition means that one of the partition names matches.</para>
		/// <para>Failure to match partitions is not considered an "incompatible" QoS and does not trigger any listeners nor conditions.</para>
		/// <para>This policy is changeable. A change of this policy can potentially modify the "match" of existing <see cref="DataReader" /> and <see cref="DataWriter" />
		/// entities. It may establish new "matchs" that did not exist before, or break existing matchs.</para>
		/// </remarks>
		public ref class PartitionQosPolicy {

		private:
			IEnumerable<System::String^>^ name;

		public:
			/// <summary>
			/// Gets or sets the sequence of partition strings. The name defaults to an empty sequence of strings.
			/// </summary>
			/// <remarks>
			/// The default partition name is an empty string and causes the entity to participate in the default partition.
			/// </remarks>
			property IEnumerable<System::String^>^ Name {
				IEnumerable<System::String^>^ get();
				void set(IEnumerable<System::String^>^ value);
			};

		internal:
			PartitionQosPolicy();						

		internal:
			::DDS::PartitionQosPolicy ToNative();
			void FromNative(::DDS::PartitionQosPolicy qos);

		};
	};
};
