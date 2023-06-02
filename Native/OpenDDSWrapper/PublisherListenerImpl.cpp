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
#include "PublisherListenerImpl.h"

::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::PublisherListenerImpl(onOfferedDeadlineMissedDeclaration* onOfferedDeadlineMissed,
                                                                          onOfferedIncompatibleQosDeclaration* onOfferedIncompatibleQos,
                                                                          onLivelinessLostDeclaration* onLivelinessLost,
                                                                          onPublicationMatchedDeclaration* onPublicationMatched) {
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
		_onOfferedDeadlineMissed(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};

void ::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::on_offered_incompatible_qos(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onOfferedIncompatibleQos) {
		_onOfferedIncompatibleQos(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};

void ::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::on_liveliness_lost(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();

	if (_onLivelinessLost) {
		_onLivelinessLost(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};

void ::OpenDDSharp::OpenDDS::DDS::PublisherListenerImpl::on_publication_matched(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus& status) {
  _lock.acquire();

  if (_disposed) {
    return;
  }

  _lock.release();
  
	if (_onPublicationMatched) {
		_onPublicationMatched(static_cast< ::DDS::Entity_ptr>(writer), status);
	}
};
