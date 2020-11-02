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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
#include "Topic.h"

::DDS::Entity_ptr Topic_NarrowBase(::DDS::Topic_ptr t) {
	return static_cast<::DDS::Entity_ptr>(t);
}

::DDS::TopicDescription_ptr Topic_NarrowTopicDescription(::DDS::Topic_ptr t) {
	return static_cast<::DDS::TopicDescription_ptr>(t);
}

::DDS::ReturnCode_t Topic_GetQos(::DDS::Topic_ptr t, TopicQosWrapper& qos_wrapper) {
    ::DDS::TopicQos qos_native;
    ::DDS::ReturnCode_t ret = t->get_qos(qos_native);

    if (ret == ::DDS::RETCODE_OK) {
        qos_wrapper = qos_native;
    }

    return ret;
}

::DDS::ReturnCode_t Topic_SetQos(::DDS::Topic_ptr t, TopicQosWrapper qos_wrapper) {
    return t->set_qos(qos_wrapper);
}

::DDS::ReturnCode_t Topic_SetListener(::DDS::Topic_ptr t, ::DDS::TopicListener_ptr listener, ::DDS::StatusMask status) {
    return t->set_listener(listener, status);
}

char* Topic_GetTypeName(::DDS::Topic_ptr t) {
    return t->get_type_name();
}

char* Topic_GetName(::DDS::Topic_ptr t) {
    return t->get_name();
}