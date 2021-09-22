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
using Cake.Frosting;
using Cake.Common.Xml;
using Cake.Core.IO;
using Cake.Common.IO;

namespace OpenDDSharp.Build.Standard.Tasks
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
            string version = $"{context.MajorVersion}.{context.MinorVersion}.{context.GetBuildRevisionVersion()}";
            if (context.IsDevelop)
            {
                version += $"-beta";
            }
            DirectoryPath path = context.MakeAbsolute(context.Directory(BuildContext.OPENDDSHARP_SOLUTION_FOLDER));

            foreach (var file in context.GetFiles($"{path}/**/*.Standard.IdlGenerator.nuspec"))
            {
                context.XmlPoke(file, "/package/metadata/version", version);
            }
        }
    }
}
