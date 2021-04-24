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
using Cake.Frosting;
using System.IO;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("PackVsix")]    
    public class PackVsix : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            string solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
            string projectFolder = Path.Combine(solutionPath, "Sources", "OpenDDSharp.IdlTemplates");
            string projectPath = Path.Combine(projectFolder, "OpenDDSharp.IdlTemplates.csproj");
            context.MSBuild(projectPath, new MSBuildSettings
            {
                Configuration = "Release",
                PlatformTarget = PlatformTarget.MSIL,
                Targets = { "Clean", "Build" },
                MaxCpuCount = 0,
                WorkingDirectory = projectFolder,
            });

            string releaseFolder = Path.Combine(solutionPath, "Release");
            if (!Directory.Exists(releaseFolder))
            {
                Directory.CreateDirectory(releaseFolder);
            }

            foreach(var file in Directory.GetFiles(releaseFolder, "*.vsix"))
            {
                File.Delete(file);
            }

            foreach (var file in Directory.GetFiles(Path.Combine(projectFolder, "bin", "Release"), "*.vsix"))
            {
                FileInfo fi = new FileInfo(file);
                File.Move(file, Path.Combine(releaseFolder, fi.Name));
            }
        }
    }
}
