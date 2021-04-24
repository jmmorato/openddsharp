using Cake.Frosting;
using Cake.Common.Xml;
using Cake.Core.IO;
using Cake.Common.IO;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("SetVersionNuspec")]    
    public class SetVersionNuspec : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            string version = $"{context.MajorVersion}.{context.MinorVersion}.{context.GetBuildRevisionVersion()}";
            if (context.IsDevelop)
            {
                version += $"-alpha{context.RunNumber}";
            }
            DirectoryPath path = context.MakeAbsolute(context.Directory(BuildContext.OPENDDSHARP_SOLUTION_FOLDER));

            foreach (var file in context.GetFiles($"{path}/**/*.nuspec"))
            {
                context.XmlPoke(file, "/package/metadata/version", version);
            }
        }
    }
}
