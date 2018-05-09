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
#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#pragma managed

namespace OpenDDSharp {
	namespace DDS {
		class TopicListenerNative : public virtual ::OpenDDS::DCPS::LocalObject<::DDS::TopicListener> {

		private:
			std::function<void(::DDS::Topic_ptr topic, ::DDS::InconsistentTopicStatus status)> _onInconsistentTopic;

		public:
			TopicListenerNative(std::function<void(::DDS::Topic_ptr topic, ::DDS::InconsistentTopicStatus status)> onInconsistentTopic);

			virtual ~TopicListenerNative(void);

			virtual void on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus & status);
		};
	};
};
