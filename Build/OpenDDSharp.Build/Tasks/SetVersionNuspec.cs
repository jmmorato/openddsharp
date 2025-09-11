/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS
Copyright (C) 2018 Jose Morato - OpenDDSharp

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the MIT License.
**********************************************************************/
using Cake.Common.IO;
using Cake.Common.Xml;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace OpenDDSharp.Build.Tasks
{
    /// <summary>
    /// Set version in the assembly info files.
    /// </summary>
    [TaskName("SetVersionNuspec")]
    public class SetVersionNuspec : FrostingTask<BuildContext>
    {
        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            context.Log.Information("Set version in NuSpec...");

            var version = $"{context.MajorVersion}.{context.MinorVersion}.{context.PatchVersion}";
            if (!string.IsNullOrWhiteSpace(context.PreReleaseTag))
            {
                version += $"-{context.PreReleaseTag}{context.BuildNumber}";
            }

            var path = context.MakeAbsolute(context.Directory(BuildContext.OPENDDSHARP_SOLUTION_FOLDER));

            foreach (var file in context.GetFiles($"{path}/**/*.IdlGenerator.nuspec"))
            {
                context.XmlPoke(file, "/package/metadata/version", version);
            }
        }
    }
}
