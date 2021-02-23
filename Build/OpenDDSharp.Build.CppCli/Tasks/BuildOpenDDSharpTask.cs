using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Frosting;
using System.IO;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("BuildOpenDDSharpTask")]
    [IsDependentOn(typeof(BuildThirdPartyTask))]
    public class BuildOpenDDSharpTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.NuGetRestore(BuildContext.OPENDDSHARP_SOLUTION_FILE, new NuGetRestoreSettings
            {
                NoCache = true
            });

            context.MSBuild(BuildContext.OPENDDSHARP_SOLUTION_FILE, new MSBuildSettings
            {
                Configuration = context.BuildConfiguration,
                PlatformTarget = context.BuildPlatform,
                Targets = { "Clean", "Build" },
                MaxCpuCount = 1,
                WorkingDirectory = Path.GetFullPath(BuildContext.OPENDDSHARP_SOLUTION_FOLDER),
                EnvironmentVariables =
                {
                    { nameof(BuildContext.DDS_ROOT), Path.GetFullPath(BuildContext.DDS_ROOT).TrimEnd('\\') },
                    { nameof(BuildContext.ACE_ROOT), Path.GetFullPath(BuildContext.ACE_ROOT).TrimEnd('\\') },
                    { nameof(BuildContext.TAO_ROOT), Path.GetFullPath(BuildContext.TAO_ROOT).TrimEnd('\\') },
                    { nameof(BuildContext.MPC_ROOT), Path.GetFullPath(BuildContext.MPC_ROOT).TrimEnd('\\') },
                },
            });
        }
    }
}
