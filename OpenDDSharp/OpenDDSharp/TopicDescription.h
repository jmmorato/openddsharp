/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
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