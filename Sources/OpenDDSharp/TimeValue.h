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
#pragma once

namespace OpenDDSharp {

    /// <summary>
    /// Structure for time value representation
    /// </summary>
    public value struct TimeValue {

    private:
        System::Int64 sec;
        long microsec;

    public:
        /// <summary>
        /// The seconds.
        /// </summary>
        property System::Int64 Seconds {
            System::Int64 get() { return sec; }
            void set(System::Int64 value) { sec = value; }
        };

        /// <summary>
        /// The microseconds.
        /// </summary>
        property long MicroSeconds {
            long get() { return microsec; }
            void set(long value) { microsec = value; }
        };

    internal:
        static operator TimeValue(ACE_Time_Value ace) {
            TimeValue t;
            t.Seconds = ace.sec();
            t.MicroSeconds = ace.usec();
            return t;
        }

        static operator ACE_Time_Value(TimeValue t) {
            return ACE_Time_Value(t.Seconds, t.MicroSeconds);
        }

        static operator TimeValue(::OpenDDS::DCPS::TimeDuration td) {
            TimeValue t;
            t.Seconds = td.value().sec();
            t.MicroSeconds = td.value().usec();
            return t;
        }

        static operator ::OpenDDS::DCPS::TimeDuration(TimeValue t) {
            return ::OpenDDS::DCPS::TimeDuration(t.Seconds, t.MicroSeconds);
        }
    };

};
