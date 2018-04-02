#pragma once

#include "DataWriterListener.h"
#include "PublisherListenerNative.h"

namespace OpenDDSharp {
	namespace DDS {		
		public ref class PublisherListener abstract : public OpenDDSharp::OpenDDS::DCPS::DataWriterListener {

		internal:
			::OpenDDSharp::DDS::PublisherListenerNative* impl_entity;

		public:
			PublisherListener();

		};
	};
};

