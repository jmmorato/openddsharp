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