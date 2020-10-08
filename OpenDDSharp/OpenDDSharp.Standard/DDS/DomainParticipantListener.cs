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
using System.Security;
using OpenDDSharp.Helpers;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="DomainParticipant" />
    /// such that the application can be notified of relevant status changes.
    /// </summary>
    /// <remarks>
    /// The purpose of the DomainParticipantListener is to be the listener of last resort that is notified of all status changes not
    /// captured by more specific listeners attached to the <see cref="Entity" /> objects.When a relevant status change occurs, DDS will first attempt
    /// to notify the listener attached to the concerned <see cref="Entity" /> if one is installed. Otherwise, DDS will notify the Listener
    /// attached to the <see cref="DomainParticipant" />.
    /// </remarks>
    public abstract class DomainParticipantListener
    {
        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnDataOnReadersDelegate(IntPtr subscriber);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnDataAvailableDelegate(IntPtr reader);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnRequestedDeadlineMissedDelegate(IntPtr reader, ref RequestedDeadlineMissedStatus status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnRequestedIncompatibleQosDelegate(IntPtr reader, ref RequestedIncompatibleQosStatusWrapper status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnSampleRejectedDelegate(IntPtr reader, ref SampleRejectedStatus status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnLivelinessChangedDelegate(IntPtr reader, ref LivelinessChangedStatus status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnSubscriptionMatchedDelegate(IntPtr reader, ref SubscriptionMatchedStatus status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnSampleLostDelegate(IntPtr reader, ref SampleLostStatus status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnOfferedDeadlineMissedDelegate(IntPtr writer, ref OfferedDeadlineMissedStatus status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnOfferedIncompatibleQosDelegate(IntPtr writer, ref OfferedIncompatibleQosStatusWrapper status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnLivelinessLostDelegate(IntPtr writer, ref LivelinessLostStatus status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnPublicationMatchedDelegate(IntPtr writer, ref PublicationMatchedStatus status);
        #endregion

        #region Fields
        private readonly IntPtr _native;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnDataOnReadersDelegate _onDataOnReaders;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnDataAvailableDelegate _onDataAvailable;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnRequestedDeadlineMissedDelegate _onRequestedDeadlineMissed;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnRequestedIncompatibleQosDelegate _onRequestedIncompatibleQos;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnSampleRejectedDelegate _onSampleRejected;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnLivelinessChangedDelegate _onLivelinessChanged;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnSubscriptionMatchedDelegate _onSubscriptionMatched;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnSampleLostDelegate _onSampleLost;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnOfferedDeadlineMissedDelegate _onOfferedDeadlineMissed;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnOfferedIncompatibleQosDelegate _onOfferedIncompatibleQos;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnLivelinessLostDelegate _onLivelinessLost;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnPublicationMatchedDelegate _onPublicationMatched;

        private GCHandle _gchDataOnReaders;
        private GCHandle _gchDataAvailable;
        private GCHandle _gchRequestedDeadlineMissed;
        private GCHandle _gchRequestedIncompatibleQos;
        private GCHandle _gchSampleRejected;
        private GCHandle _gchLivelinessChanged;
        private GCHandle _gchSubscriptionMatched;
        private GCHandle _gchSampleLost;
        private GCHandle _gchOfferedDeadlineMissed;
        private GCHandle _gchOfferedIncompatibleQos;
        private GCHandle _gchLivelinessLost;
        private GCHandle _gchPublicationMatched;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainParticipantListener"/> class.
        /// </summary>
        protected DomainParticipantListener()
        {
            _onDataOnReaders = new OnDataOnReadersDelegate(OnDataOnReadersHandler);
            _gchDataOnReaders = GCHandle.Alloc(_onDataOnReaders);

            _onDataAvailable = new OnDataAvailableDelegate(OnDataAvailableHandler);
            _gchDataAvailable = GCHandle.Alloc(_onDataAvailable);

            _onRequestedDeadlineMissed = new OnRequestedDeadlineMissedDelegate(OnRequestedDeadlineMissedHandler);
            _gchRequestedDeadlineMissed = GCHandle.Alloc(_onRequestedDeadlineMissed);

            _onRequestedIncompatibleQos = new OnRequestedIncompatibleQosDelegate(OnRequestedIncompatibleQosHandler);
            _gchRequestedIncompatibleQos = GCHandle.Alloc(_onRequestedIncompatibleQos);

            _onSampleRejected = new OnSampleRejectedDelegate(OnSampleRejectedHandler);
            _gchSampleRejected = GCHandle.Alloc(_onSampleRejected);

            _onLivelinessChanged = new OnLivelinessChangedDelegate(OnLivelinessChangedHandler);
            _gchLivelinessChanged = GCHandle.Alloc(_onLivelinessChanged);

            _onSubscriptionMatched = new OnSubscriptionMatchedDelegate(OnSubscriptionMatchedHandler);
            _gchSubscriptionMatched = GCHandle.Alloc(_onSubscriptionMatched);

            _onSampleLost = new OnSampleLostDelegate(OnSampleLostHandler);
            _gchSampleLost = GCHandle.Alloc(_onSampleLost);

            _onOfferedDeadlineMissed = new OnOfferedDeadlineMissedDelegate(OnOfferedDeadlineMissedHandler);
            _gchOfferedDeadlineMissed = GCHandle.Alloc(_onOfferedDeadlineMissed);

            _onOfferedIncompatibleQos = new OnOfferedIncompatibleQosDelegate(OnOfferedIncompatibleQosHandler);
            _gchOfferedIncompatibleQos = GCHandle.Alloc(_onOfferedIncompatibleQos);

            _onLivelinessLost = new OnLivelinessLostDelegate(OnLivelinessLostHandler);
            _gchLivelinessLost = GCHandle.Alloc(_onLivelinessLost);

            _onPublicationMatched = new OnPublicationMatchedDelegate(OnPublicationMatchedHandler);
            _gchPublicationMatched = GCHandle.Alloc(_onPublicationMatched);

            _native = NewDomainParticipantListener(_onDataOnReaders,
                                                   _onDataAvailable,
                                                   _onRequestedDeadlineMissed,
                                                   _onRequestedIncompatibleQos,
                                                   _onSampleRejected,
                                                   _onLivelinessChanged,
                                                   _onSubscriptionMatched,
                                                   _onSampleLost,
                                                   _onOfferedDeadlineMissed,
                                                   _onOfferedIncompatibleQos,
                                                   _onLivelinessLost,
                                                   _onPublicationMatched);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DomainParticipantListener"/> class.
        /// </summary>
        ~DomainParticipantListener()
        {
            if (_gchDataOnReaders.IsAllocated)
            {
                _gchDataOnReaders.Free();
            }

            if (_gchDataAvailable.IsAllocated)
            {
                _gchDataAvailable.Free();
            }

            if (_gchRequestedDeadlineMissed.IsAllocated)
            {
                _gchRequestedDeadlineMissed.Free();
            }

            if (_gchRequestedIncompatibleQos.IsAllocated)
            {
                _gchRequestedIncompatibleQos.Free();
            }

            if (_gchSampleRejected.IsAllocated)
            {
                _gchSampleRejected.Free();
            }

            if (_gchLivelinessChanged.IsAllocated)
            {
                _gchLivelinessChanged.Free();
            }

            if (_gchSubscriptionMatched.IsAllocated)
            {
                _gchSubscriptionMatched.Free();
            }

            if (_gchSampleLost.IsAllocated)
            {
                _gchSampleLost.Free();
            }

            if (_gchOfferedDeadlineMissed.IsAllocated)
            {
                _gchOfferedDeadlineMissed.Free();
            }

            if (_gchOfferedIncompatibleQos.IsAllocated)
            {
                _gchRequestedIncompatibleQos.Free();
            }

            if (_gchLivelinessLost.IsAllocated)
            {
                _gchLivelinessLost.Free();
            }

            if (_gchPublicationMatched.IsAllocated)
            {
                _gchPublicationMatched.Free();
            }

            MarshalHelper.ReleaseNativePointer(_native);
        }
        #endregion

        #region Methods
        /// <summary>
        /// <para>Handles the <see cref="StatusKind.DataOnReadersStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.DataOnReadersStatus" /> indicates that new data is available on some of the data
        /// readers associated with the subscriber. Applications receiving this status can call GetDataReaders on
        /// the subscriber to get the set of data readers with data available.</para>
        /// </summary>
        /// <param name="subscriber">The <see cref="Subscriber" /> that triggered the event.</param>
        public abstract void OnDataOnReaders(Subscriber subscriber);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.DataAvailableStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.DataAvailableStatus" /> indicates that samples are available on the <see cref="DataReader" />.
        /// Applications receiving this status can use the various take and read operations on the <see cref="DataReader" /> to retrieve the data.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        public abstract void OnDataAvailable(DataReader reader);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.RequestedDeadlineMissedStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.RequestedDeadlineMissedStatus" /> indicates that the deadline requested via the
        /// <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="RequestedDeadlineMissedStatus" />.</param>
        public abstract void OnRequestedDeadlineMissed(DataReader reader, RequestedDeadlineMissedStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.RequestedIncompatibleQosStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.RequestedIncompatibleQosStatus" /> indicates that one or more QoS policy values that
        /// were requested were incompatible with what was offered.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="RequestedIncompatibleQosStatus" />.</param>
        public abstract void OnRequestedIncompatibleQos(DataReader reader, RequestedIncompatibleQosStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.SampleRejectedStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.SampleRejectedStatus" /> indicates that a sample received by the
        /// <see cref="DataReader" /> has been rejected.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="SampleRejectedStatus" />.</param>
        public abstract void OnSampleRejected(DataReader reader, SampleRejectedStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.LivelinessChangedStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.LivelinessChangedStatus" /> indicates that there have been liveliness changes for one or
        /// more <see cref="DataWriter" />s that are publishing instances for this <see cref="DataReader" />.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="LivelinessChangedStatus" />.</param>
        public abstract void OnLivelinessChanged(DataReader reader, LivelinessChangedStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.SubscriptionMatchedStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.SubscriptionMatchedStatus" /> indicates that either a compatible <see cref="DataWriter" /> has been
        /// matched or a previously matched <see cref="DataWriter" /> has ceased to be matched.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="SubscriptionMatchedStatus" />.</param>
        public abstract void OnSubscriptionMatched(DataReader reader, SubscriptionMatchedStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.SampleLostStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.SampleLostStatus" /> indicates that a sample has been lost and 
        /// never received by the <see cref="DataReader" />.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="SampleLostStatus" />.</param>
        public abstract void OnSampleLost(DataReader reader, SampleLostStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.OfferedDeadlineMissedStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.OfferedDeadlineMissedStatus" /> indicates that the deadline offered by the
        /// <see cref="DataWriter" /> has been missed for one or more instances.</para>
        /// </summary>
        /// <param name="writer">The <see cref="DataWriter" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="OfferedDeadlineMissedStatus" />.</param>
        public abstract void OnOfferedDeadlineMissed(DataWriter writer, OfferedDeadlineMissedStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.OfferedIncompatibleQosStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.OfferedIncompatibleQosStatus" /> indicates that an offered QoS was incompatible with
        /// the requested QoS of a <see cref="DataReader" />.</para>
        /// </summary>
        /// <param name="writer">The <see cref="DataWriter" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="OfferedIncompatibleQosStatus" />.</param>
        public abstract void OnOfferedIncompatibleQos(DataWriter writer, OfferedIncompatibleQosStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.LivelinessLostStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.LivelinessLostStatus" /> indicates that the liveliness that the <see cref="DataWriter" /> committed
        /// through its Liveliness QoS has not been respected. This means that any connected <see cref="DataReader" />s will consider this
        /// <see cref="DataWriter" /> no longer active</para>
        /// </summary>
        /// <param name="writer">The <see cref="DataWriter" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="LivelinessLostStatus" />.</param>
        public abstract void OnLivelinessLost(DataWriter writer, LivelinessLostStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.PublicationMatchedStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.PublicationMatchedStatus" /> indicates that the liveliness that the <see cref="DataWriter" /> committed
        /// through its Liveliness QoS has not been respected. This means that any connected <see cref="DataReader" />s 
        /// will consider this <see cref="DataWriter" /> no longer active.</para>
        /// </summary>
        /// <param name="writer">The <see cref="DataWriter" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="PublicationMatchedStatus" />.</param>
        public abstract void OnPublicationMatched(DataWriter writer, PublicationMatchedStatus status);

        private void OnDataOnReadersHandler(IntPtr subscriber)
        {
            Subscriber sub = null;

            Entity entity = EntityManager.Instance.Find(subscriber);
            if (entity != null)
            {
                sub = entity as Subscriber;
            }

            OnDataOnReaders(sub);
        }

        private void OnDataAvailableHandler(IntPtr reader)
        {
            Entity entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnDataAvailable(dataReader);
        }

        private void OnRequestedDeadlineMissedHandler(IntPtr reader, ref RequestedDeadlineMissedStatus status)
        {
            Entity entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnRequestedDeadlineMissed(dataReader, status);
        }

        private void OnRequestedIncompatibleQosHandler(IntPtr reader, ref RequestedIncompatibleQosStatusWrapper status)
        {
            Entity entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            RequestedIncompatibleQosStatus ret = default;
            ret.FromNative(status);

            OnRequestedIncompatibleQos(dataReader, ret);
        }

        private void OnSampleRejectedHandler(IntPtr reader, ref SampleRejectedStatus status)
        {
            Entity entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnSampleRejected(dataReader, status);
        }

        private void OnLivelinessChangedHandler(IntPtr reader, ref LivelinessChangedStatus status)
        {
            Entity entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnLivelinessChanged(dataReader, status);
        }

        private void OnSubscriptionMatchedHandler(IntPtr reader, ref SubscriptionMatchedStatus status)
        {
            Entity entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnSubscriptionMatched(dataReader, status);
        }

        private void OnSampleLostHandler(IntPtr reader, ref SampleLostStatus status)
        {
            Entity entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnSampleLost(dataReader, status);
        }

        private void OnOfferedDeadlineMissedHandler(IntPtr writer, ref OfferedDeadlineMissedStatus status)
        {
            Entity entity = EntityManager.Instance.Find(writer);

            DataWriter dataWriter = null;
            if (entity != null)
            {
                dataWriter = entity as DataWriter;
            }

            OnOfferedDeadlineMissed(dataWriter, status);
        }

        private void OnOfferedIncompatibleQosHandler(IntPtr writer, ref OfferedIncompatibleQosStatusWrapper status)
        {
            Entity entity = EntityManager.Instance.Find(writer);

            DataWriter dataWriter = null;
            if (entity != null)
            {
                dataWriter = entity as DataWriter;
            }

            OfferedIncompatibleQosStatus ret = default;
            ret.FromNative(status);

            OnOfferedIncompatibleQos(dataWriter, ret);
        }

        private void OnLivelinessLostHandler(IntPtr writer, ref LivelinessLostStatus status)
        {
            Entity entity = EntityManager.Instance.Find(writer);

            DataWriter dataWriter = null;
            if (entity != null)
            {
                dataWriter = entity as DataWriter;
            }

            OnLivelinessLost(dataWriter, status);
        }

        private void OnPublicationMatchedHandler(IntPtr writer, ref PublicationMatchedStatus status)
        {
            Entity entity = EntityManager.Instance.Find(writer);

            DataWriter dataWriter = null;
            if (entity != null)
            {
                dataWriter = entity as DataWriter;
            }

            OnPublicationMatched(dataWriter, status);
        }

        private IntPtr NewDomainParticipantListener(OnDataOnReadersDelegate onDataOnReaders,
                                                    OnDataAvailableDelegate onDataAvailabe,
                                                    OnRequestedDeadlineMissedDelegate onRequestedDeadlineMissed,
                                                    OnRequestedIncompatibleQosDelegate onRequestedIncompatibleQos,
                                                    OnSampleRejectedDelegate onSampleRejected,
                                                    OnLivelinessChangedDelegate onLivelinessChanged,
                                                    OnSubscriptionMatchedDelegate onSubscriptionMatched,
                                                    OnSampleLostDelegate onSampleLost,
                                                    OnOfferedDeadlineMissedDelegate onOfferedDeadlineMissed,
                                                    OnOfferedIncompatibleQosDelegate onOfferedIncompatibleQos,
                                                    OnLivelinessLostDelegate onLivelinessLost,
                                                    OnPublicationMatchedDelegate onPublicationMatched)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NewDomainParticipantListener86(onDataOnReaders,
                                                                                                        onDataAvailabe,
                                                                                                        onRequestedDeadlineMissed,
                                                                                                        onRequestedIncompatibleQos,
                                                                                                        onSampleRejected,
                                                                                                        onLivelinessChanged,
                                                                                                        onSubscriptionMatched,
                                                                                                        onSampleLost,
                                                                                                        onOfferedDeadlineMissed,
                                                                                                        onOfferedIncompatibleQos,
                                                                                                        onLivelinessLost,
                                                                                                        onPublicationMatched),
                                               () => UnsafeNativeMethods.NewDomainParticipantListener64(onDataOnReaders,
                                                                                                        onDataAvailabe,
                                                                                                        onRequestedDeadlineMissed,
                                                                                                        onRequestedIncompatibleQos,
                                                                                                        onSampleRejected,
                                                                                                        onLivelinessChanged,
                                                                                                        onSubscriptionMatched,
                                                                                                        onSampleLost,
                                                                                                        onOfferedDeadlineMissed,
                                                                                                        onOfferedIncompatibleQos,
                                                                                                        onLivelinessLost,
                                                                                                        onPublicationMatched));
        }

        internal IntPtr ToNative()
        {
            return _native;
        }
        #endregion

        #region UnsafeNativeMethods
        /// <summary>
        /// This class suppresses stack walks for unmanaged code permission. (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
        /// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full security review to make sure that the usage
        /// is secure because no stack walk will be performed.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private static class UnsafeNativeMethods
        {
            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipantListener_New", CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr NewDomainParticipantListener64([MarshalAs(UnmanagedType.FunctionPtr)] OnDataOnReadersDelegate onDataOnReaders,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnDataAvailableDelegate onDataAvalaible,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnRequestedDeadlineMissedDelegate onRequestedDeadlineMissed,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnRequestedIncompatibleQosDelegate onRequestedIncompatibleQos,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnSampleRejectedDelegate onSampleRejected,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnLivelinessChangedDelegate onLivelinessChanged,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnSubscriptionMatchedDelegate onSubscriptionMatched,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnSampleLostDelegate onSampleLost,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnOfferedDeadlineMissedDelegate onOfferedDeadlineMissed,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnOfferedIncompatibleQosDelegate onOfferedIncompatibleQos,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnLivelinessLostDelegate onLivelinessLost,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnPublicationMatchedDelegate onPublicationMatched);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipantListener_New", CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr NewDomainParticipantListener86([MarshalAs(UnmanagedType.FunctionPtr)] OnDataOnReadersDelegate onDataOnReaders,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnDataAvailableDelegate onDataAvalaible,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnRequestedDeadlineMissedDelegate onRequestedDeadlineMissed,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnRequestedIncompatibleQosDelegate onRequestedIncompatibleQos,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnSampleRejectedDelegate onSampleRejected,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnLivelinessChangedDelegate onLivelinessChanged,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnSubscriptionMatchedDelegate onSubscriptionMatched,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnSampleLostDelegate onSampleLost,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnOfferedDeadlineMissedDelegate onOfferedDeadlineMissed,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnOfferedIncompatibleQosDelegate onOfferedIncompatibleQos,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnLivelinessLostDelegate onLivelinessLost,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnPublicationMatchedDelegate onPublicationMatched);

        }
        #endregion
    }
}
