/*********************************************************************
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
/// <see cref="TopicBuiltinTopicData"/> <see cref="DataReader"/>.
/// </summary>
public class TopicBuiltinTopicDataDataReader : DataReader
{
    #region Constants
    /// <summary>
    /// The built-in participant topic name.
    /// </summary>
    public const string BUILT_IN_TOPIC_TOPIC = "DCPSTopic";

    /// <summary>
    /// The built-in participant topic type.
    /// </summary>
    public const string BUILT_IN_TOPIC_TOPIC_TYPE = "TOPIC_BUILT_IN_TOPIC_TYPE";
    #endregion

    #region Properties
    private readonly IntPtr _native;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="TopicBuiltinTopicDataDataReader"/> class.
    /// </summary>
    /// <param name="dataReader">The built-in <see cref="DataReader"/>.</param>
    public TopicBuiltinTopicDataDataReader(DataReader dataReader) : base(dataReader.ToNative())
    {
        IntPtr ptr = base.ToNative();
        _native = UnsafeNativeMethods.TopicBuiltinNarrow(ptr);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Reads all samples.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode Read(List<TopicBuiltinTopicData> receivedData, List<SampleInfo> receivedInfo)
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
    public ReturnCode Read(List<TopicBuiltinTopicData> receivedData, List<SampleInfo> receivedInfo, int maxSamples)
    {
        return Read(receivedData,
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
    public ReturnCode Read(List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinReadWithCondition(
            _native,
            ref rd,
            ref ri,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
        List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinRead(
            _native,
            ref rd,
            ref ri,
            maxSamples,
            sampleStates,
            viewStates,
            instanceStates);

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
    /// Takes all samples.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode Take(List<TopicBuiltinTopicData> receivedData, List<SampleInfo> receivedInfo)
    {
        return Take(
            receivedData,
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
    public ReturnCode Take(List<TopicBuiltinTopicData> receivedData,
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
        List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinTakeWithCondition(
            _native,
            ref rd,
            ref ri,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
        List<TopicBuiltinTopicData> receivedData,
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
        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinTake(
            _native,
            ref rd,
            ref ri,
            maxSamples,
            sampleStates,
            viewStates,
            instanceStates);

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
        List<TopicBuiltinTopicData> receivedData,
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
        List<TopicBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle,
        int maxSamples)
    {
        return ReadInstance(receivedData,
            receivedInfo,
            handle,
            maxSamples,
            SampleStateMask.AnySampleState,
            ViewStateMask.AnyViewState,
            InstanceStateMask.AnyInstanceState);
    }

    /// <summary>
    /// Reads all samples for the specific instance based in the condition provided and with a maximum allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="handle">The provided <see cref="InstanceHandle"/> that indicates the instance to be read.</param>
    /// <param name="maxSamples">The maximum retrieved samples allowed per call.</param>
    /// <param name="condition">The <see cref="ReadCondition"/> applied.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadInstance(
        List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinReadInstanceWithCondition(
            _native,
            ref rd,
            ref ri,
            (int)handle,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
        List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinReadInstance(
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
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
        List<TopicBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle)
    {
        return TakeInstance(
            receivedData,
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
        List<TopicBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle handle,
        int maxSamples)
    {
        return TakeInstance(
            receivedData,
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
        List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinTakeInstanceWithCondition(
            _native,
            ref rd,
            ref ri,
            handle,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
        List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinTakeInstance(
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
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
    /// Reads all samples for the next instance.
    /// </summary>
    /// <param name="receivedData">The list of data samples read.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> read.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance read.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode ReadNextInstance(
        List<TopicBuiltinTopicData> receivedData,
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
        List<TopicBuiltinTopicData> receivedData,
        List<SampleInfo> receivedInfo,
        InstanceHandle previousHandle,
        int maxSamples)
    {
        return ReadNextInstance(receivedData,
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
        List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinReadNextInstanceWithCondition(
            _native,
            ref rd,
            ref ri,
            previousHandle,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
        List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinReadNextInstance(
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
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
    /// Takes all samples for the next instance.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance taken.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeNextInstance(
        List<TopicBuiltinTopicData> receivedData,
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
        List<TopicBuiltinTopicData> receivedData,
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
    /// Takes all samples for the next instance based in the condition provided and with a maximum
    /// allowed samples to be retrieved.
    /// </summary>
    /// <param name="receivedData">The list of data samples taken.</param>
    /// <param name="receivedInfo">The list of <see cref="SampleInfo"/> taken.</param>
    /// <param name="previousHandle">The <see cref="InstanceHandle"/> of the previous instance taken.</param>
    /// <param name="maxSamples">The maximum retrieved samples allowed per call.</param>
    /// <param name="condition">The <see cref="ReadCondition"/> applied.</param>
    /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
    public ReturnCode TakeNextInstance(
        List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinTakeNextInstanceWithCondition(
            _native,
            ref rd,
            ref ri,
            previousHandle,
            maxSamples,
            condition.ToNative());

        if (ret == ReturnCode.Ok && !rd.Equals(IntPtr.Zero) && !ri.Equals(IntPtr.Zero))
        {
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
        List<TopicBuiltinTopicData> receivedData,
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

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinTakeNextInstance(
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
            IList<TopicBuiltinTopicDataWrapper> data = new List<TopicBuiltinTopicDataWrapper>();
            IList<SampleInfoWrapper> info = new List<SampleInfoWrapper>();

            rd.PtrToSequence(ref data);
            ri.PtrToSequence(ref info);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var aux = default(TopicBuiltinTopicData);
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
    public ReturnCode ReadNextSample(ref TopicBuiltinTopicData data, SampleInfo sampleInfo)
    {
        if (sampleInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        var wrapper = default(TopicBuiltinTopicDataWrapper);
        var infoWrapper = default(SampleInfoWrapper);

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinReadNextSample(_native, ref wrapper, ref infoWrapper);

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
    public ReturnCode TakeNextSample(ref TopicBuiltinTopicData data, SampleInfo sampleInfo)
    {
        if (sampleInfo == null)
        {
            return ReturnCode.BadParameter;
        }

        var wrapper = default(TopicBuiltinTopicDataWrapper);
        var infoWrapper = default(SampleInfoWrapper);

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinTakeNextSample(_native, ref wrapper, ref infoWrapper);
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
    public ReturnCode GetKeyValue(ref TopicBuiltinTopicData data, InstanceHandle handle)
    {
        if (handle == InstanceHandle.HandleNil)
        {
            return ReturnCode.BadParameter;
        }

        var aux = default(TopicBuiltinTopicDataWrapper);

        ReturnCode ret = (ReturnCode)UnsafeNativeMethods.TopicBuiltinGetKeyValue(_native, ref aux, handle);

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
    public InstanceHandle LookupInstance(TopicBuiltinTopicData instance)
    {
        return UnsafeNativeMethods.TopicBuiltinLookupInstance(_native, instance.ToNative());
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
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_Narrow")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr TopicBuiltinNarrow(IntPtr dr);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_Read")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinRead(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinReadWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_Take")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinTake(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinTakeWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadInstance")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinReadInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadInstanceWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinReadInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeInstance")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinTakeInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeInstanceWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinTakeInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadNextInstance")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinReadNextInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadNextInstanceWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinReadNextInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeNextInstance")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinTakeNextInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [LibraryImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeNextInstanceWithCondition")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int TopicBuiltinTakeNextInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadNextSample", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinReadNextSample(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicBuiltinTopicDataWrapper data, [In, Out] ref SampleInfoWrapper sampleInfo);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeNextSample", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinTakeNextSample(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicBuiltinTopicDataWrapper data, [In, Out] ref SampleInfoWrapper sampleInfo);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_LookupInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinLookupInstance(IntPtr dr, TopicBuiltinTopicDataWrapper instance);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_GetKeyValue", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinGetKeyValue(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicBuiltinTopicDataWrapper data, int handle);
#else
    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_Narrow", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr TopicBuiltinNarrow(IntPtr dr);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_Read", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinRead(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinReadWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_Take", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinTake(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinTakeWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinReadInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadInstanceWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinReadInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinTakeInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeInstanceWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinTakeInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadNextInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinReadNextInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadNextInstanceWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinReadNextInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeNextInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinTakeNextInstance(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, uint sampleStates, uint viewStates, uint instanceStates);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeNextInstanceWithCondition", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinTakeNextInstanceWithCondition(IntPtr dr, ref IntPtr receivedData, ref IntPtr receivedInfo, int handle, int maxSamples, IntPtr condition);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_ReadNextSample", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinReadNextSample(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicBuiltinTopicDataWrapper data, [In, Out] ref SampleInfoWrapper sampleInfo);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_TakeNextSample", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinTakeNextSample(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicBuiltinTopicDataWrapper data, [In, Out] ref SampleInfoWrapper sampleInfo);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_LookupInstance", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinLookupInstance(IntPtr dr, TopicBuiltinTopicDataWrapper instance);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(MarshalHelper.API_DLL, EntryPoint = "TopicBuiltinTopicDataDataReader_GetKeyValue", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TopicBuiltinGetKeyValue(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicBuiltinTopicDataWrapper data, int handle);
#endif
}