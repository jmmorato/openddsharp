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
#include "DomainParticipantListener.h"

::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::DomainParticipantListener() {
	onDataOnReadersDelegate^ fpDataOnReaders = gcnew onDataOnReadersDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onDataOnReaders);
	System::Runtime::InteropServices::GCHandle gchDataOnReaders = System::Runtime::InteropServices::GCHandle::Alloc(fpDataOnReaders);
	System::IntPtr ipDataOnReaders = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpDataOnReaders);
	onDataOnReadersFunctionCpp = static_cast<onDataOnReadersDeclaration>(ipDataOnReaders.ToPointer());

	onDataAvailableDelegate^ fpDataAvailable = gcnew onDataAvailableDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onDataAvalaible);
	System::Runtime::InteropServices::GCHandle gchDataAvailable = System::Runtime::InteropServices::GCHandle::Alloc(fpDataAvailable);
	System::IntPtr ipDataAvailable = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpDataAvailable);
	onDataAvalaibleFunctionCpp = static_cast<onDataAvailabeDeclaration>(ipDataAvailable.ToPointer());

	onRequestedDeadlineMissedDelegate^ fpRequestedDeadlineMissed = gcnew onRequestedDeadlineMissedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onRequestedDeadlineMissed);
	System::Runtime::InteropServices::GCHandle gchRequestedDeadlineMissed = System::Runtime::InteropServices::GCHandle::Alloc(fpRequestedDeadlineMissed);
	System::IntPtr ipRequestedDeadlineMissed = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpRequestedDeadlineMissed);
	onRequestedDeadlineMissedFunctionCpp = static_cast<onRequestedDeadlineMissedDeclaration>(ipRequestedDeadlineMissed.ToPointer());

	onRequestedIncompatibleQosDelegate^ fpRequestedIncompatibleQos = gcnew onRequestedIncompatibleQosDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onRequestedIncompatibleQos);
	System::Runtime::InteropServices::GCHandle gchRequestedIncompatibleQos = System::Runtime::InteropServices::GCHandle::Alloc(fpRequestedIncompatibleQos);
	System::IntPtr ipRequestedIncompatibleQos = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpRequestedIncompatibleQos);
	onRequestedIncompatibleQosFunctionCpp = static_cast<onRequestedIncompatibleQosDeclaration>(ipRequestedIncompatibleQos.ToPointer());

	onSampleRejectedDelegate^ fpSampleRejected = gcnew onSampleRejectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onSampleRejected);
	System::Runtime::InteropServices::GCHandle gchSampleRejected = System::Runtime::InteropServices::GCHandle::Alloc(fpSampleRejected);
	System::IntPtr ipSampleRejected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSampleRejected);
	onSampleRejectedFunctionCpp = static_cast<onSampleRejectedDeclaration>(ipSampleRejected.ToPointer());

	onLivelinessChangedDelegate^ fpLivelinessChanged = gcnew onLivelinessChangedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onLivelinessChanged);
	System::Runtime::InteropServices::GCHandle gchLivelinessChanged = System::Runtime::InteropServices::GCHandle::Alloc(fpLivelinessChanged);
	System::IntPtr ipLivelinessChanged = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpLivelinessChanged);
	onLivelinessChangedFunctionCpp = static_cast<onLivelinessChangedDeclaration>(ipLivelinessChanged.ToPointer());

	onSubscriptionMatchedDelegate^ fpSubscriptionMatched = gcnew onSubscriptionMatchedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onSubscriptionMatched);
	System::Runtime::InteropServices::GCHandle gchSubscriptionMatched = System::Runtime::InteropServices::GCHandle::Alloc(fpSubscriptionMatched);
	System::IntPtr ipSubscriptionMatched = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSubscriptionMatched);
	onSubscriptionMatchedFunctionCpp = static_cast<onSubscriptionMatchedDeclaration>(ipSubscriptionMatched.ToPointer());

	onSampleLostDelegate^ fpSampleLost = gcnew onSampleLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onSampleLost);
	System::Runtime::InteropServices::GCHandle gchSampleLost = System::Runtime::InteropServices::GCHandle::Alloc(fpSampleLost);
	System::IntPtr ipSampleLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSampleLost);
	onSampleLostFunctionCpp = static_cast<onSampleLostDeclaration>(ipSampleLost.ToPointer());

	onSubscriptionDisconnectedDelegate^ fpSubscriptionDisconnected = gcnew onSubscriptionDisconnectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onSubscriptionDisconnected);
	System::Runtime::InteropServices::GCHandle gchSubscriptionDisconnected = System::Runtime::InteropServices::GCHandle::Alloc(fpSubscriptionDisconnected);
	System::IntPtr ipSubscriptionDisconnected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSubscriptionDisconnected);
	onSubscriptionDisconnectedFunctionCpp = static_cast<onSubscriptionDisconnectedDeclaration>(ipSubscriptionDisconnected.ToPointer());

	onSubscriptionReconnectedDelegate^ fpSubscriptionReconnected = gcnew onSubscriptionReconnectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onSubscriptionReconnected);
	System::Runtime::InteropServices::GCHandle gchSubscriptionReconnected = System::Runtime::InteropServices::GCHandle::Alloc(fpSubscriptionReconnected);
	System::IntPtr ipSubscriptionReconnected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSubscriptionReconnected);
	onSubscriptionReconnectedFunctionCpp = static_cast<onSubscriptionReconnectedDeclaration>(ipSubscriptionReconnected.ToPointer());

	onSubscriptionLostDelegate^ fpSubscriptionLost = gcnew onSubscriptionLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onSubscriptionLost);
	System::Runtime::InteropServices::GCHandle gchSubscriptionLost = System::Runtime::InteropServices::GCHandle::Alloc(fpSubscriptionLost);
	System::IntPtr ipSubscriptionLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSubscriptionLost);
	onSubscriptionLostFunctionCpp = static_cast<onSubscriptionLostDeclaration>(ipSubscriptionLost.ToPointer());

	onBudgetExceededDelegate^ fpBudgetExceeded = gcnew onBudgetExceededDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onBudgetExceeded);
	System::Runtime::InteropServices::GCHandle gchBudgetExceeded = System::Runtime::InteropServices::GCHandle::Alloc(fpBudgetExceeded);
	System::IntPtr ipBudgetExceeded = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpBudgetExceeded);
	onBudgetExceededFunctionCpp = static_cast<onBudgetExceededDeclaration>(ipBudgetExceeded.ToPointer());

	onOfferedDeadlineMissedDelegate^ fpOfferedDeadlineMissed = gcnew onOfferedDeadlineMissedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onOfferedDeadlineMissed);
	System::Runtime::InteropServices::GCHandle gchOfferedDeadlineMissed = System::Runtime::InteropServices::GCHandle::Alloc(fpOfferedDeadlineMissed);
	System::IntPtr ipOfferedDeadlineMissed = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpOfferedDeadlineMissed);
	onOfferedDeadlineMissedFunctionCpp = static_cast<onOfferedDeadlineMissedDeclaration>(ipOfferedDeadlineMissed.ToPointer());

	onOfferedIncompatibleQosDelegate^ fpOfferedIncompatibleQos = gcnew onOfferedIncompatibleQosDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onOfferedIncompatibleQos);
	System::Runtime::InteropServices::GCHandle gchOfferedIncompatibleQos = System::Runtime::InteropServices::GCHandle::Alloc(fpOfferedIncompatibleQos);
	System::IntPtr ipOfferedIncompatibleQos = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpOfferedIncompatibleQos);
	onOfferedIncompatibleQosFunctionCpp = static_cast<onOfferedIncompatibleQosDeclaration>(ipOfferedIncompatibleQos.ToPointer());

	onLivelinessLostDelegate^ fpLivelinessLost = gcnew onLivelinessLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onLivelinessLost);
	System::Runtime::InteropServices::GCHandle gchLivelinessLost = System::Runtime::InteropServices::GCHandle::Alloc(fpLivelinessLost);
	System::IntPtr ipLivelinessLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpLivelinessLost);
	onLivelinessLostFunctionCpp = static_cast<onLivelinessLostDeclaration>(ipLivelinessLost.ToPointer());

	onPublicationMatchedDelegate^ fpPublicationMatched = gcnew onPublicationMatchedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onPublicationMatched);
	System::Runtime::InteropServices::GCHandle gchPublicationMatched = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationMatched);
	System::IntPtr ipPublicationMatched = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationMatched);
	onPublicationMatchedFunctionCpp = static_cast<onPublicationMatchedDeclaration>(ipPublicationMatched.ToPointer());

	onPublicationDisconnectedDelegate^ fpPublicationDisconnected = gcnew onPublicationDisconnectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onPublicationDisconnected);
	System::Runtime::InteropServices::GCHandle gchPublicationDisconnected = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationDisconnected);
	System::IntPtr ipPublicationDisconnected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationDisconnected);
	onPublicationDisconnectedFunctionCpp = static_cast<onPublicationDisconnectedDeclaration>(ipPublicationDisconnected.ToPointer());

	onPublicationReconnectedDelegate^ fpPublicationReconnected = gcnew onPublicationReconnectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onPublicationReconnected);
	System::Runtime::InteropServices::GCHandle gchPublicationReconnected = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationReconnected);
	System::IntPtr ipPublicationReconnected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationReconnected);
	onPublicationReconnectedFunctionCpp = static_cast<onPublicationReconnectedDeclaration>(ipPublicationReconnected.ToPointer());

	onPublicationLostDelegate^ fpPublicationLost = gcnew onPublicationLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onPublicationLost);
	System::Runtime::InteropServices::GCHandle gchPublicationLost = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationLost);
	System::IntPtr ipPublicationLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationLost);
	onPublicationLostFunctionCpp = static_cast<onPublicationLostDeclaration>(ipPublicationLost.ToPointer());

	onInconsistentTopicDelegate^ fpInconsistentTopic = gcnew onInconsistentTopicDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DomainParticipantListener::onInconsistentTopic);
	System::Runtime::InteropServices::GCHandle gchInconsistentTopic = System::Runtime::InteropServices::GCHandle::Alloc(fpInconsistentTopic);
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
																				  onSubscriptionDisconnectedFunctionCpp,
																				  onSubscriptionReconnectedFunctionCpp,
																				  onSubscriptionLostFunctionCpp,
																				  onBudgetExceededFunctionCpp,																				  
																				  onOfferedDeadlineMissedFunctionCpp,
																				  onOfferedIncompatibleQosFunctionCpp,
																				  onLivelinessLostFunctionCpp,
																				  onPublicationMatchedFunctionCpp,
																				  onPublicationDisconnectedFunctionCpp,
																				  onPublicationReconnectedFunctionCpp,
																				  onPublicationLostFunctionCpp,																				  
																				  onInconsistentTopicFunctionCpp);
}