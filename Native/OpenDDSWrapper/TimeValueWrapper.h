/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 - 2022 Jose Morato

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
#pragma once

#include "Utils.h"
#include "dds/DCPS/TimeDuration.h"

EXTERN_STRUCT_EXPORT __attribute__ ((__packed__)) TimeValueWrapper {
    long seconds;
    int microseconds;

public:

    TimeValueWrapper() {
      seconds = 0;
      microseconds = 0;
    }

    TimeValueWrapper(const ::OpenDDS::DCPS::TimeDuration td) {
      seconds = td.value().sec();
      microseconds = td.value().usec();
    }

    operator ::OpenDDS::DCPS::TimeDuration() const {
      return ::OpenDDS::DCPS::TimeDuration(seconds, microseconds);
    }
};
