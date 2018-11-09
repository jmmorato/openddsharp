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
#include "TransportRegistry.h"

OpenDDSharp::OpenDDS::DCPS::TransportRegistry^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::Instance::get() {
    return %_instance;
}

OpenDDSharp::OpenDDS::DCPS::TransportRegistry::TransportRegistry() {
    impl_entity = ::OpenDDS::DCPS::TransportRegistry::instance();
};

System::Boolean OpenDDSharp::OpenDDS::DCPS::TransportRegistry::Released::get() {
    return impl_entity->released();
};

OpenDDSharp::OpenDDS::DCPS::TransportConfig^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::GlobalConfig::get() {
    msclr::interop::marshal_context context;
    
    ::OpenDDS::DCPS::TransportConfig_rch native = impl_entity->global_config();
    if (!native.is_nil()) {
        TransportConfig^ config = TransportConfigManager::get_instance()->find(native.in());
        if (config == nullptr) {            
            ::OpenDDS::DCPS::TransportConfig* pointer = native.in();
            pointer->_add_ref();
            config = gcnew TransportConfig(pointer);
            TransportConfigManager::get_instance()->add(pointer, config);
        }

        return config;
    }

    return nullptr;
};

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::GlobalConfig::set(OpenDDSharp::OpenDDS::DCPS::TransportConfig^ value) {
    ::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportConfig>(value->impl_entity);
    impl_entity->global_config(rch);
};

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::Close() {
    ::OpenDDS::DCPS::TransportRegistry::close();
};

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::Release() {
    impl_entity->release();
};

OpenDDSharp::OpenDDS::DCPS::TransportInst^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::CreateInst(System::String^ name, System::String^ transportType) {
    msclr::interop::marshal_context context;

    ::OpenDDS::DCPS::TransportInst_rch native = impl_entity->create_inst(context.marshal_as<const char*>(name), context.marshal_as<const char*>(transportType));
    if (native.is_nil()) {
        return nullptr;
    }

    ::OpenDDS::DCPS::TransportInst* pointer = native.in();
    pointer->_add_ref();    
    OpenDDSharp::OpenDDS::DCPS::TransportInst^ inst = gcnew TransportInst(pointer);
    TransportInstManager::get_instance()->add(pointer, inst);

    return inst;
};

OpenDDSharp::OpenDDS::DCPS::TransportInst^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::GetInst(System::String^ name) {
    msclr::interop::marshal_context context;
    ::OpenDDS::DCPS::TransportInst_rch native = impl_entity->get_inst(context.marshal_as<const char*>(name));
    if (native.is_nil()) {
        return nullptr;
    }

    TransportInst^ inst = TransportInstManager::get_instance()->find(native.in());
    if (inst == nullptr) {
        ::OpenDDS::DCPS::TransportInst* pointer = native.in();
        pointer->_add_ref();
        inst = gcnew TransportInst(pointer);
        TransportInstManager::get_instance()->add(pointer, inst);
    }

    return inst;
}

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::RemoveInst(TransportInst^ inst) {
    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst->impl_entity);
    impl_entity->remove_inst(rch);
    TransportInstManager::get_instance()->remove(inst->impl_entity);
};

OpenDDSharp::OpenDDS::DCPS::TransportConfig^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::CreateConfig(System::String^ name) {
    msclr::interop::marshal_context context;

    ::OpenDDS::DCPS::TransportConfig_rch native = impl_entity->create_config(context.marshal_as<const char*>(name));
    if (native.is_nil()) {
        return nullptr;
    }

    ::OpenDDS::DCPS::TransportConfig* pointer = native.in();
    pointer->_add_ref();    
    OpenDDSharp::OpenDDS::DCPS::TransportConfig^ config = gcnew TransportConfig(pointer);
    TransportConfigManager::get_instance()->add(pointer, config);

    return config;
};

OpenDDSharp::OpenDDS::DCPS::TransportConfig^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::GetConfig(System::String^ name) {
    msclr::interop::marshal_context context;

    ::OpenDDS::DCPS::TransportConfig_rch native = impl_entity->get_config(context.marshal_as<const char*>(name));
    if (native.is_nil()) {
        return nullptr;
    }

    TransportConfig^ config = TransportConfigManager::get_instance()->find(native.in());
    if (config == nullptr) {        
        ::OpenDDS::DCPS::TransportConfig* pointer = native.in();
        pointer->_add_ref();
        config = gcnew TransportConfig(pointer);
        TransportConfigManager::get_instance()->add(pointer, config);
    }

    return config;
}

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::RemoveConfig(TransportConfig^ cfg) {
    ::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportConfig>(cfg->impl_entity);
    impl_entity->remove_config(rch);
    TransportConfigManager::get_instance()->remove(cfg->impl_entity);
};

OpenDDSharp::OpenDDS::DCPS::TransportConfig^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::GetDomainDefaultConfig(System::Int32 domain) {
    ::OpenDDS::DCPS::TransportConfig_rch native = impl_entity->domain_default_config(domain);
    if (native.is_nil()) {
        return nullptr;
    }

    OpenDDSharp::OpenDDS::DCPS::TransportConfig^ config = TransportConfigManager::get_instance()->find(native.in());
    if (config == nullptr) {        
        ::OpenDDS::DCPS::TransportConfig* pointer = native.in();
        pointer->_add_ref();
        config = gcnew TransportConfig(pointer);
        TransportConfigManager::get_instance()->add(pointer, config);
    }

    return config;
};

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::SetDomainDefaultConfig(System::Int32 domain, TransportConfig^ cfg) {
    ::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportConfig>(cfg->impl_entity);
    impl_entity->domain_default_config(domain, rch);
}

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::BindConfig(System::String^ name, ::OpenDDSharp::DDS::Entity^ entity) {
    msclr::interop::marshal_context context;
    impl_entity->bind_config(context.marshal_as<const char*>(name), entity->impl_entity);
}

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::BindConfig(TransportConfig^ cfg, ::OpenDDSharp::DDS::Entity^ entity) {
    ::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportConfig>(cfg->impl_entity);
    impl_entity->bind_config(rch, entity->impl_entity);
}