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
#include "PublisherListenerImpl.h"

::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::PublisherListenerImpl(void* onOfferedDeadlineMissed,
                                                                          void* onOfferedIncompatibleQos,
                                                                          void* onLivelinessLost,
                                                                          void* onPublicationMatched) {
	_onOfferedDeadlineMissed = onOfferedDeadlineMissed;
	_onOfferedIncompatibleQos = onOfferedIncompatibleQos;
	_onLivelinessLost = onLivelinessLost;
	_onPublicationMatched = onPublicationMatched;
}

::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::~PublisherListenerImpl() {
	dispose();
};

void ::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::dispose() {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _onOfferedDeadlineMissed = NULL;
  _onOfferedIncompatibleQos = NULL;
  _onLivelinessLost = NULL;
  _onPublicationMatched = NULL;

  _disposed = true;

  _lock.release();
}

void ::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onOfferedDeadlineMissed) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity, const ::DDS::OfferedDeadlineMissedStatus& st)
        {
            reinterpret_cast<onOfferedDeadlineMissedDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onOfferedDeadlineMissed, static_cast< ::DDS::Entity_ptr>(writer), status);
        thread.join();
    }
};

void ::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus& status) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onOfferedIncompatibleQos) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity, const ::DDS::OfferedIncompatibleQosStatus& st)
        {
            reinterpret_cast<onOfferedIncompatibleQosDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onOfferedIncompatibleQos, static_cast< ::DDS::Entity_ptr>(writer), status);
        thread.join();
    }
};

void ::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus& status) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onLivelinessLost) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity, const ::DDS::LivelinessLostStatus& st)
        {
            reinterpret_cast<onLivelinessLostDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onLivelinessLost, static_cast< ::DDS::Entity_ptr>(writer), status);
        thread.join();
    }
};

void ::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus& status) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onPublicationMatched) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity, const ::DDS::PublicationMatchedStatus& st)
        {
            reinterpret_cast<onPublicationMatchedDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onPublicationMatched, static_cast< ::DDS::Entity_ptr>(writer), status);
        thread.join();
    }
};
