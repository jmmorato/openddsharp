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
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System;
using System.Security;
using System.Runtime.InteropServices;

namespace OpenDDSharp.DDS
{
    public class DomainParticipant
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        internal DomainParticipant(IntPtr native)
        {
            _native = native;
        }
        #endregion

        #region Methods
        public Publisher CreatePublisher()
        {
            if (Environment.Is64BitProcess)
            {
                PublisherQosWrapper qos = new PublisherQosWrapper();
                IntPtr native = CreatePublisher64(_native, ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new Publisher(native);
            }
            else
            {
                PublisherQosWrapper qos = new PublisherQosWrapper();
                IntPtr native = CreatePublisher86(_native, ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new Publisher(native);
            }
        }

        public Subscriber CreateSubscriber()
        {
            if (Environment.Is64BitProcess)
            {
                SubscriberQosWrapper qos = new SubscriberQosWrapper();
                IntPtr native = CreateSubscriber64(_native, ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new Subscriber(native);
            }
            else
            {
                SubscriberQosWrapper qos = new SubscriberQosWrapper();
                IntPtr native = CreateSubscriber86(_native, ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new Subscriber(native);
            }
        }

        public Topic CreateTopic(string topicName, string typeName)
        {
            if (Environment.Is64BitProcess)
            {
                TopicQosWrapper qos = new TopicQosWrapper();
                IntPtr native = CreateTopic64(_native, topicName, typeName, ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new Topic(native);
            }
            else
            {
                TopicQosWrapper qos = new TopicQosWrapper();
                IntPtr native = CreateTopic86(_native, topicName, typeName, ref qos, IntPtr.Zero, 0u);
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new Topic(native);
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public IntPtr ToNative()
        {
            return _native;
        }
        #endregion

        #region PInvoke
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "DomainParticipant_CreatePublisher", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreatePublisher64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] ref PublisherQosWrapper qos, IntPtr a_listener, uint mask);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "DomainParticipant_CreatePublisher", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreatePublisher86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] ref PublisherQosWrapper qos, IntPtr a_listener, uint mask);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "DomainParticipant_CreateSubscriber", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateSubscriber64(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] ref SubscriberQosWrapper qos, IntPtr a_listener, uint mask);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "DomainParticipant_CreateSubscriber", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateSubscriber86(IntPtr dp, [MarshalAs(UnmanagedType.Struct), In] ref SubscriberQosWrapper qos, IntPtr a_listener, uint mask);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "DomainParticipant_CreateTopic", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr CreateTopic64(IntPtr dp, string topicName, string typeName, [MarshalAs(UnmanagedType.Struct), In] ref TopicQosWrapper qos, IntPtr a_listener, uint mask);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "DomainParticipant_CreateTopic", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr CreateTopic86(IntPtr dp, string topicName, string typeName, [MarshalAs(UnmanagedType.Struct), In] ref TopicQosWrapper qos, IntPtr a_listener, uint mask);
        #endregion
    }
}
