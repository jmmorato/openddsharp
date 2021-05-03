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
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Json;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("SetVersionProjectTemplate")]    
    public class SetVersionProjectTemplate : FrostingTask<BuildContext>
    {
        public override bool ShouldRun(BuildContext context)
        {
            return !context.IsDevelop;
        }

        public override void Run(BuildContext context)
        {            
            DirectoryPath path = context.MakeAbsolute(context.Directory(BuildContext.OPENDDSHARP_SOLUTION_FOLDER));
            string filePath = System.IO.Path.Combine(path.FullPath, "Sources", "OpenDDSharp.IdlProjectTemplate", "project.json_rename_me");

            string version = $"{context.MajorVersion}.{context.MinorVersion}.{context.GetBuildRevisionVersion()}";
            if (context.IsDevelop)
            {
                version += $"-alpha";
            }

            var parsed = context.ParseJsonFromFile(filePath);
            parsed["dependencies"]["OpenDDSharp.IdlGenerator"] = version;

            context.SerializeJsonToPrettyFile(filePath, parsed);
        }
    }
}
