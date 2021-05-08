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
using System.Collections.Generic;
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
    /// <para>A domain establishes a virtual network linking all applications that share the same Domain ID and isolating them from applications running on different domains.
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
        /// <summary>
        /// Gets the domain id used to create the <see cref="DomainParticipant" />.
        /// The domain id identifies the DDS domain to which the <see cref="DomainParticipant" /> belongs.
        /// </summary>
        public int DomainId => GetDomainId();

        /// <summary>
        /// Gets the attached <see cref="DomainParticipantListener"/>.
        /// </summary>
        [SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "Keep coherency with the setter method and DDS API.")]
        public DomainParticipantListener Listener { get; internal set; }
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
            return CreatePublisher(qos, null, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="Publisher" /> with the desired QoS policies and attaches to it the specified <see cref="PublisherListener" />.
        /// The specified <see cref="PublisherListener" /> will be attached with the default <see cref="StatusMask" />.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="Publisher" /> belongs to the <see cref="DomainParticipant" /> that is its factory.</para>
        /// <para>If the specified QoS policies are not consistent, the operation will fail and no <see cref="Publisher" /> will be created.</para>
        /// </remarks>
        /// <param name="qos">The <see cref="PublisherQos" /> policies to be used for creating the new <see cref="Publisher" />.</param>
        /// <param name="listener">The <see cref="PublisherListener" /> to be attached to the newly created <see cref="Publisher" />.</param>
        /// <returns>The newly created <see cref="Publisher" /> on success, otherwise <see langword="null"/>.</returns>
        public Publisher CreatePublisher(PublisherQos qos, PublisherListener listener)
        {
            return CreatePublisher(qos, listener, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="Publisher" /> with the desired QoS policies and attaches to it the specified <see cref="PublisherListener" />.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="Publisher" /> belongs to the <see cref="DomainParticipant" /> that is its factory.</para>
        /// <para>If the specified QoS policies are not consistent, the operation will fail and no <see cref="Publisher" /> will be created.</para>
        /// </remarks>
        /// <param name="qos">The <see cref="PublisherQos" /> policies to be used for creating the new <see cref="Publisher" />.</param>
        /// <param name="listener">The <see cref="PublisherListener" /> to be attached to the newly created <see cref="Publisher" />.</param>
        /// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
        /// <returns>The newly created <see cref="Publisher" /> on success, otherwise <see langword="null"/>.</returns>
        public Publisher CreatePublisher(PublisherQos qos, PublisherListener listener, StatusMask statusMask)
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

            IntPtr nativeListener = IntPtr.Zero;
            if (listener != null)
            {
                nativeListener = listener.ToNative();
            }

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreatePublisher86(_native, qosWrapper, nativeListener, statusMask),
                                                        () => UnsafeNativeMethods.CreatePublisher64(_native, qosWrapper, nativeListener, statusMask));

            qos.Release();

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            var p = new Publisher(native)
            {
                Listener = listener,
            };

            EntityManager.Instance.Add((p as Entity).ToNative(), p);
            ContainedEntities.Add(p);

            return p;
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
        /// Deletes an existing <see cref="Publisher" />.
        /// </summary>
        /// <remarks>
        /// <para>A <see cref="Publisher" /> cannot be deleted if it has any attached <see cref="DataWriter" /> objects. If <see cref="DeletePublisher" /> is called on a <see cref="Publisher" /> with
        /// existing <see cref="DataWriter" /> object, it will return <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// <para>The <see cref="DeletePublisher" /> operation must be called on the same <see cref="DomainParticipant" /> object used to create the <see cref="Publisher" />. If
        /// <see cref="DeletePublisher" /> is called on a different <see cref="DomainParticipant" />, the operation will have no effect and it will return
        /// <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// </remarks>
        /// <param name="publisher">The <see cref="Publisher" /> to be deleted.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeletePublisher(Publisher publisher)
        {
            if (publisher == null)
            {
                return ReturnCode.Ok;
            }

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeletePublisher86(_native, publisher.ToNative()),
                                                         () => UnsafeNativeMethods.DeletePublisher64(_native, publisher.ToNative()));
            if (ret == ReturnCode.Ok)
            {
                EntityManager.Instance.Remove((publisher as Entity).ToNative());
                ContainedEntities.Remove(publisher);
            }

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
            return CreateSubscriber(qos, null, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="Subscriber" /> with the desired QoS policies and attaches to it the specified <see cref="SubscriberListener" />.
        /// The specified <see cref="SubscriberListener" /> will be attached with the default <see cref="StatusMask" />.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="Subscriber" /> belongs to the <see cref="DomainParticipant" /> that is its factory.</para>
        /// <para>If the specified QoS policies are not consistent, the operation will fail and no <see cref="Subscriber" /> will be created.</para>
        /// </remarks>
        /// <param name="qos">The <see cref="SubscriberQos" /> policies to be used for creating the new <see cref="Subscriber" />.</param>
        /// <param name="listener">The <see cref="SubscriberListener" /> to be attached to the newly created <see cref="Subscriber" />.</param>
        /// <returns>The newly created <see cref="Subscriber" /> on success, otherwise <see langword="null"/>.</returns>
        public Subscriber CreateSubscriber(SubscriberQos qos, SubscriberListener listener)
        {
            return CreateSubscriber(qos, listener, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="Subscriber" /> with the desired QoS policies and attaches to it the specified <see cref="SubscriberListener" />.
        /// </summary>
        /// <remarks>
        /// <para>The created <see cref="Subscriber" /> belongs to the <see cref="DomainParticipant" /> that is its factory.</para>
        /// <para>If the specified QoS policies are not consistent, the operation will fail and no <see cref="Subscriber" /> will be created.</para>
        /// </remarks>
        /// <param name="qos">The <see cref="SubscriberQos" /> policies to be used for creating the new <see cref="Subscriber" />.</param>
        /// <param name="listener">The <see cref="SubscriberListener" /> to be attached to the newly created <see cref="Subscriber" />.</param>
        /// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
        /// <returns>The newly created <see cref="Subscriber" /> on success, otherwise <see langword="null"/>.</returns>
        public Subscriber CreateSubscriber(SubscriberQos qos, SubscriberListener listener, StatusMask statusMask)
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

            IntPtr nativeListener = IntPtr.Zero;
            if (listener != null)
            {
                nativeListener = listener.ToNative();
            }

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateSubscriber86(_native, qosWrapper, nativeListener, statusMask),
                                                        () => UnsafeNativeMethods.CreateSubscriber64(_native, qosWrapper, nativeListener, statusMask));

            qos.Release();

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            var s = new Subscriber(native)
            {
                Listener = listener,
            };

            EntityManager.Instance.Add((s as Entity).ToNative(), s);
            ContainedEntities.Add(s);

            return s;
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
        /// Allows access to the built-in <see cref="Subscriber" />.
        /// </summary>
        /// <remarks>
        /// <para>Each <see cref="DomainParticipant" /> contains several built-in <see cref="Topic" /> objects as well as corresponding <see cref="DataReader" /> objects to access them.
        /// All these <see cref="DataReader" /> objects belong to a single built-in <see cref="Subscriber" />.</para>
        /// <para>The built-in topics are used to communicate information about other <see cref="DomainParticipant" />, <see cref="Topic" />,
        /// <see cref="DataReader" />, and <see cref="DataWriter" /> objects.</para>
        /// </remarks>
        /// <returns>The built-in <see cref="Subscriber" />.</returns>
        [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Keep coherency with the setter method and DDS API.")]
        public Subscriber GetBuiltinSubscriber()
        {
            IntPtr s = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetBuiltinSubscriber86(_native),
                                                   () => UnsafeNativeMethods.GetBuiltinSubscriber64(_native));

            Subscriber managedSubscriber = null;

            if (!s.Equals(IntPtr.Zero))
            {
                IntPtr ptr = Subscriber.NarrowBase(s);

                if (!ptr.Equals(IntPtr.Zero))
                {
                    Entity entity = EntityManager.Instance.Find(ptr);
                    if (entity != null)
                    {
                        managedSubscriber = (Subscriber)entity;
                    }
                    else
                    {
                        managedSubscriber = new Subscriber(s);
                        EntityManager.Instance.Add((managedSubscriber as Entity).ToNative(), managedSubscriber);
                    }
                }
            }

            return managedSubscriber;
        }

        /// <summary>
        /// Deletes an existing <see cref="Subscriber" />.
        /// </summary>
        /// <param name="subscriber">The <see cref="Subscriber" /> to be deleted.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteSubscriber(Subscriber subscriber)
        {
            if (subscriber == null)
            {
                return ReturnCode.Ok;
            }

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeleteSubscriber86(_native, subscriber.ToNative()),
                                                         () => UnsafeNativeMethods.DeleteSubscriber64(_native, subscriber.ToNative()));
            if (ret == ReturnCode.Ok)
            {
                EntityManager.Instance.Remove((subscriber as Entity).ToNative());
                ContainedEntities.Remove(subscriber);
            }

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
            return CreateTopic(topicName, typeName, qos, null);
        }

        /// <summary>
        /// Creates a new <see cref="Topic" /> with the desired QoS policies and attaches to it the specified <see cref="TopicListener" />.
        /// The specified <see cref="TopicListener" /> will be attached with the default <see cref="StatusMask" />.
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
        /// <param name="listener">The <see cref="TopicListener" /> to be attached to the newly created <see cref="Topic" />.</param>
        /// <returns> The newly created <see cref="Topic" /> on success, otherwise <see langword="null"/>.</returns>
        public Topic CreateTopic(string topicName, string typeName, TopicQos qos, TopicListener listener)
        {
            return CreateTopic(topicName, typeName, qos, listener, StatusMask.DefaultStatusMask);
        }

        /// <summary>
        /// Creates a new <see cref="Topic" /> with the desired QoS policies and attaches to it the specified <see cref="TopicListener" />.
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
        /// <param name="listener">The <see cref="TopicListener" /> to be attached to the newly created <see cref="Topic" />.</param>
        /// <param name="statusMask">The <see cref="StatusMask" /> of which status changes the listener should be notified.</param>
        /// <returns> The newly created <see cref="Topic" /> on success, otherwise <see langword="null"/>.</returns>
        public Topic CreateTopic(string topicName, string typeName, TopicQos qos, TopicListener listener, StatusMask statusMask)
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

            IntPtr nativeListener = IntPtr.Zero;
            if (listener != null)
            {
                nativeListener = listener.ToNative();
            }

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateTopic86(_native, topicName, typeName, qosWrapper, nativeListener, statusMask),
                                                        () => UnsafeNativeMethods.CreateTopic64(_native, topicName, typeName, qosWrapper, nativeListener, statusMask));

            qos.Release();

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            var t = new Topic(native)
            {
                Listener = listener,
            };

            EntityManager.Instance.Add(t.ToNativeTopicDescription(), t);
            ContainedEntities.Add(t);

            return t;
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
        /// Looks for an existing (or ready to exist) enabled <see cref="Topic" />, based on its name.
        /// </summary>
        /// <remarks>
        /// <para>If a <see cref="Topic" /> of the same name already exists, it gives access to it, otherwise it waits (blocks the caller) until another mechanism
        /// creates it (or the specified timeout occurs). This other mechanism can be another thread, a configuration tool, or some other
        /// middleware service. Note that the <see cref="Topic" /> is a local object that acts as a ‘proxy’ to designate the global concept of topic.</para>
        /// <para>A <see cref="Topic" /> obtained by means of <see cref="FindTopic"/>, must also be deleted by means of <see cref="DeleteTopic"/> so that the local resources can be
        /// released. If a <see cref="Topic" /> is obtained multiple times by means of <see cref="FindTopic"/> or CreateTopic, it must also be deleted that same number
        /// of times using <see cref="DeleteTopic"/>.</para>
        /// </remarks>
        /// <param name="topicName">Name of the <see cref="Topic" /> to look for.</param>
        /// <param name="timeout">The time to wait if the <see cref="Topic" /> doesn't exist already.</param>
        /// <returns>The <see cref="Topic" />, if it exists, otherwise <see langword="null"/>.</returns>
        public Topic FindTopic(string topicName, Duration timeout)
        {
            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.FindTopic86(_native, topicName, timeout),
                                                        () => UnsafeNativeMethods.FindTopic64(_native, topicName, timeout));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            IntPtr ptrTopicDescription = Topic.NarrowTopicDescription(native);

            return (Topic)EntityManager.Instance.Find(ptrTopicDescription);
        }

        /// <summary>
        /// Looks up an existing <see cref="ITopicDescription" />, based on its name. The operation never blocks.
        /// </summary>
        /// <remarks>
        /// <para>The operation <see cref="LookupTopicDescription"/> may be used to locate any locally-created <see cref="Topic" />, <see cref="ContentFilteredTopic" />, and <see cref="MultiTopic" /> object.</para>
        /// <para>Unlike <see cref="FindTopic" />, the operation <see cref="LookupTopicDescription" /> searches only among the locally created topics. Therefore, it should
        /// never create a new <see cref="ITopicDescription" />. The <see cref="ITopicDescription" /> returned by <see cref="LookupTopicDescription" /> does not require any extra
        /// deletion.</para>
        /// </remarks>
        /// <param name="name">Name of the <see cref="ITopicDescription" /> to look for.</param>
        /// <returns>The <see cref="ITopicDescription" />, if it exists, otherwise <see langword="null"/>.</returns>
        public ITopicDescription LookupTopicDescription(string name)
        {
            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.LookupTopicDescription86(_native, name),
                                                        () => UnsafeNativeMethods.LookupTopicDescription64(_native, name));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return (ITopicDescription)EntityManager.Instance.Find(native);
        }

        /// <summary>
        /// Deletes an existing <see cref="Topic" />.
        /// </summary>
        /// <remarks>
        /// <para>The deletion of a <see cref="Topic" /> is not allowed if there are any existing <see cref="DataReader" />, <see cref="DataWriter" />, <see cref="ContentFilteredTopic" />, or <see cref="MultiTopic" />
        /// objects that are using the <see cref="Topic" />.If the DeleteTopic operation is called on a <see cref="Topic" /> with any of these existing objects attached to
        /// it, it will return <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// <para>The <see cref="DeleteTopic" /> operation must be called on the same <see cref="DomainParticipant" /> object used to create the <see cref="Topic" />. If <see cref="DeleteTopic" /> is
        /// called on a different <see cref="DomainParticipant" />, the operation will have no effect and it will return <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// </remarks>
        /// <param name="topic">The <see cref="Topic" /> to be deleted.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteTopic(Topic topic)
        {
            if (topic == null)
            {
                return ReturnCode.Ok;
            }

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeleteTopic86(_native, topic.ToNative()),
                                                         () => UnsafeNativeMethods.DeleteTopic64(_native, topic.ToNative()));
            if (ret == ReturnCode.Ok)
            {
                EntityManager.Instance.Remove((topic as Entity).ToNative());
                ContainedEntities.Remove(topic);
            }

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
        [Obsolete(nameof(GetListener) + " is deprecated, please use Listener property instead.")]
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
        /// Checks whether or not the given <paramref name="handle" /> represents an <see cref="Entity" /> that was created from the <see cref="DomainParticipant" />.
        /// </summary>
        /// <remarks>
        /// <para>The containment applies recursively. That is, it applies both to entities (<see cref="ITopicDescription" />, <see cref="Publisher" />, or <see cref="Subscriber" />) created
        /// directly using the <see cref="DomainParticipant" /> as well as entities created using a contained <see cref="Publisher" />, or <see cref="Subscriber" /> as the factory, and so forth.</para>
        /// <para>The instance handle for an <see cref="Entity" /> may be obtained from built-in topic data, from various statuses, or from the <see cref="Entity" /> property <see cref="Entity.InstanceHandle" />.</para>
        /// </remarks>
        /// <param name="handle">The <see cref="InstanceHandle" /> to be checked.</param>
        /// <returns><see langword="true"/> if the <see cref="Entity" /> is contained by the <see cref="DomainParticipant" />, otherwise <see langword="false"/>.</returns>
        public bool ContainsEntity(InstanceHandle handle)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.ContainsEntity86(_native, handle),
                                               () => UnsafeNativeMethods.ContainsEntity64(_native, handle));
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
            var ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.DeleteContainedEntities86(_native),
                                                  () => UnsafeNativeMethods.DeleteContainedEntities64(_native));
            if (ret == ReturnCode.Ok)
            {
                foreach (var e in ContainedEntities)
                {
                    EntityManager.Instance.Remove(e.ToNative());
                    e.ClearContainedEntities();
                }

                ContainedEntities.Clear();
            }

            return ret;
        }

        /// <summary>
        /// Instructs DDS to locally ignore a remote <see cref="DomainParticipant" />. From that point onwards the local <see cref="DomainParticipant" /> will behave as if the remote <see cref="DomainParticipant" /> did not exist.
        /// This means it will ignore any topic, publication, or subscription that originates on that <see cref="DomainParticipant" />.
        /// </summary>
        /// <remarks>
        /// <para>This operation can be used, in conjunction with the discovery of remote participants offered by means of the
        /// "DCPSParticipant" built-in Topic, to provide, for example, access control. Application data can be associated with a
        /// <see cref="DomainParticipant" /> by means of the <see cref="UserDataQosPolicy" />.This application data is propagated as a field in the built-in
        /// topic and can be used by an application to implement its own access control policy.</para>
        /// <para>The <see cref="DomainParticipant" /> to ignore is identified by the <paramref name="handle" /> argument. This handle is the one that appears in the <see cref="SampleInfo" />
        /// retrieved when reading the data-samples available for the built-in <see cref="DataReader" /> to the “DCPSParticipant” topic. The built-in
        /// <see cref="DataReader" /> is read with the same read/take operations used for any <see cref="DataReader" />.</para>
        /// <para>There is no way to reverse this operation.</para>
        /// </remarks>
        /// <param name="handle">The <see cref="InstanceHandle" /> of the <see cref="DomainParticipant" /> to be ignored.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode IgnoreParticipant(InstanceHandle handle)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.IgnoreParticipant86(_native, handle),
                                               () => UnsafeNativeMethods.IgnoreParticipant64(_native, handle));
        }

        /// <summary>
        /// Instructs DDS to locally ignore a <see cref="Topic" />. This means it will locally ignore any
        /// publication or subscription to the <see cref="Topic" />.
        /// </summary>
        /// <remarks>
        /// <para>This operation can be used to save local resources when the application knows that it will never publish or subscribe to data under certain topics.</para>
        /// <para>The <see cref="Topic" /> to ignore is identified by the <paramref name="handle" /> argument. This handle is the one that appears in the <see cref="SampleInfo" /> retrieved when
        /// reading the data-samples from the built-in <see cref="DataReader" /> on the "DCPSTopic" topic.</para>
        /// <para>There is no way to reverse this operation.</para>
        /// </remarks>
        /// <param name="handle">The <see cref="InstanceHandle" /> of the <see cref="Topic" /> to be ignored.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode IgnoreTopic(InstanceHandle handle)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.IgnoreTopic86(_native, handle),
                                               () => UnsafeNativeMethods.IgnoreTopic64(_native, handle));
        }

        /// <summary>
        /// Instructs DDS to locally ignore a remote publication. After this call, any data written related to that publication will be ignored.
        /// </summary>
        /// <remarks>
        /// <para>The <see cref="DataWriter" /> to ignore is identified by the <paramref name="handle" /> argument. This handle is the one that appears in the <see cref="SampleInfo" /> retrieved
        /// when reading the data-samples from the built-in <see cref="DataReader" /> on the "DCPSPublication" topic. To ignore a local <see cref="DataWriter" />, the handle can be obtained with
        /// the property <see cref="Entity.InstanceHandle" /> for the local <see cref="DataWriter" />.</para>
        /// <para>There is no way to reverse this operation.</para>
        /// </remarks>
        /// <param name="handle">The <see cref="InstanceHandle" /> of the <see cref="DataWriter" /> to be ignored.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode IgnorePublication(InstanceHandle handle)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.IgnorePublication86(_native, handle),
                                               () => UnsafeNativeMethods.IgnorePublication64(_native, handle));
        }

        /// <summary>
        /// Instructs DDS to locally ignore a remote subscription. After this call, any data received related to that subscription will be ignored.
        /// </summary>
        /// <remarks>
        /// <para>The <see cref="DataReader" /> to ignore is identified by the <paramref name="handle" /> argument. This handle is the one that appears in the <see cref="SampleInfo" />
        /// retrieved when reading the data-samples from the built-in <see cref="DataReader" /> on the "DCPSSubscription" topic. To ignore a local <see cref="DataReader" />, the handle can be obtained by
        /// getting the <see cref="Entity.InstanceHandle" /> for the local <see cref="DataReader" />.</para>
        /// <para>There is no way to reverse this operation.</para>
        /// </remarks>
        /// <param name="handle">The <see cref="InstanceHandle" /> of the <see cref="DataReader" /> to be ignored.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode IgnoreSubscription(InstanceHandle handle)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.IgnoreSubscription86(_native, handle),
                                               () => UnsafeNativeMethods.IgnoreSubscription64(_native, handle));
        }

        /// <summary>
        /// Manually asserts the liveliness of the <see cref="DomainParticipant" />. This is used in combination with the <see cref="LivelinessQosPolicy" /> to indicate to DDS that
        /// the entity remains active.
        /// </summary>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode AssertLiveliness()
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.AssertLiveliness86(_native),
                                               () => UnsafeNativeMethods.AssertLiveliness64(_native));
        }

        /// <summary>
        /// Gets the current value of the timestamp that DDS uses to time-stamp data-writes and to set the reception timestamp
        /// for the data-updates it receives.
        /// </summary>
        /// <param name="currentTime">The <see cref="Timestamp" /> structure to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetCurrentTimestamp(ref Timestamp currentTime)
        {
            Timestamp ts = default;
            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetCurrentTimestamp86(_native, ref ts),
                                                         () => UnsafeNativeMethods.GetCurrentTimestamp64(_native, ref ts));

            if (ret == ReturnCode.Ok)
            {
                currentTime = ts;
            }

            return ret;
        }

        /// <summary>
        /// Creates a <see cref="ContentFilteredTopic" />. The <see cref="ContentFilteredTopic" /> can be used to do content-based subscriptions.
        /// </summary>
        /// <remarks>
        /// The related <see cref="Topic" /> being subscribed to is specified by means of the <paramref name="relatedTopic" /> parameter. The <see cref="ContentFilteredTopic" /> only
        /// relates to samples published under that <see cref="Topic" />, filtered according to their content. The filtering is done by means of evaluating a
        /// logical expression that involves the values of some of the data-fields in the sample. The logical expression is derived from the
        /// <paramref name="filterExpression" /> and <paramref name="expressionParameters" /> arguments.
        /// </remarks>
        /// <param name="name">The name of the <see cref="ContentFilteredTopic" />.</param>
        /// <param name="relatedTopic">The related <see cref="Topic" />.</param>
        /// <param name="filterExpression">The filter expression to be used for the content filter.</param>
        /// <param name="expressionParameters">The collection of parameters to be used in the filter expression.</param>
        /// <returns>The newly created <see cref="ContentFilteredTopic" /> on success, otherwise <see langword="null"/>.</returns>
        public ContentFilteredTopic CreateContentFilteredTopic(string name, Topic relatedTopic, string filterExpression, params string[] expressionParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes an existing <see cref="ContentFilteredTopic" />.
        /// </summary>
        /// <remarks>
        /// <para>The deletion of a <see cref="ContentFilteredTopic" /> is not allowed if there are any existing <see cref="DataReader" /> objects that are using the
        /// <see cref="ContentFilteredTopic" />. If the <see cref="DeleteContentFilteredTopic" /> operation is called on a <see cref="ContentFilteredTopic" /> with existing
        /// <see cref="DataReader" /> objects attached to it, it will return <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// <para>The <see cref="DeleteContentFilteredTopic" /> operation must be called on the same <see cref="DomainParticipant" /> object used to create the
        /// <see cref="ContentFilteredTopic" />. If <see cref="DeleteContentFilteredTopic" /> is called on a different <see cref="DomainParticipant" />, the operation will have no
        /// effect and it will return <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// </remarks>
        /// <param name="contentFilteredTopic">The <see cref="ContentFilteredTopic" /> to be deleted.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteContentFilteredTopic(ContentFilteredTopic contentFilteredTopic)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a <see cref="MultiTopic" />. A <see cref="MultiTopic" /> can be used to subscribe to multiple topics and combine/filter the received data into a resulting type.
        /// In particular, <see cref="MultiTopic" /> provides a content-based subscription mechanism.
        /// </summary>
        /// <remarks>
        /// <para>The resulting type is specified by the <paramref name="typeName" /> argument. Prior to creating a <see cref="MultiTopic" /> the type must have been registered.
        /// This is done using the RegisterType operation on a derived class of the TypeSupport interface.</para>
        /// <para>The list of topics and the logic used to combine filter and re-arrange the information from each <see cref="Topic" /> are specified using the
        /// <paramref name="subscriptionExpression" /> and <paramref name="expressionParameters" /> arguments.</para>
        /// </remarks>
        /// <param name="name">The name of the <see cref="MultiTopic" />.</param>
        /// <param name="typeName">The type name used for the <see cref="MultiTopic" />.</param>
        /// <param name="subscriptionExpression">The subscription expression used for the <see cref="MultiTopic" />.</param>
        /// <param name="expressionParameters">The collection of parameters to be used in the subscription expression.</param>
        /// <returns>The newly created <see cref="MultiTopic" /> on success, otherwise <see langword="null"/>.</returns>
        public MultiTopic CreateMultiTopic(string name, string typeName, string subscriptionExpression, params string[] expressionParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes an existing <see cref="MultiTopic" />.
        /// </summary>
        /// <remarks>
        /// <para>The deletion of a <see cref="MultiTopic" /> is not allowed if there are any existing <see cref="DataReader" /> objects that are using the <see cref="MultiTopic" />. If the
        /// <see cref="DeleteMultiTopic" /> operation is called on a <see cref="MultiTopic" /> with existing <see cref="DataReader" /> objects attached to it, it will return
        /// <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// <para>The <see cref="DeleteMultiTopic" /> operation must be called on the same <see cref="DomainParticipant" /> object used to create the <see cref="MultiTopic" />. If
        /// <see cref="DeleteMultiTopic" /> is called on a different <see cref="DomainParticipant" />, the operation will have no effect and it will return
        /// <see cref="ReturnCode.PreconditionNotMet" /></para>.
        /// </remarks>
        /// <param name="multitopic">The <see cref="MultiTopic" /> to be deleted.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode DeleteMultiTopic(MultiTopic multitopic)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves the list of <see cref="DomainParticipant" />s that have been discovered in the domain and that the application has not
        /// indicated should be "ignored" by means of the <see cref="DomainParticipant" /> <see cref="IgnoreParticipant" /> operation.
        /// </summary>
        /// <remarks>
        /// The operation may fail if the infrastructure does not locally maintain the connectivity information. In this case the operation
        /// will return <see cref="ReturnCode.Unsupported" />.
        /// </remarks>
        /// <param name="participantHandles">The collection of <see cref="InstanceHandle" />s to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetDiscoveredParticipants(ICollection<InstanceHandle> participantHandles)
        {
            if (participantHandles == null)
            {
                return ReturnCode.BadParameter;
            }

            participantHandles.Clear();

            IntPtr seq = IntPtr.Zero;
            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDiscoveredParticipants86(_native, ref seq),
                                                         () => UnsafeNativeMethods.GetDiscoveredParticipants64(_native, ref seq));

            if (ret == ReturnCode.Ok && !seq.Equals(IntPtr.Zero))
            {
                MarshalHelper.PtrToSequence(seq, ref participantHandles);
                MarshalHelper.ReleaseNativePointer(seq);
            }

            return ret;
        }

        /// <summary>
        /// Retrieves information on a <see cref="DomainParticipant" /> that has been discovered on the network. The participant must
        /// be in the same domain as the participant on which this operation is invoked and must not have been "ignored" by means of the
        /// <see cref="DomainParticipant" /> <see cref="IgnoreParticipant" /> operation.
        /// </summary>
        /// <remarks>
        /// <para>The <paramref name="participantHandle"/> must correspond to such a <see cref="DomainParticipant" />. Otherwise, the operation will fail and return
        /// <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// <para>Use the operation GetDiscoveredParticipants to find the <see cref="Topic" /> that are currently discovered.</para>
        /// <para>The operation may also fail if the infrastructure does not hold the information necessary to fill in the <paramref name="participantData"/>.In this
        /// case the operation will return <see cref="ReturnCode.Unsupported" />.</para>
        /// </remarks>
        /// <param name="participantData">The <see cref="ParticipantBuiltinTopicData" /> to fill up.</param>
        /// <param name="participantHandle">The <see cref="InstanceHandle" />  of the <see cref="DomainParticipant" /> to get the data.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetDiscoveredParticipantData(ref ParticipantBuiltinTopicData participantData, InstanceHandle participantHandle)
        {
            ParticipantBuiltinTopicDataWrapper data = default;

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDiscoveredParticipantData86(_native, ref data, participantHandle),
                                                         () => UnsafeNativeMethods.GetDiscoveredParticipantData64(_native, ref data, participantHandle));

            if (ret == ReturnCode.Ok)
            {
                participantData.FromNative(data);
            }

            return ret;
        }

        /// <summary>
        /// Retrieves the list of <see cref="Topic" />s that have been discovered in the domain and that the application has not indicated
        /// should be "ignored" by means of the <see cref="DomainParticipant" /> <see cref="IgnoreTopic" /> operation.
        /// </summary>
        /// <param name="topicHandles">The collection of <see cref="InstanceHandle" />s to fill up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetDiscoveredTopics(ICollection<InstanceHandle> topicHandles)
        {
            if (topicHandles == null)
            {
                return ReturnCode.BadParameter;
            }

            topicHandles.Clear();

            IntPtr seq = IntPtr.Zero;
            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDiscoveredTopics86(_native, ref seq),
                                                         () => UnsafeNativeMethods.GetDiscoveredTopics64(_native, ref seq));

            if (ret == ReturnCode.Ok && !seq.Equals(IntPtr.Zero))
            {
                MarshalHelper.PtrToSequence(seq, ref topicHandles);
                MarshalHelper.ReleaseNativePointer(seq);
            }

            return ret;
        }

        /// <summary>
        /// Retrieves information on a <see cref="Topic" /> that has been discovered on the network. The topic must have been created by
        /// a participant in the same domain as the participant on which this operation is invoked and must not have been "ignored" by
        /// means of the <see cref="DomainParticipant" /> <see cref="IgnoreTopic" /> operation.
        /// </summary>
        /// <remarks>
        /// <para>The <paramref name="topicHandle"/> must correspond to such a topic. Otherwise, the operation will fail and return
        /// <see cref="ReturnCode.PreconditionNotMet" />.</para>
        /// <para>Use the operation GetDiscoveredTopics to find the topics that are currently discovered.</para>
        /// <para>The operation may also fail if the infrastructure does not hold the information necessary to fill in the <paramref name="topicData"/>. In this case
        /// the operation will return <see cref="ReturnCode.Unsupported" />.</para>
        /// <para>The operation may fail if the infrastructure does not locally maintain the connectivity information. In this case the operation
        /// will return <see cref="ReturnCode.Unsupported" />.</para>
        /// </remarks>
        /// <param name="topicData">The <see cref="TopicBuiltinTopicData" /> to fill up.</param>
        /// <param name="topicHandle">The <see cref="InstanceHandle" />  of the <see cref="Topic" /> to get the data.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetDiscoveredTopicData(ref TopicBuiltinTopicData topicData, InstanceHandle topicHandle)
        {
            TopicBuiltinTopicDataWrapper data = default;

            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDiscoveredTopicData86(_native, ref data, topicHandle),
                                                         () => UnsafeNativeMethods.GetDiscoveredTopicData64(_native, ref data, topicHandle));

            if (ret == ReturnCode.Ok)
            {
                topicData.FromNative(data);
            }

            return ret;
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

        internal static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr),
                                               () => UnsafeNativeMethods.NarrowBase64(ptr));
        }

        private int GetDomainId()
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetDomainId86(_native),
                                               () => UnsafeNativeMethods.GetDomainId64(_native));
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetDomainId", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetDomainId64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetDomainId", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetDomainId86(IntPtr ptr);

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
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_SetQos", CallingConvention = CallingConvention.Cdecl)]
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

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_ContainsEntity", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            [return: MarshalAs(UnmanagedType.U1)]
            public static extern bool ContainsEntity64(IntPtr dp, InstanceHandle handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_ContainsEntity", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            [return: MarshalAs(UnmanagedType.U1)]
            public static extern bool ContainsEntity86(IntPtr dp, InstanceHandle handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_FindTopic", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr FindTopic64(IntPtr dp, string topicName, Duration duration);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_FindTopic", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr FindTopic86(IntPtr dp, string topicName, Duration duration);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_LookupTopicDescription", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr LookupTopicDescription64(IntPtr dp, string name);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_LookupTopicDescription", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr LookupTopicDescription86(IntPtr dp, string name);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_DeleteTopic", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern ReturnCode DeleteTopic64(IntPtr dp, IntPtr topic);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_DeleteTopic", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern ReturnCode DeleteTopic86(IntPtr dp, IntPtr topic);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_DeletePublisher", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeletePublisher64(IntPtr dp, IntPtr pub);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_DeletePublisher", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeletePublisher86(IntPtr dp, IntPtr pub);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_DeleteSubscriber", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteSubscriber64(IntPtr dp, IntPtr pub);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_DeleteSubscriber", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode DeleteSubscriber86(IntPtr dp, IntPtr pub);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetBuiltinSubscriber", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetBuiltinSubscriber64(IntPtr dp);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetBuiltinSubscriber", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetBuiltinSubscriber86(IntPtr dp);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_IgnoreParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode IgnoreParticipant64(IntPtr dp, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_IgnoreParticipant", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode IgnoreParticipant86(IntPtr dp, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_IgnoreTopic", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode IgnoreTopic64(IntPtr dp, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_IgnoreTopic", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode IgnoreTopic86(IntPtr dp, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_IgnorePublication", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode IgnorePublication64(IntPtr dp, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_IgnorePublication", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode IgnorePublication86(IntPtr dp, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_IgnoreSubscription", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode IgnoreSubscription64(IntPtr dp, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_IgnoreSubscription", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode IgnoreSubscription86(IntPtr dp, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_AssertLiveliness", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode AssertLiveliness64(IntPtr dp);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_AssertLiveliness", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode AssertLiveliness86(IntPtr dp);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetCurrentTimestamp", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetCurrentTimestamp64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref Timestamp timestamp);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetCurrentTimestamp", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetCurrentTimestamp86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref Timestamp timestamp);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetDiscoveredParticipants", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDiscoveredParticipants64(IntPtr dp, ref IntPtr handles);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetDiscoveredParticipants", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDiscoveredParticipants86(IntPtr dp, ref IntPtr handles);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetDiscoveredParticipantData", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDiscoveredParticipantData64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref ParticipantBuiltinTopicDataWrapper data, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetDiscoveredParticipantData", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDiscoveredParticipantData86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref ParticipantBuiltinTopicDataWrapper data, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetDiscoveredTopics", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDiscoveredTopics64(IntPtr dp, ref IntPtr handles);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetDiscoveredTopics", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDiscoveredTopics86(IntPtr dp, ref IntPtr handles);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_GetDiscoveredTopicData", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDiscoveredTopicData64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicBuiltinTopicDataWrapper data, int handle);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetDiscoveredTopicData", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetDiscoveredTopicData86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In, Out] ref TopicBuiltinTopicDataWrapper data, int handle);
        }
        #endregion
    }
}
