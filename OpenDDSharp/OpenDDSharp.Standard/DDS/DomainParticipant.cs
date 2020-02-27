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
using OpenDDSharp.Helpers;
using System;
using System.Runtime.InteropServices;
using System.Security;

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
            PublisherQosWrapper qos = default(PublisherQosWrapper);

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
            SubscriberQosWrapper qos = default(SubscriberQosWrapper);

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateSubscriber86(_native, ref qos, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreateSubscriber64(_native, ref qos, IntPtr.Zero, 0u));

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
            TopicQosWrapper qos = default(TopicQosWrapper);

            IntPtr native = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.CreateTopic86(_native, topicName, typeName, ref qos, IntPtr.Zero, 0u),
                                                        () => UnsafeNativeMethods.CreateTopic64(_native, topicName, typeName, ref qos, IntPtr.Zero, 0u));

            if (native.Equals(IntPtr.Zero))
            {
                return null;
            }

            return new Topic(native);
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
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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
            public static extern IntPtr CreateSubscriber64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] ref SubscriberQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_CreateSubscriber", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CreateSubscriber86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] ref SubscriberQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DomainParticipant_CreateTopic", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr CreateTopic64(IntPtr dp, string topicName, string typeName, [MarshalAs(UnmanagedType.Struct), In] ref TopicQosWrapper qos, IntPtr a_listener, uint mask);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DomainParticipant_CreateTopic", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern IntPtr CreateTopic86(IntPtr dp, string topicName, string typeName, [MarshalAs(UnmanagedType.Struct), In] ref TopicQosWrapper qos, IntPtr a_listener, uint mask);

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
