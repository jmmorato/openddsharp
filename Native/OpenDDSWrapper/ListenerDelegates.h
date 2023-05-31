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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#pragma once

#include "Utils.h"
#include "Statuses.h"

#ifdef _WIN32
// Subscriber delegates.
typedef void(__cdecl* onDataOnReadersDeclaration)(::DDS::Entity_ptr subscriber);

// DataReader delegates.
typedef void(__cdecl* onDataAvailableDeclaration)(::DDS::Entity_ptr reader);
typedef void(__cdecl* onRequestedDeadlineMissedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status);
typedef void(__cdecl* onRequestedIncompatibleQosDeclaration)(::DDS::Entity_ptr reader, const RequestedIncompatibleQosStatusWrapper& status);
typedef void(__cdecl* onSampleRejectedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::SampleRejectedStatus& status);
typedef void(__cdecl* onLivelinessChangedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::LivelinessChangedStatus& status);
typedef void(__cdecl* onSubscriptionMatchedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::SubscriptionMatchedStatus& status);
typedef void(__cdecl* onSampleLostDeclaration)(::DDS::Entity_ptr reader, const ::DDS::SampleLostStatus& status);

// DataWriter delegates
typedef void(__cdecl* onOfferedDeadlineMissedDeclaration)(::DDS::Entity_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status);
typedef void(__cdecl* onOfferedIncompatibleQosDeclaration)(::DDS::Entity_ptr writer, const OfferedIncompatibleQosStatusWrapper& status);
typedef void(__cdecl* onLivelinessLostDeclaration)(::DDS::Entity_ptr writer, const ::DDS::LivelinessLostStatus& status);
typedef void(__cdecl* onPublicationMatchedDeclaration)(::DDS::Entity_ptr writer, const ::DDS::PublicationMatchedStatus& status);

// Topic delegates
typedef void(__cdecl* onInconsistentTopicDeclaration)(::DDS::TopicDescription_ptr topic, const ::DDS::InconsistentTopicStatus& status);
#else
// Subscriber delegates.
typedef void(onDataOnReadersDeclaration)(::DDS::Entity_ptr subscriber);

// DataReader delegates.
typedef void(onDataAvailableDeclaration)(::DDS::Entity_ptr reader);
typedef void(onRequestedDeadlineMissedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status);
typedef void(onRequestedIncompatibleQosDeclaration)(::DDS::Entity_ptr reader, const RequestedIncompatibleQosStatusWrapper& status);
typedef void(onSampleRejectedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::SampleRejectedStatus& status);
typedef void(onLivelinessChangedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::LivelinessChangedStatus& status);
typedef void(onSubscriptionMatchedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::SubscriptionMatchedStatus& status);
typedef void(onSampleLostDeclaration)(::DDS::Entity_ptr reader, const ::DDS::SampleLostStatus& status);

// DataWriter delegates
typedef void(onOfferedDeadlineMissedDeclaration)(::DDS::Entity_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status);
typedef void(onOfferedIncompatibleQosDeclaration)(::DDS::Entity_ptr writer, const OfferedIncompatibleQosStatusWrapper& status);
typedef void(onLivelinessLostDeclaration)(::DDS::Entity_ptr writer, const ::DDS::LivelinessLostStatus& status);
typedef void(onPublicationMatchedDeclaration)(::DDS::Entity_ptr writer, const ::DDS::PublicationMatchedStatus& status);

// Topic delegates
typedef void(onInconsistentTopicDeclaration)(::DDS::TopicDescription_ptr topic, const ::DDS::InconsistentTopicStatus& status);
#endif