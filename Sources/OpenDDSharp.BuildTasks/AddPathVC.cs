/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

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
using System.IO;
using System.Diagnostics;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace OpenDDSharp.BuildTasks
{
    public class AddPathVC : Task
    {
        private const string VSWHERE_PATH = @"C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe";
        private string _platform;

        [Required]
        public string Platform
        {
            get { return _platform; }
            set { _platform = value; }
        }

        public override bool Execute()
        {
#if DEBUG
            Log.LogMessage(MessageImportance.High, "Adding VC to the PATH...");
            Log.LogMessage(MessageImportance.High, "Platform: {0}", _platform);
#endif

            if (!File.Exists(VSWHERE_PATH))
            {
                Log.LogError("vswhere.exe not found: Visual Studio required version >= 15.2");
                return false;
            }

            // Get the current MSBuild version
            var msbuildProcess = Process.GetCurrentProcess();
            int msbuildVersion = msbuildProcess.MainModule.FileVersionInfo.FileMajorPart;
            string strVersion = "\"[15.0,16.0)\"";
            if (msbuildVersion == 16)
            {
                strVersion = "\"[16.0,17.0)\"";
            }
            string installPath = ExecuteVSWhereCommand("-version " + strVersion  + " -products * -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 -property installationPath");
            installPath = installPath.Replace(Environment.NewLine, string.Empty);
#if DEBUG
            Log.LogMessage(MessageImportance.High, "VC Install Path: {0}", installPath);
#endif

            string versionPath = string.Format("{0}\\VC\\Auxiliary\\Build\\Microsoft.VCToolsVersion.default.txt", installPath);            
            if (!File.Exists(versionPath))
            {
#if DEBUG
                Log.LogError("Microsoft.VCToolsVersion.default.txt not found: {0}", versionPath);
#endif
                return false;
            }

            string vcVersion = File.ReadAllText(versionPath);
            vcVersion = vcVersion.Replace(Environment.NewLine, string.Empty);
#if DEBUG
            Log.LogMessage(MessageImportance.High, "VC Version: {0}", vcVersion);
#endif

            string platformPath = "Hostx86\\x86\\";
            if (Platform == "x64")
            {
                platformPath = "Hostx64\\x64\\";
            }
            string clPath = string.Format("{0}\\VC\\Tools\\MSVC\\{1}\\bin\\{2}", installPath, vcVersion, platformPath);
#if DEBUG
            Log.LogMessage(MessageImportance.High, "CL.exe Path: {0}", clPath);
#endif

            string currenPath = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", string.Format("{0};{1}", clPath, currenPath));

            return true;
        }

        private string ExecuteVSWhereCommand(string parameters)
        {            
            string output = string.Empty;

            using (Process process = new Process())
            {
                process.StartInfo.FileName = VSWHERE_PATH;
                process.StartInfo.Arguments = parameters;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                string err = process.StandardError.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(err))
                {
                    Log.LogError("VSWhere Command Error: {0}", err);
                }
                process.WaitForExit(5000);
            }

            return output;
        }
    }
}
