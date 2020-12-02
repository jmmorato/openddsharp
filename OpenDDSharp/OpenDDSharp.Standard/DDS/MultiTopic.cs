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
    /// MultiTopic is an implementation of <see cref="ITopicDescription" /> that allows subscriptions to combine/filter/rearrange data coming from
    /// several topics. MultiTopic allows a more sophisticated subscription that can select and combine data received from multiple topics into a
    /// single resulting type(specified by the inherited type name). The data will then be filtered(selection) and possibly re-arranged
    /// (aggregation/projection) according to a subscription expression with the expression parameters.
    /// </summary>
    /// <remarks>
    /// <para>The subscription expression is a string that identifies the selection and re-arrangement of data from the associated
    /// topics. It is similar to an SQL clause where the SELECT part provides the fields to be kept, the FROM part provides the
    /// names of the topics that are searched for those fields, and the WHERE clause gives the content filter.The topics
    /// combined may have different types but they are restricted in that the type of the fields used for the NATURAL JOIN
    /// operation must be the same.</para>
    /// <para>The expression parameters are a collection of strings that give values to the ‘parameters’ ("%n" tokens) in
    /// the subscription expression. The number of supplied parameters must fit with the requested values in the
    /// subscription expression (the number of %n tokens).</para>
    /// <para><see cref="DataReader" /> entities associated with a MultiTopic are alerted of data modifications by the usual listener or condition
    /// mechanisms whenever modifications occur to the data associated with any of the topics relevant to the MultiTopic.</para>
    /// <para><see cref="DataReader" /> entities associated with a MultiTopic access instances that are “constructed” at the <see cref="DataReader" /> side from
    /// the instances written by multiple <see cref="DataWriter" /> entities.The MultiTopic access instance will begin to exist as soon as all
    /// the constituting Topic instances are in existence.</para>
    /// <para>The view_state and instance_state is computed from the corresponding states of the constituting instances:
    /// <list type="bullet">
    ///     <item><description>The view state of the MultiTopic instance is <see cref="ViewStateKind.NewViewState" /> if at least one of the constituting instances has
    /// <see cref="ViewStateKind.NewViewState" />, otherwise it will be  <see cref="ViewStateKind.NotNewViewState" />.</description></item>
    ///     <item><description>The instance state of the MultiTopic instance is <see cref="InstanceStateKind.AliveInstanceState" /> if the instance state of all the
    /// constituting Topic instances is <see cref="InstanceStateKind.AliveInstanceState" />. It is <see cref="InstanceStateKind.NotAliveDisposedInstanceState" /> if at least one of the constituting <see cref="Topic" />
    /// instances is <see cref="InstanceStateKind.NotAliveDisposedInstanceState" />. Otherwise it is <see cref="InstanceStateKind.NotAliveNoWritersInstanceState" />.</description></item>
    /// </list></para>
    /// </remarks>
    public class MultiTopic
    {
    }
}