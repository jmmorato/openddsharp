/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2021 Jose Morato

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
#include "DomainParticipantListener.h"

::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::DomainParticipantListener() {
	onDataOnReadersDelegate^ fpDataOnReaders = gcnew onDataOnReadersDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onDataOnReaders);
	gchDataOnReaders = System::Runtime::InteropServices::GCHandle::Alloc(fpDataOnReaders);
	System::IntPtr ipDataOnReaders = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpDataOnReaders);
	onDataOnReadersFunctionCpp = static_cast<onDataOnReadersDeclaration>(ipDataOnReaders.ToPointer());

	onDataAvailableDelegate^ fpDataAvailable = gcnew onDataAvailableDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onDataAvalaible);
	gchDataAvailable = System::Runtime::InteropServices::GCHandle::Alloc(fpDataAvailable);
	System::IntPtr ipDataAvailable = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpDataAvailable);
	onDataAvalaibleFunctionCpp = static_cast<onDataAvailabeDeclaration>(ipDataAvailable.ToPointer());

	onRequestedDeadlineMissedDelegate^ fpRequestedDeadlineMissed = gcnew onRequestedDeadlineMissedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onRequestedDeadlineMissed);
	gchRequestedDeadlineMissed = System::Runtime::InteropServices::GCHandle::Alloc(fpRequestedDeadlineMissed);
	System::IntPtr ipRequestedDeadlineMissed = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpRequestedDeadlineMissed);
	onRequestedDeadlineMissedFunctionCpp = static_cast<onRequestedDeadlineMissedDeclaration>(ipRequestedDeadlineMissed.ToPointer());

	onRequestedIncompatibleQosDelegate^ fpRequestedIncompatibleQos = gcnew onRequestedIncompatibleQosDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onRequestedIncompatibleQos);
	gchRequestedIncompatibleQos = System::Runtime::InteropServices::GCHandle::Alloc(fpRequestedIncompatibleQos);
	System::IntPtr ipRequestedIncompatibleQos = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpRequestedIncompatibleQos);
	onRequestedIncompatibleQosFunctionCpp = static_cast<onRequestedIncompatibleQosDeclaration>(ipRequestedIncompatibleQos.ToPointer());

	onSampleRejectedDelegate^ fpSampleRejected = gcnew onSampleRejectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onSampleRejected);
	gchSampleRejected = System::Runtime::InteropServices::GCHandle::Alloc(fpSampleRejected);
	System::IntPtr ipSampleRejected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSampleRejected);
	onSampleRejectedFunctionCpp = static_cast<onSampleRejectedDeclaration>(ipSampleRejected.ToPointer());

	onLivelinessChangedDelegate^ fpLivelinessChanged = gcnew onLivelinessChangedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onLivelinessChanged);
	gchLivelinessChanged = System::Runtime::InteropServices::GCHandle::Alloc(fpLivelinessChanged);
	System::IntPtr ipLivelinessChanged = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpLivelinessChanged);
	onLivelinessChangedFunctionCpp = static_cast<onLivelinessChangedDeclaration>(ipLivelinessChanged.ToPointer());

	onSubscriptionMatchedDelegate^ fpSubscriptionMatched = gcnew onSubscriptionMatchedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onSubscriptionMatched);
	gchSubscriptionMatched = System::Runtime::InteropServices::GCHandle::Alloc(fpSubscriptionMatched);
	System::IntPtr ipSubscriptionMatched = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSubscriptionMatched);
	onSubscriptionMatchedFunctionCpp = static_cast<onSubscriptionMatchedDeclaration>(ipSubscriptionMatched.ToPointer());

	onSampleLostDelegate^ fpSampleLost = gcnew onSampleLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onSampleLost);
	gchSampleLost = System::Runtime::InteropServices::GCHandle::Alloc(fpSampleLost);
	System::IntPtr ipSampleLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSampleLost);
	onSampleLostFunctionCpp = static_cast<onSampleLostDeclaration>(ipSampleLost.ToPointer());

	onOfferedDeadlineMissedDelegate^ fpOfferedDeadlineMissed = gcnew onOfferedDeadlineMissedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onOfferedDeadlineMissed);
	gchOfferedDeadlineMissed = System::Runtime::InteropServices::GCHandle::Alloc(fpOfferedDeadlineMissed);
	System::IntPtr ipOfferedDeadlineMissed = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpOfferedDeadlineMissed);
	onOfferedDeadlineMissedFunctionCpp = static_cast<onOfferedDeadlineMissedDeclaration>(ipOfferedDeadlineMissed.ToPointer());

	onOfferedIncompatibleQosDelegate^ fpOfferedIncompatibleQos = gcnew onOfferedIncompatibleQosDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onOfferedIncompatibleQos);
	gchOfferedIncompatibleQos = System::Runtime::InteropServices::GCHandle::Alloc(fpOfferedIncompatibleQos);
	System::IntPtr ipOfferedIncompatibleQos = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpOfferedIncompatibleQos);
	onOfferedIncompatibleQosFunctionCpp = static_cast<onOfferedIncompatibleQosDeclaration>(ipOfferedIncompatibleQos.ToPointer());

	onLivelinessLostDelegate^ fpLivelinessLost = gcnew onLivelinessLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onLivelinessLost);
	gchLivelinessLost = System::Runtime::InteropServices::GCHandle::Alloc(fpLivelinessLost);
	System::IntPtr ipLivelinessLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpLivelinessLost);
	onLivelinessLostFunctionCpp = static_cast<onLivelinessLostDeclaration>(ipLivelinessLost.ToPointer());

	onPublicationMatchedDelegate^ fpPublicationMatched = gcnew onPublicationMatchedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onPublicationMatched);
	gchPublicationMatched = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationMatched);
	System::IntPtr ipPublicationMatched = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationMatched);
	onPublicationMatchedFunctionCpp = static_cast<onPublicationMatchedDeclaration>(ipPublicationMatched.ToPointer());

	onInconsistentTopicDelegate^ fpInconsistentTopic = gcnew onInconsistentTopicDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onInconsistentTopic);
	gchInconsistentTopic = System::Runtime::InteropServices::GCHandle::Alloc(fpInconsistentTopic);
	System::IntPtr ipInconsistentTopic = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpInconsistentTopic);
	onInconsistentTopicFunctionCpp = static_cast<onInconsistentTopicDeclaration>(ipInconsistentTopic.ToPointer());

	impl_entity = new OpenDDSharp::OpenDDS::DCPS::DomainParticipantListenerNative(onDataOnReadersFunctionCpp,
																				  onDataAvalaibleFunctionCpp,
																				  onRequestedDeadlineMissedFunctionCpp,
																				  onRequestedIncompatibleQosFunctionCpp,
																				  onSampleRejectedFunctionCpp,
																				  onLivelinessChangedFunctionCpp,
																				  onSubscriptionMatchedFunctionCpp,
																				  onSampleLostFunctionCpp,																				  																	  
																				  onOfferedDeadlineMissedFunctionCpp,
																				  onOfferedIncompatibleQosFunctionCpp,
																				  onLivelinessLostFunctionCpp,
																				  onPublicationMatchedFunctionCpp,																	  
																				  onInconsistentTopicFunctionCpp);
}

::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::!DomainParticipantListener() {
    gchInconsistentTopic.Free();
    gchDataOnReaders.Free();
    gchDataAvailable.Free();
    gchRequestedDeadlineMissed.Free();
    gchRequestedIncompatibleQos.Free();
    gchSampleRejected.Free();
    gchLivelinessChanged.Free();
    gchSubscriptionMatched.Free();
    gchSampleLost.Free();
    gchOfferedDeadlineMissed.Free();
    gchOfferedIncompatibleQos.Free();
    gchLivelinessLost.Free();
    gchPublicationMatched.Free();
}