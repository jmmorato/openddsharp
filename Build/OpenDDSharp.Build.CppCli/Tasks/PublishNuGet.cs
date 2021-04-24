using Cake.Common.Tools.NuGet.Push;
using Cake.Common.Tools.NuGet;
using Cake.Frosting;
using System.IO;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("PublishNuGet")]    
    public class PublishNuGet : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            string solutionPath = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER);
            string releaseFolder = Path.Combine(solutionPath, "Release");

            foreach (var file in Directory.GetFiles(releaseFolder, "*.nupkg"))
            {
                context.NuGetPush(file, new NuGetPushSettings
                {
                    ApiKey = context.NugetApiKey,
                    Source = "https://api.nuget.org/v3/index.json",
                });
            }
        }
    }
}
