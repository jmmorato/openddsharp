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
using Cake.Common.Tools.DotNet.Pack;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Core;
using Cake.Frosting;

namespace OpenDDSharp.Build.Standard.Tasks
{
    /// <summary>
    /// Pack NuGet packages taks.
    /// </summary>
    [TaskName("PackNuGet")]
    [IsDependentOn(typeof(PrepareLinuxLibraries))]
    [IsDependentOn(typeof(PrepareCMakeConfig))]
    public class PackNuGet : FrostingTask<BuildContext>
    {
        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            string version = $"{context.MajorVersion}.{context.MinorVersion}.{context.GetBuildRevisionVersion()}";
            if (context.IsDevelop)
            {
                version += $"-beta";
            }

            string solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
            string path = Path.Combine(solutionPath, "Sources", "OpenDDSharp.Standard", "OpenDDSharp.Standard.csproj");
            context.DotNetPack(path, new DotNetPackSettings
            {
                Configuration = "Release",
                ArgumentCustomization = args => args.Append($"/p:Version={version}"),
                OutputDirectory = ".",
            });

            path = Path.Combine(solutionPath, "Sources", "OpenDDSharp.Templates", "OpenDDSharp.Templates.csproj");
            context.DotNetPack(path, new DotNetPackSettings
            {
                Configuration = "Release",
                NoBuild = true,
                ArgumentCustomization = args => args.Append($"/p:Version={version}"),
                OutputDirectory = ".",
            });

            path = Path.Combine(solutionPath, "Sources", "OpenDDSharp.Native", "OpenDDSharp.Native.csproj");
            context.DotNetPack(path, new DotNetPackSettings
            {
                Configuration = "Release",
                NoBuild = false,
                ArgumentCustomization = args => args.Append($"/p:Version={version}"),
                OutputDirectory = ".",
            });

            path = Path.Combine(solutionPath, "Sources", "OpenDDSharp.Marshaller", "OpenDDSharp.Marshaller.csproj");
            context.DotNetPack(path, new DotNetPackSettings
            {
                Configuration = "Release",
                NoBuild = false,
                ArgumentCustomization = args => args.Append($"/p:Version={version}"),
                OutputDirectory = ".",
            });

            path = Path.Combine(solutionPath, "Native", "OpenDDSharp.Standard.IdlGenerator.nuspec");
            var filePath = new Cake.Core.IO.FilePath(path);
            context.NuGetPack(filePath, new NuGetPackSettings
            {
                Verbosity = NuGetVerbosity.Detailed,
            });

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
