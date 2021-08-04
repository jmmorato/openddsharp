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
using Cake.Common.Tools.NuGet;
using Cake.Frosting;
using Cake.Common.Tools.NuGet.Pack;
using System.IO;

namespace OpenDDSharp.Build.Standard.Tasks
{
    /// <summary>
    /// Pack NuGet packages taks.
    /// </summary>
    [TaskName("PackNuGet")]
    public class PackNuGet : FrostingTask<BuildContext>
    {
        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            string solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
            string path = Path.Combine(solutionPath, "Sources", "OpenDDSharp.Standard", "OpenDDSharp.Standard.csproj");
            Cake.Core.IO.FilePath filePath = new Cake.Core.IO.FilePath(path);
            context.NuGetPack(filePath, new NuGetPackSettings());
            
            path = Path.Combine(solutionPath, "Sources", "OpenDDSharp.Templates", "OpenDDSharp.Templates.csproj");
            filePath = new Cake.Core.IO.FilePath(path);
            context.NuGetPack(filePath, new NuGetPackSettings());

            path = Path.Combine(solutionPath, "Sources", "OpenDDSharp.IdlGenerator", "OpenDDSharp.Standard.IdlGenerator.nuspec");
            filePath = new Cake.Core.IO.FilePath(path);
            context.NuGetPack(filePath, new NuGetPackSettings());

            string releaseFolder = Path.Combine(solutionPath, "Release");
            if (!Directory.Exists(releaseFolder))
            {
                Directory.CreateDirectory(releaseFolder);
            }

            foreach (var file in Directory.GetFiles(releaseFolder, "*.nupkg"))
            {
                File.Delete(file);
            }

            foreach (var file in Directory.GetFiles(".", "*.nupkg"))
            {
                FileInfo fi = new FileInfo(file);
                File.Move(file, Path.Combine(releaseFolder, fi.Name));
            }
        }
    }
}
