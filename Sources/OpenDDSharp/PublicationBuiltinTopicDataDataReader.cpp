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

#include "PublicationBuiltinTopicDataDataReader.h"

OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::PublicationBuiltinTopicDataDataReader(::OpenDDSharp::DDS::DataReader^ dataReader) : OpenDDSharp::DDS::DataReader(dataReader->impl_entity) {
	impl_entity = ::DDS::PublicationBuiltinTopicDataDataReader::_narrow(dataReader->impl_entity);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Read(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo) {
	return OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Read(receivedData, receivedInfo, ::DDS::LENGTH_UNLIMITED, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Read(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, System::Int32 maxSamples) {
	return OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Read(receivedData, receivedInfo, maxSamples, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Read(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, System::Int32 maxSamples, OpenDDSharp::DDS::ReadCondition^ condition) {
	if (condition == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();
    
	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->read_w_condition(received_data, info_seq, maxSamples, condition->impl_entity);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Read(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, System::Int32 maxSamples, OpenDDSharp::DDS::SampleStateMask sampleStates, OpenDDSharp::DDS::ViewStateMask viewStates, OpenDDSharp::DDS::InstanceStateMask instanceStates) {
    if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();
    
	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->read(received_data, info_seq, maxSamples, sampleStates, viewStates, instanceStates);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Take(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo) {
	return OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Take(receivedData, receivedInfo, ::DDS::LENGTH_UNLIMITED, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Take(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, System::Int32 maxSamples) {
	return OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Take(receivedData, receivedInfo, maxSamples, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Take(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, System::Int32 maxSamples, OpenDDSharp::DDS::ReadCondition^ condition) {
	if (condition == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();

	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->take_w_condition(received_data, info_seq, maxSamples, condition->impl_entity);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::Take(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, System::Int32 maxSamples, OpenDDSharp::DDS::SampleStateMask sampleStates, OpenDDSharp::DDS::ViewStateMask viewStates, OpenDDSharp::DDS::InstanceStateMask instanceStates) {
    if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();

	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->take(received_data, info_seq, maxSamples, sampleStates, viewStates, instanceStates);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle handle) {
	return  OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadInstance(receivedData, receivedInfo, handle, ::DDS::LENGTH_UNLIMITED, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle handle, System::Int32 maxSamples) {
	return  OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadInstance(receivedData, receivedInfo, handle, maxSamples, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle handle, System::Int32 maxSamples, OpenDDSharp::DDS::ReadCondition^ condition) {
	if (condition == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	receivedData->Clear();
	receivedInfo->Clear();

	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->read_instance_w_condition(received_data, info_seq, maxSamples, handle, condition->impl_entity);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle handle, System::Int32 maxSamples, OpenDDSharp::DDS::SampleStateMask sampleStates, OpenDDSharp::DDS::ViewStateMask viewStates, OpenDDSharp::DDS::InstanceStateMask instanceStates) {
    if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();

	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->read_instance(received_data, info_seq, maxSamples, handle, sampleStates, viewStates, instanceStates);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle handle) {
	return  OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeInstance(receivedData, receivedInfo, handle, ::DDS::LENGTH_UNLIMITED, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle handle, System::Int32 maxSamples) {
	return  OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeInstance(receivedData, receivedInfo, handle, maxSamples, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle handle, System::Int32 maxSamples, OpenDDSharp::DDS::ReadCondition^ condition) {
	if (condition == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();

	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->take_instance_w_condition(received_data, info_seq, maxSamples, handle, condition->impl_entity);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle handle, System::Int32 maxSamples, OpenDDSharp::DDS::SampleStateMask sampleStates, OpenDDSharp::DDS::ViewStateMask viewStates, OpenDDSharp::DDS::InstanceStateMask instanceStates) {
    if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();

	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->take_instance(received_data, info_seq, maxSamples, handle, sampleStates, viewStates, instanceStates);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadNextInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle previousHandle) {
	return  OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadNextInstance(receivedData, receivedInfo, previousHandle, ::DDS::LENGTH_UNLIMITED, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadNextInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle previousHandle, System::Int32 maxSamples) {
	return  OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadNextInstance(receivedData, receivedInfo, previousHandle, maxSamples, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadNextInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle previousHandle, System::Int32 maxSamples, OpenDDSharp::DDS::ReadCondition^ condition) {
	if (condition == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();

	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->read_next_instance_w_condition(received_data, info_seq, maxSamples, previousHandle, condition->impl_entity);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadNextInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle previousHandle, System::Int32 maxSamples, OpenDDSharp::DDS::SampleStateMask sampleStates, OpenDDSharp::DDS::ViewStateMask viewStates, OpenDDSharp::DDS::InstanceStateMask instanceStates) {
    if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();

	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->read_next_instance(received_data, info_seq, maxSamples, previousHandle, sampleStates, viewStates, instanceStates);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeNextInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle previousHandle) {
	return  OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeNextInstance(receivedData, receivedInfo, previousHandle, ::DDS::LENGTH_UNLIMITED, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeNextInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle previousHandle, System::Int32 maxSamples) {
	return  OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeNextInstance(receivedData, receivedInfo, previousHandle, maxSamples, OpenDDSharp::DDS::SampleStateMask::AnySampleState, OpenDDSharp::DDS::ViewStateMask::AnyViewState, OpenDDSharp::DDS::InstanceStateMask::AnyInstanceState);
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeNextInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle previousHandle, System::Int32 maxSamples, OpenDDSharp::DDS::ReadCondition^ condition) {
	if (condition == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

	if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();

	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->take_next_instance_w_condition(received_data, info_seq, maxSamples, previousHandle, condition->impl_entity);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeNextInstance(List<PublicationBuiltinTopicData>^ receivedData, List<::OpenDDSharp::DDS::SampleInfo^>^ receivedInfo, OpenDDSharp::DDS::InstanceHandle previousHandle, System::Int32 maxSamples, OpenDDSharp::DDS::SampleStateMask sampleStates, OpenDDSharp::DDS::ViewStateMask viewStates, OpenDDSharp::DDS::InstanceStateMask instanceStates) {
    if (receivedData == nullptr || receivedInfo == nullptr) {
		return OpenDDSharp::DDS::ReturnCode::BadParameter;
	}

    receivedData->Clear();
	receivedInfo->Clear();

	::DDS::PublicationBuiltinTopicDataSeq received_data;
	::DDS::SampleInfoSeq info_seq;
	::DDS::ReturnCode_t ret = impl_entity->take_next_instance(received_data, info_seq, maxSamples, previousHandle, sampleStates, viewStates, instanceStates);

	if (ret == ::DDS::RETCODE_OK) {
		for (unsigned int i = 0; i < received_data.length(); i++) {
			OpenDDSharp::DDS::PublicationBuiltinTopicData data;
			::OpenDDSharp::DDS::SampleInfo^ sampleInfo = gcnew ::OpenDDSharp::DDS::SampleInfo();

			data.FromNative(received_data[i]);
			sampleInfo->FromNative(info_seq[i]);

			receivedData->Add(data);
			receivedInfo->Add(sampleInfo);
		}
	}

    impl_entity->return_loan(received_data, info_seq);

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::ReadNextSample(PublicationBuiltinTopicData% data, ::OpenDDSharp::DDS::SampleInfo^ sampleInfo) {
    ::DDS::PublicationBuiltinTopicData aux;
    ::DDS::SampleInfo sample_info;
	::DDS::ReturnCode_t ret = impl_entity->read_next_sample(aux, sample_info);

    if (ret == ::DDS::RETCODE_OK) {
	    data.FromNative(aux);
        sampleInfo->FromNative(sample_info);
    }

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::TakeNextSample(PublicationBuiltinTopicData% data, ::OpenDDSharp::DDS::SampleInfo^ sampleInfo) {
    ::DDS::PublicationBuiltinTopicData aux;	
    ::DDS::SampleInfo sample_info;
	::DDS::ReturnCode_t ret = impl_entity->take_next_sample(aux, sample_info);

    if (ret == ::DDS::RETCODE_OK) {
	    data.FromNative(aux);
        sampleInfo->FromNative(sample_info);
    }

	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::ReturnCode OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::GetKeyValue(PublicationBuiltinTopicData% data, OpenDDSharp::DDS::InstanceHandle handle) {
	::DDS::PublicationBuiltinTopicData aux;
    ::DDS::ReturnCode_t ret = impl_entity->get_key_value(aux, handle);
    if (ret == ::DDS::RETCODE_OK) {
        data.FromNative(aux);
    }
	return (OpenDDSharp::DDS::ReturnCode)ret;
};

OpenDDSharp::DDS::InstanceHandle OpenDDSharp::DDS::PublicationBuiltinTopicDataDataReader::LookupInstance(PublicationBuiltinTopicData instance) {
	return impl_entity->lookup_instance(instance.ToNative());
};