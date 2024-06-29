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

using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using Path = System.IO.Path;

namespace OpenDDSharp.Build.Tasks
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
            var path = Path.Combine(solutionFullPath, $"Tests/OpenDDSharp.UnitTest/bin/{context.BuildConfiguration}/net6.0/{context.RunTime}");
            context.Log.Information($"Unit test path: {path}");
            var testAdapterPath = Path.Combine(BuildContext.OPENDDSHARP_SOLUTION_FOLDER, "packages/coverlet.collector/6.0.2/build/netstandard2.0");
            var settingsFile = Path.Combine(solutionFullPath, "Tests.runsettings");
            context.Log.Information($"Settings file: {settingsFile}");

            var dotnetTestSettings = new DotNetTestSettings
            {
                TestAdapterPath = Path.GetFullPath(testAdapterPath),
                WorkingDirectory = path,
                EnvironmentVariables =
                {
                    { "DDS_ROOT", Path.GetFullPath(context.DdsRoot).TrimEnd('\\') },
                    { "ACE_ROOT", Path.GetFullPath(context.AceRoot).TrimEnd('\\') },
                    { "TAO_ROOT", Path.GetFullPath(context.TaoRoot).TrimEnd('\\') },
                    { "MPC_ROOT", Path.GetFullPath(context.MpcRoot).TrimEnd('\\') },
                },
                Settings = settingsFile,
                Runtime = context.RunTime,
                NoBuild = true,
                NoRestore = true,
                Verbosity = DotNetVerbosity.Detailed,
                Configuration = context.BuildConfiguration,
                Loggers = { "trx;LogFileName=test-results.trx", "console;verbosity=detailed" },
            };

            context.DotNetTest(solutionFullPath + "/Tests/OpenDDSharp.UnitTest/OpenDDSharp.UnitTest.csproj", dotnetTestSettings);
        }
    }
}
