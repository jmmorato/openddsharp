#pragma once

#pragma unmanaged 
#include <dds/DdsDcpsTopicC.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		
		ref class Topic;
		ref class ContentFilteredTopic;
		ref class MultiTopic;
		ref class DomainParticipant;

		/// <summary>
		/// ITopicDescription represents the fact that both publications and subscriptions are tied to a single data-type. 
		/// The interface is implemented on <see cref="Topic" />, <see cref="ContentFilteredTopic" />, and <see cref="MultiTopic" />. 
		/// </summary>
		/// <remarks>
		/// Its property TypeName defines a unique resulting type for the publication or the subscription and therefore creates an implicit association
		///	with a TypeSupport. ITopicDescription has also a Name property that allows it to be retrieved locally.
		/// </remarks>
		public interface class ITopicDescription {

			/// <summary>
			/// Gets type name used to create the <see cref="ITopicDescription" />.
			/// </summary>
			property System::String^ TypeName {
				System::String^ get();
			};

			/// <summary>
			/// Gets the name used to create the <see cref="ITopicDescription" />.
			/// </summary>
			property System::String^ Name {
				System::String^ get();
			};

			/// <summary>
			/// Gets the <see cref="DomainParticipant" /> to which the <see cref="ITopicDescription" /> belongs.
			/// </summary>
			property OpenDDSharp::DDS::DomainParticipant^ Participant {
				OpenDDSharp::DDS::DomainParticipant^ get();
			};

			/// <summary>
			/// Gets the native TopicDescription pointer.
			/// Internal use only.			
			/// </summary>
			::DDS::TopicDescription_ptr ToNative();
		};
	};
};