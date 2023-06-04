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
#include "DataReaderListenerImpl.h"

::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::DataReaderListenerImpl(void* onDataAvailable,
                                                                            void* onRequestedDeadlineMissed,
                                                                            void* onRequestedIncompatibleQos,
                                                                            void* onSampleRejected,
                                                                            void* onLivelinessChanged,
                                                                            void* onSubscriptionMatched,
                                                                            void* onSampleLost) {
  _onDataAvailable = onDataAvailable;
  _onRequestedDeadlineMissed = onRequestedDeadlineMissed;
  _onRequestedIncompatibleQos = onRequestedIncompatibleQos;
  _onSampleRejected = onSampleRejected;
  _onLivelinessChanged = onLivelinessChanged;
  _onSubscriptionMatched = onSubscriptionMatched;
  _onSampleLost = onSampleLost;
}

::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::~DataReaderListenerImpl() {
  dispose();
};

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::dispose() {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _onDataAvailable = NULL;
  _onRequestedDeadlineMissed = NULL;
  _onRequestedIncompatibleQos = NULL;
  _onSampleRejected = NULL;
  _onLivelinessChanged = NULL;
  _onSubscriptionMatched = NULL;
  _onSampleLost = NULL;

  _disposed = true;

  _lock.release();
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_data_available(::DDS::DataReader_ptr reader) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onDataAvailable) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity)
        {
            reinterpret_cast<onDataAvailableDeclaration>(ptr)(entity);
        };

        std::thread thread(f, _onDataAvailable, static_cast< ::DDS::Entity_ptr>(reader));
        thread.join();
    }
};

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_requested_deadline_missed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onRequestedDeadlineMissed) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity, const ::DDS::RequestedDeadlineMissedStatus& st)
        {
            reinterpret_cast<onRequestedDeadlineMissedDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onRequestedDeadlineMissed, static_cast< ::DDS::Entity_ptr>(reader), status);
        thread.join();
    }
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_requested_incompatible_qos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onRequestedIncompatibleQos) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity, const ::DDS::RequestedIncompatibleQosStatus& st)
        {
            reinterpret_cast<onRequestedIncompatibleQosDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onRequestedIncompatibleQos, static_cast< ::DDS::Entity_ptr>(reader), status);
        thread.join();
    }
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onSampleRejected) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity, const ::DDS::SampleRejectedStatus& st)
        {
            reinterpret_cast<onSampleRejectedDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onSampleRejected, static_cast< ::DDS::Entity_ptr>(reader), status);
        thread.join();
    }
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onLivelinessChanged) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity, const ::DDS::LivelinessChangedStatus& st)
        {
            reinterpret_cast<onLivelinessChangedDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onLivelinessChanged, static_cast< ::DDS::Entity_ptr>(reader), status);
        thread.join();
    }
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onSubscriptionMatched) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity, const ::DDS::SubscriptionMatchedStatus& st)
        {
            reinterpret_cast<onSubscriptionMatchedDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onSubscriptionMatched, static_cast< ::DDS::Entity_ptr>(reader), status);
        thread.join();
    }
}

void ::OpenDDSharp::OpenDDS::DDS::DataReaderListenerImpl::on_sample_lost( ::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status) {
    _lock.acquire();

    if (_disposed) {
        return;
    }

    _lock.release();

    if (_onSampleLost) {
        auto f = [](void* ptr, ::DDS::Entity_ptr entity, const ::DDS::SampleLostStatus& st)
        {
            reinterpret_cast<onSampleLostDeclaration>(ptr)(entity, st);
        };

        std::thread thread(f, _onSampleLost, static_cast< ::DDS::Entity_ptr>(reader), status);
        thread.join();
    }
}
