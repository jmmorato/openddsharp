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
#include "PublisherListener.h"

::OpenDDSharp::DDS::PublisherListener::PublisherListener() {
    onOfferedDeadlineMissedDelegate^ fpOfferedDeadlineMissed = gcnew onOfferedDeadlineMissedDelegate(this, &::OpenDDSharp::DDS::PublisherListener::onOfferedDeadlineMissed);
    gchOfferedDeadlineMissed = System::Runtime::InteropServices::GCHandle::Alloc(fpOfferedDeadlineMissed);
    System::IntPtr ipOfferedDeadlineMissed = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpOfferedDeadlineMissed);
    onOfferedDeadlineMissedFunctionCpp = static_cast<onOfferedDeadlineMissedDeclaration>(ipOfferedDeadlineMissed.ToPointer());

    onOfferedIncompatibleQosDelegate^ fpOfferedIncompatibleQos = gcnew onOfferedIncompatibleQosDelegate(this, &::OpenDDSharp::DDS::PublisherListener::onOfferedIncompatibleQos);
    gchOfferedIncompatibleQos = System::Runtime::InteropServices::GCHandle::Alloc(fpOfferedIncompatibleQos);
    System::IntPtr ipOfferedIncompatibleQos = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpOfferedIncompatibleQos);
    onOfferedIncompatibleQosFunctionCpp = static_cast<onOfferedIncompatibleQosDeclaration>(ipOfferedIncompatibleQos.ToPointer());

    onLivelinessLostDelegate^ fpLivelinessLost = gcnew onLivelinessLostDelegate(this, &::OpenDDSharp::DDS::PublisherListener::onLivelinessLost);
    gchLivelinessLost = System::Runtime::InteropServices::GCHandle::Alloc(fpLivelinessLost);
    System::IntPtr ipLivelinessLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpLivelinessLost);
    onLivelinessLostFunctionCpp = static_cast<onLivelinessLostDeclaration>(ipLivelinessLost.ToPointer());

    onPublicationMatchedDelegate^ fpPublicationMatched = gcnew onPublicationMatchedDelegate(this, &::OpenDDSharp::DDS::PublisherListener::onPublicationMatched);
    gchPublicationMatched = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationMatched);
    System::IntPtr ipPublicationMatched = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationMatched);
    onPublicationMatchedFunctionCpp = static_cast<onPublicationMatchedDeclaration>(ipPublicationMatched.ToPointer());

    impl_entity = new OpenDDSharp::DDS::PublisherListenerNative(onOfferedDeadlineMissedFunctionCpp,
																onOfferedIncompatibleQosFunctionCpp,
																onLivelinessLostFunctionCpp,
																onPublicationMatchedFunctionCpp);

}

::OpenDDSharp::DDS::PublisherListener::!PublisherListener() {
    gchOfferedDeadlineMissed.Free();
    gchOfferedIncompatibleQos.Free();
    gchLivelinessLost.Free();
    gchPublicationMatched.Free();
}
