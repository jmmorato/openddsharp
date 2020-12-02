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

namespace OpenDDSharp.OpenDDS.DCPS
{
    /// <summary>
    /// Provides access to the configurable options for the TCP/IP transport.
    /// </summary>
    /// <remarks>
    /// A properly confgured transport provides added resilience to underlying stack disturbances.Almost all of the
    /// options available to customize the connection and reconnection strategies have reasonable
    /// defaults, but ultimately these values should to be chosen based upon a careful study of the
    /// quality of the network and the desired QoS in the specifc DDS application and target environment.
    /// </remarks>
    public class TcpInst : TransportInst
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TcpInst"/> class.
        /// </summary>
        /// <param name="inst">The base <see cref="TransportInst" /> object created with the <see cref="TransportRegistry" />.</param>
        public TcpInst(TransportInst inst) : base(inst != null ? inst.ToNative() : IntPtr.Zero)
        {
        }
        #endregion
    }
}
