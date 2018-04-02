#include "DataWriterListener.h"

::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::DataWriterListener() {
	onOfferedDeadlineMissedDelegate^ fpOfferedDeadlineMissed = gcnew onOfferedDeadlineMissedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onOfferedDeadlineMissed);
	System::Runtime::InteropServices::GCHandle gchOfferedDeadlineMissed = System::Runtime::InteropServices::GCHandle::Alloc(fpOfferedDeadlineMissed);
	System::IntPtr ipOfferedDeadlineMissed = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpOfferedDeadlineMissed);	
	onOfferedDeadlineMissedFunctionCpp = static_cast<onOfferedDeadlineMissedDeclaration>(ipOfferedDeadlineMissed.ToPointer());

	onOfferedIncompatibleQosDelegate^ fpOfferedIncompatibleQos = gcnew onOfferedIncompatibleQosDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onOfferedIncompatibleQos);
	System::Runtime::InteropServices::GCHandle gchOfferedIncompatibleQos = System::Runtime::InteropServices::GCHandle::Alloc(fpOfferedIncompatibleQos);
	System::IntPtr ipOfferedIncompatibleQos = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpOfferedIncompatibleQos);	
	onOfferedIncompatibleQosFunctionCpp = static_cast<onOfferedIncompatibleQosDeclaration>(ipOfferedIncompatibleQos.ToPointer());

	onLivelinessLostDelegate^ fpLivelinessLost = gcnew onLivelinessLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onLivelinessLost);
	System::Runtime::InteropServices::GCHandle gchLivelinessLost = System::Runtime::InteropServices::GCHandle::Alloc(fpLivelinessLost);
	System::IntPtr ipLivelinessLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpLivelinessLost);	
	onLivelinessLostFunctionCpp = static_cast<onLivelinessLostDeclaration>(ipLivelinessLost.ToPointer());

	onPublicationMatchedDelegate^ fpPublicationMatched = gcnew onPublicationMatchedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onPublicationMatched);
	System::Runtime::InteropServices::GCHandle gchPublicationMatched = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationMatched);
	System::IntPtr ipPublicationMatched = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationMatched);	
	onPublicationMatchedFunctionCpp = static_cast<onPublicationMatchedDeclaration>(ipPublicationMatched.ToPointer());

	onPublicationDisconnectedDelegate^ fpPublicationDisconnected = gcnew onPublicationDisconnectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onPublicationDisconnected);
	System::Runtime::InteropServices::GCHandle gchPublicationDisconnected = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationDisconnected);
	System::IntPtr ipPublicationDisconnected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationDisconnected);	
	onPublicationDisconnectedFunctionCpp = static_cast<onPublicationDisconnectedDeclaration>(ipPublicationDisconnected.ToPointer());

	onPublicationReconnectedDelegate^ fpPublicationReconnected = gcnew onPublicationReconnectedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onPublicationReconnected);
	System::Runtime::InteropServices::GCHandle gchPublicationReconnected = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationReconnected);
	System::IntPtr ipPublicationReconnected = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationReconnected);	
	onPublicationReconnectedFunctionCpp = static_cast<onPublicationReconnectedDeclaration>(ipPublicationReconnected.ToPointer());

	onPublicationLostDelegate^ fpPublicationLost = gcnew onPublicationLostDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onPublicationLost);
	System::Runtime::InteropServices::GCHandle gchPublicationLost = System::Runtime::InteropServices::GCHandle::Alloc(fpPublicationLost);
	System::IntPtr ipPublicationLost = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpPublicationLost);	
	onPublicationLostFunctionCpp = static_cast<onPublicationLostDeclaration>(ipPublicationLost.ToPointer());

	onConnectionDeletedDelegate^ fpConnectionDeleted = gcnew onConnectionDeletedDelegate(this, &::OpenDDSharp::OpenDDS::DCPS::DataWriterListener::onConnectionDeleted);
	System::Runtime::InteropServices::GCHandle gchConnectionDeleted = System::Runtime::InteropServices::GCHandle::Alloc(fpConnectionDeleted);
	System::IntPtr ipConnectionDeleted = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(fpConnectionDeleted);	
	onConnectionDeletedFunctionCpp = static_cast<onConnectionDeletedDeclaration>(ipConnectionDeleted.ToPointer());


	impl_entity = new OpenDDSharp::OpenDDS::DCPS::DataWriterListenerNative(onOfferedDeadlineMissedFunctionCpp,
																		   onOfferedIncompatibleQosFunctionCpp,
																		   onLivelinessLostFunctionCpp,
																		   onPublicationMatchedFunctionCpp,
																		   onPublicationDisconnectedFunctionCpp,
																		   onPublicationReconnectedFunctionCpp,
																		   onPublicationLostFunctionCpp,
																		   onConnectionDeletedFunctionCpp);
}