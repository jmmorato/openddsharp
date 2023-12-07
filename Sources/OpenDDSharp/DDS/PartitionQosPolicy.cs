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
using OpenDDSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS;

/// <summary>
/// This policy allows the introduction of a logical partition concept inside the 'physical' partition induced by a domain.
/// </summary>
/// <remarks>
/// <para>
/// For a <see cref="DataReader" /> to see the changes made to an instance by a <see cref="DataWriter" />, not only
/// the <see cref="Topic" /> must match, but also they must share a common partition. Each string in the list that
/// defines this QoS policy defines a partition name. A partition name may contain wildcards. Sharing a common partition
/// means that one of the partition names matches.
/// </para>
/// <para>
/// Failure to match partitions is not considered an "incompatible" QoS and does not trigger any listeners
/// nor conditions.
/// </para>
/// <para>
/// This policy is changeable. A change of this policy can potentially modify the "match" of existing
/// <see cref="DataReader" /> and <see cref="DataWriter" />
/// entities. It may establish new "match" that did not exist before, or break existing match.
/// </para>
/// </remarks>
public sealed class PartitionQosPolicy : IEquatable<PartitionQosPolicy>
{
    #region Fields
    private readonly List<IntPtr> toRelease = new List<IntPtr>();
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the sequence of partition strings. The name defaults to an empty sequence of strings.
    /// </summary>
    /// <remarks>
    /// The default partition name is an empty string and causes the entity to participate in the default partition.
    /// </remarks>
    public IList<string> Name { get; set; }
    #endregion

    #region Constructors
    internal PartitionQosPolicy()
    {
        Name = new List<string>();
    }
    #endregion

    #region Methods
    internal PartitionQosPolicyWrapper ToNative()
    {
        IntPtr ptr = IntPtr.Zero;

        if (Name != null)
        {
            toRelease.AddRange(Name.StringSequenceToPtr(ref ptr, false));
            toRelease.Add(ptr);
        }

        return new PartitionQosPolicyWrapper
        {
            Name = ptr,
        };
    }

    internal void FromNative(PartitionQosPolicyWrapper wrapper)
    {
        IList<string> list = new List<string>();

        if (wrapper.Name != IntPtr.Zero)
        {
            wrapper.Name.PtrToStringSequence(ref list, false);
        }

        Name = list;
    }

    internal void Release()
    {
        if (toRelease == null)
        {
            return;
        }

        foreach (IntPtr ptr in toRelease)
        {
            Marshal.FreeHGlobal(ptr);
        }

        toRelease.Clear();
    }
    #endregion

    #region IEquatable<PartitionQosPolicy> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
    public bool Equals(PartitionQosPolicy other)
    {
        if (other == null)
        {
            return false;
        }

        return Name.SequenceEqual(other.Name);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        return (obj is PartitionQosPolicy other) && Equals(other);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        var hashCode = 1476352029;
        if (Name != null)
        {
            foreach (var s in Name)
            {
                hashCode = (hashCode * -1521134295) + s.GetHashCode();
            }
        }
        return hashCode;
    }
    #endregion
}

[StructLayout(LayoutKind.Sequential)]
internal struct PartitionQosPolicyWrapper
{
    #region Fields
    public IntPtr Name;
    #endregion
}