﻿/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace OpenDDSharp.Standard.UnitTest.Helpers
{
    internal class SupportProcessHelper
    {
        #region Constants
        private const string DDS_ROOT = @"..\..\..\..\..\..\ext\OpenDDS";
        private const string ACE_ROOT = @"..\..\..\..\..\..\ext\OpenDDS\ACE_TAO\ACE";
        private const string TAO_ROOT = @"..\..\..\..\..\..\ext\OpenDDS\ACE_TAO\TAO";

#if Windows
        private const string DEBUG_TARGET_FOLDER = @"Debug/";
        private const string RELEASE_TARGET_FOLDER = @"Release/";
        private const string SIXTY_FOUR_PLATFORM_FOLDER = @"x64/";
        private const string EIGHTY_SIX_PLATFORM_FOLDER = @"x86/";
        private const string TEST_SUPPORT_PROCESS_PATH = @"../../../../../TestSupportProcessCore/bin/";
        private const string TEST_SUPPORT_PROCESS_EXE_NAME = @"TestSupportProcessCore.exe";
        private const string DCPSINFOREPO_PROCESS_EXE_NAME = @"DCPSInfoRepo.exe";
#else
        private const string DEBUG_TARGET_FOLDER = @"LinuxDebug/";
        private const string RELEASE_TARGET_FOLDER = @"LinuxRelease/";
        private const string SIXTY_FOUR_PLATFORM_FOLDER = @"x64/";        
        private const string TEST_SUPPORT_PROCESS_PATH = @"./../../../../TestSupportProcessCore/bin/";
        private const string TEST_SUPPORT_PROCESS_EXE_NAME = @"TestSupportProcessCore.dll";
        private const string DCPSINFOREPO_PROCESS_EXE_NAME = @"DCPSInfoRepo";
#endif
        #endregion

        #region Fields
        private readonly TestContext _testContext;
        private string _platformFolder;
        private string _targetFolder;
        #endregion

        #region Constructors
        public SupportProcessHelper(TestContext testContext)
        {
            _testContext = testContext;
            _platformFolder = SIXTY_FOUR_PLATFORM_FOLDER;
            _targetFolder = RELEASE_TARGET_FOLDER;
            SetDebugTarget();
#if Windows
            SetEightySixPlatform();
#endif
            var ddsPath = Path.GetFullPath(DDS_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var acePath = Path.GetFullPath(ACE_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var taoPath = Path.GetFullPath(TAO_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var ddsBin = Path.Combine(ddsPath, $"bin");
            var ddsLib = Path.Combine(ddsPath, $"lib");
            var aceBin = Path.Combine(acePath, $"bin");
            var aceLib = Path.Combine(acePath, $"lib");
            var ddsBinPlatform = Path.Combine(ddsPath, $"bin_{_platformFolder.TrimEnd('/')}");
            var ddsLibPlatform = Path.Combine(ddsPath, $"lib_{_platformFolder.TrimEnd('/')}");
            var aceBinPlatform = Path.Combine(acePath, $"bin_{_platformFolder.TrimEnd('/')}");
            var aceLibPlatform = Path.Combine(acePath, $"lib_{_platformFolder.TrimEnd('/')}");
            string path = $"{ddsBinPlatform};{ddsLibPlatform};{aceBinPlatform};{aceLibPlatform};{ddsBin};{ddsLib};{aceBin};{aceLib};";
            Environment.SetEnvironmentVariable("Path", path + Environment.GetEnvironmentVariable("Path"));
            Environment.SetEnvironmentVariable("DDS_ROOT", ddsPath);
            Environment.SetEnvironmentVariable("ACE_ROOT", acePath);
            Environment.SetEnvironmentVariable("TAO_ROOT", taoPath);
        }
        #endregion

        #region Methods
        public Process SpawnSupportProcess(SupportTestKind teskKind)
        {
            string supportProcessPath = Path.Combine(TEST_SUPPORT_PROCESS_PATH, _platformFolder, _targetFolder, "netcoreapp3.1", TEST_SUPPORT_PROCESS_EXE_NAME);
            supportProcessPath = Path.GetFullPath(supportProcessPath);
            Console.WriteLine(supportProcessPath);
            if (!File.Exists(supportProcessPath))
            {
                _testContext.WriteLine($"The support process executable could not be located at {supportProcessPath}.");
                throw new FileNotFoundException($"The support process executable could not be located at {supportProcessPath}.");
            }
#if Linux
            var arguments = supportProcessPath + " " + teskKind.ToString();

            return SpawnProcess("dotnet", arguments);
#else
            return SpawnProcess(supportProcessPath, teskKind.ToString());
#endif
        }

        public Process SpawnDCPSInfoRepo()
        {
            string ddsPath = Environment.GetEnvironmentVariable("DDS_ROOT");
#if Windows
            string infoRepoPath = Path.Combine(ddsPath, "bin_" + _platformFolder, DCPSINFOREPO_PROCESS_EXE_NAME);
#else
            string infoRepoPath = Path.Combine(ddsPath, "bin", DCPSINFOREPO_PROCESS_EXE_NAME);
#endif
            if (!File.Exists(infoRepoPath))
            {
                _testContext.WriteLine($"The support process executable could not be located at {infoRepoPath}.");
                throw new FileNotFoundException($"The support process executable could not be located at {infoRepoPath}.");
            }

            return SpawnProcess(infoRepoPath, string.Empty);
        }

        private Process SpawnProcess(string path, string arguments)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(path)
            {
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
            };

            Process process = new Process
            {
                StartInfo = processInfo,
                EnableRaisingEvents = true,
            };

            process.OutputDataReceived += SupportProcessOnOutputDataReceived;
            process.ErrorDataReceived += SupportProcessOnErrorDataReceived;

            bool processStarted = false;
            try
            {
                processStarted = process.Start();
            }
            catch (Win32Exception e)
            {
                if (File.Exists(path))
                {
                    _testContext.WriteLine($"The support process executable at {path} could not be executed.");
                    throw new FileNotFoundException($"The support process executable at {path} could not be executed.", e);
                }
                _testContext.WriteLine($"The support process executable can not be located at {path}.");
                throw new InvalidOperationException($"The support process executable can not be located at {path}.", e);
            }

            if (!processStarted)
            {
                throw new InvalidOperationException("Support process could not be started.");
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process;
        }

        public void KillProcess(Process process)
        {
            process.OutputDataReceived -= SupportProcessOnOutputDataReceived;
            process.ErrorDataReceived -= SupportProcessOnErrorDataReceived;

            while (!process.HasExited)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit(5000);
                }
                catch (Exception ex)
                {
                    _testContext.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} Exception: {1}", nameof(KillProcess), ex.Message));
                }
            }

            process.Dispose();
        }

        private void SupportProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                _testContext.WriteLine(e.Data);
            }
        }

        private void SupportProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                _testContext.WriteLine(e.Data);
            }
        }

#if Windows
        [Conditional("X86")]
        private void SetEightySixPlatform()
        {
            _platformFolder = EIGHTY_SIX_PLATFORM_FOLDER;
        }
#endif

        [Conditional("DEBUG")]
        private void SetDebugTarget()
        {
            _targetFolder = DEBUG_TARGET_FOLDER;
        }
#endregion
    }
}
