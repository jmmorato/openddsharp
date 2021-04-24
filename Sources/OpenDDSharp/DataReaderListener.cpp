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
#include "DataReaderListener.h"

::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::DataReaderListener() {
	onDataAvailableDelegate^ fpDataAvailable = gcnew onDataAvailableDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onDataAvalaible);
	gchDataAvailable = System::Runtime::InteropServices::GCHandle::Alloc(fpDataAvailable);
	System::IntPtr ipDataAvailable = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpDataAvailable);	
	onDataAvalaibleFunctionCpp = static_cast<onDataAvailabeDeclaration>(ipDataAvailable.ToPointer());

	onRequestedDeadlineMissedDelegate^ fpRequestedDeadlineMissed = gcnew onRequestedDeadlineMissedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onRequestedDeadlineMissed);
	gchRequestedDeadlineMissed = System::Runtime::InteropServices::GCHandle::Alloc(fpRequestedDeadlineMissed);
	System::IntPtr ipRequestedDeadlineMissed = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpRequestedDeadlineMissed);	
	onRequestedDeadlineMissedFunctionCpp = static_cast<onRequestedDeadlineMissedDeclaration>(ipRequestedDeadlineMissed.ToPointer());

	onRequestedIncompatibleQosDelegate^ fpRequestedIncompatibleQos = gcnew onRequestedIncompatibleQosDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onRequestedIncompatibleQos);
	gchRequestedIncompatibleQos = System::Runtime::InteropServices::GCHandle::Alloc(fpRequestedIncompatibleQos);
	System::IntPtr ipRequestedIncompatibleQos = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpRequestedIncompatibleQos);	
	onRequestedIncompatibleQosFunctionCpp = static_cast<onRequestedIncompatibleQosDeclaration>(ipRequestedIncompatibleQos.ToPointer());

	onSampleRejectedDelegate^ fpSampleRejected = gcnew onSampleRejectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onSampleRejected);
	gchSampleRejected = System::Runtime::InteropServices::GCHandle::Alloc(fpSampleRejected);
	System::IntPtr ipSampleRejected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSampleRejected);	
	onSampleRejectedFunctionCpp = static_cast<onSampleRejectedDeclaration>(ipSampleRejected.ToPointer());

	onLivelinessChangedDelegate^ fpLivelinessChanged = gcnew onLivelinessChangedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onLivelinessChanged);
	gchLivelinessChanged = System::Runtime::InteropServices::GCHandle::Alloc(fpLivelinessChanged);
	System::IntPtr ipLivelinessChanged = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpLivelinessChanged);
	onLivelinessChangedFunctionCpp = static_cast<onLivelinessChangedDeclaration>(ipLivelinessChanged.ToPointer());

	onSubscriptionMatchedDelegate^ fpSubscriptionMatched = gcnew onSubscriptionMatchedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onSubscriptionMatched);
	gchSubscriptionMatched = System::Runtime::InteropServices::GCHandle::Alloc(fpSubscriptionMatched);
	System::IntPtr ipSubscriptionMatched = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSubscriptionMatched);	
	onSubscriptionMatchedFunctionCpp = static_cast<onSubscriptionMatchedDeclaration>(ipSubscriptionMatched.ToPointer());

	onSampleLostDelegate^ fpSampleLost = gcnew onSampleLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onSampleLost);
	gchSampleLost = System::Runtime::InteropServices::GCHandle::Alloc(fpSampleLost);
	System::IntPtr ipSampleLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSampleLost);	
	onSampleLostFunctionCpp = static_cast<onSampleLostDeclaration>(ipSampleLost.ToPointer());

	onSubscriptionDisconnectedDelegate^ fpSubscriptionDisconnected = gcnew onSubscriptionDisconnectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onSubscriptionDisconnected);
	gchSubscriptionDisconnected = System::Runtime::InteropServices::GCHandle::Alloc(fpSubscriptionDisconnected);
	System::IntPtr ipSubscriptionDisconnected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSubscriptionDisconnected);	
	onSubscriptionDisconnectedFunctionCpp = static_cast<onSubscriptionDisconnectedDeclaration>(ipSubscriptionDisconnected.ToPointer());

	onSubscriptionReconnectedDelegate^ fpSubscriptionReconnected = gcnew onSubscriptionReconnectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onSubscriptionReconnected);
	gchSubscriptionReconnected = System::Runtime::InteropServices::GCHandle::Alloc(fpSubscriptionReconnected);
	System::IntPtr ipSubscriptionReconnected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSubscriptionReconnected);	
	onSubscriptionReconnectedFunctionCpp = static_cast<onSubscriptionReconnectedDeclaration>(ipSubscriptionReconnected.ToPointer());

	onSubscriptionLostDelegate^ fpSubscriptionLost = gcnew onSubscriptionLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onSubscriptionLost);
	gchSubscriptionLost = System::Runtime::InteropServices::GCHandle::Alloc(fpSubscriptionLost);
	System::IntPtr ipSubscriptionLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpSubscriptionLost);
	onSubscriptionLostFunctionCpp = static_cast<onSubscriptionLostDeclaration>(ipSubscriptionLost.ToPointer());

	onBudgetExceededDelegate^ fpBudgetExceeded = gcnew onBudgetExceededDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::onBudgetExceeded);
	gchBudgetExceeded = System::Runtime::InteropServices::GCHandle::Alloc(fpBudgetExceeded);
	System::IntPtr ipBudgetExceeded = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpBudgetExceeded);	
	onBudgetExceededFunctionCpp = static_cast<onBudgetExceededDeclaration>(ipBudgetExceeded.ToPointer());
	
	impl_entity = new OpenDDSharp::OpenDDS::DCPS::DataReaderListenerNative(onDataAvalaibleFunctionCpp, 
																		   onRequestedDeadlineMissedFunctionCpp,
																		   onRequestedIncompatibleQosFunctionCpp,
																		   onSampleRejectedFunctionCpp,
																		   onLivelinessChangedFunctionCpp,
																		   onSubscriptionMatchedFunctionCpp,
																	       onSampleLostFunctionCpp,
																		   onSubscriptionDisconnectedFunctionCpp,
																		   onSubscriptionReconnectedFunctionCpp,
																		   onSubscriptionLostFunctionCpp,
																		   onBudgetExceededFunctionCpp);
};

::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::!DataReaderListener() {
    gchDataAvailable.Free();
    gchRequestedDeadlineMissed.Free();
    gchRequestedIncompatibleQos.Free();
    gchSampleRejected.Free();
    gchLivelinessChanged.Free();
    gchSubscriptionMatched.Free();
    gchSampleLost.Free();
    gchSubscriptionDisconnected.Free();
    gchSubscriptionReconnected.Free();
    gchSubscriptionLost.Free();
    gchBudgetExceeded.Free();
}