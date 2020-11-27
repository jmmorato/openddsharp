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

// Subscriber delegates.
typedef void(__stdcall* onDataOnReadersDeclaration)(::DDS::Entity_ptr subscriber);

// DataReader delegates.
typedef void(__stdcall* onDataAvailabeDeclaration)(::DDS::Entity_ptr reader);
typedef void(__stdcall* onRequestedDeadlineMissedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status);
typedef void(__stdcall* onRequestedIncompatibleQosDeclaration)(::DDS::Entity_ptr reader, const RequestedIncompatibleQosStatusWrapper& status);
typedef void(__stdcall* onSampleRejectedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::SampleRejectedStatus& status);
typedef void(__stdcall* onLivelinessChangedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::LivelinessChangedStatus& status);
typedef void(__stdcall* onSubscriptionMatchedDeclaration)(::DDS::Entity_ptr reader, const ::DDS::SubscriptionMatchedStatus& status);
typedef void(__stdcall* onSampleLostDeclaration)(::DDS::Entity_ptr reader, const ::DDS::SampleLostStatus& status);

// DataWriter delegates
typedef void(__stdcall* onOfferedDeadlineMissedDeclaration)(::DDS::Entity_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status);
typedef void(__stdcall* onOfferedIncompatibleQosDeclaration)(::DDS::Entity_ptr writer, const OfferedIncompatibleQosStatusWrapper& status);
typedef void(__stdcall* onLivelinessLostDeclaration)(::DDS::Entity_ptr writer, const ::DDS::LivelinessLostStatus& status);
typedef void(__stdcall* onPublicationMatchedDeclaration)(::DDS::Entity_ptr writer, const ::DDS::PublicationMatchedStatus& status);

// Topic delegates
typedef void(__stdcall* onInconsistentTopicDeclaration)(::DDS::TopicDescription_ptr topic, const ::DDS::InconsistentTopicStatus& status);