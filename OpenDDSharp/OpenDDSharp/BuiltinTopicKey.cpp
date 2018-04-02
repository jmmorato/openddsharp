#include "BuiltinTopicKey.h"

array<System::Int32, 1>^ OpenDDSharp::DDS::BuiltinTopicKey::Value::get() {
	return m_value;
};

void OpenDDSharp::DDS::BuiltinTopicKey::FromNative(::DDS::BuiltinTopicKey_t native) {
	m_value = gcnew array<System::Int32, 1>(3);
	for (int i = 0; i < 3; i++) {
		m_value[i] = native.value[i];
	}
}
