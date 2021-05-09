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

        /// <summary>
        /// Gets the <see cref="DomainParticipant" /> to which the <see cref="Publisher" /> belongs.
        /// </summary>
        public DomainParticipant Participant => GetParticipant();
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

            IntPtr native = UnsafeNativeMethods.CreateDataWriter(_native, topic.ToNative(), qosWrapper, nativeListener, statusMask);

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
            var ret = UnsafeNativeMethods.DeleteDataWriter(_native, native);

            if (ret == ReturnCode.Ok)
            {
                EntityManager.Instance.Remove((datawriter as Entity).ToNative());
                ContainedEntities.Remove(datawriter);
            }

            return ret;
        }

        /// <summary>
        /// Gets a previously created <see cref="DataWriter" /> belonging to the <see cref="Publisher" /> that is attached to a <see cref="Topic" /> with a matching
        /// topic name. If no such <see cref="DataWriter" /> exists, the operation will return <see langword="null"/>.
        /// </summary>
        /// <remarks>
        /// If multiple <see cref="DataWriter" /> attached to the <see cref="Publisher" /> satisfy the topic name condition, then the operation will return one of them. It is not
        /// specified which one.
        /// </remarks>
        /// <param name="topicName">The <see cref="Topic" />'s name related with the <see cref="DataWriter" /> to look up.</param>
        /// <returns>The <see cref="DataWriter" />, if it exists, otherwise <see langword="null"/>.</returns>
        public DataWriter LookupDataWriter(string topicName)
        {
            IntPtr native = UnsafeNativeMethods.LookupDataWriter(_native, topicName);

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return (DataWriter)EntityManager.Instance.Find(native);
        }

        /// <summary>
        /// This operation deletes all the entities that were created by means of the "create" operations on the <see cref="Publisher" />. That is, it deletes
        /// all contained <see cref="DataWriter" /> objects.
        /// </summary>
        /// <remarks>
        /// <para>The operation will return <see cref="ReturnCode.PreconditionNotMet" /> if the any of the contained entities is in a state where it cannot be deleted.</para>
        /// <para>Once DeleteContainedEntities returns successfully, the application may delete the <see cref="Publisher" /> knowing that it has no
        /// contained <see cref="DataWriter" /> objects.</para>
        /// </remarks>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteContainedEntities()
        {
            ReturnCode ret = UnsafeNativeMethods.DeleteContainedEntities(_native);
            if (ret == ReturnCode.Ok)
            {
                foreach (Entity e in ContainedEntities)
                {
                    EntityManager.Instance.Remove(e.ToNative());
                }

                ContainedEntities.Clear();
            }

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
            var ret = UnsafeNativeMethods.GetQos(_native, ref qosWrapper);

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

            var ret = UnsafeNativeMethods.SetQos(_native, qosNative);

            qos.Release();

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

            return UnsafeNativeMethods.SetListener(_native, ptr, mask);
        }

        /// <summary>
        /// This operation indicates to DDS that the application is about to make multiple modifications using <see cref="DataWriter" /> objects
        /// belonging to the <see cref="Publisher" />. It is a hint to DDS so it can optimize its performance by e.g., holding the dissemination of the modifications and then
        /// batching them.
        /// </summary>
        /// <remarks>
        /// The use of this operation must be matched by a corresponding call to <see cref="Publisher.ResumePublications" /> indicating that the set of
        /// modifications has completed. If the <see cref="Publisher" /> is deleted before <see cref="Publisher.ResumePublications" /> is called, any suspended updates yet to
        /// be published will be discarded.
        /// </remarks>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SuspendPublications()
        {
            return UnsafeNativeMethods.SuspendPublications(_native);
        }

        /// <summary>
        /// This operation indicates to DDS that the application has completed the multiple changes initiated by the previous <see cref="Publisher.SuspendPublications" />.
        /// </summary>
        /// <remarks>
        /// The call to ResumePublications must match a previous call to <see cref="Publisher.SuspendPublications" />.
        /// Otherwise the operation will return the error <see cref="ReturnCode.PreconditionNotMet" />.
        /// </remarks>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode ResumePublications()
        {
            return UnsafeNativeMethods.ResumePublications(_native);
        }

        /// <summary>
        /// Requests that the application will begin a 'coherent set' of modifications using <see cref="DataWriter" /> objects attached to
        /// the <see cref="Publisher" />. The 'coherent set' will be completed by a matching call to <see cref="Publisher.EndCoherentChanges" />.
        /// </summary>
        /// <remarks>
        /// <para>A 'coherent set' is a set of modifications that must be propagated in such a way that they are interpreted at the receivers' side
        /// as a consistent set of modifications; that is, the receiver will only be able to access the data after all the modifications in the set
        /// are available at the receiver end.</para>
        /// <para>A connectivity change may occur in the middle of a set of coherent changes; for example, the set of partitions used by the
        /// <see cref="Publisher" /> or one of its <see cref="Subscriber" />s may change, a late-joining <see cref="DataReader" /> may appear on the network, or a communication
        /// failure may occur. In the event that such a change prevents an entity from receiving the entire set of coherent changes, that
        /// entity must behave as if it had received none of the set.</para>
        /// <para>These calls can be nested. In that case, the coherent set terminates only with the last call to <see cref="Publisher.EndCoherentChanges" />.</para>
        /// <para>The support for 'coherent changes' enables a publishing application to change the value of several data-instances that could
        /// belong to the same or different topics and have those changes be seen 'atomically' by the readers. This is useful in cases where
        /// the values are inter-related (for example, if there are two data-instances representing the 'altitude' and 'velocity vector' of the
        /// same aircraft and both are changed, it may be useful to communicate those values in a way the reader can see both together;
        /// otherwise, it may e.g., erroneously interpret that the aircraft is on a collision course).</para>
        /// </remarks>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode BeginCoherentChanges()
        {
            return UnsafeNativeMethods.BeginCoherentChanges(_native);
        }

        /// <summary>
        /// Terminates the 'coherent set' initiated by the matching call to <see cref="Publisher.BeginCoherentChanges" />. If there is no matching
        /// call to <see cref="Publisher.BeginCoherentChanges" />, the operation will return the error <see cref="ReturnCode.PreconditionNotMet" />.
        /// </summary>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode EndCoherentChanges()
        {
            return UnsafeNativeMethods.EndCoherentChanges(_native);
        }

        /// <summary>
        /// Blocks the calling thread until either all data written by the reliable <see cref="DataWriter" /> entities is acknowledged by all
        /// matched reliable <see cref="DataReader" /> entities, or else the duration specified by the maxWait parameter elapses, whichever happens
        /// first. A return value of <see cref="ReturnCode.Ok" /> indicates that all the samples written have been acknowledged by all reliable matched data readers;
        /// a return value of <see cref="ReturnCode.Timeout" /> indicates that maxWait elapsed before all the data was acknowledged.
        /// </summary>
        /// <param name="maxWait">The maximum <see cref="Duration" /> time to wait for the acknowledgments.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode WaitForAcknowledgments(Duration maxWait)
        {
            return UnsafeNativeMethods.WaitForAcknowledgments(_native, maxWait);
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
            var ret = UnsafeNativeMethods.GetDefaultDataWriterQos(_native, ref qosWrapper);

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
            var ret = UnsafeNativeMethods.SetDefaultDataWriterQos(_native, qosNative);
            qos.Release();

            return ret;
        }

        internal static IntPtr NarrowBase(IntPtr ptr)
        {
            return UnsafeNativeMethods.NativeNarrowBase(ptr);
        }

        internal new IntPtr ToNative()
        {
            return _native;
        }

        private DomainParticipant GetParticipant()
        {
            IntPtr ptrParticipant = UnsafeNativeMethods.GetParticipant(_native);

            DomainParticipant participant = null;

            if (!ptrParticipant.Equals(IntPtr.Zero))
            {
                IntPtr ptr = DomainParticipant.NarrowBase(ptrParticipant);

                Entity entity = EntityManager.Instance.Find(ptr);
                if (entity != null)
                {
                    participant = (DomainParticipant)entity;
                }
                else
                {
                    participant = new DomainParticipant(ptrParticipant);
                    EntityManager.Instance.Add(ptrParticipant, participant);
                }
            }

            return participant;
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
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NativeNarrowBase(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_CreateDataWriter", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateDataWriter(IntPtr pub, IntPtr topic, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_GetDefaultDataWriterQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultDataWriterQos(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In, Out] ref DataWriterQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_SetDefaultDataWriterQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultDataWriterQos(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In] DataWriterQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos(IntPtr pub, [MarshalAs(UnmanagedType.Struct), In] PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_DeleteDataWriter", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteDataWriter(IntPtr pub, IntPtr dataWriter);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_SetListener", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetListener(IntPtr pub, IntPtr listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_GetParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetParticipant(IntPtr pub);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_LookupDataWriter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr LookupDataWriter(IntPtr pub, string topicName);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_DeleteContainedEntities", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteContainedEntities(IntPtr pub);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_WaitForAcknowledgments", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode WaitForAcknowledgments(IntPtr pub, Duration maxWait);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_SuspendPublications", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SuspendPublications(IntPtr pub);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_ResumePublications", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode ResumePublications(IntPtr pub);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_BeginCoherentChanges", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode BeginCoherentChanges(IntPtr pub);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL, EntryPoint = "Publisher_EndCoherentChanges", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode EndCoherentChanges(IntPtr pub);
        }
        #endregion
    }
}
