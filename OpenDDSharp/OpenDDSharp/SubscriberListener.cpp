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

::OpenDDSharp::DDS::SubscriberListener::SubscriberListener() : ::OpenDDSharp::OpenDDS::DCPS::DataReaderListener::DataReaderListener() {
	onDataOnReadersDelegate^ fpDataOnReaders = gcnew onDataOnReadersDelegate(this, &::OpenDDSharp::DDS::SubscriberListener::onDataOnReaders);
	System::Runtime::InteropServices::GCHandle gchDataOnReaders = System::Runtime::InteropServices::GCHandle::Alloc(fpDataOnReaders);
	System::IntPtr ipDataOnReaders = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpDataOnReaders);	
	onDataOnReadersFunctionCpp = static_cast<onDataOnReadersDeclaration>(ipDataOnReaders.ToPointer());

	impl_entity = new OpenDDSharp::DDS::SubscriberListenerNative(onDataOnReadersFunctionCpp,
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
																 onConnectionDeletedFunctionCpp);
}