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
using Cake.Common.IO;
using Cake.Common.Xml;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace OpenDDSharp.Build.Tasks
{
    /// <summary>
    /// Set version in the assembly info files.
    /// </summary>
    [TaskName("SetVersionProjectTemplate")]
    public class SetVersionProjectTemplate : FrostingTask<BuildContext>
    {
        /// <inheritdoc/>
        public override void Run(BuildContext context)
        {
            context.Log.Information("Set version in ProjectTemplate...");

            var version = $"{context.MajorVersion}.{context.MinorVersion}.{context.PatchVersion}";
            if (!string.IsNullOrWhiteSpace(context.PreReleaseTag))
            {
                version += $"{context.PreReleaseTag}{context.BuildNumber}";
            }

            var path = context.MakeAbsolute(context.Directory(BuildContext.OPENDDSHARP_SOLUTION_FOLDER));

            foreach (var file in context.GetFiles($"{path}/**/OpenDDSharp.IdlProject.csproj"))
            {
                context.XmlPoke(file, "/Project/ItemGroup/PackageReference/@Version", version);
            }

            foreach (var file in context.GetFiles($"{path}/**/OpenDDSharp.ConsoleApp.csproj"))
            {
                context.XmlPoke(file, "/Project/ItemGroup/PackageReference/@Version", version);
            }
        }
    }
}
