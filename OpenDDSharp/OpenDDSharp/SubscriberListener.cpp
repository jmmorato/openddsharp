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
#include "SubscriberListener.h"

::OpenDDSharp::DDS::SubscriberListener::SubscriberListener() {
	onDataOnReadersDelegate^ fpDataOnReaders = gcnew onDataOnReadersDelegate(this, &::OpenDDSharp::DDS::SubscriberListener::onDataOnReaders);
	System::Runtime::InteropServices::GCHandle gchDataOnReaders = System::Runtime::InteropServices::GCHandle::Alloc(fpDataOnReaders);
	System::IntPtr ipDataOnReaders = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpDataOnReaders);	
	onDataOnReadersFunctionCpp = static_cast<onDataOnReadersDeclaration>(ipDataOnReaders.ToPointer());

    onDataAvailableDelegate^ fpDataAvailable = gcnew onDataAvailableDelegate(this, &::OpenDDSharp::DDS::SubscriberListener::onDataAvalaible);
    System::Runtime::InteropServices::GCHandle gchDataAvailable = System::Runtime::InteropServices::GCHandle::Alloc(fpDataAvailable);
    System::IntPtr ipDataAvailable = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpDataAvailable);
    onDataAvalaibleFunctionCpp = static_cast<onDataAvailabeDeclaration>(ipDataAvailable.ToPointer());

    onRequestedDeadlineMissedDelegate^ fpRequestedDeadlineMissed = gcnew onRequestedDeadlineMissedDelegate(this, &::OpenDDSharp::DDS::SubscriberListener::onRequestedDeadlineMissed);
    System::Runtime::InteropServices::GCHandle gchRequestedDeadlineMissed = System::Runtime::InteropServices::GCHandle::Alloc(fpRequestedDeadlineMissed);
    System::IntPtr ipRequestedDeadlineMissed = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpRequestedDeadlineMissed);
    onRequestedDeadlineMissedFunctionCpp = static_cast<onRequestedDeadlineMissedDeclaration>(ipRequestedDeadlineMissed.ToPointer());

    onRequestedIncompatibleQosDelegate^ fpRequestedIncompatibleQos = gcnew onRequestedIncompatibleQosDelegate(this, &::OpenDDSharp::DDS::SubscriberListener::onRequestedIncompatibleQos);
    System::Runtime::InteropServices::GCHandle gchRequestedIncompatibleQos = System::Runtime::InteropServices::GCHandle::Alloc(fpRequestedIncompatibleQos);
    System::IntPtr ipRequestedIncompatibleQos = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpRequestedIncompatibleQos);
    onRequestedIncompatibleQosFunctionCpp = static_cast<onRequestedIncompatibleQosDeclaration>(ipRequestedIncompatibleQos.ToPointer());

    onSampleRejectedDelegate^ fpSampleRejected = gcnew onSampleRejectedDelegate(this, &::OpenDDSharp::DDS::SubscriberListener::onSampleRejected);
    System::Runtime::InteropServices::GCHandle gchSampleRejected = System::Runtime::InteropServices::GCHandle::Alloc(fpSampleRejected);
    System::IntPtr ipSampleRejected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSampleRejected);
    onSampleRejectedFunctionCpp = static_cast<onSampleRejectedDeclaration>(ipSampleRejected.ToPointer());

    onLivelinessChangedDelegate^ fpLivelinessChanged = gcnew onLivelinessChangedDelegate(this, &::OpenDDSharp::DDS::SubscriberListener::onLivelinessChanged);
    System::Runtime::InteropServices::GCHandle gchLivelinessChanged = System::Runtime::InteropServices::GCHandle::Alloc(fpLivelinessChanged);
    System::IntPtr ipLivelinessChanged = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpLivelinessChanged);
    onLivelinessChangedFunctionCpp = static_cast<onLivelinessChangedDeclaration>(ipLivelinessChanged.ToPointer());

    onSubscriptionMatchedDelegate^ fpSubscriptionMatched = gcnew onSubscriptionMatchedDelegate(this, &::OpenDDSharp::DDS::SubscriberListener::onSubscriptionMatched);
    System::Runtime::InteropServices::GCHandle gchSubscriptionMatched = System::Runtime::InteropServices::GCHandle::Alloc(fpSubscriptionMatched);
    System::IntPtr ipSubscriptionMatched = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSubscriptionMatched);
    onSubscriptionMatchedFunctionCpp = static_cast<onSubscriptionMatchedDeclaration>(ipSubscriptionMatched.ToPointer());

    onSampleLostDelegate^ fpSampleLost = gcnew onSampleLostDelegate(this, &::OpenDDSharp::DDS::SubscriberListener::onSampleLost);
    System::Runtime::InteropServices::GCHandle gchSampleLost = System::Runtime::InteropServices::GCHandle::Alloc(fpSampleLost);
    System::IntPtr ipSampleLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSampleLost);
    onSampleLostFunctionCpp = static_cast<onSampleLostDeclaration>(ipSampleLost.ToPointer());

	impl_entity = new OpenDDSharp::DDS::SubscriberListenerNative(onDataOnReadersFunctionCpp,
																 onDataAvalaibleFunctionCpp,
																 onRequestedDeadlineMissedFunctionCpp,
																 onRequestedIncompatibleQosFunctionCpp,
																 onSampleRejectedFunctionCpp,
																 onLivelinessChangedFunctionCpp,
																 onSubscriptionMatchedFunctionCpp,
																 onSampleLostFunctionCpp);
}