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
using System.Linq;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.MSBuild;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using OpenDDSharp.Build.Exceptions;

namespace OpenDDSharp.Build.Tasks
{
    /// <summary>
    /// Build third-party projects task.
    /// </summary>
    [TaskName("BuildThirdParty")]
    [IsDependentOn(typeof(SetupThirdPartyTask))]
    public class BuildThirdPartyTask : FrostingTask<BuildContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(BuildContext context)
        {
            if (context.IgnoreThirdPartyBuild)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            var ddsPath = Path.GetFullPath(context.DdsRoot).TrimEnd(Path.DirectorySeparatorChar);
            var acePath = Path.GetFullPath(context.AceRoot).TrimEnd(Path.DirectorySeparatorChar);

            var platform = context.BuildPlatform;
            if (platform == PlatformTarget.x86)
            {
                platform = PlatformTarget.Win32;
            }

            const string vsVersion = "2022";
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var toolPath = @$"{programFiles}\Microsoft Visual Studio\{vsVersion}\{context.VisualStudioEdition}\MSBuild\Current\Bin\MSBuild.exe";
            if (platform == PlatformTarget.x64)
            {
                toolPath = @$"{programFiles}\Microsoft Visual Studio\{vsVersion}\{context.VisualStudioEdition}\MSBuild\Current\Bin\amd64\MSBuild.exe";
            }

            if (context.VisualStudioVersion == MSBuildToolVersion.VS2019)
            {
                programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                toolPath = @$"{programFiles}\Microsoft Visual Studio\{vsVersion}\BuildTools\MSBuild\Current\Bin\MSBuild.exe";
                if (platform == PlatformTarget.x64)
                {
                    toolPath = @$"{programFiles}\Microsoft Visual Studio\{vsVersion}\BuildTools\MSBuild\Current\Bin\amd64\MSBuild.exe";
                }
            }

            context.Log.Information($"Tool Path: {toolPath}");
            if (BuildContext.IsWindows)
            {
                context.MSBuild(context.OpenDdsSolutionFile, new MSBuildSettings
                {
                    Configuration = context.BuildConfiguration,
                    PlatformTarget = platform,
                    Targets = { "Build" },
                    MaxCpuCount = 0,
                    WorkingDirectory = Path.GetFullPath(context.DdsRoot),
                    EnvironmentVariables =
                    {
                        { "DDS_ROOT", ddsPath },
                        { "ACE_ROOT", acePath },
                        { "TAO_ROOT", Path.GetFullPath(context.TaoRoot).TrimEnd(Path.DirectorySeparatorChar) },
                        { "MPC_ROOT", Path.GetFullPath(context.MpcRoot).TrimEnd(Path.DirectorySeparatorChar) },
                    },
                    ToolVersion = context.VisualStudioVersion,
                    ToolPath = toolPath,
                    MSBuildPlatform = platform == PlatformTarget.Win32 ? MSBuildPlatform.x86 : MSBuildPlatform.x64,
                });
            }
            else
            {
                var exit = context.StartProcess("make", new Cake.Core.IO.ProcessSettings
                {
                    Arguments = "-j4",
                    WorkingDirectory = ddsPath,
                });

                if (exit != 0)
                {
                    throw new BuildException($"Error calling 'make'. Exit code: {exit}");
                }
            }

            if (context.CleanupTemporalFiles)
            {
                CleanupTemporalFiles(context, ddsPath);
            }
        }

        private static void CleanupTemporalFiles(BuildContext context, string ddsPath)
        {
            string[] cleanupFolders =
            {
                Path.Combine(ddsPath, "dds", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\idl", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\DCPS", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\DCPS\InfoRepoDiscovery", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\DCPS\transport\rtps_udp", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\DCPS\RTPS", context.BuildConfiguration),
                Path.Combine(ddsPath, @"tools\rtpsrelay\lib", context.BuildConfiguration),
                Path.Combine(ddsPath, @"tools\modeling\codegen\model", context.BuildConfiguration),
                Path.Combine(ddsPath, @"tools\repoctl", context.BuildConfiguration),
                Path.Combine(ddsPath, @"tools\dcpsinfo_dump", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\monitor", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\InfoRepo", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\DCPS\transport\udp", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\DCPS\transport\shmem", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\DCPS\transport\multicast", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\DCPS\transport\tcp", context.BuildConfiguration),
                Path.Combine(ddsPath, @"dds\FACE", context.BuildConfiguration),
                Path.Combine(ddsPath, @"DevGuideExamples\FACE\Simple", context.BuildConfiguration),
                Path.Combine(ddsPath, @"DevGuideExamples\DCPS\Messenger", context.BuildConfiguration),
                Path.Combine(ddsPath, @"DevGuideExamples\DCPS\Messenger_ZeroCopy", context.BuildConfiguration),
                Path.Combine(ddsPath, @"DevGuideExamples\DCPS\Messenger.minimal", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\IORTable", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\BiDir_GIOP", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\PI", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\ImR_Client", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\IORManipulation", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\Valuetype", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\orbsvcs\orbsvcs", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\TAO_IDL", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\DynamicInterface", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\CodecFactory", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\PortableServer", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\AnyTypeCode", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\Codeset", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\Messaging", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\CSD_ThreadPool", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao\CSD_Framework", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\apps\gperf\src", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\ace", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_wrappers\TAO\tao", context.BuildConfiguration),
            };

            foreach (var folder in cleanupFolders.Where(Directory.Exists))
            {
                context.DeleteDirectory(folder, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });
            }
        }
    }
}
