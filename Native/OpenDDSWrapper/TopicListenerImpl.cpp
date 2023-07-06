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
#include <thread>
#include "TopicListenerImpl.h"

::OpenDDSharp::OpenDDS::DDS::TopicListenerImpl::TopicListenerImpl(void* onInconsistentTopic) {
	_onInconsistentTopic = onInconsistentTopic;
}

::OpenDDSharp::OpenDDS::DDS::TopicListenerImpl::~TopicListenerImpl() {
  dispose();
};

void ::OpenDDSharp::OpenDDS::DDS::TopicListenerImpl::dispose() {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  _onInconsistentTopic = NULL;

  _disposed = true;

  _lock.release();
}

void ::OpenDDSharp::OpenDDS::DDS::TopicListenerImpl::on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status) {
    _lock.acquire();

    if (_disposed) {
      _lock.release();
      return;
    }

    if (_onInconsistentTopic) {
        auto f = [](void* ptr, ::DDS::TopicDescription_ptr entity, const ::DDS::InconsistentTopicStatus& st)
        {
            reinterpret_cast<onInconsistentTopicDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onInconsistentTopic, static_cast< ::DDS::TopicDescription_ptr>(topic), status);
        thread.join();
    }

    _lock.release();
};
