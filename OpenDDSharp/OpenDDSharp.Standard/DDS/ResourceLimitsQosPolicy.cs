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
    /// This policy controls the resources that DDS can use in order to meet the requirements imposed by the application and other QoS settings.
    /// </summary>
    /// <remarks>
    /// <para>If the <see cref="DataWriter" /> objects are communicating samples faster than they are ultimately taken by the <see cref="DataReader" /> objects, the
    /// middleware will eventually hit against some of the QoS-imposed resource limits. Note that this may occur when just a single
    /// <see cref="DataReader" /> cannot keep up with its corresponding <see cref="DataWriter" />. The behavior in this case depends on the setting for the
    /// Reliability QoS. If reliability is BestEffort then DDS is allowed to drop samples. If the reliability is Reliable, DDS will block the <see cref="DataWriter" />
    /// or discard the sample at the <see cref="DataReader" /> in order not to lose existing samples.</para>
    /// <para>The setting of ResourceLimits MaxSamples must be consistent with the MaxSamplesPerInstance. For these two
    /// values to be consistent they must verify that “MaxSamples &gt;= MaxSamplesPerInstance.”</para>
    /// <para>The setting of ResourceLimits MaxSamplesPerInstance must be consistent with the History Depth. For these two
    /// QoS to be consistent, they must verify that "depth &lt;= MaxSamplesPerInstance".</para>
    /// <para>An attempt to set this policy to inconsistent values when an entity is created of via a SetQos operation will cause the operation to fail.</para>
    /// </remarks>
    public sealed class ResourceLimitsQosPolicy
    {
        #region Constants
        /// <summary>
        /// Used to indicate the absence of a particular limit.
        /// </summary>
        public const int LengthUnlimited = -1;
        #endregion
    }
}
