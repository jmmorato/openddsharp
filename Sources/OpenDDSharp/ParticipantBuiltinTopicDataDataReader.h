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
#pragma once

#pragma unmanaged
#include <dds/DCPS/DiscoveryBase.h>
#pragma managed

#include <ParticipantBuiltinTopicData.h>
#include <ReturnCode.h>
#include <SampleInfo.h>
#include <ReadCondition.h>

using namespace System::Collections::Generic;

namespace OpenDDSharp {
	namespace DDS {
		public ref class ParticipantBuiltinTopicDataDataReader : public  OpenDDSharp::DDS::DataReader {

		public:
			static const System::String^ BUILT_IN_PARTICIPANT_TOPIC = "DCPSParticipant";
			static const System::String^ BUILT_IN_PARTICIPANT_TOPIC_TYPE = "PARTICIPANT_BUILT_IN_TOPIC_TYPE";

		private:			
			::DDS::ParticipantBuiltinTopicDataDataReader_ptr impl_entity;

		public:
			ParticipantBuiltinTopicDataDataReader(::OpenDDSharp::DDS::DataReader^ dataReader);

			OpenDDSharp::DDS::ReturnCode Read(List<ParticipantBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo);

			OpenDDSharp::DDS::ReturnCode Read(List<ParticipantBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode Read(List<ParticipantBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples,
											  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode Read(List<ParticipantBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples,
											  OpenDDSharp::DDS::SampleStateMask sampleStates,
											  OpenDDSharp::DDS::ViewStateMask viewStates,
											  OpenDDSharp::DDS::InstanceStateMask instanceStates);

			OpenDDSharp::DDS::ReturnCode Take(List<ParticipantBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo);

			OpenDDSharp::DDS::ReturnCode Take(List<ParticipantBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode Take(List<ParticipantBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples,
											  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode Take(List<ParticipantBuiltinTopicData>^ receivedData,
											  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
											  System::Int32 maxSamples,
											  OpenDDSharp::DDS::SampleStateMask sampleStates,
											  OpenDDSharp::DDS::ViewStateMask viewStates,
											  OpenDDSharp::DDS::InstanceStateMask instanceStates);

			OpenDDSharp::DDS::ReturnCode ReadInstance(List<ParticipantBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle);

			OpenDDSharp::DDS::ReturnCode ReadInstance(List<ParticipantBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode ReadInstance(List<ParticipantBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples,
													  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode ReadInstance(List<ParticipantBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples,
													  OpenDDSharp::DDS::SampleStateMask sampleStates,
													  OpenDDSharp::DDS::ViewStateMask viewStates,
													  OpenDDSharp::DDS::InstanceStateMask instanceStates);

			OpenDDSharp::DDS::ReturnCode TakeInstance(List<ParticipantBuiltinTopicData>^ receivedData,
												      List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
												      OpenDDSharp::DDS::InstanceHandle handle);

			OpenDDSharp::DDS::ReturnCode TakeInstance(List<ParticipantBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode TakeInstance(List<ParticipantBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples,
													  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode TakeInstance(List<ParticipantBuiltinTopicData>^ receivedData,
													  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
													  OpenDDSharp::DDS::InstanceHandle handle,
													  System::Int32 maxSamples,
													  OpenDDSharp::DDS::SampleStateMask sampleStates,
													  OpenDDSharp::DDS::ViewStateMask viewStates,
													  OpenDDSharp::DDS::InstanceStateMask instanceStates);

			OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<ParticipantBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle);

			OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<ParticipantBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<ParticipantBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples,
														  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode ReadNextInstance(List<ParticipantBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples,
														  OpenDDSharp::DDS::SampleStateMask sampleStates,
														  OpenDDSharp::DDS::ViewStateMask viewStates,
														  OpenDDSharp::DDS::InstanceStateMask instanceStates);

			OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<ParticipantBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle);

			OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<ParticipantBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples);

			OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<ParticipantBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples,
														  OpenDDSharp::DDS::ReadCondition^ condition);

			OpenDDSharp::DDS::ReturnCode TakeNextInstance(List<ParticipantBuiltinTopicData>^ receivedData,
														  List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo,
														  OpenDDSharp::DDS::InstanceHandle previousHandle,
														  System::Int32 maxSamples,
														  OpenDDSharp::DDS::SampleStateMask sampleStates,
														  OpenDDSharp::DDS::ViewStateMask viewStates,
														  OpenDDSharp::DDS::InstanceStateMask instanceStates);
			OpenDDSharp::DDS::ReturnCode ReadNextSample(ParticipantBuiltinTopicData% data, ::OpenDDSharp::DDS::SampleInfo^ sampleInfo);

			OpenDDSharp::DDS::ReturnCode TakeNextSample(ParticipantBuiltinTopicData% data, ::OpenDDSharp::DDS::SampleInfo^ sampleInfo);

			OpenDDSharp::DDS::ReturnCode GetKeyValue(ParticipantBuiltinTopicData% data, OpenDDSharp::DDS::InstanceHandle handle);

			OpenDDSharp::DDS::InstanceHandle LookupInstance(ParticipantBuiltinTopicData instance);
		};
	};
};
