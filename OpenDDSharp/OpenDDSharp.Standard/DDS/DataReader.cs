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
using OpenDDSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenDDSharp.DDS
{
    public class DataReader
    {
        #region Fields
        protected readonly IntPtr _native;
        #endregion

        #region Constructors
        protected internal DataReader(IntPtr native)
        {
            _native = native;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the list of publications currently "associated" with the <see cref="DataReader" />; that is, publications that have a
        /// matching <see cref="Topic" /> and compatible QoS that the application has not indicated should be "ignored" by means of the
        ///	<see cref="DomainParticipant" /> IgnorePublication operation.
        /// </summary>
        /// <remarks>
        /// The handles returned in the 'publicationHandles' collection are the ones that are used by the DDS implementation to locally identify
        /// the corresponding matched <see cref="DataWriter" /> entities. These handles match the ones that appear in the <see cref="SampleInfo.InstanceHandle" /> property of the
        ///	<see cref="SampleInfo" /> when reading the "DCPSPublications" builtin topic.
        /// </remarks>
        /// <param name="publicationHandles">The collection of publication <see cref="InstanceHandle" />s to be filled up.</param> 
        /// <returns>The <see cref="ReturnCode" /> that indicates the operation result.</returns>
        public ReturnCode GetMatchedPublications(ICollection<InstanceHandle> publicationHandles)
        {
            if (publicationHandles == null)
            {
                throw new ArgumentNullException(nameof(publicationHandles));
            }

            ReturnCode ret = ReturnCode.Error;

            IntPtr ptr = IntPtr.Zero;
            if (Environment.Is64BitProcess)
            {
                ret = GetMatchedPublications64(_native, ref ptr);
            }
            else
            {
                ret = GetMatchedPublications86(_native, ref ptr);
            }

            if (ret == ReturnCode.Ok && ptr != IntPtr.Zero)
            {                
                ptr.PtrToSequence(ref publicationHandles);

                if (Environment.Is64BitProcess)
                {
                    ptr.ReleaseNativePointer64();
                }
                else
                {
                    ptr.ReleaseNativePointer86();
                }
            }

            return ret;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public IntPtr ToNative()
        {
            return _native;
        }
        #endregion

        #region PInvoke
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "DataReader_GetMatchedPublications", CallingConvention = CallingConvention.Cdecl)]
        private static extern ReturnCode GetMatchedPublications64(IntPtr dr, ref IntPtr publicationHandles);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "DataReader_GetMatchedPublications", CallingConvention = CallingConvention.Cdecl)]
        private static extern ReturnCode GetMatchedPublications86(IntPtr dr, ref IntPtr publicationHandles);
        #endregion
    }
}
