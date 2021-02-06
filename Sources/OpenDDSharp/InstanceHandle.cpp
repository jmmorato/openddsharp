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
#include "InstanceHandle.h"

OpenDDSharp::DDS::InstanceHandle::InstanceHandle(System::Int32 value) {
	m_value = value;
};

bool OpenDDSharp::DDS::InstanceHandle::Equals(System::Object^ other) {
    if (other == nullptr) {        
        return false;
    }

    if (GetType() != other->GetType() && other->GetType() != System::Int32::typeid) {
        return false;
    }

    InstanceHandle aux = InstanceHandle::HandleNil;
    if (other->GetType() == System::Int32::typeid) {        
        aux = (System::Int32)other;
    }
    else {
        aux = (InstanceHandle)other;
    }
    
    return (m_value == aux.m_value);
}

int OpenDDSharp::DDS::InstanceHandle::GetHashCode()
{
    return m_value;
}