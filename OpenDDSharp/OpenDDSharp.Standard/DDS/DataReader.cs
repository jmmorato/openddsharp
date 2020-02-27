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
using System.Runtime.InteropServices;
using System.Security;
using OpenDDSharp.Helpers;

namespace OpenDDSharp.DDS
{
    /// <summary>
    /// A DataReader allows the application to declare the data it wishes to receive (i.e., make a subscription) and to access the
    /// data received by the attached <see cref="Subscriber" />.
    /// </summary>
    /// <remarks>
    /// <para>A DataReader refers to exactly one <see cref="ITopicDescription" /> (either a <see cref="Topic" />, a <see cref="ContentFilteredTopic" />, or a <see cref="MultiTopic" />)
    /// that identifies the data to be read. The subscription has a unique resulting type. The data-reader may give access to several instances of the
    /// resulting type, which can be distinguished from each other by their key.</para>
    /// <para>All operations except for the operations <see cref="SetQos" />, <see cref="GetQos" />, SetListener,
    /// <see cref="GetListener" />, <see cref="Entity.Enable" />, and <see cref="Entity.StatusCondition" />
    /// return the value <see cref="ReturnCode.NotEnabled" /> if the DataReader has not been enabled yet.</para>
    /// </remarks>
    public class DataReader : Entity
    {
        #region Fields
        private readonly IntPtr _native;
        #endregion

        #region Constructors
        protected internal DataReader(IntPtr native) : base(NarrowBase(native))
        {
            _native = native;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the list of publications currently "associated" with the <see cref="DataReader" />; that is, publications that have a
        /// matching <see cref="Topic" /> and compatible QoS that the application has not indicated should be "ignored" by means of the
        /// <see cref="DomainParticipant" /> IgnorePublication operation.
        /// </summary>
        /// <remarks>
        /// The handles returned in the 'publicationHandles' collection are the ones that are used by the DDS implementation to locally identify
        /// the corresponding matched <see cref="DataWriter" /> entities. These handles match the ones that appear in the <see cref="SampleInfo.InstanceHandle" /> property of the
        /// <see cref="SampleInfo" /> when reading the "DCPSPublications" builtin topic.
        /// </remarks>
        /// <param name="publicationHandles">The collection of publication <see cref="InstanceHandle" />s to be filled up.</param>
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetMatchedPublications(ICollection<InstanceHandle> publicationHandles)
        {
            if (publicationHandles == null)
            {
                throw new ArgumentNullException(nameof(publicationHandles));
            }

            IntPtr ptr = IntPtr.Zero;
            ReturnCode ret = MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.GetMatchedPublications86(_native, ref ptr),
                                                         () => UnsafeNativeMethods.GetMatchedPublications64(_native, ref ptr));

            if (ret == ReturnCode.Ok && ptr != IntPtr.Zero)
            {
                ptr.PtrToSequence(ref publicationHandles);

                ptr.ReleaseNativePointer();
            }

            return ret;
        }

        private static IntPtr NarrowBase(IntPtr ptr)
        {
            return MarshalHelper.ExecuteAnyCpu(() => UnsafeNativeMethods.NarrowBase86(ptr), () => UnsafeNativeMethods.NarrowBase64(ptr));
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
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase64(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_NarrowBase", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NarrowBase86(IntPtr ptr);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X64, EntryPoint = "DataReader_GetMatchedPublications", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetMatchedPublications64(IntPtr dr, ref IntPtr publicationHandles);

            [SuppressUnmanagedCodeSecurity]
            [DllImport(MarshalHelper.API_DLL_X86, EntryPoint = "DataReader_GetMatchedPublications", CallingConvention = CallingConvention.Cdecl)]
            public static extern ReturnCode GetMatchedPublications86(IntPtr dr, ref IntPtr publicationHandles);
        }
        #endregion
    }
}
