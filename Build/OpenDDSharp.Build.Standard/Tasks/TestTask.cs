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
using Cake.Common.Tools.DotNet.Test;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace OpenDDSharp.Build.Standard.Tasks
{
    /// <summary>
    /// Run OpenDDSharp unit test task.
    /// </summary>
    [TaskName("TestTask")]
    public class TestTask : FrostingTask<BuildContext>
    {
        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            context.Log.Information("Starting test task...");

            var solutionFullPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
            var path = Path.Combine(solutionFullPath, $"Tests/OpenDDSharp.Standard.UnitTest/bin/{context.BuildConfiguration}/net6.0/{context.RunTime}");
            context.Log.Information($"Unit test path: {path}");
            var testAdapterPath = Path.Combine(BuildContext.OPENDDSHARP_SOLUTION_FOLDER, "packages/coverlet.collector/3.1.2/build/netstandard1.0");
            var settingsFile = Path.Combine(solutionFullPath, "Tests.runsettings");
            context.Log.Information($"Settings file: {settingsFile}");

            context.DotNetTest(solutionFullPath + "/Tests/OpenDDSharp.Standard.UnitTest/OpenDDSharp.Standard.UnitTest.csproj", new DotNetTestSettings
            {
                TestAdapterPath = Path.GetFullPath(testAdapterPath),
                WorkingDirectory = path,
                EnvironmentVariables =
                {
                    { "DDS_ROOT", Path.GetFullPath(context.DdsRoot).TrimEnd('\\') },
                    { "ACE_ROOT", Path.GetFullPath(context.AceRoot).TrimEnd('\\') },
                    { "TAO_ROOT", Path.GetFullPath(context.TaoRoot).TrimEnd('\\') },
                    { "MPC_ROOT", Path.GetFullPath(context.MpcRoot).TrimEnd('\\') },
                    { "LD_LIBRARY_PATH", "$LD_LIBRARY_PATH:$DDS_ROOT/lib:$ACE_ROOT/lib" },
                    { "DYLD_FALLBACK_LIBRARY_PATH", "DYLD_FALLBACK_LIBRARY_PATH:$DDS_ROOT/lib:$ACE_ROOT/lib" },
                },
                Verbosity = DotNetVerbosity.Detailed,
                Settings = settingsFile,
                Runtime = context.RunTime,
                NoBuild = true,
                NoRestore = true,
                Configuration = context.BuildConfiguration,
            });
        }
    }
}
