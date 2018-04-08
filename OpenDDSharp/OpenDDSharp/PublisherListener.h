#pragma once

#include "DataWriterListener.h"
#include "PublisherListenerNative.h"

namespace OpenDDSharp {
	namespace DDS {

		/// <summary>
		/// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="Publisher" />
		/// such that the application can be notified of relevant status changes.		
		/// <summary>
		public ref class PublisherListener abstract : public OpenDDSharp::OpenDDS::DCPS::DataWriterListener {

		internal:
			::OpenDDSharp::DDS::PublisherListenerNative* impl_entity;

		public:
			/// <summary>
			/// Creates a new instance of the <see cref="PublisherListener" />
			/// </summary>
			PublisherListener();

		};
	};
};

