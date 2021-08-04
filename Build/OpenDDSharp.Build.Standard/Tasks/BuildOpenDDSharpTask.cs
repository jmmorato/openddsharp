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
using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Core;
using Cake.Frosting;
using System.IO;

namespace OpenDDSharp.Build.Standard.Tasks
{
    /// <summary>
    /// Build OpenDDSharp task.
    /// </summary>
    [TaskName("BuildOpenDDSharpTask")]
    [IsDependentOn(typeof(BuildThirdPartyTask))]
    public class BuildOpenDDSharpTask : FrostingTask<BuildContext>
    {
        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            var acePath = Path.GetFullPath(context.AceRoot).TrimEnd(Path.DirectorySeparatorChar);
            var taoPath = Path.GetFullPath(context.TaoRoot).TrimEnd(Path.DirectorySeparatorChar);
            System.Environment.SetEnvironmentVariable("ACE_ROOT", acePath);
            System.Environment.SetEnvironmentVariable("TAO_ROOT", taoPath);

            context.NuGetRestore(BuildContext.OPENDDSHARP_SOLUTION_FILE, new NuGetRestoreSettings
            {
                NoCache = true,
            });

            var settings = new MSBuildSettings
            {
                Configuration = context.BuildConfiguration,
                PlatformTarget = context.BuildPlatform,
                Targets = { "Clean", "Build" },
                MaxCpuCount = 0,
                WorkingDirectory = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER),
                EnvironmentVariables =
                {
                    { "DDS_ROOT", Path.GetFullPath(context.DdsRoot).TrimEnd('\\') },
                    { "ACE_ROOT", Path.GetFullPath(context.AceRoot).TrimEnd('\\') },
                    { "TAO_ROOT", Path.GetFullPath(context.TaoRoot).TrimEnd('\\') },
                    { "MPC_ROOT", Path.GetFullPath(context.MpcRoot).TrimEnd('\\') },
                },
                ToolVersion = context.VisualStudioVersion,
            };

            if (!context.IsLinuxBuild)
            {
                settings.ArgumentCustomization = args => context.VisualStudioVersion == MSBuildToolVersion.VS2019 ? args.Append("/p:PlatformToolset=v142") : args.Append(string.Empty);
            }

            context.MSBuild(BuildContext.OPENDDSHARP_SOLUTION_FILE, settings);
        }
    }
}
