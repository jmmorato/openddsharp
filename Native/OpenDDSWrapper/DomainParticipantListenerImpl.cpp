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
#include "DomainParticipantListenerImpl.h"

::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::DomainParticipantListenerImpl(void *onDataOnReaders,
                                                                                          void *onDataAvailable,
                                                                                          void *onRequestedDeadlineMissed,
                                                                                          void *onRequestedIncompatibleQos,
                                                                                          void *onSampleRejected,
                                                                                          void *onLivelinessChanged,
                                                                                          void *onSubscriptionMatched,
                                                                                          void *onSampleLost,
                                                                                          void *onOfferedDeadlineMissed,
                                                                                          void *onOfferedIncompatibleQos,
                                                                                          void *onLivelinessLost,
                                                                                          void *onPublicationMatched,
                                                                                          void *onInconsistentTopic) {
  _onDataOnReaders = onDataOnReaders;
  _onDataAvailable = onDataAvailable;
  _onRequestedDeadlineMissed = onRequestedDeadlineMissed;
  _onRequestedIncompatibleQos = onRequestedIncompatibleQos;
  _onSampleRejected = onSampleRejected;
  _onLivelinessChanged = onLivelinessChanged;
  _onSubscriptionMatched = onSubscriptionMatched;
  _onSampleLost = onSampleLost;
  _onOfferedDeadlineMissed = onOfferedDeadlineMissed;
  _onOfferedIncompatibleQos = onOfferedIncompatibleQos;
  _onLivelinessLost = onLivelinessLost;
  _onPublicationMatched = onPublicationMatched;
  _onInconsistentTopic = onInconsistentTopic;
}

::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::~DomainParticipantListenerImpl() {
  dispose();
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::dispose() {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  _onDataOnReaders = NULL;
  _onDataAvailable = NULL;
  _onRequestedDeadlineMissed = NULL;
  _onRequestedIncompatibleQos = NULL;
  _onSampleRejected = NULL;
  _onLivelinessChanged = NULL;
  _onSubscriptionMatched = NULL;
  _onSampleLost = NULL;
  _onOfferedDeadlineMissed = NULL;
  _onOfferedIncompatibleQos = NULL;
  _onLivelinessLost = NULL;
  _onPublicationMatched = NULL;
  _onInconsistentTopic = NULL;

  _disposed = true;

  _lock.release();
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_data_on_readers(::DDS::Subscriber_ptr subscriber) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onDataOnReaders) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity) {
        reinterpret_cast<onDataOnReadersDeclaration>(ptr)(entity);
    };

    std::thread thread(f, _onDataOnReaders, static_cast< ::DDS::Entity_ptr>(subscriber));
    thread.join();
  }

  _lock.release();
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_data_available(::DDS::DataReader_ptr reader) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onDataAvailable) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity) {
        reinterpret_cast<onDataAvailableDeclaration>(ptr)(entity);
    };

    std::thread thread(f, _onDataAvailable, static_cast< ::DDS::Entity_ptr>(reader));
    thread.join();
  }

  _lock.release();
};

void
::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_requested_deadline_missed(::DDS::DataReader_ptr reader,
                                                                                         const ::DDS::RequestedDeadlineMissedStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onRequestedDeadlineMissed) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity, const ::DDS::RequestedDeadlineMissedStatus &st) {
        reinterpret_cast<onRequestedDeadlineMissedDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onRequestedDeadlineMissed, static_cast< ::DDS::Entity_ptr>(reader), status);
    thread.join();
  }

  _lock.release();
}

