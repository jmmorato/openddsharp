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
#pragma once

#pragma unmanaged
#include <dds/DCPS/DiscoveryBase.h>
#pragma managed

#include <SubscriptionBuiltinTopicData.h>
#include <ReturnCode.h>
#include <SampleInfo.h>
#include <ReadCondition.h>

using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {
		public ref class SubscriptionBuiltinTopicDataDataReader : public  OpenDDSharp::DDS::DataReader {

		public:
			static const System::String^ BUILT_IN_SUBSCRIPTION_TOPIC = "DCPSSubscription";
			static const System::String^ BUILT_IN_SUBSCRIPTION_TOPIC_TYPE = "SUBSCRIPTION_BUILT_IN_TOPIC_TYPE";

		private:			
			::DDS::SubscriptionBuiltinTopicDataDataReader_ptr impl_entity;

		public:
			SubscriptionBuiltinTopicDataDataReader(::OpenDDSharp::DDS::DataReader^ dataReader);

			OpenDDSharp::DDS::ReturnCode Read(List<SubscriptionBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo);

			OpenDDSharp::DDS::ReturnCode Read(List<SubscriptionBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode Read(List<SubscriptionBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples,
											  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode Read(List<SubscriptionBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples,
											  OpenDDSharp::DDS::SampleStateMask sampleStates,
											  OpenDDSharp::DDS::ViewStateMask viewStates,
											  OpenDDSharp::DDS::InstanceStateMask instanceStates);

			OpenDDSharp::DDS::ReturnCode Take(List<SubscriptionBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo);

			OpenDDSharp::DDS::ReturnCode Take(List<SubscriptionBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode Take(List<SubscriptionBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples,
											  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode Take(List<SubscriptionBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples,
											  OpenDDSharp::DDS::SampleStateMask sampleStates,
											  OpenDDSharp::DDS::ViewStateMask viewStates,
											  OpenDDSharp::DDS::InstanceStateMask instanceStates);

			OpenDDSharp::DDS::ReturnCode ReadInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle);

			OpenDDSharp::DDS::ReturnCode ReadInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode ReadInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples,
													  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode ReadInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples,
													  OpenDDSharp::DDS::SampleStateMask sampleStates,
													  OpenDDSharp::DDS::ViewStateMask viewStates,
													  OpenDDSharp::DDS::InstanceStateMask instanceStates);

			OpenDDSharp::DDS::ReturnCode TakeInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
												      List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												      OpenDDSharp::DDS::InstanceHandle handle);

			OpenDDSharp::DDS::ReturnCode TakeInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode TakeInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples,
													  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode TakeInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples,
													  OpenDDSharp::DDS::SampleStateMask sampleStates,
													  OpenDDSharp::DDS::ViewStateMask viewStates,
													  OpenDDSharp::DDS::InstanceStateMask instanceStates);

			OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle);

			OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples,
														  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples,
														  OpenDDSharp::DDS::SampleStateMask sampleStates,
														  OpenDDSharp::DDS::ViewStateMask viewStates,
														  OpenDDSharp::DDS::InstanceStateMask instanceStates);

			OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle);

			OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples,
														  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<SubscriptionBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples,
														  OpenDDSharp::DDS::SampleStateMask sampleStates,
														  OpenDDSharp::DDS::ViewStateMask viewStates,
														  OpenDDSharp::DDS::InstanceStateMask instanceStates);
			OpenDDSharp::DDS::ReturnCode ReadNextSample(SubscriptionBuiltinTopicData% data, ::OpenDDSharp::DDS::SampleInfo^ sampleInfo);

			OpenDDSharp::DDS::ReturnCode TakeNextSample(SubscriptionBuiltinTopicData% data, ::OpenDDSharp::DDS::SampleInfo^ sampleInfo);

			OpenDDSharp::DDS::ReturnCode GetKeyValue(SubscriptionBuiltinTopicData% data, OpenDDSharp::DDS::InstanceHandle handle);

			OpenDDSharp::DDS::InstanceHandle LookupInstance(SubscriptionBuiltinTopicData instance);
		};
	};
};
