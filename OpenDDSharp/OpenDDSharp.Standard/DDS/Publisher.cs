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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// A Publisher is the object responsible for the actual dissemination of publications.
    /// </summary>
    /// <remarks>
    /// <para>The Publisher acts on the behalf of one or several <see cref="DataWriter" /> objects that belong to it. When it is informed of a change to the
    /// data associated with one of its <see cref="DataWriter" /> objects, it decides when it is appropriate to actually send the data-update message.
    /// In making this decision, it considers any extra information that goes with the data(timestamp, writer, etc.) as well as the QoS
    /// of the Publisher and the <see cref="DataWriter" />.</para>
    /// <para>All operations except for the operations SetQos, GetQos, SetListener, GetListener, Enable, GetStatusCondition,
    /// CreateDataWriter, and DeleteDataWriter return the value <see cref="ReturnCode.NotEnabled" /> if the Publisher has not been enabled yet.</para>
    /// </remarks>
    public class Publisher : Entity
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the attached <see cref="PublisherListener"/>.
        /// </summary>
        [SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "Keep coherency with the setter method and DDS API.")]
        public PublisherListener Listener { get; internal set; }
        #endregion

        #region Constructors
        internal Publisher(IntPtr native) : base(NarrowBase(native))
        {
            _native = native;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="DataWriter" /> with the default QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
        /// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
        /// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />, 
        /// the operation will fail and return a <see langword="null"/> result.</para>
        /// </remarks>
        /// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
        /// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
        public DataWriter CreateDataWriter(Topic topic)
        {
            return CreateDataWriter(topic, null);
        }

        /// <summary>
        /// Creates a new <see cref="DataWriter" /> with the desired QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
        /// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
        /// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />, 
        /// the operation will fail and return a <see langword="null"/> result.</para>
        /// </remarks>
        /// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
        /// <param name="qos">The <see cref="DataWriterQos" /> policies to be used for creating the new <see cref="DataWriter" />.</param>
        /// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
        public DataWriter CreateDataWriter(Topic topic, DataWriterQos qos)
        {
            return CreateDataWriter(topic, qos, null, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="DataWriter" /> with the desired QoS policies and attaches to it the specified <see cref="DataWriterListener" />.
        /// The specified <see cref="DataWriterListener" /> will be attached with the default <see cref="StatusMask" />.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
        /// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
        /// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />,
        /// the operation will fail and return a <see langword="null"/> result.</para>
        /// </remarks>
        /// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
        /// <param name="qos">The <see cref="DataWriterQos" /> policies to be used for creating the new <see cref="DataWriter" />.</param>
        /// <param name="listener">The <see cref="DataWriterListener" /> to be attached to the newly created <see cref="DataWriter" />.</param>
        /// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
        public DataWriter CreateDataWriter(Topic topic, DataWriterQos qos, DataWriterListener listener)
        {
            return CreateDataWriter(topic, qos, listener, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="DataWriter" /> with the desired QoS policies and attaches to it the specified <see cref="DataWriterListener" />
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="DataWriter" /> will be attached and belongs to the <see cref="Publisher" /> that is its factory.</para>
        /// <para>The <see cref="Topic" /> passed to this operation must have been created from the same <see cref="DomainParticipant" /> that was used to create this
        /// <see cref="Publisher" />. If the <see cref="Topic" /> was created from a different <see cref="DomainParticipant" />,
        /// the operation will fail and return a <see langword="null"/> result.</para>
        /// </remarks>
        /// <param name="topic">The <see cref="Topic" /> that the <see cref="DataWriter" /> will be associated with.</param>
        /// <param name="qos">The <see cref="DataWriterQos" /> policies to be used for creating the new <see cref="DataWriter" />.</param>
        /// <param name="listener">The <see cref="DataWriterListener" /> to be attached to the newly created <see cref="DataWriter" />.</param>
        /// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
        /// <returns>The newly created <see cref="DataWriter" /> on success, otherwise <see langword="null"/>.</returns>
        public DataWriter CreateDataWriter(Topic topic, DataWriterQos qos, DataWriterListener listener, StatusMask statusMask)
        {
            if (topic is null)
            {
                throw new ArgumentNullException(nameof(topic));
            }

            DataWriterQosWrapper qosWrapper = default;
            if (qos is null)
            {
                qos = new DataWriterQos();
                var ret = GetDefaultDataWriterQos(qos);
                if (ret == ReturnCode.Ok)
                {
                    qosWrapper = qos.ToNative();
                }
            }
            else
            {
                qosWrapper = qos.ToNative();
            }

            IntPtr nativeListener = IntPtr.Zero;
            if (listener != null)
            {
                nativeListener = listener.ToNative();
            }

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateDataWriter86(_native, topic.ToNative(), qosWrapper, nativeListener, statusMask),
                                                        () => UnsafeNativeMethods.CreateDataWriter64(_native, topic.ToNative(), qosWrapper, nativeListener, statusMask));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            qos.Release();

            var dw = new DataWriter(native)
            {
                Listener = listener,
            };

            EntityManager.Instance.Add((dw as Entity).ToNative(), dw);
            ContainedEntities.Add(dw);

            return dw;
        }

        /// <summary>
        /// Gets the default value of the <see cref="DataWriter" /> QoS, that is, the QoS policies which will be used for newly created
        /// <see cref="DataWriter" /> entities in the case where the QoS policies are defaulted in the CreateDataWriter operation.
        /// </summary>
        /// <remarks>
        /// The values retrieved by GetDefaultDataWriterQos will match the set of values specified on the last successful call to
        /// <see cref="SetDefaultDataWriterQos" />, or else, if the call was never made, the default DDS standard values.
        /// </remarks>
        /// <param name="qos">The <see cref="DataWriterQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetDefaultDataWriterQos(DataWriterQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            DataWriterQosWrapper qosWrapper = default;
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDefaultDataWriterQos86(_native, ref qosWrapper),
                                                  () => UnsafeNativeMethods.GetDefaultDataWriterQos64(_native, ref qosWrapper));

            if (ret == ReturnCode.Ok)
            {
                qos.FromNative(qosWrapper);
            }

            qos.Release();

            return ret;
        }

        /// <summary>
        /// Sets a default value of the <see cref="Publisher" /> QoS policies which will be used for newly created <see cref="Publisher" /> entities in the
        /// case where the QoS policies are defaulted in the CreatePublisher operation.
        /// </summary>
        /// <remarks>
        /// This operation will check that the resulting policies are self consistent; if they are not, the operation will have no effect and
        /// return <see cref="ReturnCode.InconsistentPolicy" />.
        /// </remarks>
        /// <param name="qos">The default <see cref="PublisherQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetDefaultDataWriterQos(DataWriterQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            var qosNative = qos.ToNative();
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetDefaultDataWriterQos86(_native, qosNative),
                                                  () => UnsafeNativeMethods.SetDefaultDataWriterQos64(_native, qosNative));
            qos.Release();

            return ret;
        }

        /// <summary>
        /// Gets the <see cref="Publisher" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="PublisherQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetQos(PublisherQos qos)
        {
            if (qos == null)
            {
                return ReturnCode.BadParameter;
            }

            PublisherQosWrapper qosWrapper = default;
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetQos86(_native, ref qosWrapper),
                                                  () => UnsafeNativeMethods.GetQos64(_native, ref qosWrapper));

            if (ret == ReturnCode.Ok)
            {
                qos.FromNative(qosWrapper);
            }

            qos.Release();

            return ret;
        }

        /// <summary>
        /// Sets the <see cref="Publisher" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="PublisherQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetQos(PublisherQos qos)
        {
            if (qos == null)
            {
                return ReturnCode.BadParameter;
            }

            var qosNative = qos.ToNative();

            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetQos86(_native, qosNative),
                                                  () => UnsafeNativeMethods.SetQos64(_native, qosNative));

            qos.Release();

            return ret;
        }

        /// <summary>
        /// Deletes a <see cref="DataWriter" /> that belongs to the <see cref="Publisher" />.
        /// </summary>
        /// <remarks>
        /// <para>The DeleteDataWriter operation must be called on the same <see cref="Publisher" /> object used to create the <see cref="DataWriter" />. If
        /// DeleteDataWriter operation is called on a different <see cref="Publisher" />, the operation will have no effect and it will return
        /// <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// <para>The deletion of the <see cref="DataWriter" /> will automatically unregister all instances. Depending on the settings of the
        /// <see cref="WriterDataLifecycleQosPolicy" />, the deletion of the <see cref="DataWriter" /> may also dispose all instances.</para>
        /// </remarks>
        /// <param name="datawriter">The <see cref="DataWriter" /> to be deleted.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteDataWriter(DataWriter datawriter)
        {
            if (datawriter == null)
            {
                return ReturnCode.Ok;
            }

            var native = datawriter.ToNative();
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeleteDataWriter86(_native, native),
                                                  () => UnsafeNativeMethods.DeleteDataWriter64(_native, native));
            if (ret == ReturnCode.Ok)
            {
                EntityManager.Instance.Remove(native);
                ContainedEntities.Remove(datawriter);
            }

            return ret;
        }

        /// <summary>
        /// Allows access to the attached <see cref="PublisherListener" />.
        /// </summary>
        /// <returns>The attached <see cref="PublisherListener" />.</returns>
        [Obsolete(nameof(GetListener) + " is deprecated, please use Listener property instead.")]
        [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Keep coherency with the setter method and DDS API.")]
        public PublisherListener GetListener()
        {
            return Listener;
        }

        /// <summary>
        /// Sets the <see cref="PublisherListener" /> using the <see cref="StatusMask.DefaultStatusMask" />.
        /// </summary>
        /// <param name="listener">The <see cref="PublisherListener" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetListener(PublisherListener listener)
        {
            return SetListener(listener, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Sets the <see cref="PublisherListener" />.
        /// </summary>
        /// <param name="listener">The <see cref="PublisherListener" /> to be set.</param>
        /// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetListener(PublisherListener listener, StatusMask mask)
        {
            Listener = listener;
            IntPtr ptr = IntPtr.Zero;
            if (listener != null)
            {
                ptr = listener.ToNative();
            }

            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetListener86(_native, ptr, mask),
                                               () => UnsafeNativeMethods.SetListener64(_native, ptr, mask));
        }

        private static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
                                               () => UnsafeNativeMethods.NarrowBase64(ptr));
        }

        /// <summary>
        /// Internal use only.
        /// </summary>
        /// <returns>The native pointer.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal new IntPtr ToNative()
        {
            return _native;
        }
        #endregion

        #region Unsafe Native Methods
        /// <summary>
        /// This class suppresses stack walks for unmanaged code permission. (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.)
        /// This class is for methods that are potentially dangerous. Any caller of these methods must perform a full security review to make sure that the usage
        /// is secure because no stack walk will be performed.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private static class UnsafeNativeMethods
        {
            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_CreateDataWriter", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateDataWriter64(IntPtr pub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_CreateDataWriter", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateDataWriter86(IntPtr pub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_GetDefaultDataWriterQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultDataWriterQos64(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataWriterQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_GetDefaultDataWriterQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultDataWriterQos86(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataWriterQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_SetDefaultDataWriterQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultDataWriterQos64(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_SetDefaultDataWriterQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultDataWriterQos86(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos64(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos86(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos64(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In] PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos86(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In] PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_DeleteDataWriter", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteDataWriter64(IntPtr pub, IntPtr dataWriter);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_DeleteDataWriter", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteDataWriter86(IntPtr pub, IntPtr dataWriter);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "Publisher_SetListener", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetListener64(IntPtr pub, IntPtr listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "Publisher_SetListener", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetListener86(IntPtr pub, IntPtr listener, uint mask);
        }
        #endregion
    }
}
