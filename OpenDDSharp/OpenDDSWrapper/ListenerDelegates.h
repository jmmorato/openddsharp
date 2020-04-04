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

// Subscriber delegates.
typedef void(__stdcall* onDataOnReadersDeclaration)(::DDS::Subscriber_ptr subscriber);

// DataReader delegates.
typedef void(__stdcall* onDataAvailabeDeclaration)(::DDS::DataReader_ptr reader);
typedef void(__stdcall* onRequestedDeadlineMissedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::RequestedDeadlineMissedStatus& status);
typedef void(__stdcall* onRequestedIncompatibleQosDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::RequestedIncompatibleQosStatus& status);
typedef void(__stdcall* onSampleRejectedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::SampleRejectedStatus& status);
typedef void(__stdcall* onLivelinessChangedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::LivelinessChangedStatus& status);
typedef void(__stdcall* onSubscriptionMatchedDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::SubscriptionMatchedStatus& status);
typedef void(__stdcall* onSampleLostDeclaration)(::DDS::DataReader_ptr reader, const ::DDS::SampleLostStatus& status);

// DataWriter delegates
typedef void(__stdcall* onOfferedDeadlineMissedDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::OfferedDeadlineMissedStatus& status);
typedef void(__stdcall* onOfferedIncompatibleQosDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::OfferedIncompatibleQosStatus& status);
typedef void(__stdcall* onLivelinessLostDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::LivelinessLostStatus& status);
typedef void(__stdcall* onPublicationMatchedDeclaration)(::DDS::DataWriter_ptr writer, const ::DDS::PublicationMatchedStatus& status);

// Topic delegates
typedef void(__stdcall* onInconsistentTopicDeclaration)(::DDS::Topic_ptr topic, const ::DDS::InconsistentTopicStatus& status);