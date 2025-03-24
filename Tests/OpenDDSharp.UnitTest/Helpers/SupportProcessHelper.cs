/*********************************************************************
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
using System.Runtime.InteropServices;
using JsonWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest.Helpers
{
    internal class SupportProcessHelper
    {
        #region Constants
#if Windows
        private const string DDS_ROOT = @"../../../../../../ext/OpenDDS";
        private const string ACE_ROOT = @"../../../../../../ext/OpenDDS/ACE_wrappers";
        private const string TAO_ROOT = @"../../../../../../ext/OpenDDS/ACE_wrappers/TAO";
        private const string DEBUG_TARGET_FOLDER = @"Debug";
        private const string RELEASE_TARGET_FOLDER = @"Release";
        private const string SIXTY_FOUR_PLATFORM_FOLDER = @"x64";
        private const string EIGHTY_SIX_PLATFORM_FOLDER = @"x86";
        private const string TEST_SUPPORT_PROCESS_PATH = @"../../../../../TestSupportProcessCore/bin/";
        private const string TEST_SUPPORT_PROCESS_EXE_NAME = @"TestSupportProcessCore.exe";
        private const string DCPSINFOREPO_PROCESS_EXE_NAME = @"DCPSInfoRepo.exe";
#elif Linux && X64
        private const string DDS_ROOT = @"../../../../../../ext/OpenDDS_linux-x64";
        private const string ACE_ROOT = @"../../../../../../ext/OpenDDS_linux-x64/ACE_wrappers";
        private const string TAO_ROOT = @"../../../../../../ext/OpenDDS_linux-x64/ACE_wrappers/TAO";
        private const string DEBUG_TARGET_FOLDER = @"Debug/";
        private const string RELEASE_TARGET_FOLDER = @"Release/";
        private const string SIXTY_FOUR_PLATFORM_FOLDER = @"x64/";
        private const string TEST_SUPPORT_PROCESS_PATH = @"../../../../../TestSupportProcessCore/bin/";
        private const string TEST_SUPPORT_PROCESS_EXE_NAME = @"TestSupportProcessCore.dll";
        private const string DCPSINFOREPO_PROCESS_EXE_NAME = @"DCPSInfoRepo";
#elif Linux && ARM64
        private const string DDS_ROOT = @"../../../../../../ext/OpenDDS_linux-arm64";
        private const string ACE_ROOT = @"../../../../../../ext/OpenDDS_linux-arm64/ACE_wrappers";
        private const string TAO_ROOT = @"../../../../../../ext/OpenDDS_linux-arm64/ACE_wrappers/TAO";
        private const string DEBUG_TARGET_FOLDER = @"Debug/";
        private const string RELEASE_TARGET_FOLDER = @"Release/";
        private const string SIXTY_FOUR_PLATFORM_FOLDER = @"ARM64/";
        private const string TEST_SUPPORT_PROCESS_PATH = @"../../../../../TestSupportProcessCore/bin/";
        private const string TEST_SUPPORT_PROCESS_EXE_NAME = @"TestSupportProcessCore.dll";
        private const string DCPSINFOREPO_PROCESS_EXE_NAME = @"DCPSInfoRepo";
#elif OSX && X64
        private const string DDS_ROOT = @"../../../../../../ext/OpenDDS_osx-x64";
        private const string ACE_ROOT = @"../../../../../../ext/OpenDDS_osx-x64/ACE_wrappers";
        private const string TAO_ROOT = @"../../../../../../ext/OpenDDS_osx-x64/ACE_wrappers/TAO";
        private const string DEBUG_TARGET_FOLDER = @"Debug/";
        private const string RELEASE_TARGET_FOLDER = @"Release/";
        private const string SIXTY_FOUR_PLATFORM_FOLDER = @"x64/";
        private const string ARM_SIXTY_FOUR_PLATFORM_FOLDER = @"ARM64/";
        private const string TEST_SUPPORT_PROCESS_PATH = @"../../../../../TestSupportProcessCore/bin/";
        private const string TEST_SUPPORT_PROCESS_EXE_NAME = @"TestSupportProcessCore.dll";
        private const string DCPSINFOREPO_PROCESS_EXE_NAME = @"DCPSInfoRepo";
#elif OSX && ARM64
        private const string DDS_ROOT = "../../../../../../ext/OpenDDS_osx-arm64";
        private const string ACE_ROOT = "../../../../../../ext/OpenDDS_osx-arm64/ACE_wrappers";
        private const string TAO_ROOT = "../../../../../../ext/OpenDDS_osx-arm64/ACE_wrappers/TAO";
        private const string DEBUG_TARGET_FOLDER = "Debug/";
        private const string RELEASE_TARGET_FOLDER = "Release/";
        private const string SIXTY_FOUR_PLATFORM_FOLDER = "x64/";
        private const string ARM_SIXTY_FOUR_PLATFORM_FOLDER = "ARM64/";
        private const string TEST_SUPPORT_PROCESS_PATH = "../../../../../TestSupportProcessCore/bin/";
        private const string TEST_SUPPORT_PROCESS_EXE_NAME = "TestSupportProcessCore.dll";
        private const string DCPSINFOREPO_PROCESS_EXE_NAME = "DCPSInfoRepo";
#endif
        #endregion

        #region Fields
        private readonly string _runtime;
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
#elif OSX
            if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
            {
                _platformFolder = ARM_SIXTY_FOUR_PLATFORM_FOLDER;
            }
#endif
            var ddsPath = Path.GetFullPath(DDS_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var acePath = Path.GetFullPath(ACE_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var taoPath = Path.GetFullPath(TAO_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var ddsBin = Path.Combine(ddsPath, "bin");
            var ddsLib = Path.Combine(ddsPath, "lib");
            var aceBin = Path.Combine(acePath, "bin");
            var aceLib = Path.Combine(acePath, "lib");
            string path = $"{ddsBin};{ddsLib};{aceBin};{aceLib};";
            Environment.SetEnvironmentVariable("Path", path + Environment.GetEnvironmentVariable("Path"));
            Environment.SetEnvironmentVariable("DDS_ROOT", ddsPath);
            Environment.SetEnvironmentVariable("ACE_ROOT", acePath);
            Environment.SetEnvironmentVariable("TAO_ROOT", taoPath);
            _runtime = "win-x64/";
            if (_platformFolder == "x86")
            {
                _runtime = "win-x86/";
            }
#if Linux
            Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", "$LD_LIBRARY_PATH:$DDS_ROOT/lib:$ACE_ROOT/lib");

            _runtime = "linux-x64/";
            if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
            {
                _runtime = "linux-arm64";
            }
#elif OSX
            Environment.SetEnvironmentVariable("DYLD_FALLBACK_LIBRARY_PATH", "DYLD_FALLBACK_LIBRARY_PATH:$DDS_ROOT/lib:$ACE_ROOT/lib");

            _runtime = "osx-x64/";
            if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
            {
                _runtime = "osx-arm64";
            }
#endif
        }
        #endregion

        #region Methods
        public Process SpawnSupportProcess(SupportTestKind testKind)
        {
            var supportProcessPath = Path.Combine(TEST_SUPPORT_PROCESS_PATH, _platformFolder, _targetFolder, "net8.0", TEST_SUPPORT_PROCESS_EXE_NAME);
            supportProcessPath = Path.GetFullPath(supportProcessPath);
            Console.WriteLine(supportProcessPath);

            if (!File.Exists(supportProcessPath))
            {
                supportProcessPath = Path.Combine(TEST_SUPPORT_PROCESS_PATH, _platformFolder, _targetFolder, "net8.0", _runtime, TEST_SUPPORT_PROCESS_EXE_NAME);

                if (!File.Exists(supportProcessPath))
                {
                    _testContext.WriteLine($"The support process executable could not be located at {supportProcessPath}.");
                    throw new FileNotFoundException($"The support process executable could not be located at {supportProcessPath}.");
                }
            }
#if Linux || OSX
            var arguments = supportProcessPath + " " + testKind;

            return SpawnProcess("dotnet", arguments);
#else
            return SpawnProcess(supportProcessPath, teskKind.ToString());
#endif
        }

        public Process SpawnDCPSInfoRepo()
        {
            string ddsPath = Environment.GetEnvironmentVariable("DDS_ROOT");
#if Windows
            string infoRepoPath = Path.Combine($"{ddsPath}_{_platformFolder}", $"bin", DCPSINFOREPO_PROCESS_EXE_NAME);
#else
            string infoRepoPath = Path.Combine(ddsPath, "bin", DCPSINFOREPO_PROCESS_EXE_NAME);
#endif
            if (!File.Exists(infoRepoPath))
            {
                _testContext.WriteLine($"The DCPSInfoRepo executable could not be located at {infoRepoPath}.");
                throw new FileNotFoundException($"The support process executable could not be located at {infoRepoPath}.");
            }

            return SpawnProcess(infoRepoPath, @"-o repo.ior -ORBListenEndpoints iiop://localhost:12345");
        }

        private Process SpawnProcess(string path, string arguments)
        {
            var ddsPath = Path.GetFullPath(DDS_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var acePath = Path.GetFullPath(ACE_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var taoPath = Path.GetFullPath(TAO_ROOT).TrimEnd(Path.DirectorySeparatorChar);

            ProcessStartInfo processInfo = new ProcessStartInfo(path)
            {
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
            };

            processInfo.EnvironmentVariables["DDS_ROOT"] = ddsPath;
            processInfo.EnvironmentVariables["ACE_ROOT"] = acePath;
            processInfo.EnvironmentVariables["TAO_ROOT"] = taoPath;
#if Linux
            processInfo.EnvironmentVariables["LD_LIBRARY_PATH"] = $"$LD_LIBRARY_PATH:{ddsPath}/lib:{acePath}/lib:.";
#elif OSX
            processInfo.EnvironmentVariables["DYLD_FALLBACK_LIBRARY_PATH"] = $"DYLD_FALLBACK_LIBRARY_PATH:{ddsPath}/lib:{acePath}/lib:.";
#endif

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
