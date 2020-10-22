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
    /// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="DataWriterListener" />
    /// such that the application can be notified of relevant status changes.
    /// </summary>
    public abstract class DataWriterListener
    {
        #region Delegates
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
        private readonly OnOfferedDeadlineMissedDelegate _onOfferedDeadlineMissed;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnOfferedIncompatibleQosDelegate _onOfferedIncompatibleQos;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnLivelinessLostDelegate _onLivelinessLost;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnPublicationMatchedDelegate _onPublicationMatched;

        private GCHandle _gchOfferedDeadlineMissed;
        private GCHandle _gchOfferedIncompatibleQos;
        private GCHandle _gchLivelinessLost;
        private GCHandle _gchPublicationMatched;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataWriterListener"/> class.
        /// </summary>
        protected DataWriterListener()
        {
            _onOfferedDeadlineMissed = new OnOfferedDeadlineMissedDelegate(OnOfferedDeadlineMissedHandler);
            _gchOfferedDeadlineMissed = GCHandle.Alloc(_onOfferedDeadlineMissed);

            _onOfferedIncompatibleQos = new OnOfferedIncompatibleQosDelegate(OnOfferedIncompatibleQosHandler);
            _gchOfferedIncompatibleQos = GCHandle.Alloc(_onOfferedIncompatibleQos);

            _onLivelinessLost = new OnLivelinessLostDelegate(OnLivelinessLostHandler);
            _gchLivelinessLost = GCHandle.Alloc(_onLivelinessLost);

            _onPublicationMatched = new OnPublicationMatchedDelegate(OnPublicationMatchedHandler);
            _gchPublicationMatched = GCHandle.Alloc(_onPublicationMatched);

            _native = NewDataWriterListener(_onOfferedDeadlineMissed,
                                            _onOfferedIncompatibleQos,
                                            _onLivelinessLost,
                                            _onPublicationMatched);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DataWriterListener"/> class.
        /// </summary>
        ~DataWriterListener()
        {
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

            MarshalHelper.ReleaseNativePointer(_native);
        }
        #endregion

        #region Methods
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

        private IntPtr NewDataWriterListener(OnOfferedDeadlineMissedDelegate onOfferedDeadlineMissed,
                                             OnOfferedIncompatibleQosDelegate onOfferedIncompatibleQos,
                                             OnLivelinessLostDelegate onLivelinessLost,
                                             OnPublicationMatchedDelegate onPublicationMatched)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NewDataWriterListener86(onOfferedDeadlineMissed,
                                                                                                 onOfferedIncompatibleQos,
                                                                                                 onLivelinessLost,
                                                                                                 onPublicationMatched),
                                               () => UnsafeNativeMethods.NewDataWriterListener64(onOfferedDeadlineMissed,
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataWriterListener_New", CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr NewDataWriterListener64([MarshalAs(UnmanagedType.FunctionPtr)] OnOfferedDeadlineMissedDelegate onOfferedDeadlineMissed,
                                                                [MarshalAs(UnmanagedType.FunctionPtr)] OnOfferedIncompatibleQosDelegate onOfferedIncompatibleQos,
                                                                [MarshalAs(UnmanagedType.FunctionPtr)] OnLivelinessLostDelegate onLivelinessLost,
                                                                [MarshalAs(UnmanagedType.FunctionPtr)] OnPublicationMatchedDelegate onPublicationMatched);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataWriterListener_New", CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr NewDataWriterListener86([MarshalAs(UnmanagedType.FunctionPtr)] OnOfferedDeadlineMissedDelegate onOfferedDeadlineMissed,
                                                                [MarshalAs(UnmanagedType.FunctionPtr)] OnOfferedIncompatibleQosDelegate onOfferedIncompatibleQos,
                                                                [MarshalAs(UnmanagedType.FunctionPtr)] OnLivelinessLostDelegate onLivelinessLost,
                                                                [MarshalAs(UnmanagedType.FunctionPtr)] OnPublicationMatchedDelegate onPublicationMatched);
        }
        #endregion
    }
}
