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
using System;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// ReadCondition objects are conditions specifically dedicated to read operations and attached to one <see cref="DataReader" />.
    /// </summary>
    /// <remarks>
    /// ReadCondition objects allow an application to specify the data samples it is interested in (by specifying the desired sample-states,
    /// view-states, and instance-states). This allows the middleware to enable the condition only when suitable information is available.
    /// They are to be used in conjunction with a WaitSet as normal conditions. More than one ReadCondition may be attached to the same <see cref="DataReader" />.
    /// </remarks>
    public class ReadCondition : Condition
    {
        #region Fields

        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="DataReader" /> associated with the <see cref="ReadCondition" />.
        /// </summary>
        /// <remarks>
        /// Note that there is exactly one <see cref="DataReader" /> associated with each <see cref="ReadCondition" />.
        /// </remarks>
        public DataReader DataReader { get; }
        #endregion

        #region Constructors
        internal ReadCondition(IntPtr native, DataReader reader) : base(NarrowBase(native))
        {
            DataReader = reader;
        }
        #endregion

        #region Methods
        private static IntPtr NarrowBase(IntPtr ptr)
        {
            throw new NotImplementedException();
            //return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
            //                                   () => UnsafeNativeMethods.NarrowBase64(ptr));
        }
        #endregion
    }
}
