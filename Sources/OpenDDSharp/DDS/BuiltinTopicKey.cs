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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS;

/// <summary>
/// Global unique identifier of the built-in topics.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct BuiltinTopicKey : IEquatable<BuiltinTopicKey>
{
    #region Fields
    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 16)]
    private readonly byte[] _value;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the value of the <see cref="BuiltinTopicKey" />.
    /// </summary>
    public byte[] Value => _value;
    #endregion

    #region IEquatable<BuiltinTopicKey> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
    public bool Equals(BuiltinTopicKey other)
    {
        return _value.SequenceEqual(other.Value);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        return GetType() == obj.GetType() && Equals((BuiltinTopicKey)obj);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return -1939223833 + EqualityComparer<byte[]>.Default.GetHashCode(_value);
    }
    #endregion

    #region Operators
    /// <summary>
    /// Equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(BuiltinTopicKey left, BuiltinTopicKey right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Not equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns><see langword="false" /> if the left object is equal to the right object;
    /// otherwise, <see langword="true" />.</returns>
    public static bool operator !=(BuiltinTopicKey left, BuiltinTopicKey right)
    {
        return !left.Equals(right);
    }
    #endregion
}