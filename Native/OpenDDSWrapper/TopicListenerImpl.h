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

#include <dds/DCPS/LocalObject.h>
#include <dds/DCPS/Service_Participant.h>
#include "ListenerDelegates.h"

namespace OpenDDSharp {
	namespace OpenDDS {
		namespace DDS {

			class TopicListenerImpl : public virtual ::OpenDDS::DCPS::LocalObject< ::DDS::TopicListener> {
			private:
                ACE_Thread_Mutex _lock;
                bool _disposed = false;

                void* _onInconsistentTopic;

			public:
				TopicListenerImpl(void* onInconsistentTopic);

			protected:
				virtual ~TopicListenerImpl();

			public:
				virtual void on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status);

        void dispose();
			};

			typedef OpenDDSharp::OpenDDS::DDS::TopicListenerImpl* TopicListenerImpl_ptr;

		};
	};
};
