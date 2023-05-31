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
    /// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="DataReader" />
    /// such that the application can be notified of relevant status changes.
    /// </summary>
    public abstract class DataReaderListener : IDisposable
    {
        #region Delegates
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
        #endregion

        #region Fields
        private readonly IntPtr _native;
        private bool _disposed;

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

        private GCHandle _gchDataAvailable;
        private GCHandle _gchRequestedDeadlineMissed;
        private GCHandle _gchRequestedIncompatibleQos;
        private GCHandle _gchSampleRejected;
        private GCHandle _gchLivelinessChanged;
        private GCHandle _gchSubscriptionMatched;
        private GCHandle _gchSampleLost;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataReaderListener"/> class.
        /// </summary>
        protected DataReaderListener()
        {
            _onDataAvailable = OnDataAvailableHandler;
            _gchDataAvailable = GCHandle.Alloc(_onDataAvailable);

            _onRequestedDeadlineMissed = OnRequestedDeadlineMissedHandler;
            _gchRequestedDeadlineMissed = GCHandle.Alloc(_onRequestedDeadlineMissed);

            _onRequestedIncompatibleQos = OnRequestedIncompatibleQosHandler;
            _gchRequestedIncompatibleQos = GCHandle.Alloc(_onRequestedIncompatibleQos);

            _onSampleRejected = OnSampleRejectedHandler;
            _gchSampleRejected = GCHandle.Alloc(_onSampleRejected);

            _onLivelinessChanged = OnLivelinessChangedHandler;
            _gchLivelinessChanged = GCHandle.Alloc(_onLivelinessChanged);

            _onSubscriptionMatched = OnSubscriptionMatchedHandler;
            _gchSubscriptionMatched = GCHandle.Alloc(_onSubscriptionMatched);

            _onSampleLost = OnSampleLostHandler;
            _gchSampleLost = GCHandle.Alloc(_onSampleLost);

            _native = UnsafeNativeMethods.NewDataReaderListener(_onDataAvailable,
                                                                _onRequestedDeadlineMissed,
                                                                _onRequestedIncompatibleQos,
                                                                _onSampleRejected,
                                                                _onLivelinessChanged,
                                                                _onSubscriptionMatched,
                                                                _onSampleLost);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DataReaderListener"/> class.
        /// </summary>
        ~DataReaderListener()
        {
            Dispose(false);
        }
        #endregion

        #region Methods
        /// <summary>
        /// <para>Handles the <see cref="StatusKind.DataAvailableStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.DataAvailableStatus" /> indicates that samples are available on the <see cref="DataReader" />.
        /// Applications receiving this status can use the various take and read operations on the <see cref="DataReader" /> to retrieve the data.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        protected abstract void OnDataAvailable(DataReader reader);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.RequestedDeadlineMissedStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.RequestedDeadlineMissedStatus" /> indicates that the deadline requested via the
        /// <see cref="DeadlineQosPolicy" /> was not respected for a specific instance.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="RequestedDeadlineMissedStatus" />.</param>
        protected abstract void OnRequestedDeadlineMissed(DataReader reader, RequestedDeadlineMissedStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.RequestedIncompatibleQosStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.RequestedIncompatibleQosStatus" /> indicates that one or more QoS policy values that
        /// were requested were incompatible with what was offered.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="RequestedIncompatibleQosStatus" />.</param>
        protected abstract void OnRequestedIncompatibleQos(DataReader reader, RequestedIncompatibleQosStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.SampleRejectedStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.SampleRejectedStatus" /> indicates that a sample received by the
        /// <see cref="DataReader" /> has been rejected.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="SampleRejectedStatus" />.</param>
        protected abstract void OnSampleRejected(DataReader reader, SampleRejectedStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.LivelinessChangedStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.LivelinessChangedStatus" /> indicates that there have been liveliness changes for one or
        /// more <see cref="DataWriter" />s that are publishing instances for this <see cref="DataReader" />.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="LivelinessChangedStatus" />.</param>
        protected abstract void OnLivelinessChanged(DataReader reader, LivelinessChangedStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.SubscriptionMatchedStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.SubscriptionMatchedStatus" /> indicates that either a compatible <see cref="DataWriter" /> has been
        /// matched or a previously matched <see cref="DataWriter" /> has ceased to be matched.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="SubscriptionMatchedStatus" />.</param>
        protected abstract void OnSubscriptionMatched(DataReader reader, SubscriptionMatchedStatus status);

        /// <summary>
        /// <para>Handles the <see cref="StatusKind.SampleLostStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.SampleLostStatus" /> indicates that a sample has been lost and 
        /// never received by the <see cref="DataReader" />.</para>
        /// </summary>
        /// <param name="reader">The <see cref="DataReader" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="SampleLostStatus" />.</param>
        protected abstract void OnSampleLost(DataReader reader, SampleLostStatus status);

        private void OnDataAvailableHandler(IntPtr reader)
        {
            if (_disposed)
            {
                return;
            }

            var entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnDataAvailable(dataReader);
        }

        private void OnRequestedDeadlineMissedHandler(IntPtr reader, ref RequestedDeadlineMissedStatus status)
        {
            if (_disposed)
            {
                return;
            }

            var entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnRequestedDeadlineMissed(dataReader, status);
        }

        private void OnRequestedIncompatibleQosHandler(IntPtr reader, ref RequestedIncompatibleQosStatusWrapper status)
        {
            if (_disposed)
            {
                return;
            }

            var entity = EntityManager.Instance.Find(reader);

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
            if (_disposed)
            {
                return;
            }

            var entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnSampleRejected(dataReader, status);
        }

        private void OnLivelinessChangedHandler(IntPtr reader, ref LivelinessChangedStatus status)
        {
            if (_disposed)
            {
                return;
            }

            var entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnLivelinessChanged(dataReader, status);
        }

        private void OnSubscriptionMatchedHandler(IntPtr reader, ref SubscriptionMatchedStatus status)
        {
            if (_disposed)
            {
                return;
            }

            var entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnSubscriptionMatched(dataReader, status);
        }

        private void OnSampleLostHandler(IntPtr reader, ref SampleLostStatus status)
        {
            if (_disposed)
            {
                return;
            }

            var entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnSampleLost(dataReader, status);
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
            UnsafeNativeMethods.DisposeDataReaderListener(_native);

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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataReaderListener_New", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NewDataReaderListener([MarshalAs(UnmanagedType.FunctionPtr)] OnDataAvailableDelegate onDataAvailable,
                                                              [MarshalAs(UnmanagedType.FunctionPtr)] OnRequestedDeadlineMissedDelegate onRequestedDeadlineMissed,
                                                              [MarshalAs(UnmanagedType.FunctionPtr)] OnRequestedIncompatibleQosDelegate onRequestedIncompatibleQos,
                                                              [MarshalAs(UnmanagedType.FunctionPtr)] OnSampleRejectedDelegate onSampleRejected,
                                                              [MarshalAs(UnmanagedType.FunctionPtr)] OnLivelinessChangedDelegate onLivelinessChanged,
                                                              [MarshalAs(UnmanagedType.FunctionPtr)] OnSubscriptionMatchedDelegate onSubscriptionMatched,
                                                              [MarshalAs(UnmanagedType.FunctionPtr)] OnSampleLostDelegate onSampleLost);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "DataReaderListener_Dispose", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr DisposeDataReaderListener(IntPtr native);
        }
        #endregion
    }
}
