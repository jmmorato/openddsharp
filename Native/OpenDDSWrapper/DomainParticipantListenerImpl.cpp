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
#include "DomainParticipantListenerImpl.h"

::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::DomainParticipantListenerImpl(onDataOnReadersDeclaration* onDataOnReaders,
                                                                                          onDataAvailableDeclaration* onDataAvailable,
                                                                                          onRequestedDeadlineMissedDeclaration* onRequestedDeadlineMissed,
                                                                                          onRequestedIncompatibleQosDeclaration* onRequestedIncompatibleQos,
                                                                                          onSampleRejectedDeclaration* onSampleRejected,
                                                                                          onLivelinessChangedDeclaration* onLivelinessChanged,
                                                                                          onSubscriptionMatchedDeclaration* onSubscriptionMatched,
                                                                                          onSampleLostDeclaration* onSampleLost,
                                                                                          onOfferedDeadlineMissedDeclaration* onOfferedDeadlineMissed,
                                                                                          onOfferedIncompatibleQosDeclaration* onOfferedIncompatibleQos,
                                                                                          onLivelinessLostDeclaration* onLivelinessLost,
                                                                                          onPublicationMatchedDeclaration* onPublicationMatched,
                                                                                          onInconsistentTopicDeclaration* onInconsistentTopic) {
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
    return;
  }

  _lock.release();

	if (_onDataOnReaders) {
		_onDataOnReaders(static_cast< ::DDS::Entity_ptr>(subscriber));
	}
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_data_available(::DDS::DataReader_ptr reader) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onDataAvailable) {
		_onDataAvailable(static_cast< ::DDS::Entity_ptr>(reader));
	}
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_requested_deadline_missed(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onRequestedDeadlineMissed) {
		_onRequestedDeadlineMissed(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_requested_incompatible_qos(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onRequestedIncompatibleQos) {
		_onRequestedIncompatibleQos(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_sample_rejected(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onSampleRejected) {
		_onSampleRejected(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_liveliness_changed(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onLivelinessChanged) {
		_onLivelinessChanged(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_subscription_matched(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onSubscriptionMatched) {
		_onSubscriptionMatched(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_sample_lost(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onSampleLost) {
		_onSampleLost(static_cast< ::DDS::Entity_ptr>(reader), status);
	}
}

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_offered_deadline_missed(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onOfferedDeadlineMissed) {
		_onOfferedDeadlineMissed(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onOfferedIncompatibleQos) {
		_onOfferedIncompatibleQos(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onLivelinessLost) {
		_onLivelinessLost(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onPublicationMatched) {
		_onPublicationMatched(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};

void ::OpenDDSharp::OpenDDS::DDS::DomainParticipantListenerImpl::on_inconsistent_topic(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onInconsistentTopic) {
		_onInconsistentTopic(static_cast< ::DDS::TopicDescription_ptr>(topic), status);
	}
};