void
::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_requested_incompatible_qos(::DDS::DataReader_ptr reader,
                                                                                          const ::DDS::RequestedIncompatibleQosStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onRequestedIncompatibleQos) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity, const ::DDS::RequestedIncompatibleQosStatus &st) {
        reinterpret_cast<onRequestedIncompatibleQosDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onRequestedIncompatibleQos, static_cast< ::DDS::Entity_ptr>(reader), status);
    thread.join();
  }

  _lock.release();
}

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_sample_rejected(::DDS::DataReader_ptr reader,
                                                                                    const ::DDS::SampleRejectedStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onSampleRejected) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity, const ::DDS::SampleRejectedStatus &st) {
        reinterpret_cast<onSampleRejectedDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onSampleRejected, static_cast< ::DDS::Entity_ptr>(reader), status);
    thread.join();
  }

  _lock.release();
}

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_liveliness_changed(::DDS::DataReader_ptr reader,
                                                                                       const ::DDS::LivelinessChangedStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onLivelinessChanged) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity, const ::DDS::LivelinessChangedStatus &st) {
        reinterpret_cast<onLivelinessChangedDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onLivelinessChanged, static_cast< ::DDS::Entity_ptr>(reader), status);
    thread.join();
  }

  _lock.release();
}

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_subscription_matched(::DDS::DataReader_ptr reader,
                                                                                         const ::DDS::SubscriptionMatchedStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onSubscriptionMatched) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity, const ::DDS::SubscriptionMatchedStatus &st) {
        reinterpret_cast<onSubscriptionMatchedDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onSubscriptionMatched, static_cast< ::DDS::Entity_ptr>(reader), status);
    thread.join();
  }

  _lock.release();
}

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_sample_lost(::DDS::DataReader_ptr reader,
                                                                                const ::DDS::SampleLostStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onSampleLost) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity, const ::DDS::SampleLostStatus &st) {
        reinterpret_cast<onSampleLostDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onSampleLost, static_cast< ::DDS::Entity_ptr>(reader), status);
    thread.join();
  }

  _lock.release();
}

void
::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_offered_deadline_missed(::DDS::DataWriter_ptr writer,
                                                                                       const ::DDS::OfferedDeadlineMissedStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onOfferedDeadlineMissed) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity, const ::DDS::OfferedDeadlineMissedStatus &st) {
        reinterpret_cast<onOfferedDeadlineMissedDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onOfferedDeadlineMissed, static_cast< ::DDS::Entity_ptr>(writer), status);
    thread.join();
  }

  _lock.release();
};

void
::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_offered_incompatible_qos(::DDS::DataWriter_ptr writer,
                                                                                        const ::DDS::OfferedIncompatibleQosStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onOfferedIncompatibleQos) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity, const ::DDS::OfferedIncompatibleQosStatus &st) {
        reinterpret_cast<onOfferedIncompatibleQosDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onOfferedIncompatibleQos, static_cast< ::DDS::Entity_ptr>(writer), status);
    thread.join();
  }

  _lock.release();
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_liveliness_lost(::DDS::DataWriter_ptr writer,
                                                                                    const ::DDS::LivelinessLostStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onLivelinessLost) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity, const ::DDS::LivelinessLostStatus &st) {
        reinterpret_cast<onLivelinessLostDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onLivelinessLost, static_cast< ::DDS::Entity_ptr>(writer), status);
    thread.join();
  }

  _lock.release();
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_publication_matched(::DDS::DataWriter_ptr writer,
                                                                                        const ::DDS::PublicationMatchedStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onPublicationMatched) {
    auto f = [](void *ptr, ::DDS::Entity_ptr entity, const ::DDS::PublicationMatchedStatus &st) {
        reinterpret_cast<onPublicationMatchedDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onPublicationMatched, static_cast< ::DDS::Entity_ptr>(writer), status);
    thread.join();
  }

  _lock.release();
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_inconsistent_topic(::DDS::Topic_ptr topic,
                                                                                       const ::DDS::InconsistentTopicStatus &status) {
  _lock.acquire();

  if (_disposed) {
    _lock.release();
    return;
  }

  if (_onInconsistentTopic) {
    auto f = [](void *ptr, ::DDS::TopicDescription_ptr entity, const ::DDS::InconsistentTopicStatus &st) {
        reinterpret_cast<onInconsistentTopicDeclaration>(ptr)(entity, st);
    };

    std::thread thread(f, _onInconsistentTopic, static_cast< ::DDS::TopicDescription_ptr>(topic), status);
    thread.join();
  }

  _lock.release();
};
