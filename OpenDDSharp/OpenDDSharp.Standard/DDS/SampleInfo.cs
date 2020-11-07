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
    [StructLayout(LayoutKind.Sequential)]
    public sealed class SampleInfo : IEquatable<SampleInfo>
    {
        #region Fields
        private uint _sampleState;
        private uint _viewState;
        private uint _instanceState;
        private Timestamp _sourceTimestamp;
        private int _instanceHandle;
        private int _publicationHandle;
        private int _disposedGenerationCount;
        private int _noWritersGenerationCount;
        private int _sampleRank;
        private int _generationRank;
        private int _absoluteGenerationRank;
        [MarshalAs(UnmanagedType.I1)]
        private bool _validData;
        private long _openddsReservedPublicationSeq;
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether the associated DataSample contains data.
        /// The associated DataSample may not contain data if it this sample indicates a change in sample state (for example Alive -> Disposed).
        /// </summary>
        public bool ValidData
        {
            get => _validData;
            internal set => _validData = value;
        }

        /// <summary>
        /// Gets a value indicating whether the associated data sample has/has not been read previously.
        /// </summary>
        public SampleStateKind SampleState
        {
            get => _sampleState;
            internal set => _sampleState = value;
        }

        /// <summary>
        /// Gets a value indicating whether  the associated instance has/has not been seen before. ViewState indicates whether the <see cref="DataReader" /> has already seen
        /// samples for the most current generation of the related instance.
        /// </summary>
        public ViewStateKind ViewState
        {
            get => _viewState;
            internal set => _viewState = value;
        }

        /// <summary>
        /// Gets a value indicating whether the associated instance currently exists.
        /// </summary>
        public InstanceStateKind InstanceState
        {
            get => _instanceState;
            internal set => _instanceState = value;
        }

        /// <summary>
        /// Gets the time provided by the <see cref="DataWriter" /> when the sample was written.
        /// </summary>
        public Timestamp SourceTimestamp
        {
            get => _sourceTimestamp;
            internal set => _sourceTimestamp = value;
        }

        /// <summary>
        /// Gets the handle that locally identifies the associated instance.
        /// </summary>
        public InstanceHandle InstanceHandle
        {
            get => _instanceHandle;
            internal set => _instanceHandle = value;
        }

        /// <summary>
        /// Gets the local handle of the source <see cref="DataWriter" />.
        /// </summary>
        public InstanceHandle PublicationHandle
        {
            get => _publicationHandle;
            internal set => _publicationHandle = value;
        }

        /// <summary>
        /// Gets the number of times the instance has become 'Alive' after being explicitly disposed.
        /// </summary>
        public int DisposedGenerationCount
        {
            get => _disposedGenerationCount;
            internal set => _disposedGenerationCount = value;
        }

        /// <summary>
        /// Gets the number of times the instance has become 'Alive' after being automatically disposed due to no active writers.
        /// </summary>
        public int NoWritersGenerationCount
        {
            get => _noWritersGenerationCount;
            internal set => _noWritersGenerationCount = value;
        }

        /// <summary>
        /// Gets number of samples related to this instances that follow in the collection returned by the <see cref="DataReader" /> read or take operations.
        /// </summary>
        public int SampleRank
        {
            get => _sampleRank;
            internal set => _sampleRank = value;
        }

        /// <summary>
        /// Gets the generation difference of this sample and the most recent sample in the collection. GenerationRank indicates the generation difference 
        /// (ie, the number of times the instance was disposed and became alive again) between this sample, and the most recent sample in the collection related to this instance.
        /// </summary>
        public int GenerationRank
        {
            get => _generationRank;
            internal set => _generationRank = value;
        }

        /// <summary>
        /// Gets the generation difference between this sample and the most recent sample. The AbsoluteGenerationRank indicates the generation difference 
        /// (ie, the number of times the instance was disposed and became alive again) between this sample, and the most recent sample 
        /// (possibly not in the returned collection) of this instance.
        /// </summary>
        public int AbsoluteGenerationRank
        {
            get => _absoluteGenerationRank;
            internal set => _absoluteGenerationRank = value;
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
}
