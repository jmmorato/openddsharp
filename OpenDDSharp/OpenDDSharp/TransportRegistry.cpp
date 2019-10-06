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

OpenDDSharp::OpenDDS::DCPS::TransportRegistry::TransportRegistry() { };

System::Boolean OpenDDSharp::OpenDDS::DCPS::TransportRegistry::Released::get() {
    return ::OpenDDS::DCPS::TransportRegistry::instance()->released();
};

OpenDDSharp::OpenDDS::DCPS::TransportConfig^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::GlobalConfig::get() {
    msclr::interop::marshal_context context;
    
    ::OpenDDS::DCPS::TransportConfig_rch native = ::OpenDDS::DCPS::TransportRegistry::instance()->global_config();
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
    if (value == nullptr) {
        throw gcnew System::ArgumentNullException("value", "The GlobalConfig cannot be null");
    }
    ::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportConfig>(value->impl_entity);
	::OpenDDS::DCPS::TransportRegistry::instance()->global_config(rch);
};

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::Close() {
    ::OpenDDS::DCPS::TransportRegistry::close();
};

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::Release() {
	::OpenDDS::DCPS::TransportRegistry::instance()->release();
};

OpenDDSharp::OpenDDS::DCPS::TransportInst^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::CreateInst(System::String^ name, System::String^ transportType) {
    if (System::String::IsNullOrWhiteSpace(name)) {
        throw gcnew System::ArgumentNullException("name", "The transport instance's name cannot be null or an empty string.");
    }
    msclr::interop::marshal_context context;

    ::OpenDDS::DCPS::TransportInst_rch native = ::OpenDDS::DCPS::TransportRegistry::instance()->create_inst(context.marshal_as<const char*>(name), context.marshal_as<const char*>(transportType));
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
    if (System::String::IsNullOrWhiteSpace(name)) {
        return nullptr;
    }

    msclr::interop::marshal_context context;
    ::OpenDDS::DCPS::TransportInst_rch native = ::OpenDDS::DCPS::TransportRegistry::instance()->get_inst(context.marshal_as<const char*>(name));
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
    if (inst == nullptr) {
        return;
    }

    ::OpenDDS::DCPS::TransportInst_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportInst>(inst->impl_entity);
	::OpenDDS::DCPS::TransportRegistry::instance()->remove_inst(rch);
    TransportInstManager::get_instance()->remove(inst->impl_entity);
};

OpenDDSharp::OpenDDS::DCPS::TransportConfig^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::CreateConfig(System::String^ name) {
    if (System::String::IsNullOrWhiteSpace(name)) {
        throw gcnew System::ArgumentNullException("name", "The transport config's name cannot be null or an empty string.");
    }

    msclr::interop::marshal_context context;
    
    ::OpenDDS::DCPS::TransportConfig_rch native = ::OpenDDS::DCPS::TransportRegistry::instance()->create_config(context.marshal_as<const char*>(name));
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
    if (System::String::IsNullOrWhiteSpace(name)) {
        return nullptr;
    }

    msclr::interop::marshal_context context;

    ::OpenDDS::DCPS::TransportConfig_rch native = ::OpenDDS::DCPS::TransportRegistry::instance()->get_config(context.marshal_as<const char*>(name));
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
    if (cfg == nullptr) {
        return;
    }

    ::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportConfig>(cfg->impl_entity);
	::OpenDDS::DCPS::TransportRegistry::instance()->remove_config(rch);
    TransportConfigManager::get_instance()->remove(cfg->impl_entity);
};

OpenDDSharp::OpenDDS::DCPS::TransportConfig^ OpenDDSharp::OpenDDS::DCPS::TransportRegistry::GetDomainDefaultConfig(System::Int32 domain) {
    ::OpenDDS::DCPS::TransportConfig_rch native = ::OpenDDS::DCPS::TransportRegistry::instance()->domain_default_config(domain);
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
    if (domain < 0) {
        throw gcnew System::ArgumentOutOfRangeException("domain", "The domain must be greater or equal to zero.");
    }

    if (cfg == nullptr) {
        throw gcnew System::ArgumentNullException("cfg", "The transport configuration cannot be null.");
    }

    ::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportConfig>(cfg->impl_entity);
	::OpenDDS::DCPS::TransportRegistry::instance()->domain_default_config(domain, rch);
}

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::BindConfig(System::String^ name, ::OpenDDSharp::DDS::Entity^ entity) {
    if (System::String::IsNullOrWhiteSpace(name)) {
        throw gcnew System::ArgumentNullException("name", "The transport config's name cannot be null or an empty string.");
    }

    if (entity == nullptr) {
        throw gcnew System::ArgumentNullException("entity", "The entity parameter cannot be null");
    }

    msclr::interop::marshal_context context;
	::OpenDDS::DCPS::TransportRegistry::instance()->bind_config(context.marshal_as<const char*>(name), entity->impl_entity);
}

void OpenDDSharp::OpenDDS::DCPS::TransportRegistry::BindConfig(TransportConfig^ cfg, ::OpenDDSharp::DDS::Entity^ entity) {
    if (cfg == nullptr) {
        throw gcnew System::ArgumentNullException("entity", "The transport config parameter cannot be null");
    }

    if (entity == nullptr) {
        throw gcnew System::ArgumentNullException("entity", "The entity parameter cannot be null");
    }

    ::OpenDDS::DCPS::TransportConfig_rch rch = ::OpenDDS::DCPS::rchandle_from<::OpenDDS::DCPS::TransportConfig>(cfg->impl_entity);
	::OpenDDS::DCPS::TransportRegistry::instance()->bind_config(rch, entity->impl_entity);
}