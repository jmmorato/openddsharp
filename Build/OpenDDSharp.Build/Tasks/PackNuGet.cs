/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using System.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Pack;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Core;
using Cake.Frosting;

namespace OpenDDSharp.Build.Tasks
{
    /// <summary>
    /// Pack NuGet packages tasks.
    /// </summary>
    [TaskName("PackNuGet")]
    [IsDependentOn(typeof(PrepareLinuxLibraries))]
    [IsDependentOn(typeof(PrepareCMakeConfig))]
    public class PackNuGet : FrostingTask<BuildContext>
    {
        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            var version = $"{context.MajorVersion}.{context.MinorVersion}.{context.PatchVersion}";
            if (!string.IsNullOrWhiteSpace(context.PreReleaseTag))
            {
                version += $"-{context.PreReleaseTag}{context.BuildNumber}";
            }

            var solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
            var path = Path.Combine(solutionPath, "Sources", "OpenDDSharp", "OpenDDSharp.csproj");
            context.DotNetPack(path, new DotNetPackSettings
            {
                Configuration = "Release",
                NoBuild = true,
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

            path = Path.Combine(solutionPath, "Native", "OpenDDSharp.IdlGenerator.nuspec");
            var filePath = new Cake.Core.IO.FilePath(path);
            context.NuGetPack(filePath, new NuGetPackSettings
            {
                Verbosity = NuGetVerbosity.Detailed,
            });

            var releaseFolder = Path.Combine(solutionPath, "Release");
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
                var fi = new FileInfo(file);
                File.Move(file, Path.Combine(releaseFolder, fi.Name));
            }
        }
    }
}
