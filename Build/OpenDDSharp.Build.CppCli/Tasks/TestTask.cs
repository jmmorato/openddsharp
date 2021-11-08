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
using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.VSTest;
using Cake.Core;
using Cake.Frosting;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("Test")]    
    public class TestTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var path = $"../../Tests/OpenDDSharp.UnitTest/bin/{context.BuildPlatform}/{context.BuildConfiguration}/";
            var file = "OpenDDSharp.UnitTest.dll";
            var testAdapterPath = Path.Combine(BuildContext.OPENDDSHARP_SOLUTION_FOLDER, "packages/MSTest.TestAdapter.2.2.7/build/_common");
            var settingsFile = Path.Combine(Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER), "CodeCoverage.runsettings");
            var platform = context.BuildPlatform == PlatformTarget.x64 ? VSTestPlatform.x64 : VSTestPlatform.x86;

            context.VSTest(path + file, new VSTestSettings
            {
                ToolPath = context.Tools.Resolve("vstest.console.exe"),
                TestAdapterPath = Path.GetFullPath(testAdapterPath),
                FrameworkVersion = VSTestFrameworkVersion.Default,
                EnableCodeCoverage = true,
                PlatformArchitecture = platform,
                WorkingDirectory = Path.GetFullPath(path),
                EnvironmentVariables =
                {
                    { nameof(BuildContext.DDS_ROOT), Path.GetFullPath(BuildContext.DDS_ROOT).TrimEnd('\\') },
                    { nameof(BuildContext.ACE_ROOT), Path.GetFullPath(BuildContext.ACE_ROOT).TrimEnd('\\') },
                    { nameof(BuildContext.TAO_ROOT), Path.GetFullPath(BuildContext.TAO_ROOT).TrimEnd('\\') },
                    { nameof(BuildContext.MPC_ROOT), Path.GetFullPath(BuildContext.MPC_ROOT).TrimEnd('\\') },
                },
                SettingsFile = settingsFile,
                Logger = "trx",
            });
        }
    }
}
