﻿/*********************************************************************
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
using Cake.CMake;
using Cake.Common.Tools.MSBuild;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace OpenDDSharp.Build.Tasks
{
    /// <summary>
    /// Build OpenDDSharp Native task.
    /// </summary>
    [TaskName("BuildOpenDDSharpNativeTask")]
    [IsDependentOn(typeof(BuildThirdPartyTask))]
    public class BuildOpenDDSharpNativeTask : FrostingTask<BuildContext>
    {
        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            context.Log.Information(Path.GetFullPath(BuildContext.NATIVE_FOLDER));
            var nativeFolder = Path.GetFullPath(BuildContext.NATIVE_FOLDER).Replace("\\", "/");
            var buildFoder = nativeFolder + "build";
            var platform = context.BuildPlatform == PlatformTarget.x86 ? "Win32" : "x64";

            if (BuildContext.IsWindows)
            {
                buildFoder += $"_{context.BuildPlatform}";
            }
            var arguments = $"--no-warn-unused-cli -DCMAKE_BUILD_TYPE={context.BuildConfiguration} -DCMAKE_PREFIX_PATH={Path.GetFullPath(context.DdsRoot)} -DCMAKE_EXPORT_COMPILE_COMMANDS:BOOL=TRUE -A {platform} -H{nativeFolder} -B{buildFoder}";

            if (BuildContext.IsLinux)
            {
                buildFoder += "_Linux";
                arguments = $"--no-warn-unused-cli -DCMAKE_BUILD_TYPE=Release -DCMAKE_PREFIX_PATH={Path.GetFullPath(context.DdsRoot)} -DCMAKE_EXPORT_COMPILE_COMMANDS:BOOL=TRUE -H{nativeFolder} -B{buildFoder}";
            }
            else if (BuildContext.IsOSX && BuildContext.IsARM64)
            {
                buildFoder += "_osx-arm64";
                arguments = $"--no-warn-unused-cli -DCMAKE_BUILD_TYPE=Release -DCMAKE_PREFIX_PATH={Path.GetFullPath(context.DdsRoot)} -DCMAKE_EXPORT_COMPILE_COMMANDS:BOOL=TRUE -H{nativeFolder} -B{buildFoder}";
            }
            else if (BuildContext.IsOSX && !BuildContext.IsARM64)
            {
                buildFoder += "_osx-x64";
                arguments = $"--no-warn-unused-cli -DCMAKE_BUILD_TYPE=Release -DCMAKE_PREFIX_PATH={Path.GetFullPath(context.DdsRoot)} -DCMAKE_EXPORT_COMPILE_COMMANDS:BOOL=TRUE -H{nativeFolder} -B{buildFoder}";
            }

            context.CMake(new CMakeSettings
            {
                SourcePath = nativeFolder,
                OutputPath = buildFoder,
                ArgumentCustomization = args => arguments,
                WorkingDirectory = nativeFolder,
                EnvironmentVariables =
                {
                    { "DDS_ROOT", Path.GetFullPath(context.DdsRoot).TrimEnd(Path.DirectorySeparatorChar) },
                    { "ACE_ROOT", Path.GetFullPath(context.AceRoot).TrimEnd(Path.DirectorySeparatorChar) },
                    { "TAO_ROOT", Path.GetFullPath(context.TaoRoot).TrimEnd(Path.DirectorySeparatorChar) },
                    { "MPC_ROOT", Path.GetFullPath(context.MpcRoot).TrimEnd(Path.DirectorySeparatorChar) },
                },
            });

            var buildSettings = new CMakeBuildSettings
            {
                BinaryPath = buildFoder,
                WorkingDirectory = nativeFolder,
                CleanFirst = true,
                EnvironmentVariables =
                {
                    { "DDS_ROOT", Path.GetFullPath(context.DdsRoot).TrimEnd(Path.DirectorySeparatorChar) },
                    { "ACE_ROOT", Path.GetFullPath(context.AceRoot).TrimEnd(Path.DirectorySeparatorChar) },
                    { "TAO_ROOT", Path.GetFullPath(context.TaoRoot).TrimEnd(Path.DirectorySeparatorChar) },
                    { "MPC_ROOT", Path.GetFullPath(context.MpcRoot).TrimEnd(Path.DirectorySeparatorChar) },
                },
            };
            if (BuildContext.IsWindows)
            {
                buildSettings.Configuration = context.BuildConfiguration;
            }
            else
            {
                buildSettings.Configuration = "Release";
            }
            context.CMakeBuild(buildSettings);
        }
    }
}
