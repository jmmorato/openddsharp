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
using System.ComponentModel;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// ITopicDescription represents the fact that both publications and subscriptions are tied to a single data-type.
    /// The interface is implemented on <see cref="Topic" />, <see cref="ContentFilteredTopic" />, and <see cref="MultiTopic" />.
    /// </summary>
    /// <remarks>
    /// Its property TypeName defines a unique resulting type for the publication or the subscription and therefore creates an implicit association
    /// with a TypeSupport. ITopicDescription has also a Name property that allows it to be retrieved locally.
    /// </remarks>
    public interface ITopicDescription
    {
        /// <summary>
        /// Gets type name used to create the <see cref="ITopicDescription" />.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Gets the name used to create the <see cref="ITopicDescription" />.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="DomainParticipant" /> to which the <see cref="ITopicDescription" /> belongs.
        /// </summary>
        DomainParticipant Participant { get; }

        /// <summary>
        /// Internal use only.
        /// </summary>
        /// <returns>The native pointer.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        IntPtr ToNative();

        /// <summary>
        /// Internal use only.
        /// </summary>
        /// <returns>The native pointer.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        IntPtr ToNativeTopicDescription();
    }
}
