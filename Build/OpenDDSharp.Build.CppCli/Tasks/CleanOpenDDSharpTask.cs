using Cake.Common.Tools.MSBuild;
using Cake.Frosting;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("CleanOpenDDSharp")]    
    public class CleanOpenDDSharpTask : FrostingTask<BuildContext>
    {
        public override bool ShouldRun(BuildContext context)
        {
            return true;
        }

        public override void Run(BuildContext context)
        {
            context.MSBuild(BuildContext.OPENDDSHARP_SOLUTION_FILE, new MSBuildSettings
            {
                Configuration = context.BuildConfiguration,
                PlatformTarget = context.BuildPlatform,
                Targets = { "Clean" },
            });
        }
    }
}
