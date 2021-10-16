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
using System.IO;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.MSBuild;
using Cake.Frosting;
using OpenDDSharp.Build.Standard.Exceptions;

namespace OpenDDSharp.Build.Standard.Tasks
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

            RestoreOriginalBinLibFolders(context, ddsPath, acePath);

            var platform = context.BuildPlatform;
            if (platform == PlatformTarget.x86)
            {
                platform = PlatformTarget.Win32;
            }

            if (!context.IsLinuxBuild)
            {
                context.MSBuild(context.OpenDdsSolutionFile, new MSBuildSettings
                {
                    Configuration = context.BuildConfiguration,
                    PlatformTarget = platform,
                    Targets = { "Build" },
                    MaxCpuCount = 1,
                    WorkingDirectory = Path.GetFullPath(context.DdsRoot),
                    EnvironmentVariables =
                    {
                        { "DDS_ROOT", ddsPath },
                        { "ACE_ROOT", acePath },
                        { "TAO_ROOT", Path.GetFullPath(context.TaoRoot).TrimEnd(Path.DirectorySeparatorChar) },
                        { "MPC_ROOT", Path.GetFullPath(context.MpcRoot).TrimEnd(Path.DirectorySeparatorChar) },
                    },
                    ToolVersion = context.VisualStudioVersion,
                });
            }
            else
            {
                var exit = context.StartProcess("wsl", new Cake.Core.IO.ProcessSettings
                {
                    Arguments = "make",
                    WorkingDirectory = ddsPath,
                });

                if (exit != 0)
                {
                    throw new BuildException($"Error calling the OpenDDS configure script. Exit code: {exit}");
                }
            }

            var ddsBinPlatform = Path.Combine(ddsPath, $"bin_{context.BuildPlatform}_{context.BuildConfiguration}");
            if (context.DirectoryExists(ddsBinPlatform))
            {
                context.DeleteDirectory(ddsBinPlatform, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });
            }
            context.CreateDirectory(ddsBinPlatform);
            context.CopyFiles(Path.Combine(ddsPath, "bin/*"), ddsBinPlatform);

            var ddsLibPlatform = Path.Combine(ddsPath, $"lib_{context.BuildPlatform}_{context.BuildConfiguration}");
            if (context.DirectoryExists(ddsLibPlatform))
            {
                context.DeleteDirectory(ddsLibPlatform, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });
            }
            context.CreateDirectory(ddsLibPlatform);
            context.CopyFiles(Path.Combine(ddsPath, "lib/*"), ddsLibPlatform);

            var aceBinPlatform = Path.Combine(acePath, $"bin_{context.BuildPlatform}_{context.BuildConfiguration}");
            if (context.DirectoryExists(aceBinPlatform))
            {
                context.DeleteDirectory(aceBinPlatform, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });
            }
            context.CreateDirectory(aceBinPlatform);
            context.CopyFiles(Path.Combine(acePath, "bin/*"), aceBinPlatform);

            var aceLibPlatform = Path.Combine(acePath, $"lib_{context.BuildPlatform}_{context.BuildConfiguration}");
            if (context.DirectoryExists(aceLibPlatform))
            {
                context.DeleteDirectory(aceLibPlatform, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });
            }

            context.CreateDirectory(aceLibPlatform);
            context.CopyFiles(Path.Combine(acePath, "lib/*"), aceLibPlatform);

            CleanupTemporalFiles(context, ddsPath);
        }

        private static void RestoreOriginalBinLibFolders(BuildContext context, string ddsPath, string acePath)
        {
            string ddsBin = Path.Combine(ddsPath, "bin");
            string ddsLib = Path.Combine(ddsPath, "lib");
            string ddsBinOriginal = Path.Combine(ddsPath, "original_bin");
            string ddsLibOriginal = Path.Combine(ddsPath, "original_lib");

            if (Directory.Exists(ddsBinOriginal) && Directory.Exists(ddsLibOriginal))
            {
                context.DeleteDirectory(ddsBin, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });

                context.CreateDirectory(ddsBin);
                context.CopyFiles(ddsBinOriginal, ddsBin);

                context.DeleteDirectory(ddsLib, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });

                context.CreateDirectory(ddsLib);
                context.CopyFiles(ddsLibOriginal, ddsLib);
            }

            string aceBin = Path.Combine(acePath, "bin");
            string aceLib = Path.Combine(acePath, "lib");
            string aceBinOriginal = Path.Combine(acePath, "original_bin");
            string aceLibOriginal = Path.Combine(acePath, "original_lib");

            if (Directory.Exists(aceBinOriginal) && Directory.Exists(aceLibOriginal))
            {
                context.DeleteDirectory(aceBin, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });

                context.CreateDirectory(aceBin);
                context.CopyFiles(aceBinOriginal, aceBin);

                context.DeleteDirectory(aceLib, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });

                context.CreateDirectory(aceLib);
                context.CopyFiles(aceLibOriginal, aceLib);
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
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\IORTable", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\BiDir_GIOP", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\PI", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\ImR_Client", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\IORManipulation", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\Valuetype", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\orbsvcs\orbsvcs", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\TAO_IDL", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\DynamicInterface", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\CodecFactory", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\PortableServer", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\AnyTypeCode", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\Codeset", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\Messaging", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\CSD_ThreadPool", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao\CSD_Framework", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\ACE\apps\gperf\src", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\ACE\ace", context.BuildConfiguration),
                Path.Combine(ddsPath, @"ACE_TAO\TAO\tao", context.BuildConfiguration),
            };

            foreach (var folder in cleanupFolders)
            {
                if (Directory.Exists(folder))
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
}
