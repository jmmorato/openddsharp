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
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.Clean;
using Cake.Common.Tools.DotNet.Restore;
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace OpenDDSharp.Build.Standard.Tasks
{
    /// <summary>
    /// Build OpenDDSharp task.
    /// </summary>
    [TaskName("BuildOpenDDSharpTask")]
    [IsDependentOn(typeof(BuildOpenDDSharpNativeTask))]
    public class BuildOpenDDSharpTask : FrostingTask<BuildContext>
    {
        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            var acePath = Path.GetFullPath(context.AceRoot).TrimEnd(Path.DirectorySeparatorChar);
            var taoPath = Path.GetFullPath(context.TaoRoot).TrimEnd(Path.DirectorySeparatorChar);
            System.Environment.SetEnvironmentVariable("ACE_ROOT", acePath);
            System.Environment.SetEnvironmentVariable("TAO_ROOT", taoPath);

            var solutionFolder = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);

            context.Log.Information("Restoring NuGet packages...");
            context.DotNetRestore("./Tests/OpenDDSharp.Standard.UnitTest/OpenDDSharp.Standard.UnitTest.csproj", new DotNetRestoreSettings
            {
                ConfigFile = Path.Combine(BuildContext.OPENDDSHARP_SOLUTION_FOLDER, "nuget.config"),
                NoCache = true,
                PackagesDirectory = Path.Combine(BuildContext.OPENDDSHARP_SOLUTION_FOLDER, "/packages"),
                WorkingDirectory = solutionFolder,
                Runtime = context.RunTime,
            });

            context.Log.Information("Clean solution...");
            context.DotNetClean("./Tests/OpenDDSharp.Standard.UnitTest/OpenDDSharp.Standard.UnitTest.csproj", new DotNetCleanSettings
            {
                Configuration = context.BuildConfiguration,
                WorkingDirectory = solutionFolder,
                EnvironmentVariables =
                {
                    { "DDS_ROOT", Path.GetFullPath(context.DdsRoot).TrimEnd('\\') },
                    { "ACE_ROOT", Path.GetFullPath(context.AceRoot).TrimEnd('\\') },
                    { "TAO_ROOT", Path.GetFullPath(context.TaoRoot).TrimEnd('\\') },
                    { "MPC_ROOT", Path.GetFullPath(context.MpcRoot).TrimEnd('\\') },
                },
                Runtime = context.RunTime,
            });

            context.Log.Information("Build OpenDDSharp.BuildTasks project...");
            context.DotNetBuild($"{solutionFolder}Sources/OpenDDSharp.BuildTasks/OpenDDSharp.BuildTasks.csproj", new DotNetBuildSettings
            {
                Configuration = context.BuildConfiguration,
                WorkingDirectory = solutionFolder,
            });

            context.Log.Information("Build OpenDDSharp solution...");
            context.DotNetBuild("./Tests/OpenDDSharp.Standard.UnitTest/OpenDDSharp.Standard.UnitTest.csproj", new DotNetBuildSettings
            {
                Configuration = context.BuildConfiguration,
                WorkingDirectory = solutionFolder,
                EnvironmentVariables =
                {
                    { "DDS_ROOT", Path.GetFullPath(context.DdsRoot).TrimEnd('\\') },
                    { "ACE_ROOT", Path.GetFullPath(context.AceRoot).TrimEnd('\\') },
                    { "TAO_ROOT", Path.GetFullPath(context.TaoRoot).TrimEnd('\\') },
                    { "MPC_ROOT", Path.GetFullPath(context.MpcRoot).TrimEnd('\\') },
                },
                Runtime = context.RunTime,
            });
        }
    }
}
