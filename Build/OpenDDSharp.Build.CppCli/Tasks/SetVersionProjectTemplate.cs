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
                version += $"-alpha{context.RunNumber}";
            }

            var parsed = context.ParseJsonFromFile(filePath);
            parsed["dependencies"]["OpenDDSharp.IdlGenerator"] = version;

            context.SerializeJsonToPrettyFile(filePath, parsed);
        }
    }
}
