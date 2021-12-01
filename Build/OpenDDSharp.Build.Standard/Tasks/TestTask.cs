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
using System.Text;
using Cake.Common;
using Cake.Common.Tools.DotNetCore;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using OpenDDSharp.Build.Standard.Exceptions;

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
            var path = Path.Combine(solutionFullPath, $"Tests/OpenDDSharp.Standard.UnitTest/bin/{context.BuildPlatform}/{context.BuildConfiguration}/netcoreapp3.1/");
            context.Log.Information($"Unit test path: {path}");
            var file = "OpenDDSharp.Standard.UnitTest.dll";
            var testAdapterPath = Path.Combine(BuildContext.OPENDDSHARP_SOLUTION_FOLDER, "packages/mstest.testadapter/2.2.8/build/_common");
            var settingsFile = Path.Combine(solutionFullPath, "CodeCoverage.runsettings");
            context.Log.Information($"Settings file: {settingsFile}");

            if (BuildContext.IsWindows)
            {
                context.DotNetCoreTest(path + file, new Cake.Common.Tools.DotNetCore.Test.DotNetCoreTestSettings
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
                    Verbosity = DotNetCoreVerbosity.Normal,
                    Settings = settingsFile,
                });
            }
            else
            {
                try
                {
                    var dllPath = Path.Combine(solutionFullPath, $"Tests/OpenDDSharp.Standard.UnitTest/bin/{context.BuildPlatform}/{context.BuildConfiguration}/netcoreapp3.1/", "OpenDDSharp.Standard.UnitTest.dll");

                    var linuxPath = path;

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append($"export DDS_ROOT={Path.GetFullPath(context.DdsRoot)} && ");
                    stringBuilder.Append($"export LD_LIBRARY_PATH=$DDS_ROOT/lib:$DDS_ROOT/ACE_TAO/ACE/lib:{linuxPath} && ");
                    stringBuilder.Append($"dotnet test {dllPath} -v n");

                    var exit = context.StartProcess("bash", new Cake.Core.IO.ProcessSettings
                    {
                        Arguments = stringBuilder.ToString(),
                        WorkingDirectory = path,
                    });

                    if (exit != 0)
                    {
                        throw new BuildException($"Error calling the WSL dotnet test. Exit code: {exit}");
                    }
                }
                catch (System.Exception ex)
                {
                    context.Log.Error(ex.StackTrace.ToString());
                    throw;
                }
            }
        }
    }
}
