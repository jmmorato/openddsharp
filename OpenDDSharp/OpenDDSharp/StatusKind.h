#pragma once

#pragma unmanaged
#include "dds/DdsDcpsInfrastructureC.h"
#pragma managed

#include "StatusMask.h"

namespace OpenDDSharp {
	namespace DDS {
		/*public enum class StatusKind : System::UInt32 {
			InconsistentTopicStatus = ::DDS::INCONSISTENT_TOPIC_STATUS,
			OfferedDeadlineMissedStatus = ::DDS::OFFERED_DEADLINE_MISSED_STATUS,
			RequestedDeadlineMissedStatus = ::DDS::REQUESTED_DEADLINE_MISSED_STATUS,
			OfferedIncompatibleQosStatus = ::DDS::OFFERED_INCOMPATIBLE_QOS_STATUS,
			RequestedIncompatibleQosStatus = ::DDS::REQUESTED_INCOMPATIBLE_QOS_STATUS,
			SampleLostStatus = ::DDS::SAMPLE_LOST_STATUS,
			SampleRejectedStatus = ::DDS::SAMPLE_REJECTED_STATUS,
			DataOnReadersStatus = ::DDS::DATA_ON_READERS_STATUS,
			DataAvailableStatus = ::DDS::DATA_AVAILABLE_STATUS,
			LivelinessLostStatus = ::DDS::LIVELINESS_LOST_STATUS,
			LivelinessChangedStatus = ::DDS::LIVELINESS_CHANGED_STATUS,
			PublicationMatchedStatus = ::DDS::PUBLICATION_MATCHED_STATUS,
			SubscriptionMatchedStatus = ::DDS::SUBSCRIPTION_MATCHED_STATUS
		};*/

		public value struct StatusKind {

		public:
			static const StatusKind InconsistentTopicStatus = ::DDS::INCONSISTENT_TOPIC_STATUS;
			static const StatusKind OfferedDeadlineMissedStatus = ::DDS::OFFERED_DEADLINE_MISSED_STATUS;
			static const StatusKind RequestedDeadlineMissedStatus = ::DDS::REQUESTED_DEADLINE_MISSED_STATUS;
			static const StatusKind OfferedIncompatibleQosStatus = ::DDS::OFFERED_INCOMPATIBLE_QOS_STATUS;
			static const StatusKind RequestedIncompatibleQosStatus = ::DDS::REQUESTED_INCOMPATIBLE_QOS_STATUS;
			static const StatusKind SampleLostStatus = ::DDS::SAMPLE_LOST_STATUS;
			static const StatusKind SampleRejectedStatus = ::DDS::SAMPLE_REJECTED_STATUS;
			static const StatusKind DataOnReadersStatus = ::DDS::DATA_ON_READERS_STATUS;
			static const StatusKind DataAvailableStatus = ::DDS::DATA_AVAILABLE_STATUS;
			static const StatusKind LivelinessLostStatus = ::DDS::LIVELINESS_LOST_STATUS;
			static const StatusKind LivelinessChangedStatus = ::DDS::LIVELINESS_CHANGED_STATUS;
			static const StatusKind PublicationMatchedStatus = ::DDS::PUBLICATION_MATCHED_STATUS;
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