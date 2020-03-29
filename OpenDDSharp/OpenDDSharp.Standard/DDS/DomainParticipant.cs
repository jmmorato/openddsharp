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
    /// <para>The DomainParticipant represents the participation of the application on a communication plane that isolates applications
    /// running on the same set of physical computers from each other.</para>
    /// <para>A domain establishes a virtual network linking all applications that share the same <see cref="DomainId" /> and isolating them from applications running on different domains.
    /// In this way, several independent distributed applications can coexist in the same physical network without interfering, or even being aware of each other.</para>
    /// </summary>
    /// <remarks>
    /// The DomainParticipant also acts as a container for all other <see cref="Entity" /> objects and as factory for the <see cref="Publisher" />,
    /// <see cref="Subscriber" />, <see cref="Topic" />, and <see cref="MultiTopic" /> <see cref="Entity" /> objects. In addition, the Domain Participant
    /// provides administration services in the domain, offering operations that allow the application to ‘ignore’ locally any
    /// information about a given participant, publication, subscription, or topic.
    /// </remarks>
    public class DomainParticipant : Entity
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Properties
        internal DomainParticipantListener Listener { get; set; }
        #endregion

        #region Constructors
        internal DomainParticipant(IntPtr native) : base(NarrowBase(native))
        {
            _native = native;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="Publisher" /> with the default QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="Publisher" /> belongs to the <see cref="DomainParticipant" /> that is its factory.</para>
        /// </remarks>
        /// <returns>The newly created <see cref="Publisher" /> on success, otherwise <see langword="null"/>.</returns>
        public Publisher CreatePublisher()
        {
            return CreatePublisher(null);
        }

        /// <summary>
        /// Creates a new <see cref="Publisher" /> with the desired QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="Publisher" /> belongs to the <see cref="DomainParticipant" /> that is its factory.</para>
        /// <para>If the specified QoS policies are not consistent, the operation will fail and no <see cref="Publisher" /> will be created.</para>
        /// </remarks>
        /// <param name="qos">The <see cref="PublisherQos" /> policies to be used for creating the new <see cref="Publisher" />.</param>
        /// <returns> The newly created <see cref="Publisher" /> on success, otherwise <see langword="null"/>.</returns>
        public Publisher CreatePublisher(PublisherQos qos)
        {
            PublisherQosWrapper qosWrapper = default;
            if (qos is null)
            {
                qos = new PublisherQos();
                var ret = GetDefaultPublisherQos(qos);
                if (ret == ReturnCode.Ok)
                {
                    qosWrapper = qos.ToNative();
                }
            }
            else
            {
                qosWrapper = qos.ToNative();
            }

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreatePublisher86(_native, qosWrapper, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreatePublisher64(_native, qosWrapper, IntPtr.Zero, 0u));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new Publisher(native);
        }

        /// <summary>
        /// Gets the default value of the <see cref="Publisher" /> QoS, that is, the QoS policies which will be used for newly created
        /// <see cref="Publisher" /> entities in the case where the QoS policies are defaulted in the CreatePublisher operation.			
        /// </summary>
        /// <remarks>
        /// The values retrieved by the <see cref="GetDefaultPublisherQos" /> call will match the set of values specified on the last successful call to
        /// <see cref="SetDefaultPublisherQos" />, or else, if the call was never made, the default values defined by the DDS standard.
        /// </remarks>
        /// <param name="qos">The <see cref="PublisherQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetDefaultPublisherQos(PublisherQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            PublisherQosWrapper qosWrapper = default;
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDefaultPublisherQos86(_native, ref qosWrapper),
                                                  () => UnsafeNativeMethods.GetDefaultPublisherQos64(_native, ref qosWrapper));

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
        public ReturnCode SetDefaultPublisherQos(PublisherQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            var qosNative = qos.ToNative();
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetDefaultPublisherQos86(_native, qosNative),
                                                  () => UnsafeNativeMethods.SetDefaultPublisherQos64(_native, qosNative));
            qos.Release();

            return ret;
        }

        /// <summary>
        /// Creates a new <see cref="Subscriber" /> with the default QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="Subscriber" /> belongs to the <see cref="DomainParticipant" /> that is its factory.</para>
        /// </remarks>
        /// <returns>The newly created <see cref="Subscriber" /> on success, otherwise <see langword="null"/>.</returns>
        public Subscriber CreateSubscriber()
        {
            return CreateSubscriber(null);
        }

        /// <summary>
        /// Creates a new <see cref="Subscriber" /> with the desired QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="Subscriber" /> belongs to the <see cref="DomainParticipant" /> that is its factory.</para>
        /// <para>If the specified QoS policies are not consistent, the operation will fail and no <see cref="Subscriber" /> will be created.</para>
        /// </remarks>
        /// <param name="qos">The <see cref="SubscriberQos" /> policies to be used for creating the new <see cref="Subscriber" />.</param>
        /// <returns>The newly created <see cref="Subscriber" /> on success, otherwise <see langword="null"/>.</returns>
        public Subscriber CreateSubscriber(SubscriberQos qos)
        {
            SubscriberQosWrapper qosWrapper = default;
            if (qos is null)
            {
                qos = new SubscriberQos();
                var ret = GetDefaultSubscriberQos(qos);
                if (ret == ReturnCode.Ok)
                {
                    qosWrapper = qos.ToNative();
                }
            }
            else
            {
                qosWrapper = qos.ToNative();
            }

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateSubscriber86(_native, qosWrapper, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreateSubscriber64(_native, qosWrapper, IntPtr.Zero, 0u));

            qos.Release();

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new Subscriber(native);
        }

        /// <summary>
        /// Gets the default value of the <see cref="Subscriber" /> QoS, that is, the QoS policies which will be used for newly created
        /// <see cref="Subscriber" /> entities in the case where the QoS policies are defaulted in the CreateSubscriber operation.
        /// </summary>
        /// <remarks>
        /// The values retrieved by the GetDefaultSubscriberQos call will match the set of values specified on the last successful call to
        /// <see cref="SetDefaultSubscriberQos" />, or else, if the call was never made, the default values defined by the DDS standard.
        /// </remarks>
        /// <param name="qos">The <see cref="SubscriberQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetDefaultSubscriberQos(SubscriberQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            SubscriberQosWrapper qosWrapper = default;
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDefaultSubscriberQos86(_native, ref qosWrapper),
                                                  () => UnsafeNativeMethods.GetDefaultSubscriberQos64(_native, ref qosWrapper));

            if (ret == ReturnCode.Ok)
            {
                qos.FromNative(qosWrapper);
            }

            qos.Release();

            return ret;
        }

        /// <summary>
        /// Sets a default value of the <see cref="Subscriber" /> QoS policies which will be used for newly created <see cref="Subscriber" /> entities in the
        /// case where the QoS policies are defaulted in the CreateSubscriber operation.
        /// </summary>
        /// <remarks>
        /// This operation will check that the resulting policies are self consistent; if they are not, the operation will have no effect and
        /// return <see cref="ReturnCode.InconsistentPolicy" />.
        /// </remarks>
        /// <param name="qos">The default <see cref="SubscriberQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetDefaultSubscriberQos(SubscriberQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            var qosNative = qos.ToNative();
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetDefaultSubscriberQos86(_native, qosNative),
                                                  () => UnsafeNativeMethods.SetDefaultSubscriberQos64(_native, qosNative));
            qos.Release();

            return ret;
        }

        /// <summary>
        /// Creates a new <see cref="Topic" /> with the default QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="Topic" /> belongs to the <see cref="DomainParticipant" /> that is its factory.</para>
        /// <para>The <see cref="Topic" /> is bound to a type described by the <paramref name="typeName"/> argument. Prior to creating a <see cref="Topic" /> the type must have been
        /// registered. This is done using the RegisterType operation on a derived class of the TypeSupport interface.</para>
        /// </remarks>
        /// <param name="topicName">The name for the new topic.</param>
        /// <param name="typeName">The name of the type which the new <see cref="Topic" /> will be bound.</param>
        /// <returns> The newly created <see cref="Topic" /> on success, otherwise <see langword="null"/>.</returns>
        public Topic CreateTopic(string topicName, string typeName)
        {
            return CreateTopic(topicName, typeName, null);
        }

        /// <summary>
        /// Creates a <see cref="Topic" /> with the desired QoS policies and without listener attached.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="Topic" /> belongs to the <see cref="DomainParticipant" /> that is its factory.</para>
        /// <para>The <see cref="Topic" /> is bound to a type described by the <paramref name="typeName"/> argument. Prior to creating a <see cref="Topic" /> the type must have been
        /// registered. This is done using the RegisterType operation on a derived class of the TypeSupport interface.</para>
        /// <para>If the specified QoS policies are not consistent, the operation will fail and no <see cref="Topic" /> will be created.</para>
        /// </remarks>
        /// <param name="topicName">The name for the new topic.</param>
        /// <param name="typeName">The name of the type which the new <see cref="Topic" /> will be bound.</param>
        /// <param name="qos">The <see cref="TopicQos" /> policies to be used for creating the new <see cref="Topic" />.</param>
        /// <returns> The newly created <see cref="Topic" /> on success, otherwise <see langword="null"/>.</returns>
        public Topic CreateTopic(string topicName, string typeName, TopicQos qos)
        {
            if (string.IsNullOrWhiteSpace(topicName))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(typeName))
            {
                return null;
            }

            TopicQosWrapper qosWrapper = default;
            if (qos is null)
            {
                qos = new TopicQos();
                var ret = GetDefaultTopicQos(qos);
                if (ret == ReturnCode.Ok)
                {
                    qosWrapper = qos.ToNative();
                }
            }
            else
            {
                qosWrapper = qos.ToNative();
            }

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateTopic86(_native, topicName, typeName, qosWrapper, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreateTopic64(_native, topicName, typeName, qosWrapper, IntPtr.Zero, 0u));

            qos.Release();

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new Topic(native);
        }

        /// <summary>
        /// Gets the default value of the <see cref="Topic" /> QoS, that is, the QoS policies that will be used for newly created <see cref="Topic" />
        /// entities in the case where the QoS policies are defaulted in the CreateTopic operation.
        /// </summary>
        /// <remarks>
        /// The values retrieved <see cref="GetDefaultTopicQos" /> will match the set of values specified on the last successful call to
        /// <see cref="SetDefaultTopicQos" />, or else, if the call was never made, the default values defined by the DDS standard.
        /// </remarks>
        /// <param name="qos">The <see cref="TopicQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetDefaultTopicQos(TopicQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            TopicQosWrapper qosWrapper = default;
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDefaultTopicQos86(_native, ref qosWrapper),
                                                  () => UnsafeNativeMethods.GetDefaultTopicQos64(_native, ref qosWrapper));

            if (ret == ReturnCode.Ok)
            {
                qos.FromNative(qosWrapper);
            }

            qos.Release();

            return ret;
        }

        /// <summary>
        /// Sets a default value of the <see cref="Topic" /> QoS policies which will be used for newly created <see cref="Topic" /> entities in the
        /// case where the QoS policies are defaulted in the CreateTopic operation.
        /// </summary>
        /// <remarks>
        /// This operation will check that the resulting policies are self consistent; if they are not, the operation will have no effect and
        /// return <see cref="ReturnCode.InconsistentPolicy" />.
        /// </remarks>
        /// <param name="qos">The default <see cref="TopicQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetDefaultTopicQos(TopicQos qos)
        {
            if (qos is null)
            {
                return ReturnCode.BadParameter;
            }

            var qosNative = qos.ToNative();
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.SetDefaultTopicQos86(_native, qosNative),
                                                  () => UnsafeNativeMethods.SetDefaultTopicQos64(_native, qosNative));
            qos.Release();

            return ret;
        }

        /// <summary>
        /// Gets the <see cref="DomainParticipant" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="DomainParticipantQos" /> to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetQos(DomainParticipantQos qos)
        {
            if (qos == null)
            {
                return ReturnCode.BadParameter;
            }

            DomainParticipantQosWrapper qosWrapper = default;
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
        /// Sets the <see cref="DomainParticipant" /> QoS policies.
        /// </summary>
        /// <param name="qos">The <see cref="DomainParticipantQos" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetQos(DomainParticipantQos qos)
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
        /// Allows access to the attached <see cref="DomainParticipantListener" />.
        /// </summary>
        /// <returns>The attached <see cref="DomainParticipantListener" />.</returns>
        [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Keep coherency with the setter method and DDS API.")]
        public DomainParticipantListener GetListener()
        {
            return Listener;
        }

        /// <summary>
        /// Sets the <see cref="DomainParticipantListener" /> using the <see cref="StatusMask.DefaultStatusMask" />.
        /// </summary>
        /// <param name="listener">The <see cref="DomainParticipantListener" /> to be set.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetListener(DomainParticipantListener listener)
        {
            return SetListener(listener, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Sets the <see cref="DomainParticipantListener" />.
        /// </summary>
        /// <param name="listener">The <see cref="DomainParticipantListener" /> to be set.</param>
        /// <param name="mask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode SetListener(DomainParticipantListener listener, StatusMask mask)
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

        /// <summary>
        /// Deletes all the entities that were created by means of the “create” operations on the <see cref="DomainParticipant" />. That is,
        /// it deletes all contained <see cref="Publisher" />, <see cref="Subscriber" />, <see cref="Topic" />, <see cref="ContentFilteredTopic" />, and <see cref="MultiTopic" />.
        /// This method is applied recursively to the deleted entities.
        /// </summary>
        /// <remarks>
        /// <para>Prior to deleting each contained entity, this operation will recursively call the corresponding DeleteContainedEntities
        /// operation on each contained entity (if applicable).This pattern is applied recursively. In this manner the operation
        /// DeleteContainedEntities on the <see cref="DomainParticipant" /> will end up deleting all the entities recursively contained in the
        /// <see cref="DomainParticipant" />, that is also the <see cref="DataWriter" />, <see cref="DataReader" />, as well as the <see cref="QueryCondition" />
        /// and <see cref="ReadCondition" /> objects belonging to the contained DataReaders.</para>
        /// </remarks>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteContainedEntities()
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeleteContainedEntities86(_native),
                                               () => UnsafeNativeMethods.DeleteContainedEntities64(_native));
        }

        /// <summary>
        /// Internal use only.
        /// </summary>
        /// <returns>The native pointer.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new IntPtr ToNative()
        {
            return _native;
        }

        private static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
                                               () => UnsafeNativeMethods.NarrowBase64(ptr));
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_CreatePublisher", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreatePublisher64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] PublisherQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_CreatePublisher", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreatePublisher86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] PublisherQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_CreateSubscriber", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateSubscriber64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] SubscriberQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_CreateSubscriber", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateSubscriber86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] SubscriberQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_CreateTopic", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr CreateTopic64(IntPtr dp, string topicName, string typeName, [MarshalAs(UnmanagedType.Struct), In] TopicQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_CreateTopic", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr CreateTopic86(IntPtr dp, string topicName, string typeName, [MarshalAs(UnmanagedType.Struct), In] TopicQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref DomainParticipantQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref DomainParticipantQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] DomainParticipantQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_SetDefaultPublisherQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] DomainParticipantQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetDefaultPublisherQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultPublisherQos64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetDefaultPublisherQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultPublisherQos86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_SetDefaultPublisherQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultPublisherQos64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_SetDefaultPublisherQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultPublisherQos86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] PublisherQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetDefaultSubscriberQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultSubscriberQos64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetDefaultSubscriberQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultSubscriberQos86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_SetDefaultSubscriberQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultSubscriberQos64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_SetDefaultSubscriberQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultSubscriberQos86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] SubscriberQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetDefaultTopicQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultTopicQos64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetDefaultTopicQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDefaultTopicQos86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_SetDefaultTopicQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultTopicQos64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] TopicQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_SetDefaultTopicQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetDefaultTopicQos86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] TopicQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_SetListener", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetListener64(IntPtr dp, IntPtr listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_SetListener", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetListener86(IntPtr dp, IntPtr listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_DeleteContainedEntities", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern ReturnCode DeleteContainedEntities64(IntPtr dp);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_DeleteContainedEntities", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern ReturnCode DeleteContainedEntities86(IntPtr dp);
        }
        #endregion
    }
}
