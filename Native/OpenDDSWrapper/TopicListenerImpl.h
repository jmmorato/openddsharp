#pragma once
/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato

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

#include "all.h"

#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DDS {

			class TopicListenerImpl : public virtual ::OpenDDS::DCPS::LocalObject< ::DDS::TopicListener> {
			private:
				std::function<void(::DDS::TopicDescription_ptr topic, ::DDS::InconsistentTopicStatus status)> _onInconsistentTopic;

			public:
				TopicListenerImpl(std::function<void(::DDS::TopicDescription_ptr topic, ::DDS::InconsistentTopicStatus status)> onInconsistentTopic);

			protected:
				virtual ~TopicListenerImpl();

			public:
				virtual void on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status);
			};

			typedef OpenDDSharp::OpenDDS::DDS::TopicListenerImpl* TopicListenerImpl_ptr;

		};
	};
};
