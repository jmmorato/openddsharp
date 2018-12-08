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
using OpenDDSharp.DDS;

namespace OpenDDSharp.OpenDDS.DCPS
{
    public class ParticipantService
    {
        #region Singleton
        private static ParticipantService _instance;
        public static ParticipantService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ParticipantService();
                }

                return _instance;
            }
        }
        #endregion

        #region Constructors
        private ParticipantService()
        {
            if (Environment.Is64BitProcess)
            {
                ParticipantServiceNew64();
            }
            else
            {
                ParticipantServiceNew86();
            }
        }

        #endregion

        #region Methods
        public DomainParticipantFactory GetDomainParticipantFactory()
        {
            if (Environment.Is64BitProcess)
            {
                IntPtr native = GetDomainParticipantFactory64();
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new DomainParticipantFactory(native);
            }
            else
            {
                IntPtr native = GetDomainParticipantFactory86();
                if (native.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new DomainParticipantFactory(native);
            }
        }

        public DomainParticipantFactory GetDomainParticipantFactory(params string[] args)
        {
            int argc = args.Length + 1;
            string[] argv = new string[argc];

            // Don't need the program name (can't be NULL though, else ACE_Arg_Shifter fails)
            argv[0] = "";
            for (int i = 0; i < args.Length; i++)
            {
                argv[i + 1] = args[i];
            }

            if (Environment.Is64BitProcess)
            {
                return new DomainParticipantFactory(GetDomainParticipantFactory64(argc, argv));
            }
            else
            {
                return new DomainParticipantFactory(GetDomainParticipantFactory86(argc, argv));
            }
        }
        #endregion

        #region PInvoke
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "ParticipantService_new", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ParticipantServiceNew64();

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "ParticipantService_new", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ParticipantServiceNew86();

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "ParticipantService_GetDomainParticipantFactory", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetDomainParticipantFactory64();

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X64, EntryPoint = "ParticipantService_GetDomainParticipantFactoryParameters", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr GetDomainParticipantFactory64(int argc, string[] argv);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "ParticipantService_GetDomainParticipantFactory")]
        private static extern IntPtr GetDomainParticipantFactory86();

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.API_DLL_X86, EntryPoint = "ParticipantService_GetDomainParticipantFactoryParameters", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr GetDomainParticipantFactory86(int argc, string[] argv);
        #endregion
    }
}
