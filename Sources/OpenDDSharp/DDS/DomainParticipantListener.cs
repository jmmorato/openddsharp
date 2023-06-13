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
    public abstract class DomainParticipantListener : IDisposable
    {
        #region Delegates
        private delegate void OnDataOnReadersDelegate(IntPtr subscriber);
        private delegate void OnDataAvailableDelegate(IntPtr reader);
        private delegate void OnRequestedDeadlineMissedDelegate(IntPtr reader, ref RequestedDeadlineMissedStatus status);
        private delegate void OnRequestedIncompatibleQosDelegate(IntPtr reader, ref RequestedIncompatibleQosStatusWrapper status);
        private delegate void OnSampleRejectedDelegate(IntPtr reader, ref SampleRejectedStatus status);
        private delegate void OnLivelinessChangedDelegate(IntPtr reader, ref LivelinessChangedStatus status);
        private delegate void OnSubscriptionMatchedDelegate(IntPtr reader, ref SubscriptionMatchedStatus status);
        private delegate void OnSampleLostDelegate(IntPtr reader, ref SampleLostStatus status);
        private delegate void OnOfferedDeadlineMissedDelegate(IntPtr writer, ref OfferedDeadlineMissedStatus status);
        private delegate void OnOfferedIncompatibleQosDelegate(IntPtr writer, ref OfferedIncompatibleQosStatusWrapper status);
        private delegate void OnLivelinessLostDelegate(IntPtr writer, ref LivelinessLostStatus status);
        private delegate void OnPublicationMatchedDelegate(IntPtr writer, ref PublicationMatchedStatus status);
        private delegate void OnInconsistentTopicDelegate(IntPtr topic, ref InconsistentTopicStatus status);
        #endregion

        #region Fields
        private readonly IntPtr _native;
        private bool _disposed;

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
        private GCHandle _gchInconsistentTopic;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainParticipantListener"/> class.
        /// </summary>
        protected DomainParticipantListener()
        {
            OnDataOnReadersDelegate onDataOnReaders = OnDataOnReadersHandler;
            _gchDataOnReaders = GCHandle.Alloc(onDataOnReaders);

            OnDataAvailableDelegate onDataAvailable = OnDataAvailableHandler;
            _gchDataAvailable = GCHandle.Alloc(onDataAvailable);

            OnRequestedDeadlineMissedDelegate onRequestedDeadlineMissed = OnRequestedDeadlineMissedHandler;
            _gchRequestedDeadlineMissed = GCHandle.Alloc(onRequestedDeadlineMissed);

            OnRequestedIncompatibleQosDelegate onRequestedIncompatibleQos = OnRequestedIncompatibleQosHandler;
            _gchRequestedIncompatibleQos = GCHandle.Alloc(onRequestedIncompatibleQos);

            OnSampleRejectedDelegate onSampleRejected = OnSampleRejectedHandler;
            _gchSampleRejected = GCHandle.Alloc(onSampleRejected);

            OnLivelinessChangedDelegate onLivelinessChanged = OnLivelinessChangedHandler;
            _gchLivelinessChanged = GCHandle.Alloc(onLivelinessChanged);

            OnSubscriptionMatchedDelegate onSubscriptionMatched = OnSubscriptionMatchedHandler;
            _gchSubscriptionMatched = GCHandle.Alloc(onSubscriptionMatched);

            OnSampleLostDelegate onSampleLost = OnSampleLostHandler;
            _gchSampleLost = GCHandle.Alloc(onSampleLost);

            OnOfferedDeadlineMissedDelegate onOfferedDeadlineMissed = OnOfferedDeadlineMissedHandler;
            _gchOfferedDeadlineMissed = GCHandle.Alloc(onOfferedDeadlineMissed);

            OnOfferedIncompatibleQosDelegate onOfferedIncompatibleQos = OnOfferedIncompatibleQosHandler;
            _gchOfferedIncompatibleQos = GCHandle.Alloc(onOfferedIncompatibleQos);

            OnLivelinessLostDelegate onLivelinessLost = OnLivelinessLostHandler;
            _gchLivelinessLost = GCHandle.Alloc(onLivelinessLost);

            OnPublicationMatchedDelegate onPublicationMatched = OnPublicationMatchedHandler;
            _gchPublicationMatched = GCHandle.Alloc(onPublicationMatched);

            OnInconsistentTopicDelegate onInconsistentTopic = OnInconsistentTopicHandler;
            _gchInconsistentTopic = GCHandle.Alloc(onInconsistentTopic);

            _native = UnsafeNativeMethods.NewDomainParticipantListener(
                Marshal.GetFunctionPointerForDelegate(onDataOnReaders),
                Marshal.GetFunctionPointerForDelegate(onDataAvailable),
                Marshal.GetFunctionPointerForDelegate(onRequestedDeadlineMissed),
                Marshal.GetFunctionPointerForDelegate(onRequestedIncompatibleQos),
                Marshal.GetFunctionPointerForDelegate(onSampleRejected),
                Marshal.GetFunctionPointerForDelegate(onLivelinessChanged),
                Marshal.GetFunctionPointerForDelegate(onSubscriptionMatched),
                Marshal.GetFunctionPointerForDelegate(onSampleLost),
                Marshal.GetFunctionPointerForDelegate(onOfferedDeadlineMissed),
                Marshal.GetFunctionPointerForDelegate(onOfferedIncompatibleQos),
                Marshal.GetFunctionPointerForDelegate(onLivelinessLost),
                Marshal.GetFunctionPointerForDelegate(onPublicationMatched),
                Marshal.GetFunctionPointerForDelegate(onInconsistentTopic));
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DomainParticipantListener"/> class.
        /// </summary>
        ~DomainParticipantListener()
        {
            Dispose(false);
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

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.InconsistentTopicStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.InconsistentTopicStatus" /> indicates that a <see cref="Topic" /> was attempted to be registered that
        /// already exists with different characteristics. Typically, the existing <see cref="Topic" /> may have a different type associated with it.</para>
        /// </summary>
        /// <param name="topic">The <see cref="Topic" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="InconsistentTopicStatus" />.</param>
        public abstract void OnInconsistentTopic(Topic topic, InconsistentTopicStatus status);

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

        private void OnInconsistentTopicHandler(IntPtr topic, ref InconsistentTopicStatus status)
        {
            Entity entity = EntityManager.Instance.Find(topic);

            Topic t = null;
            if (entity != null)
            {
                t = entity as Topic;
            }

            OnInconsistentTopic(t, status);
        }

        internal IntPtr ToNative()
        {
            return _native;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DataReaderListener" />.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True to free managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            ReleaseUnmanagedResources();
        }

        private void ReleaseUnmanagedResources()
        {
            UnsafeNativeMethods.DisposeDomainParticipantListener(_native);

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
                _gchOfferedIncompatibleQos.Free();
            }

            if (_gchLivelinessLost.IsAllocated)
            {
                _gchLivelinessLost.Free();
            }

            if (_gchPublicationMatched.IsAllocated)
            {
                _gchPublicationMatched.Free();
            }

            if (_gchInconsistentTopic.IsAllocated)
            {
                _gchInconsistentTopic.Free();
            }

            _native.ReleaseNativePointer();
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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DomainParticipantListener_New", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NewDomainParticipantListener(IntPtr onDataOnReaders,
                IntPtr onDataAvailable,
                IntPtr onRequestedDeadlineMissed,
                IntPtr onRequestedIncompatibleQos,
                IntPtr onSampleRejected,
                IntPtr onLivelinessChanged,
                IntPtr onSubscriptionMatched,
                IntPtr onSampleLost,
                IntPtr onOfferedDeadlineMissed,
                IntPtr onOfferedIncompatibleQos,
                IntPtr onLivelinessLost,
                IntPtr onPublicationMatched,
                IntPtr onInconsistentTopic);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DomainParticipantListener_Dispose", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr DisposeDomainParticipantListener(IntPtr native);
        }
        #endregion
    }
}
