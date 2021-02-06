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
#include "DataWriterListener.h"

::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::DataWriterListener() {
	onOfferedDeadlineMissedDelegate^ fpOfferedDeadlineMissed = gcnew onOfferedDeadlineMissedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onOfferedDeadlineMissed);
	gchOfferedDeadlineMissed = System::Runtime::InteropServices::GCHandle::Alloc(fpOfferedDeadlineMissed);
	System::IntPtr ipOfferedDeadlineMissed = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpOfferedDeadlineMissed);	
	onOfferedDeadlineMissedFunctionCpp = static_cast<onOfferedDeadlineMissedDeclaration>(ipOfferedDeadlineMissed.ToPointer());

	onOfferedIncompatibleQosDelegate^ fpOfferedIncompatibleQos = gcnew onOfferedIncompatibleQosDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onOfferedIncompatibleQos);
	gchOfferedIncompatibleQos = System::Runtime::InteropServices::GCHandle::Alloc(fpOfferedIncompatibleQos);
	System::IntPtr ipOfferedIncompatibleQos = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpOfferedIncompatibleQos);	
	onOfferedIncompatibleQosFunctionCpp = static_cast<onOfferedIncompatibleQosDeclaration>(ipOfferedIncompatibleQos.ToPointer());

	onLivelinessLostDelegate^ fpLivelinessLost = gcnew onLivelinessLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onLivelinessLost);
	gchLivelinessLost = System::Runtime::InteropServices::GCHandle::Alloc(fpLivelinessLost);
	System::IntPtr ipLivelinessLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpLivelinessLost);	
	onLivelinessLostFunctionCpp = static_cast<onLivelinessLostDeclaration>(ipLivelinessLost.ToPointer());

	onPublicationMatchedDelegate^ fpPublicationMatched = gcnew onPublicationMatchedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onPublicationMatched);
	gchPublicationMatched = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationMatched);
	System::IntPtr ipPublicationMatched = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationMatched);	
	onPublicationMatchedFunctionCpp = static_cast<onPublicationMatchedDeclaration>(ipPublicationMatched.ToPointer());

	onPublicationDisconnectedDelegate^ fpPublicationDisconnected = gcnew onPublicationDisconnectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onPublicationDisconnected);
	gchPublicationDisconnected = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationDisconnected);
	System::IntPtr ipPublicationDisconnected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationDisconnected);	
	onPublicationDisconnectedFunctionCpp = static_cast<onPublicationDisconnectedDeclaration>(ipPublicationDisconnected.ToPointer());

	onPublicationReconnectedDelegate^ fpPublicationReconnected = gcnew onPublicationReconnectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onPublicationReconnected);
	gchPublicationReconnected = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationReconnected);
	System::IntPtr ipPublicationReconnected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationReconnected);	
	onPublicationReconnectedFunctionCpp = static_cast<onPublicationReconnectedDeclaration>(ipPublicationReconnected.ToPointer());

	onPublicationLostDelegate^ fpPublicationLost = gcnew onPublicationLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onPublicationLost);
	gchPublicationLost = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationLost);
	System::IntPtr ipPublicationLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationLost);	
	onPublicationLostFunctionCpp = static_cast<onPublicationLostDeclaration>(ipPublicationLost.ToPointer());

	impl_entity = new OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative(onOfferedDeadlineMissedFunctionCpp,
																		   onOfferedIncompatibleQosFunctionCpp,
																		   onLivelinessLostFunctionCpp,
																		   onPublicationMatchedFunctionCpp,
																		   onPublicationDisconnectedFunctionCpp,
																		   onPublicationReconnectedFunctionCpp,
																		   onPublicationLostFunctionCpp);
}

::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::!DataWriterListener() {
    gchOfferedDeadlineMissed.Free();
    gchOfferedIncompatibleQos.Free();
    gchLivelinessLost.Free();
    gchPublicationMatched.Free();
    gchPublicationDisconnected.Free();
    gchPublicationLost.Free();
    gchPublicationReconnected.Free();
}