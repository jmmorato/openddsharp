#pragma once

#pragma unmanaged 
#include <dds\DdsDcpsCoreC.h>
#pragma managed

#include "BuiltinTopicKey.h"
#include "UserDataQosPolicy.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DomainParticipant;

		/// <summary>
		/// Class that contains information about available <see cref="DomainParticipant" />s within the system.
		/// </summary>
		/// <remarks>
		/// The DCPSParticipant topic communicates the existence of <see cref="DomainParticipant" />s by means of the ParticipantBuiltinTopicData datatype. 
		/// Each ParticipantBuiltinTopicData sample in a Domain represents a <see cref="DomainParticipant" /> that participates in that Domain: a new ParticipantBuiltinTopicData instance 
		/// is created when a newly-added <see cref="DomainParticipant" /> is enabled, and it is disposed when that <see cref="DomainParticipant" /> is deleted. 
		/// An updated ParticipantBuiltinTopicData sample is written each time the <see cref="DomainParticipant" /> modifies its <see cref="UserDataQosPolicy" />.
		/// </remarks>
		public value struct ParticipantBuiltinTopicData {

		private:
			OpenDDSharp::DDS::BuiltinTopicKey key;
			OpenDDSharp::DDS::UserDataQosPolicy^ user_data;

		public:
			/// <summary>
			/// Gets the globally unique identifier of the <see cref="DomainParticipant" />.
			/// </summary>
			property OpenDDSharp::DDS::BuiltinTopicKey Key {
				OpenDDSharp::DDS::BuiltinTopicKey get();
			};

			/// <summary>
			/// Gets the <see cref="UserDataQosPolicy" /> attached to the <see cref="DomainParticipant" />.
			/// </summary>
			property OpenDDSharp::DDS::UserDataQosPolicy^ UserData {
				OpenDDSharp::DDS::UserDataQosPolicy^ get();
			};

		internal:
			void FromNative(::DDS::ParticipantBuiltinTopicData native);
		};
	};
};