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
    /// Abstract class that can be implemented by an application-provided class and then registered with the <see cref="Topic" />
    /// such that the application can be notified of relevant status changes.
    /// </summary>
    public abstract class TopicListener
    {
        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnInconsistentTopicDelegate(IntPtr topic, ref InconsistentTopicStatus status);
        #endregion

        #region Fields
        private readonly IntPtr _native;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly OnInconsistentTopicDelegate _onInconsistentTopic;

        private GCHandle _gchInconsistentTopic;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicListener"/> class.
        /// </summary>
        protected TopicListener()
        {
            _onInconsistentTopic = new OnInconsistentTopicDelegate(OnInconsistentTopicHandler);
            _gchInconsistentTopic = GCHandle.Alloc(_onInconsistentTopic);

            _native = NewTopicListener(_onInconsistentTopic);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TopicListener"/> class.
        /// </summary>
        ~TopicListener()
        {
            if (_gchInconsistentTopic.IsAllocated)
            {
                _gchInconsistentTopic.Free();
            }

            MarshalHelper.ReleaseNativePointer(_native);
        }
        #endregion

        #region Methods
        /// <summary>
        /// <para>Handles the <see cref="StatusKind.InconsistentTopicStatus" /> communication status.</para>
        /// <para>The <see cref="StatusKind.InconsistentTopicStatus" /> indicates that a <see cref="Topic" /> was attempted to be registered that
        /// already exists with different characteristics. Typically, the existing <see cref="Topic" /> may have a different type associated with it.</para>
        /// </summary>
        /// <param name="topic">The <see cref="Topic" /> that triggered the event.</param>
        /// <param name="status">The current <see cref="InconsistentTopicStatus" />.</param>
        public abstract void OnInconsistentTopic(Topic topic, InconsistentTopicStatus status);

        private void OnInconsistentTopicHandler(IntPtr topic, ref InconsistentTopicStatus status)
        {
            Entity entity = EntityManager.Instance.Find(topic);

            Topic t = null;
            if (entity != null)
            {
                t = entity as Topic;
            }

            OnInconsistentTopic(t, status);
        }

        private IntPtr NewTopicListener(OnInconsistentTopicDelegate onInconsistentTopic)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NewTopicListener86(onInconsistentTopic),
                                               () => UnsafeNativeMethods.NewTopicListener64(onInconsistentTopic));
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "TopicListener_New", CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr NewTopicListener64([MarshalAs(UnmanagedType.FunctionPtr)] OnInconsistentTopicDelegate onInconsistentTopic);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "TopicListener_New", CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr NewTopicListener86([MarshalAs(UnmanagedType.FunctionPtr)] OnInconsistentTopicDelegate onInconsistentTopic);
        }
        #endregion
    }
}
