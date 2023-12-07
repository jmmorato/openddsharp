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
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS;

/// <summary>
/// The SampleLost status indicates that a sample has been lost and never received by the <see cref="DataReader" />.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct SampleLostStatus : IEquatable<SampleLostStatus>
{
    #region Fields
    private int _totalCount;
    private int _totalCountChange;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the cumulative count of samples reported as lost.
    /// </summary>
    public int TotalCount => _totalCount;

    /// <summary>
    /// Gets the incremental count of lost samples since the last time this status was accessed.
    /// </summary>
    public int TotalCountChange => _totalCountChange;
    #endregion

    #region IEquatable<SampleLostStatus> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the other parameter;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(SampleLostStatus other)
    {
        return TotalCount == other.TotalCount &&
               TotalCountChange == other.TotalCountChange;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object is equal to the current object;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public override bool Equals(object obj)
    {
        return (obj is SampleLostStatus other) && Equals(other);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        var hashCode = 543595584;
        hashCode = (hashCode * -1521134295) + TotalCount.GetHashCode();
        hashCode = (hashCode * -1521134295) + TotalCountChange.GetHashCode();
        return hashCode;
    }
    #endregion

    #region Operators
    /// <summary>
    /// Equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns>
    /// <see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.
    /// </returns>
    public static bool operator ==(SampleLostStatus left, SampleLostStatus right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Not equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns>
    /// <see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.
    /// </returns>
    public static bool operator !=(SampleLostStatus left, SampleLostStatus right)
    {
        return !(left == right);
    }
    #endregion
}