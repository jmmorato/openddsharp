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
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// Information that accompanies each sample that is read or taken.
    /// </summary>
    /// <remarks>
    /// The SampleInfo structure contains information associated with each Sample. The <see cref="DataReader"/> read and take operations return two vectors.
    /// One vector contains Sample(s) and the other contains SampleInfo(s). There is a one-to-one correspondence between items in these two vectors.
    /// Each Sample is described by the corresponding SampleInfo instance.
    /// </remarks>
    public sealed class SampleInfo : IEquatable<SampleInfo>
    {
        #region Properties
        /// <summary>
        /// Gets a value indicating whether the associated DataSample contains data.
        /// The associated DataSample may not contain data if it this sample indicates a change in sample state (for example Alive -> Disposed).
        /// </summary>
        public bool ValidData { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the associated data sample has/has not been read previously.
        /// </summary>
        public SampleStateKind SampleState { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether  the associated instance has/has not been seen before. ViewState indicates whether the <see cref="DataReader" /> has already seen
        /// samples for the most current generation of the related instance.
        /// </summary>
        public ViewStateKind ViewState { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the associated instance currently exists.
        /// </summary>
        public InstanceStateKind InstanceState { get; internal set; }

        /// <summary>
        /// Gets the time provided by the <see cref="DataWriter" /> when the sample was written.
        /// </summary>
        public Timestamp SourceTimestamp { get; internal set; }

        /// <summary>
        /// Gets the handle that locally identifies the associated instance.
        /// </summary>
        public InstanceHandle InstanceHandle { get; internal set; }

        /// <summary>
        /// Gets the local handle of the source <see cref="DataWriter" />.
        /// </summary>
        public InstanceHandle PublicationHandle { get; internal set; }

        /// <summary>
        /// Gets the number of times the instance has become 'Alive' after being explicitly disposed.
        /// </summary>
        public int DisposedGenerationCount { get; internal set; }

        /// <summary>
        /// Gets the number of times the instance has become 'Alive' after being automatically disposed due to no active writers.
        /// </summary>
        public int NoWritersGenerationCount { get; internal set; }

        /// <summary>
        /// Gets number of samples related to this instances that follow in the collection returned by the <see cref="DataReader" /> read or take operations.
        /// </summary>
        public int SampleRank { get; internal set; }

        /// <summary>
        /// Gets the generation difference of this sample and the most recent sample in the collection. GenerationRank indicates the generation difference 
        /// (ie, the number of times the instance was disposed and became alive again) between this sample, and the most recent sample in the collection related to this instance.
        /// </summary>
        public int GenerationRank { get; internal set; }

        /// <summary>
        /// Gets the generation difference between this sample and the most recent sample. The AbsoluteGenerationRank indicates the generation difference 
        /// (ie, the number of times the instance was disposed and became alive again) between this sample, and the most recent sample 
        /// (possibly not in the returned collection) of this instance.
        /// </summary>
        public int AbsoluteGenerationRank { get; internal set; }
        #endregion

        #region Methods
        internal SampleInfoWrapper ToNative()
        {
            return new SampleInfoWrapper
            {
                AbsoluteGenerationRank = AbsoluteGenerationRank,
                DisposedGenerationCount = DisposedGenerationCount,
                GenerationRank = GenerationRank,
                InstanceHandle = InstanceHandle,
                InstanceState = InstanceState,
                NoWritersGenerationCount = NoWritersGenerationCount,
                PublicationHandle = PublicationHandle,
                SampleRank = SampleRank,
                SampleState = SampleState,
                SourceTimestamp = SourceTimestamp,
                ValidData = ValidData,
                ViewState = ViewState,
            };
        }

        /// <summary>
        /// Internal usage only.
        /// </summary>
        /// <param name="wrapper">The wrapper structure.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void FromNative(SampleInfoWrapper wrapper)
        {
            AbsoluteGenerationRank = wrapper.AbsoluteGenerationRank;
            DisposedGenerationCount = wrapper.DisposedGenerationCount;
            GenerationRank = wrapper.GenerationRank;
            InstanceHandle = wrapper.InstanceHandle;
            InstanceState = wrapper.InstanceState;
            NoWritersGenerationCount = wrapper.NoWritersGenerationCount;
            PublicationHandle = wrapper.PublicationHandle;
            SampleRank = wrapper.SampleRank;
            SampleState = wrapper.SampleState;
            SourceTimestamp = wrapper.SourceTimestamp;
            ValidData = wrapper.ValidData;
            ViewState = wrapper.ViewState;
        }
        #endregion

        #region IEquatable<SampleInfo> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(SampleInfo other)
        {
            if (other == null)
            {
                return false;
            }

            return ValidData == other.ValidData &&
                   SampleState == other.SampleState &&
                   ViewState == other.ViewState &&
                   InstanceState == other.InstanceState &&
                   SourceTimestamp == other.SourceTimestamp &&
                   InstanceHandle == other.InstanceHandle &&
                   PublicationHandle == other.PublicationHandle &&
                   DisposedGenerationCount == other.DisposedGenerationCount &&
                   NoWritersGenerationCount == other.NoWritersGenerationCount &&
                   SampleRank == other.SampleRank &&
                   GenerationRank == other.GenerationRank &&
                   AbsoluteGenerationRank == other.AbsoluteGenerationRank;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            return (obj is SampleInfo other) && Equals(other);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = -910965429;
            hashCode = (hashCode * -1521134295) + ValidData.GetHashCode();
            hashCode = (hashCode * -1521134295) + SampleState.GetHashCode();
            hashCode = (hashCode * -1521134295) + ViewState.GetHashCode();
            hashCode = (hashCode * -1521134295) + SourceTimestamp.GetHashCode();
            hashCode = (hashCode * -1521134295) + InstanceHandle.GetHashCode();
            hashCode = (hashCode * -1521134295) + PublicationHandle.GetHashCode();
            hashCode = (hashCode * -1521134295) + DisposedGenerationCount.GetHashCode();
            hashCode = (hashCode * -1521134295) + NoWritersGenerationCount.GetHashCode();
            hashCode = (hashCode * -1521134295) + SampleRank.GetHashCode();
            hashCode = (hashCode * -1521134295) + GenerationRank.GetHashCode();
            hashCode = (hashCode * -1521134295) + AbsoluteGenerationRank.GetHashCode();
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
        public static bool operator ==(SampleInfo left, SampleInfo right)
        {
            if (left is null && right is null)
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Not equals comparison operator.
        /// </summary>
        /// <param name="left">The left value for the comparison.</param>
        /// <param name="right">The right value for the comparison.</param>
        /// <returns><see langword="false" /> if the left object is equal to the right object; otherwise, <see langword="true" />.</returns>
        public static bool operator !=(SampleInfo left, SampleInfo right)
        {
            if (left is null && right is null)
            {
                return false;
            }

            if (left is null || right is null)
            {
                return true;
            }

            return !left.Equals(right);
        }
        #endregion
    }

#pragma warning disable
    [EditorBrowsable(EditorBrowsableState.Never)]
    [StructLayout(LayoutKind.Sequential)]
    public struct SampleInfoWrapper
    {
        public uint SampleState;
        public uint ViewState;
        public uint InstanceState;
        public Timestamp SourceTimestamp;
        public int InstanceHandle;
        public int PublicationHandle;
        public int DisposedGenerationCount;
        public int NoWritersGenerationCount;
        public int SampleRank;
        public int GenerationRank;
        public int AbsoluteGenerationRank;
        [MarshalAs(UnmanagedType.I1)]
        public bool ValidData;
        public long OpenddsReservedPublicationSeq;
    }
#pragma warning enable
}
