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
        private delegate void OnRequestedDeadlineMissedDelegate(IntPtr reader, RequestedDeadlineMissedStatus status);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnRequestedIncompatibleQosDelegate(IntPtr reader, RequestedIncompatibleQosStatusWrapper status);
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

        private GCHandle _gchDataOnReaders;
        private GCHandle _gchDataAvailable;
        private GCHandle _gchRequestedDeadlineMissed;
        private GCHandle _gchRequestedIncompatibleQos;
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

            _native = NewDomainParticipantListener(_onDataOnReaders,
                                                   _onDataAvailable,
                                                   _onRequestedDeadlineMissed,
                                                   _onRequestedIncompatibleQos);
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

        private void OnRequestedDeadlineMissedHandler(IntPtr reader, RequestedDeadlineMissedStatus status)
        {
            Entity entity = EntityManager.Instance.Find(reader);

            DataReader dataReader = null;
            if (entity != null)
            {
                dataReader = entity as DataReader;
            }

            OnRequestedDeadlineMissed(dataReader, status);
        }

        private void OnRequestedIncompatibleQosHandler(IntPtr reader, RequestedIncompatibleQosStatusWrapper status)
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

        private IntPtr NewDomainParticipantListener(OnDataOnReadersDelegate onDataOnReaders,
                                                    OnDataAvailableDelegate onDataAvailabe,
                                                    OnRequestedDeadlineMissedDelegate onRequestedDeadlineMissed,
                                                    OnRequestedIncompatibleQosDelegate onRequestedIncompatibleQos)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NewDomainParticipantListener86(onDataOnReaders, onDataAvailabe, onRequestedDeadlineMissed, onRequestedIncompatibleQos),
                                               () => UnsafeNativeMethods.NewDomainParticipantListener64(onDataOnReaders, onDataAvailabe, onRequestedDeadlineMissed, onRequestedIncompatibleQos));
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
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnRequestedIncompatibleQosDelegate onRequestedIncompatibleQos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipantListener_New", CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr NewDomainParticipantListener86([MarshalAs(UnmanagedType.FunctionPtr)] OnDataOnReadersDelegate onDataOnReaders,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnDataAvailableDelegate onDataAvalaible,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnRequestedDeadlineMissedDelegate onRequestedDeadlineMissed,
                                                                       [MarshalAs(UnmanagedType.FunctionPtr)] OnRequestedIncompatibleQosDelegate onRequestedIncompatibleQos);

        }
        #endregion
    }
}
