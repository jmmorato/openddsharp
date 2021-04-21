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

                context.XmlPoke(file, "/ns:PackageManifest/ns:Metadata/ns:Preview", context.IsDevelop ? "true" : "false", new XmlPokeSettings
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
