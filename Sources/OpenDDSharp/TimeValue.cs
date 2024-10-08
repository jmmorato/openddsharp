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

namespace OpenDDSharp;

/// <summary>
/// Structure for time value representation.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TimeValue : IEquatable<TimeValue>
{
    [MarshalAs(UnmanagedType.I8)]
    private long _seconds;
    [MarshalAs(UnmanagedType.I4)]
    private int _microSeconds;

    #region Properties
    /// <summary>
    /// Gets or sets the seconds.
    /// </summary>
    public long Seconds
    {
        get => _seconds;
        set => _seconds = value;
    }

    /// <summary>
    /// Gets or sets the microseconds.
    /// </summary>
    public int MicroSeconds
    {
        get => _microSeconds;
        set => _microSeconds = value;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Converts the time value to a CDR representation.
    /// </summary>
    /// <returns>The byte span serialized.</returns>
    internal ReadOnlySpan<byte> ToCDR()
    {
        var writer = new Marshaller.Cdr.CdrWriter();
        writer.WriteInt64(Seconds);
        writer.WriteInt32(MicroSeconds);
        return writer.GetBuffer();
    }

    /// <summary>
    /// Updates the time value from a CDR representation.
    /// </summary>
    /// <param name="data">The byte span serialized.</param>
    internal void FromCDR(ReadOnlySpan<byte> data)
    {
        var reader = new Marshaller.Cdr.CdrReader(data.ToArray());
        Seconds = reader.ReadInt64();
        MicroSeconds = reader.ReadInt32();
    }
    #endregion

    #region IEquatable<TimeValue> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
    public bool Equals(TimeValue other)
    {
        return Seconds == other.Seconds && MicroSeconds == other.MicroSeconds;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        return (obj is TimeValue other) && Equals(other);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        var hashCode = -1725149974;
        hashCode = (hashCode * -1521134295) + Seconds.GetHashCode();
        hashCode = (hashCode * -1521134295) + MicroSeconds.GetHashCode();
        return hashCode;
    }
    #endregion

    #region Operators
    /// <summary>
    /// Equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns><see langword="true" /> if the left object is equal to the right object; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(TimeValue left, TimeValue right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Not equals comparison operator.
    /// </summary>
    /// <param name="left">The left value for the comparison.</param>
    /// <param name="right">The right value for the comparison.</param>
    /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
    public static bool operator !=(TimeValue left, TimeValue right)
    {
        return !(left == right);
    }
    #endregion
}