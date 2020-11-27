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
namespace OpenDDSharp.DDS
{
    /// <summary>
    /// A WaitSet object allows an application to wait until one or more of the attached <see cref="Condition" /> objects has a <see cref="Condition.TriggerValue" /> of
    /// <see langword="true"/> or else until the timeout expires.
    /// </summary>
    /// <remarks>
    /// WaitSet has no factory. This is because it is not necessarily associated with a single DomainParticipant and could be used to wait on
    /// <see cref="Condition" /> objects associated with different <see cref="DomainParticipant" /> objects.
    /// </remarks>
    public class WaitSet
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WaitSet"/> class.
        /// </summary>
        public WaitSet()
        {
        }
        #endregion
    }
}