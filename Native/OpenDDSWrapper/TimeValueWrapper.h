/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
#pragma once

#include <dds/DCPS/TimeDuration.h>

#pragma pack(push)
#pragma pack(1)
extern "C" struct TimeValueWrapper {
    ::CORBA::LongLong sec = 0;
    ::CORBA::Long microsec = 0;

public:

    TimeValueWrapper(const ::OpenDDS::DCPS::TimeDuration td) {
      sec = td.value().sec();
      microsec = td.value().usec();
    }

    operator ::OpenDDS::DCPS::TimeDuration() const {
      return ::OpenDDS::DCPS::TimeDuration(sec, microsec);
    }
};
#pragma pack(pop)
