#include "PartitionQosPolicy.h"

OpenDDSharp::DDS::PartitionQosPolicy::PartitionQosPolicy() {
	name = gcnew List<System::String^>();
};

IEnumerable<System::String^>^ OpenDDSharp::DDS::PartitionQosPolicy::Name::get() {
	return name;
};

void OpenDDSharp::DDS::PartitionQosPolicy::Name::set(IEnumerable<System::String^>^ value) {
	name = value;
};

::DDS::PartitionQosPolicy OpenDDSharp::DDS::PartitionQosPolicy::ToNative() {
	msclr::interop::marshal_context context;

	if (name == nullptr) {
		name = gcnew List<System::String^>();
	}

	::DDS::PartitionQosPolicy* qos = new ::DDS::PartitionQosPolicy();

	int count = System::Linq::Enumerable::Count(name);
	qos->name.length(count);

	int i = 0;
	while (i < count) {
		System::String^ str = System::Linq::Enumerable::ElementAt(name, i);
		qos->name[i] = context.marshal_as<const char*>(str);
		i++;
	}

	return *qos;
};

void OpenDDSharp::DDS::PartitionQosPolicy::FromNative(::DDS::PartitionQosPolicy qos) {
	msclr::interop::marshal_context context;

	List<System::String^>^ list = gcnew List<System::String^>();
	int length = qos.name.length();
	int i = 0;
	while (i < length) {
		const char * str = qos.name[i];
		list->Add(context.marshal_as<System::String^>(str));
		i++;
	}

	name = list;
};