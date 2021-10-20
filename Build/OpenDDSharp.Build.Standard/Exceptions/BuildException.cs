/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

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
using System;
using System.Runtime.Serialization;

namespace OpenDDSharp.Build.Standard.Exceptions
{
    /// <summary>
    /// Build exception class.
    /// </summary>
    [Serializable]
    public class BuildException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildException"/> class.
        /// </summary>
        public BuildException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public BuildException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BuildException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildException"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected BuildException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
