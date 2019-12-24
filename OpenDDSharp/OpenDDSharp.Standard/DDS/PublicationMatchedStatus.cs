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
    /// <summary>
    /// The PublicationMatched status indicates that either a compatible <see cref="DataReader" /> has been matched or 
    /// a previously matched <see cref="DataReader" /> has ceased to be matched.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PublicationMatchedStatus
    {
        #region Properties
        /// <summary>
        /// Gets the cumulative count of data readers that have compatibly matched this <see cref="DataWriter" />.
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Gets the incremental change in the total count since the last time this status was accessed.
        /// </summary>
        public int TotalCountChange { get; }

        /// <summary>
        /// Gets the current number of data readers matched to this <see cref="DataWriter" />.
        /// </summary>
        public int CurrentCount { get; }

        /// <summary>
        /// Gets the change in the current count since the last time this status was accessed.
        /// </summary>
        public int CurrentCountChange { get; }

        /// <summary>
        /// Gets the handle for the last <see cref="DataReader" /> matched.
        /// </summary>
        public InstanceHandle LastSubscriptionHandle { get; }
        #endregion
    }
}
