using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.MSBuild;
using Cake.Frosting;
using System.IO;

namespace OpenDDSharp.Build.CppCli.Tasks
{
    [TaskName("BuildThirdParty")]
    [IsDependentOn(typeof(SetupThirdPartyTask))]
    public class BuildThirdPartyTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var ddsPath = Path.GetFullPath(BuildContext.DDS_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var acePath = Path.GetFullPath(BuildContext.ACE_ROOT).TrimEnd(Path.DirectorySeparatorChar);
            var platform = context.BuildPlatform;
            if (platform == PlatformTarget.x86)
            {
                platform = PlatformTarget.Win32;
            }
            context.MSBuild(BuildContext.OPENDDS_SOLUTION_FILE, new MSBuildSettings
            {
                Configuration = context.BuildConfiguration,
                PlatformTarget = platform,
                Targets = { "Build" },
                MaxCpuCount = 1,
                WorkingDirectory = Path.GetFullPath(BuildContext.DDS_ROOT),
                EnvironmentVariables =
                {
                    { nameof(BuildContext.DDS_ROOT), ddsPath },
                    { nameof(BuildContext.ACE_ROOT), acePath },
                    { nameof(BuildContext.TAO_ROOT), Path.GetFullPath(BuildContext.TAO_ROOT).TrimEnd(Path.DirectorySeparatorChar) },
                    { nameof(BuildContext.MPC_ROOT), Path.GetFullPath(BuildContext.MPC_ROOT).TrimEnd(Path.DirectorySeparatorChar) },
                },
                ToolVersion = context.VisualStudioVersion,
            });

            var ddsBinPlatform = Path.Combine(ddsPath, "bin_" + context.BuildPlatform);
            if (context.DirectoryExists(ddsBinPlatform))
            {
                context.DeleteDirectory(ddsBinPlatform, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });
            }
            context.CreateDirectory(ddsBinPlatform);
            context.CopyFiles(Path.Combine(ddsPath, "bin/*"), ddsBinPlatform);

            var ddsLibPlatform = Path.Combine(ddsPath, "lib_" + context.BuildPlatform);
            if (context.DirectoryExists(ddsLibPlatform))
            {
                context.DeleteDirectory(ddsLibPlatform, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });
            }
            context.CreateDirectory(ddsLibPlatform);
            context.CopyFiles(Path.Combine(ddsPath, "lib/*"), ddsLibPlatform);

            var aceBinPlatform = Path.Combine(acePath, "bin_" + context.BuildPlatform);
            if (context.DirectoryExists(aceBinPlatform))
            {
                context.DeleteDirectory(aceBinPlatform, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });
            }
            context.CreateDirectory(aceBinPlatform);
            context.CopyFiles(Path.Combine(acePath, "bin/*"), aceBinPlatform);

            var aceLibPlatform = Path.Combine(acePath, "lib_" + context.BuildPlatform);
            if (context.DirectoryExists(aceLibPlatform))
            {
                context.DeleteDirectory(aceLibPlatform, new DeleteDirectorySettings
                {
                    Force = true,
                    Recursive = true,
                });
            }
            context.CreateDirectory(aceLibPlatform);
            context.CopyFiles(Path.Combine(acePath, "lib/*"), aceLibPlatform);
        }
    }
}
