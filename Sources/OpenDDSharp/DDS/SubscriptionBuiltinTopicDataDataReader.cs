﻿/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 Jose Morato

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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace OpenDDSharp.DDS;

/// <summary>
/// <see cref="SubscriptionBuiltinTopicData"/> <see cref="DataReader"/>.
/// </summary>
public class SubscriptionBuiltinTopicDataDataReader : DataReader
{
    #region Constants
    /// <summary>
    /// The built-in participant topic name.
    /// </summary>
    public const string BUILT_IN_SUBSCRIPTION_TOPIC = "DCPSSubscription";

    /// <summary>
    /// The built-in participant topic type.
    /// </summary>
    public const string BUILT_IN_SUBSCRIPTION_TOPIC_TYPE = "SUBSCRIPTION_BUILT_IN_TOPIC_TYPE";
    #endregion

    #region Properties
    private readonly IntPtr _native;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionBuiltinTopicDataDataReader"/> class.
    /// </summary>
    /// <param name="dataReader">The built-in <see cref="DataReader"/>.</param>
    public SubscriptionBuiltinTopicDataDataReader(DataReader dataReader) : base(dataReader.ToNative())
    {
        IntPtr ptr = base.ToNative();
        _native = UnsafeNativeMethods.SubscriptionBuiltinNarrow(ptr);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Reads all samples.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode Read(List<SubscriptionBuiltinTopicData> receivedData, List<SampleInfo> receivedInfo)
    {
        return Read(receivedData,
            receivedInfo,
            ResourceLimitsQosPolicy.LengthUnlimited,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Reads all samples with a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="maxSamples">The maximum allowed samples to be read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode Read(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        int maxSamples)
    {
        return Read(
            receivedData,
            receivedInfo,
            maxSamples,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Reads all samples based in the condition provided and a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="maxSamples">The maximum allowed samples to be read.</param>
    /// <param name="condition">The <see cref="ReadCondition"/> applied.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode Read(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        int maxSamples,
        ReadCondition condition)
    {
        if (condition == null)
        {
            return ReturnCode.BadParameter;
        }

        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinReadWithCondition(
            _native,
            ref rd,
            ref ri,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info is { Count: > 0 })
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Reads all samples based in the state parameters provided and a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="maxSamples">The maximum allowed samples to be read.</param>
    /// <param name="sampleStates">The <see cref="SampleStateMask"/> state to be read.</param>
    /// <param name="viewStates">The <see cref="ViewStateMask"/> state to be read.</param>
    /// <param name="instanceStates">The <see cref="InstanceStateMask"/> state to be read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode Read(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        int maxSamples,
        SampleStateMask sampleStates,
        ViewStateMask viewStates,
        InstanceStateMask instanceStates)
    {
        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinRead(
            _native,
            ref rd,
            ref ri,
            maxSamples,
            sampleStates,
            viewStates,
            instanceStates);

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info != null && info.Count > 0)
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Takes all samples.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode Take(List<SubscriptionBuiltinTopicData> receivedData, List<SampleInfo> receivedInfo)
    {
        return Take(receivedData,
            receivedInfo,
            ResourceLimitsQosPolicy.LengthUnlimited,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Reads all samples with a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="maxSamples">The maximum allowed samples to be read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode Take(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        int maxSamples)
    {
        return Take(
            receivedData,
            receivedInfo,
            maxSamples,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Takes all samples based in the condition provided and a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="maxSamples">The maximum allowed samples to be taken.</param>
    /// <param name="condition">The <see cref="ReadCondition"/> applied.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode Take(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        int maxSamples,
        ReadCondition condition)
    {
        if (condition == null)
        {
            return ReturnCode.BadParameter;
        }

        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinTakeWithCondition(
            _native,
            ref rd,
            ref ri,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info is { Count: > 0 })
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Takes all samples based in the state parameters provided and a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="maxSamples">The maximum allowed samples to be taken.</param>
    /// <param name="sampleStates">The <see cref="SampleStateMask"/> state to be taken.</param>
    /// <param name="viewStates">The <see cref="ViewStateMask"/> state to be taken.</param>
    /// <param name="instanceStates">The <see cref="InstanceStateMask"/> state to be taken.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode Take(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        int maxSamples,
        SampleStateMask sampleStates,
        ViewStateMask viewStates,
        InstanceStateMask instanceStates)
    {
        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;
        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinTake(_native,
            ref rd,
            ref ri,
            maxSamples,
            sampleStates,
            viewStates,
            instanceStates);

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info is { Count: > 0 })
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Reads all samples for the specific instance.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="handle">The provided <see cref="InstanceHandle"/> that indicates the instance to be read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle)
    {
        return ReadInstance(
            receivedData,
            receivedInfo,
            handle,
            ResourceLimitsQosPolicy.LengthUnlimited,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Reads all samples for the specific instance with a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="handle">The provided <see cref="InstanceHandle"/> that indicates the instance to be read.</param>
    /// <param name="maxSamples">The maximum retrieved samples allowed per call.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle,
        int maxSamples)
    {
        return ReadInstance(
            receivedData,
            receivedInfo,
            handle,
            maxSamples,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Reads all samples for the specific instance based in the condition provided and with a maximum
    /// allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="handle">The provided <see cref="InstanceHandle"/> that indicates the instance to be read.</param>
    /// <param name="maxSamples">The maximum retrieved samples allowed per call.</param>
    /// <param name="condition">The <see cref="ReadCondition"/> applied.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle,
        int maxSamples,
        ReadCondition condition)
    {
        if (condition == null)
        {
            return ReturnCode.BadParameter;
        }

        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinReadInstanceWithCondition(
            _native,
            ref rd,
            ref ri,
            (int)handle,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info is { Count: > 0 })
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Reads all samples for the specific instance based in the state parameters provided and a maximum
    /// allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="handle">The provided <see cref="InstanceHandle"/> that indicates the instance to be read.</param>
    /// <param name="maxSamples">The maximum allowed samples to be read.</param>
    /// <param name="sampleStates">The <see cref="SampleStateMask"/> state to be read.</param>
    /// <param name="viewStates">The <see cref="ViewStateMask"/> state to be read.</param>
    /// <param name="instanceStates">The <see cref="InstanceStateMask"/> state to be read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle,
        int maxSamples,
        SampleStateMask sampleStates,
        ViewStateMask viewStates,
        InstanceStateMask instanceStates)
    {
        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinReadInstance(
            _native,
            ref rd,
            ref ri,
            handle,
            maxSamples,
            sampleStates,
            viewStates,
            instanceStates);

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info is { Count: > 0 })
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Takes all samples for the specific instance.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="handle">The provided <see cref="InstanceHandle"/> that indicates the instance to be read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle)
    {
        return TakeInstance(receivedData,
            receivedInfo,
            handle,
            ResourceLimitsQosPolicy.LengthUnlimited,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Takes all samples for the specific instance with a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="handle">The provided <see cref="InstanceHandle"/> that indicates the instance to be taken.</param>
    /// <param name="maxSamples">The maximum retrieved samples allowed per call.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle,
        int maxSamples)
    {
        return TakeInstance(receivedData,
            receivedInfo,
            handle,
            maxSamples,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Takes all samples for the specific instance based in the condition provided and with a maximum
    /// allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="handle">The provided <see cref="InstanceHandle"/> that indicates the instance to be taken.</param>
    /// <param name="maxSamples">The maximum retrieved samples allowed per call.</param>
    /// <param name="condition">The <see cref="ReadCondition"/> applied.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle,
        int maxSamples,
        ReadCondition condition)
    {
        if (condition == null)
        {
            return ReturnCode.BadParameter;
        }

        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinTakeInstanceWithCondition(
            _native,
            ref rd,
            ref ri,
            (int)handle,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info is { Count: > 0 })
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Takes all samples for the specific instance based in the state parameters provided and a maximum
    /// allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="handle">The provided <see cref="InstanceHandle"/> that indicates the instance to be taken.</param>
    /// <param name="maxSamples">The maximum allowed samples to be taken.</param>
    /// <param name="sampleStates">The <see cref="SampleStateMask"/> state to be taken.</param>
    /// <param name="viewStates">The <see cref="ViewStateMask"/> state to be taken.</param>
    /// <param name="instanceStates">The <see cref="InstanceStateMask"/> state to be taken.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle,
        int maxSamples,
        SampleStateMask sampleStates,
        ViewStateMask viewStates,
        InstanceStateMask instanceStates)
    {
        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinTakeInstance(
            _native,
            ref rd,
            ref ri,
            handle,
            maxSamples,
            sampleStates,
            viewStates,
            instanceStates);

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info != null && info.Count > 0)
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Reads all samples for the next instance.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadNextInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle previousHandle)
    {
        return ReadNextInstance(
            receivedData,
            receivedInfo,
            previousHandle,
            ResourceLimitsQosPolicy.LengthUnlimited,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Reads all samples for the next instance with a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance read.</param>
    /// <param name="maxSamples">The maximum retrieved samples allowed per call.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadNextInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle previousHandle,
        int maxSamples)
    {
        return ReadNextInstance(
            receivedData,
            receivedInfo,
            previousHandle,
            maxSamples,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Reads all samples for the next instance based in the condition provided and with a maximum
    /// allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance read.</param>
    /// <param name="maxSamples">The maximum retrieved samples allowed per call.</param>
    /// <param name="condition">The <see cref="ReadCondition"/> applied.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadNextInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle previousHandle,
        int maxSamples,
        ReadCondition condition)
    {
        if (condition == null)
        {
            return ReturnCode.BadParameter;
        }

        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinReadNextInstanceWithCondition(
            _native,
            ref rd,
            ref ri,
            previousHandle,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info is { Count: > 0 })
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Reads all samples for the next instance based in the state parameters provided and a maximum
    /// allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance read.</param>
    /// <param name="maxSamples">The maximum allowed samples to be read.</param>
    /// <param name="sampleStates">The <see cref="SampleStateMask"/> state to be read.</param>
    /// <param name="viewStates">The <see cref="ViewStateMask"/> state to be read.</param>
    /// <param name="instanceStates">The <see cref="InstanceStateMask"/> state to be read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadNextInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle previousHandle,
        int maxSamples,
        SampleStateMask sampleStates,
        ViewStateMask viewStates,
        InstanceStateMask instanceStates)
    {
        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinReadNextInstance(
            _native,
            ref rd,
            ref ri,
            previousHandle,
            maxSamples,
            sampleStates,
            viewStates,
            instanceStates);

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info != null && info.Count > 0)
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Takes all samples for the next instance.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance taken.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeNextInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle previousHandle)
    {
        return TakeNextInstance(
            receivedData,
            receivedInfo,
            previousHandle,
            ResourceLimitsQosPolicy.LengthUnlimited,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Takes all samples for the next instance with a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance taken.</param>
    /// <param name="maxSamples">The maximum retrieved samples allowed per call.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeNextInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle previousHandle,
        int maxSamples)
    {
        return TakeNextInstance(
            receivedData,
            receivedInfo,
            previousHandle,
            maxSamples,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Takes all samples for the next instance based in the condition provided and with a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance taken.</param>
    /// <param name="maxSamples">The maximum retrieved samples allowed per call.</param>
    /// <param name="condition">The <see cref="ReadCondition"/> applied.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeNextInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle previousHandle,
        int maxSamples,
        ReadCondition condition)
    {
        if (condition == null)
        {
            return ReturnCode.BadParameter;
        }

        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinTakeNextInstanceWithCondition(
            _native,
            ref rd,
            ref ri,
            previousHandle,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info is { Count: > 0 })
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Takes all samples for the next instance based in the state parameters provided and a maximum
    /// allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance taken.</param>
    /// <param name="maxSamples">The maximum allowed samples to be taken.</param>
    /// <param name="sampleStates">The <see cref="SampleStateMask"/> state to be taken.</param>
    /// <param name="viewStates">The <see cref="ViewStateMask"/> state to be taken.</param>
    /// <param name="instanceStates">The <see cref="InstanceStateMask"/> state to be taken.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeNextInstance(
        List<SubscriptionBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle previousHandle,
        int maxSamples,
        SampleStateMask sampleStates,
        ViewStateMask viewStates,
        InstanceStateMask instanceStates)
    {
        if (receivedData == null || receivedInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        receivedData.Clear();
        receivedInfo.Clear();

        IntPtr rd = IntPtr.Zero;
        IntPtr ri = IntPtr.Zero;

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinTakeNextInstance(
            _native,
            ref rd,
            ref ri,
            previousHandle,
            maxSamples,
            sampleStates,
            viewStates,
            instanceStates);

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<SubscriptionBuiltinTopicDataWrapper> data = new List<SubscriptionBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(SubscriptionBuiltinTopicData);
                    aux.FromNative(d);
                    receivedData.Add(aux);
                }
            }

            if (info is { Count: > 0 })
            {
                foreach (var i in info)
                {
                    SampleInfo aux = new ();
                    aux.FromNative(i);
                    receivedInfo.Add(aux);
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Reads next sample in the queue.
    /// </summary>
    /// <param name="data">The data sample received.</param>
    /// <param name="sampleInfo">The <see cref="SampleInfo"/> received.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadNextSample(ref SubscriptionBuiltinTopicData data, SampleInfo sampleInfo)
    {
        if (sampleInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        var wrapper = default(SubscriptionBuiltinTopicDataWrapper);
        var infoWrapper = default(SampleInfoWrapper);

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinReadNextSample(_native, ref wrapper, ref infoWrapper);

        if (ret == ReturnCode.Ok)
        {
            data.FromNative(wrapper);
            sampleInfo.FromNative(infoWrapper);
        }

        return ret;
    }

    /// <summary>
    /// Takes next sample in the queue.
    /// </summary>
    /// <param name="data">The data sample received.</param>
    /// <param name="sampleInfo">The <see cref="SampleInfo"/> received.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeNextSample(ref SubscriptionBuiltinTopicData data, SampleInfo sampleInfo)
    {
        if (sampleInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        var wrapper = default(SubscriptionBuiltinTopicDataWrapper);
        var infoWrapper = default(SampleInfoWrapper);

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinTakeNextSample(_native, ref wrapper, ref infoWrapper);
        if (ret == ReturnCode.Ok)
        {
            data.FromNative(wrapper);
            sampleInfo.FromNative(infoWrapper);
        }

        return ret;
    }

    /// <summary>
    /// Retrieve the instance key that corresponds to an instance handle.
    /// </summary>
    /// <param name="data">
    /// A user data type specific key holder of type T whose key fields are filled by this operation.
    /// </param>
    /// <param name="handle">The <see cref="InstanceHandle"/> whose key is to be retrieved.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode GetKeyValue(ref SubscriptionBuiltinTopicData data, InstanceHandle handle)
    {
        if (handle == InstanceHandle.HandleNil)
        {
            return ReturnCode.BadParameter;
        }

        var aux = default(SubscriptionBuiltinTopicDataWrapper);

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.SubscriptionBuiltinGetKeyValue(_native, ref aux, handle);

        if (ret == ReturnCode.Ok)
        {
            data.FromNative(aux);
        }

        return ret;
    }

    /// <summary>
    /// Retrieve the <see cref="InstanceHandle"/> that corresponds to an instance key holder.
    /// </summary>
    /// <param name="instance">The key holder.</param>
    /// <returns>The <see cref="InstanceHandle"/> retrieved.</returns>
    public InstanceHandle LookupInstance(SubscriptionBuiltinTopicData instance)
    {
        return UnsafeNativeMethods.SubscriptionBuiltinLookupInstance(_native, instance.ToNative());
    }
    #endregion
}

/// <summary>
/// This class suppresses stack walks for unmanaged code permission.
/// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
/// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full
/// security review to make sure that the usage
/// is secure because no stack walk will be performed.
/// </summary>
[SuppressUnmanagedCodeSecurity]
internal static partial class UnsafeNativeMethods
{
#if NET7_0_OR_GREATER
    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_Narrow")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr SubscriptionBuiltinNarrow(IntPtr dr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_Read")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinRead(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinReadWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_Take")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinTake(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinTakeWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadInstance")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinReadInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadInstanceWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinReadInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeInstance")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinTakeInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeInstanceWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinTakeInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadNextInstance")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinReadNextInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadNextInstanceWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinReadNextInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeNextInstance")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinTakeNextInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeNextInstanceWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int SubscriptionBuiltinTakeNextInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadNextSample", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinReadNextSample(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriptionBuiltinTopicDataWrapper data, [In, Out] ref SampleInfoWrapper sampleInfo);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeNextSample", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinTakeNextSample(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriptionBuiltinTopicDataWrapper data, [In, Out] ref SampleInfoWrapper sampleInfo);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_LookupInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinLookupInstance(IntPtr dr, SubscriptionBuiltinTopicDataWrapper instance);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_GetKeyValue", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinGetKeyValue(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriptionBuiltinTopicDataWrapper data, int handle);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_Narrow", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SubscriptionBuiltinNarrow(IntPtr dr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_Read", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinRead(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinReadWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_Take", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinTake(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinTakeWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinReadInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadInstanceWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinReadInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinTakeInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeInstanceWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinTakeInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadNextInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinReadNextInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadNextInstanceWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinReadNextInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeNextInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinTakeNextInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeNextInstanceWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinTakeNextInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_ReadNextSample", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinReadNextSample(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriptionBuiltinTopicDataWrapper data, [In, Out] ref SampleInfoWrapper sampleInfo);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_TakeNextSample", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinTakeNextSample(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriptionBuiltinTopicDataWrapper data, [In, Out] ref SampleInfoWrapper sampleInfo);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_LookupInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinLookupInstance(IntPtr dr, SubscriptionBuiltinTopicDataWrapper instance);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "SubscriptionBuiltinTopicDataDataReader_GetKeyValue", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SubscriptionBuiltinGetKeyValue(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriptionBuiltinTopicDataWrapper data, int handle);
#endif
}