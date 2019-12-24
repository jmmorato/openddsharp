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
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Duration
    {
        #region Constants
        /// <summary>
        /// Infinite seconds duration.
        /// </summary>
        public const int InfiniteSeconds = 2147483647;

        /// <summary>
        /// Infinite nanoseconds duration.
        /// </summary>
        public const uint InfiniteNanoseconds = 2147483647U;

        /// <summary>
        /// Zero seconds duration.
        /// </summary>
        public const int ZeroSeconds = 0;

        /// <summary>
        /// Zero nanoseconds duration.
        /// </summary>
        public const uint ZeroNanoseconds = 0U;
        #endregion

        #region Property
        public int Seconds { get; set; }

        public int NanoSeconds { get; set; }
        #endregion
    }
}
