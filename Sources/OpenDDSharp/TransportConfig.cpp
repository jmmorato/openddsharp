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
#include "TransportConfig.h"

OpenDDSharp::OpenDDS::DCPS::TransportConfig::TransportConfig(::OpenDDS::DCPS::TransportConfig* native) {
    impl_entity = native;
};

System::String^ OpenDDSharp::OpenDDS::DCPS::TransportConfig::Name::get() {
    msclr::interop::marshal_context context;
    return context.marshal_as<System::String^>(impl_entity->name().c_str());
};

System::Collections::Generic::IReadOnlyCollection<OpenDDSharp::OpenDDS::DCPS::TransportInst^>^ OpenDDSharp::OpenDDS::DCPS::TransportConfig::Transports::get() {
    List<TransportInst^>^ list = gcnew List<TransportInst^>();
    for (auto inst = impl_entity->instances_.begin(); inst != impl_entity->instances_.end(); ++inst) {
        TransportInst^ managed = TransportInstManager::get_instance()->find(inst->in());
        if (managed == nullptr) {
            ::OpenDDS::DCPS::TransportInst* pointer = inst->in();
            pointer->_add_ref();
            managed = gcnew TransportInst(pointer);
            TransportInstManager::get_instance()->add(pointer, managed);
        }

        list->Add(managed);
    }
    return list->AsReadOnly();    
};

System::Boolean OpenDDSharp::OpenDDS::DCPS::TransportConfig::SwapBytes::get() {
    return impl_entity->swap_bytes_;
};

void OpenDDSharp::OpenDDS::DCPS::TransportConfig::SwapBytes::set(System::Boolean value) {
    impl_entity->swap_bytes_ = value;
};

System::UInt32 OpenDDSharp::OpenDDS::DCPS::TransportConfig::PassiveConnectDuration::get() {
    return impl_entity->passive_connect_duration_;
};

void OpenDDSharp::OpenDDS::DCPS::TransportConfig::PassiveConnectDuration::set(System::UInt32 value) {
    impl_entity->passive_connect_duration_ = value;
};

void OpenDDSharp::OpenDDS::DCPS::TransportConfig::Insert(TransportInst^ inst) {
    if (inst == nullptr) {
        throw gcnew System::ArgumentNullException("inst", "Transport instance parameter cannot be null");
    }

    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst->impl_entity);    
    impl_entity->instances_.push_back(rch);
};

void OpenDDSharp::OpenDDS::DCPS::TransportConfig::SortedInsert(TransportInst^ inst) {
    if (inst == nullptr) {
        throw gcnew System::ArgumentNullException("inst", "Transport instance parameter cannot be null");
    }

    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst->impl_entity);
    impl_entity->sorted_insert(rch);
};