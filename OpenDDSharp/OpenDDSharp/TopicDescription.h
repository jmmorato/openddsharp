#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsTopicC.h>
#pragma managed

#include "DomainParticipant.h"

#include <vcclr.h>
#include <msclr/marshal.h>

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Default implementation of the <see cref="ITopicDescription"> interface.
		/// It is the base class for <see cref="ContentFilteredTopic" />, and <see cref="MultiTopic" />.
		/// <summary>
		public ref class TopicDescription : ITopicDescription {

		internal:
			::DDS::TopicDescription_ptr impl_entity;

		internal:
			TopicDescription(::DDS::TopicDescription_ptr topicDescription);

		public:
			/// <summary>
			/// Gets type name used to create the <see cref="ITopicDescription" />.
			/// </summary>
			property System::String^ TypeName {
				virtual System::String^ get();
			};

			/// <summary>
			/// Gets the name used to create the <see cref="ITopicDescription" />.
			/// </summary>
			property System::String^ Name {
				virtual System::String^ get();
			};

			/// <summary>
			/// Gets the <see cref="DomainParticipant" /> to which the <see cref="ITopicDescription" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::DomainParticipant^ Participant {
				virtual OpenDDSharp::DDS::DomainParticipant^ get();
			};

		private:
			System::String^ GetTypeName();
			System::String^ GetName();
			OpenDDSharp::DDS::DomainParticipant^ GetParticipant();

		public:
			/// <summary>
			/// Gets the native TopicDescription pointer.
			/// Internal use only.			
			/// </summary>
			virtual ::DDS::TopicDescription_ptr ToNative();
		};
	};
};