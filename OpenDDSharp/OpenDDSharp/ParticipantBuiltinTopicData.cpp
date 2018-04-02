#include "ParticipantBuiltinTopicData.h"

OpenDDSharp::DDS::BuiltinTopicKey OpenDDSharp::DDS::ParticipantBuiltinTopicData::Key::get() {
	return key;
};

OpenDDSharp::DDS::UserDataQosPolicy^ OpenDDSharp::DDS::ParticipantBuiltinTopicData::UserData::get() {
	return user_data;
};

void OpenDDSharp::DDS::ParticipantBuiltinTopicData::FromNative(::DDS::ParticipantBuiltinTopicData native) {
	key.FromNative(native.key);
	user_data = gcnew OpenDDSharp::DDS::UserDataQosPolicy();
	user_data->FromNative(native.user_data);
}
