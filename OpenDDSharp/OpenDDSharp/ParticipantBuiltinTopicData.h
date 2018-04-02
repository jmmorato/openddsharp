#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "BuiltinTopicKey.h"
#include "UserDataQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {		
		public value struct ParticipantBuiltinTopicData {

		private:
			OpenDDSharp::DDS::BuiltinTopicKey key;
			OpenDDSharp::DDS::UserDataQosPolicy^ user_data;

		public:
			property OpenDDSharp::DDS::BuiltinTopicKey Key {
				OpenDDSharp::DDS::BuiltinTopicKey get();
			};

			property OpenDDSharp::DDS::UserDataQosPolicy^ UserData {
				OpenDDSharp::DDS::UserDataQosPolicy^ get();
			};

		internal:
			void FromNative(::DDS::ParticipantBuiltinTopicData native);
		};
	};
};