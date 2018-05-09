#pragma once

#pragma unmanaged
#include "dds/DdsDcpsInfrastructureC.h"
#pragma managed

#include "StatusMask.h"

namespace OpenDDSharp {
	namespace DDS {

		ref class DataWriter;
		ref class DataReader;
		ref class Topic;
		ref class DeadlineQosPolicy;
		ref class LivelinessQosPolicy; 

		/// <summary>
		/// Kinds of communication status.
		/// </summary>
		public value struct StatusKind {

		public:
			/// <summary>
			/// Another topic exists with the same name but different characteristics.
			/// </summary>
			static const StatusKind InconsistentTopicStatus = ::DDS::INCONSISTENT_TOPIC_STATUS;

			/// <summary>
			/// The deadline that the <see cref="DataWriter" /> has committed through its <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.
			/// </summary>
			static const StatusKind OfferedDeadlineMissedStatus = ::DDS::OFFERED_DEADLINE_MISSED_STATUS;

			/// <summary>
			/// The deadline that the <see cref="DataReader" /> was expecting through its <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.
			/// </summary>
			static const StatusKind RequestedDeadlineMissedStatus = ::DDS::REQUESTED_DEADLINE_MISSED_STATUS;

			/// <summary>
			/// A QoS policy value was incompatible with what was requested.
			/// </summary>
			static const StatusKind OfferedIncompatibleQosStatus = ::DDS::OFFERED_INCOMPATIBLE_QOS_STATUS;

			/// <summary>
			/// A QoS policy value was incompatible with what is offered.
			/// </summary>
			static const StatusKind RequestedIncompatibleQosStatus = ::DDS::REQUESTED_INCOMPATIBLE_QOS_STATUS;

			/// <summary>
			/// A sample has been lost (i.e. was never received).
			/// </summary>
			static const StatusKind SampleLostStatus = ::DDS::SAMPLE_LOST_STATUS;

			/// <summary>
			/// A (received) sample has been rejected.
			/// </summary>
			static const StatusKind SampleRejectedStatus = ::DDS::SAMPLE_REJECTED_STATUS;

			/// <summary>
			/// New data is available.
			/// </summary>
			static const StatusKind DataOnReadersStatus = ::DDS::DATA_ON_READERS_STATUS;

			/// <summary>
			/// One or more new data samples have been received.
			/// </summary>
			static const StatusKind DataAvailableStatus = ::DDS::DATA_AVAILABLE_STATUS;

			/// <summary>
			/// The liveliness that the <see cref="DataWriter" /> has committed to through its  <see cref="LivelinessQosPolicy" /> was not respected, 
			/// thus <see cref="DataReader" /> entities will consider the <see cref="DataWriter" /> as no longer alive.
			/// </summary>
			static const StatusKind LivelinessLostStatus = ::DDS::LIVELINESS_LOST_STATUS;

			/// <summary>
			/// The liveliness of one or more <see cref="DataWriter" /> that were writing instances read through the <see cref="DataReader" /> has changed. 
			/// Some <see cref="DataWriter" /> have become alive or not alive.
			/// </summary>
			static const StatusKind LivelinessChangedStatus = ::DDS::LIVELINESS_CHANGED_STATUS;

			/// <summary>
			/// The <see cref="DataWriter" /> has found <see cref="DataReader" /> that matches the <see cref="Topic" /> and has compatible QoS.
			/// </summary>
			static const StatusKind PublicationMatchedStatus = ::DDS::PUBLICATION_MATCHED_STATUS;

			/// <summary>
			/// The <see cref="DataReader" /> has found <see cref="DataWriter" /> that matches the <see cref="Topic" /> and has compatible QoS.
			/// </summary>
			static const StatusKind SubscriptionMatchedStatus = ::DDS::SUBSCRIPTION_MATCHED_STATUS;			

		private:
			System::UInt32 m_value;

		internal:
			StatusKind(System::UInt32 value);

		public:
			static operator System::UInt32(StatusKind self) {
				return self.m_value;
			}

			static operator StatusKind(System::UInt32 value) {
				StatusKind r(value);
				return r;
			}

			static operator StatusMask(StatusKind value) {
				StatusMask r(value);
				return r;
			}

			static StatusMask operator  | (StatusKind a, StatusKind b) {
				return static_cast<StatusMask>(static_cast<unsigned int>(a) | static_cast<unsigned int>(b));
			}

		};
	};
};