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
using Cake.Core.IO;
using Cake.Frosting;
using System.Collections.Generic;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("SetVersionVsix")]    
    public class SetVersionVsix : FrostingTask<BuildContext>
    {        
        public override void Run(BuildContext context)
        {
            int majorVersion = int.Parse(context.MajorVersion) + 1;
            string version = $"{majorVersion}.{context.MinorVersion}.{context.GetBuildRevisionVersion()}";
            DirectoryPath path = context.MakeAbsolute(context.Directory(BuildContext.OPENDDSHARP_SOLUTION_FOLDER));

            foreach (var file in context.GetFiles($"{path}/**/*.vsixmanifest"))
            {
                context.XmlPoke(file, "/ns:PackageManifest/ns:Metadata/ns:Identity/@Version", version, new XmlPokeSettings
                {
                    Namespaces = new Dictionary<string, string> 
                    { 
                        { "ns", "http://schemas.microsoft.com/developer/vsx-schema/2011" },
                        { "d", "http://schemas.microsoft.com/developer/vsx-schema-design/2011" }
                    }
                });
            }
        }
    }
}
