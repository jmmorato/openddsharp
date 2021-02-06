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

::DDS::ParticipantBuiltinTopicData OpenDDSharp::DDS::ParticipantBuiltinTopicData::ToNative() {
	::DDS::ParticipantBuiltinTopicData native;

	native.key = key.ToNative();
	native.user_data = user_data->ToNative();

	return native;
}
