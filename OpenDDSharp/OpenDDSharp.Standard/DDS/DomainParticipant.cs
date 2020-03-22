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
using System.Globalization;
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
            PublisherQosWrapper qos = default;

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreatePublisher86(_native, ref qos, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreatePublisher64(_native, ref qos, IntPtr.Zero, 0u));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new Publisher(native);
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
            return CreateSubscriber(new SubscriberQos());
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
            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

            SubscriberQosWrapper qosWrapper = qos.ToNative();
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
            return CreateTopic(topicName, typeName, new TopicQos());
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
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The argument {0} cannot be null or empty.", nameof(topicName)), nameof(topicName));
            }

            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The argument {0} cannot be null or empty.", nameof(typeName)), nameof(typeName));
            }

            if (qos is null)
            {
                throw new ArgumentNullException(nameof(qos));
            }

            TopicQosWrapper qosWrapper = qos.ToNative();

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
            public static extern IntPtr CreatePublisher64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] ref PublisherQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_CreatePublisher", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreatePublisher86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] ref PublisherQosWrapper qos, IntPtr a_listener, uint mask);

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
            public static extern ReturnCode GetQos64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref DomainParticipantQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_GetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetQos86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In, Out] ref DomainParticipantQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos64(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] DomainParticipantQosWrapper qos);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_SetQos", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode SetQos86(IntPtr dr, [MarshalAs(UnmanagedType.Struct), In] DomainParticipantQosWrapper qos);

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
